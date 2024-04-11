using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

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
    private bool hasUpdatedGained = false;
    private int remainingExp;
    private float fillAmountVal;

    private int gainedExperience = 20;
    private int currentExperience = 0;
    private Coroutine gainExperienceCoroutine;

    private void OnEnable()
    {
        //play the you win animation
        am.playBGM("T9", 0.1f);
        youWinText.DOMove(locations[0].position, 0.35f).OnComplete(() => {
            youWinText.DOMove(locations[1].position, 0.7f).OnComplete(() => {
                expBar.gameObject.SetActive(true);
                expBar.DOFade(1, 1).OnComplete(() => {
                    gainExperienceCoroutine = StartCoroutine(ShowGainedExperience());
                });
            });
        });
    }

    IEnumerator ShowGainedExperience()
    {
        yield return new WaitForSeconds(0.75f); // Wait for 0.75 second after expBar fades in
        gainedEXP.gameObject.SetActive(true);
        if (!hasUpdatedGained)
        {
            gainedEXP.text = "+" + gainedExperience + " EXP";
        }

        yield return new WaitForSeconds(1); // Wait for 1 second before increasing current EXP
        int totalEXP = currentExperience + gainedExperience;

        float duration = 1f; // Duration for the increase animation
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
            gainedEXP.gameObject.SetActive(false);
            expBar.gameObject.SetActive(false);
            levelUpObject.SetActive(true);
            StopCoroutine(gainExperienceCoroutine);
            yield return new WaitForSeconds(1); // Wait for player interaction with level up screen
            newValue = 0;
            currentExperience = 0; // Reset current EXP to 0 after leveling up
            remainingExp -= 100; // Deduct 100 from remaining EXP
        }
        //they should not level up past this
        while (newValue < remainingExp)
        {
            // Check if current experience reaches 100, if so, pause the coroutine
            if (currentExperience >= 100)
            {
                newValue = Mathf.Lerp(currentExperience, 100, elapsedTime / duration);
                currentEXP.text = Mathf.RoundToInt(newValue).ToString() + "/100";
                // Clamp the float value between 0 and 1
                fillAmountVal = Mathf.Lerp(currentExperience / 100f, 1, elapsedTime / duration);
                expSliderBar.fillAmount = fillAmountVal;
                elapsedTime += Time.deltaTime;
            }
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
        yield return new WaitForSeconds(1);
        currentEXP.text = Mathf.RoundToInt(newValue).ToString() + "/100";
        hasUpdatedGained = false;
        expSliderBar.fillAmount = fillAmountVal;
        endBattleButton.gameObject.SetActive(true);
        endBattleButton.DOMove(locations[6].position, 1f);
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
        currentExperience = 0; // Reset current EXP to 0 after leveling up
        gainedExperience -= 100;
        hasUpdatedGained = true;
        levelUpObject.SetActive(false);
        expBar.gameObject.SetActive(true);
        gainedEXP.gameObject.SetActive(true);
        gainExperienceCoroutine = StartCoroutine(ShowGainedExperience());
    }

    public void closeYouWinMenu()
    {
        expBar.DOFade(0, 0.5f).OnComplete(() => { expBar.gameObject.SetActive(false); });
        am.playBGM("T2");
        battleEnterAnimator.SetBool("BattleOver", true);
        endBattleButton.DOMove(locations[5].position, 1f).OnComplete(() => { endBattleButton.gameObject.SetActive(false); });
        youWinText.DOMove(locations[2].position, 0.35f).OnComplete(() => {
            youWinText.DOMove(locations[3].position, 0.25f).OnComplete(() => {
                youWinText.position = locations[4].position;
                battleEnterAnimator.SetBool("BattleOver", false);
            });
        });
    }
}

public class FillSetter : MonoBehaviour
{
    public Image imageToFill;
    public float myFloat;

    void Update()
    {
        // Ensure the imageToFill and myFloat are set correctly
        if (imageToFill != null)
        {
            // Clamp the float value between 0 and 1
            myFloat = Mathf.Clamp01(myFloat);

            // Set the fill amount of the image
            imageToFill.fillAmount = myFloat;
        }
    }
}

//old script, the one above was made using the help of mr GPT
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro;
//using DG.Tweening;

//public class youWinMenu : MonoBehaviour
//{
//    [SerializeField] private RectTransform[] locations;
//    [SerializeField] private RectTransform youWinText;
//    [SerializeField] private CanvasGroup expBar;
//    [SerializeField] private RectTransform endBattleButton;
//    [SerializeField] private TextMeshProUGUI gainedEXP;
//    [SerializeField] private TextMeshProUGUI CurrentEXP;
//    private void OnEnable()
//    {
//        youWinText.DOMove(locations[0].position, 0.35f).OnComplete(() => {
//            youWinText.DOMove(locations[1].position, 0.7f).OnComplete(() => {
//                expBar.gameObject.SetActive(true);
//                expBar.DOFade(1, 1);
//            });
//        });
//    }




//    public void closeYouWinMenu()
//    {
//        //do whatever you need to do to end battle
//        expBar.DOFade(0, 1).OnComplete(() => {expBar.gameObject.SetActive(false);});
//        endBattleButton.DOMove(locations[5].position, 1f);
//        youWinText.DOMove(locations[2].position, 0.35f).OnComplete(() => {
//            youWinText.DOMove(locations[3].position, 0.25f).OnComplete(() => {
//                youWinText.position = locations[4].position;
//            });
//        });
//    }
//}
