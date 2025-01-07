using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class youWinMenu : MonoBehaviour
{

    [SerializeField] private RectTransform[] locations;
    [SerializeField] private RectTransform youWinText;
    [SerializeField] private CanvasGroup expBar;
    [SerializeField] private Image expSliderBar;
    [SerializeField] private RectTransform endBattleButton;
    [SerializeField] private TextMeshProUGUI gainedEXP;
    [SerializeField] private TextMeshProUGUI currentEXP;
    [SerializeField] private GameObject levelUpObject;
    [SerializeField] private audioManager am;
    [SerializeField] Animator battleEnterAnimator;
    [SerializeField] private mainDialogueManager mainDialogueManager;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private CanvasGroup killedPartyMemberBG;
    public static bool killedPartyMember = false;

    public static string loadedDialogue = "...";
    private bool hasUpdatedGained = false;
    private int remainingExp;
    private float fillAmountVal = 0;

    public LevelManager levelManager;

    private int gainedExperience = 20;
    private int currentExperience;
    private Coroutine gainExperienceCoroutine;

    private void OnEnable()
    {
        //cannot open pause menu at this time
        PauseMenu.canOpenPause = false;
        expBar.gameObject.SetActive(false);
        gainedEXP.gameObject.SetActive(false);
        SetGainedExperience(levelManager.gainedEXP);
        am.stopBGM(0.1f);
        am.stopHeartbeatSFX();
        currentExperience = levelManager.currentEXP;

        fillAmountVal = currentExperience / 100;

        currentEXP.text = currentExperience + "/100";

        //play the you win animation
        am.playBGM("T9", 0.1f);
        youWinText.DOMove(locations[0].position, 0.35f).OnComplete(() => {
            youWinText.DOMove(locations[1].position, 0.7f).OnComplete(() => {
                expBar.gameObject.SetActive(true);
                expBar.DOFade(1, .5f).OnComplete(() => {
                    gainExperienceCoroutine = StartCoroutine(ShowGainedExperience());
                });
            });
        });
    }

    IEnumerator ShowGainedExperience()
    {
        currentEXP.text = currentExperience + "/100";
        yield return new WaitForSeconds(0.25f); // Wait for 0.75 second after expBar fades in
        gainedEXP.gameObject.SetActive(true);
        if (!hasUpdatedGained)
        {
            gainedEXP.text = "+" + gainedExperience + " EXP";
        }

        yield return new WaitForSeconds(0.5f); // Wait for 1 second before increasing current EXP
        int totalEXP = currentExperience + gainedExperience;

        float duration = 0.75f; // Duration for the increase animation
        float elapsedTime = 0f;

        int levelUpCount = totalEXP / 100; // Check how many times player leveled up
        remainingExp = totalEXP % 100; // Calculate remaining experience after leveling up
        float newValue = currentExperience;

        //if they've leveled up
        for (int i = 0; i < levelUpCount; i++)
        {
            while (newValue < 100)
            {
                newValue = Mathf.Lerp(currentExperience, 100, elapsedTime / duration);
                currentEXP.text = Mathf.RoundToInt(newValue).ToString() + "/100";
                // Clamp the float value between 0 and 1
                fillAmountVal = Mathf.Lerp(currentExperience / 100f, 1, elapsedTime / duration);
                expSliderBar.fillAmount = fillAmountVal;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            am.playSFX(15);
            //levelManager.LevelUp();
            //gainedEXP.gameObject.SetActive(false);
            //expBar.gameObject.SetActive(false);
            levelUpObject.SetActive(true);
            StopCoroutine(gainExperienceCoroutine);
            yield return new WaitForSeconds(0.25f); // Wait for player interaction with level up screen
            newValue = 0;
            expSliderBar.fillAmount = 0;
            currentExperience = 0; // Reset current EXP to 0 after leveling up
            remainingExp -= 100; // Deduct 100 from remaining EXP
        }
        //they should not level up past this
        while (newValue < remainingExp)
        {
            // Check if current experience reaches 100, if so, pause the coroutine
            //if (currentExperience >= 100)
            //{

            //    newValue = Mathf.Lerp(currentExperience, 100, elapsedTime / duration);
            //    currentEXP.text = Mathf.RoundToInt(newValue).ToString() + "/100";
            //    Clamp the float value between 0 and 1
            //    fillAmountVal = Mathf.Lerp(currentExperience / 100f, 1, elapsedTime / duration);
            //    expSliderBar.fillAmount = fillAmountVal;
            //    elapsedTime += Time.deltaTime;
            //}
            //if they didnt level up, show gaining exp
            newValue = Mathf.Lerp(currentExperience, totalEXP, elapsedTime / duration);
            currentEXP.text = Mathf.RoundToInt(newValue).ToString() + "/100";
            // Clamp the float value between 0 and 1
            fillAmountVal = Mathf.Lerp(currentExperience/100f, totalEXP/100f, elapsedTime / duration);
            expSliderBar.fillAmount = fillAmountVal;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // If coroutine completes without pausing, update the current EXP to the target value
        yield return new WaitForSeconds(0.5f);
        expSliderBar.fillAmount = fillAmountVal;
        levelManager.currentEXP = Mathf.RoundToInt(newValue);
        currentExperience = Mathf.RoundToInt(newValue);
        currentEXP.text = Mathf.RoundToInt(currentExperience).ToString() + "/100";
        hasUpdatedGained = false;
        endBattleButton.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        endBattleButton.DOMove(locations[6].position, 0.35f).OnComplete(() => { EventSystem.current.SetSelectedGameObject(endBattleButton.gameObject); });
    }

    // USE THIS TO SET HOW MUCH EXP HAS BEEN GAINED!!
    //set this before leveling up!!
    public void SetGainedExperience(int exp)
    {
        gainedExperience = exp;
    }

    // Method to continue the level gaining animation after clicking the continue button
    public void ContinueLevelGaining()
    {
        gainedExperience = (currentExperience + gainedExperience) - 100;
        currentExperience = 0; // Reset current EXP to 0 after leveling up
        expSliderBar.fillAmount = 0;
        fillAmountVal = 0;
        hasUpdatedGained = true;
        levelUpObject.SetActive(false);
        //expBar.gameObject.SetActive(true);
        //gainedEXP.gameObject.SetActive(true);
        gainExperienceCoroutine = StartCoroutine(ShowGainedExperience());
    }

    public void closeYouWinMenu()
    {
        killedPartyMember = false;
        expBar.DOFade(0, 0.5f).OnComplete(() => { expBar.gameObject.SetActive(false); });
        battleEnterAnimator.SetBool("BattleOver", true);
        endBattleButton.DOMove(locations[5].position, 0.5f).OnComplete(() => { endBattleButton.gameObject.SetActive(false); });
        youWinText.DOMove(locations[2].position, 0.35f).OnComplete(() => {
            youWinText.DOMove(locations[3].position, 0.75f).OnComplete(() => {
                PauseMenu.canOpenPause = true;
                battleEnterAnimator.SetBool("BattleOver", false);
                youWinText.position = locations[4].position;
                if (loadedDialogue == "Cutscene") // this is the end of the game. send to maindialoguemanager
                {
                    //find id
                    mainDialogueManager.dialogueSTART(findEndingID());
                    loadedDialogue = "...";
                    this.gameObject.SetActive(false);
                    return;
                }
                if (loadedDialogue != "...")
                {
                    mainDialogueManager.dialogueSTART(loadedDialogue);
                    loadedDialogue = "...";
                    this.gameObject.SetActive(false);
                    return;
                }
                am.playBGM("T2");
                playerController.isfrozen = false;
                expBar.gameObject.SetActive(false);
                gainedEXP.gameObject.SetActive(false);
                this.gameObject.SetActive(false);
            });
        });
    }

    // run only when loaded dialogue == cutscene. this is for the end of the game
    private string findEndingID()
    {

        //this is a very long and confusing nested if statement that is kinda like a tree
        // i dont know how best to explain it but it does what it needs to
        if (playerController.hasKisa)
        {
            if (playerController.hasNicol)
            {
                if (playerController.hasSophie)
                {
                    return "end_kns";
                }
                return "end_ns";
            }
            if (playerController.hasSophie)
            {
                return "end_ks";
            }
            return "end_k";
        }
        if (playerController.hasNicol)
        {
            if (playerController.hasSophie)
            {
                return "end_ns";
            }
            return "end_n";
        }
        return "end_genocide";
    }
}