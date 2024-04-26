using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using DIALOGUE;

public class killFriendManager : MonoBehaviour
{
    [SerializeField] private GameObject tutorialMenu;
    [SerializeField] private GameObject areYouSureMenu;
    [SerializeField] private TextMeshProUGUI areYouSureText;
    [SerializeField] private CanvasGroup tutorialBG;
    [SerializeField] private GameObject truebgFade;
    [SerializeField] private RectTransform[] locations;
    [SerializeField] private RectTransform[] tweenInObjects;
    [SerializeField] private TextMeshProUGUI[] killText;
    [SerializeField] private TextMeshProUGUI[] befriendText;
    [SerializeField] private Image[] elementIcon;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Animator alanAnimator;
    [SerializeField] private Image PartyMemberInQuestion;
    [SerializeField] private Sprite[] partyMembersAvailable;
    // might be needed later, commented out for now
    //[SerializeField] private LevelManager levelManager;
    [SerializeField] private Sprite[] elementsSprites;
    [SerializeField] private CanvasGroup partymembersFadeOut;
    [SerializeField] private partyFinalWords finalWordsScript;

    private string charInQuestion;

    private bool killingMaybe;
    private bool befriendingMaybe;

    public BattleController battleController;
    
    private void OnEnable()
    {
        partymembersFadeOut.DOFade(1, 1.5f);
        killingMaybe = false;
        befriendingMaybe = false;
        PauseMenu.canOpenPause = false;
        if (playerController.KisaBoss)
        {
            Debug.Log("kisa");
            charInQuestion = "Kisa";
            PartyMemberInQuestion.sprite = partyMembersAvailable[0];
        } else if (playerController.NicolBoss)
        {
            Debug.Log("nicol");
            charInQuestion = "Nicol";
            PartyMemberInQuestion.sprite = partyMembersAvailable[1];
        } else if (playerController.SophieBoss)
        {
            Debug.Log(playerController.absorbKisa);
            Debug.Log(playerController.absorbNicol);
            if (playerController.absorbKisa && playerController.absorbNicol)
            {
                StartCoroutine(genocideEncounter());
                return;
            }

            Debug.Log("sophie");
            charInQuestion = "Sophie";
            PartyMemberInQuestion.sprite = partyMembersAvailable[2];
        }
        Debug.Log(charInQuestion);
        EditTextInformation(charInQuestion);
        if (true) // replace this with if they've seen the tutorial
        {
            tutorialBG.DOFade(1, 1);
            tutorialMenu.SetActive(true);
            tutorialMenu.GetComponent<RectTransform>().DOMove(locations[0].position, 1);
        }
        //else
        //{
        //    truebgFade.SetActive(true);
        //    tweenInObjects[2].GetComponent<RectTransform>().DOMove(locations[0].position, 1).OnComplete(() => {
        //        tweenInObjects[0].GetComponent<RectTransform>().DOMove(locations[0].position, 1);
        //        tweenInObjects[1].GetComponent<RectTransform>().DOMove(locations[0].position, 1);
        //    });
        //    tutorialBG.DOFade(0, 1).OnComplete(() => { tutorialBG.gameObject.SetActive(false); truebgFade.SetActive(true); });
        //}
    }

    IEnumerator genocideEncounter()
    {
        Debug.Log("genocide GO!!!");
        killingMaybe = true;
        charInQuestion = "Sophie";
        PartyMemberInQuestion.sprite = partyMembersAvailable[2];
        truebgFade.SetActive(true);
        yield return new WaitForSeconds(5f);
        //increase heartbeat sound?
        exitKillFriendMenu();
    }

    public void closeKillFriendTutorial()
    {
        tutorialMenu.GetComponent<RectTransform>().DOMove(locations[1].position, 1).OnComplete(() => { tutorialMenu.SetActive(false); });
        tweenInObjects[2].GetComponent<RectTransform>().DOMove(locations[0].position, 1).OnComplete(() => {
            tweenInObjects[0].GetComponent<RectTransform>().DOMove(locations[0].position, 1);
            tweenInObjects[1].GetComponent<RectTransform>().DOMove(locations[0].position, 1);
        });
        tutorialBG.DOFade(0, 1).OnComplete(() => { tutorialBG.gameObject.SetActive(false); truebgFade.SetActive(true); });
    }

    ///NOTE!!!! THIS IS DONE!!!! You just need to call the function and pass in the boss's name, you MUST pass in the boss name as either "Kisa", "Nicol", or "Sophie"!!! please lol
    public void EditTextInformation(string bossName)
    {
        switch (bossName.ToUpper())
        {
            //placeholders right now because my computer is stupid
            case "KISA":
                elementIcon[0].sprite = elementsSprites[0];
                elementIcon[1].sprite = elementsSprites[0];
                killText[0].text = "Alan gains a bonus:\r\n" +
                    "Strength: <color=#3B7D4F>+0</color>\r\n" +
                    "Magic Attack: <color=#3B7D4F>+"+ (LevelManager.level) +"</color>\r\n" +
                    "Defense: <color=#3B7D4F>+0</color>\r\n" +
                    "Speed: <color=#3B7D4F>+0</color>\r\n" +
                    "Max Health: <color=#3B7D4F>+"+ (2*(LevelManager.level-1)) +"</color>\r\n" +
                    "Max Mana: <color=#3B7D4F>+"+ (LevelManager.level) +"</color>";
                killText[1].text = "New Magic Element!\r\n\r\n\r\n\r\nWind";
                befriendText[0].text = "New ally!\r\n<color=#3B7D4F>Kisa</color>";
                befriendText[1].text = "\r\nSpecial Ability: <color=#cf752b>Sing</color>\r\nHeals all allies by 50% of their maximum Health.";
                befriendText[2].text = "Kisa's Magic Element:\r\n\r\n\r\n\r\nWind";
                break;
            case "NICOL":
                elementIcon[0].sprite = elementsSprites[1];
                elementIcon[1].sprite = elementsSprites[1];
                killText[0].text = "Alan gains a bonus:\r\n" +
                    "Strength: <color=#3B7D4F>+0</color>\r\n" +
                    "Magic Attack: <color=#3B7D4F>+" + (2 * (LevelManager.level - 1)) + "</color>\r\n" +
                    "Defense: <color=#3B7D4F>+0</color>\r\n" +
                    "Speed: <color=#3B7D4F>+" + (LevelManager.level) + "</color>\r\n" +
                    "Max Health: <color=#3B7D4F>+0</color>\r\n" +
                    "Max Mana: <color=#3B7D4F>+" + (LevelManager.level) + "</color>";
                killText[1].text = "New Magic Element!\r\n\r\n\r\n\r\nIce";
                befriendText[0].text = "New ally!\r\n<color=#3B7D4F>Nicol</color>";
                befriendText[1].text = "\r\nSpecial Ability: <color=#cf752b>Examine</color>\r\nView one enemy's weakness.";
                befriendText[2].text = "Nicol's Magic Element:\r\n\r\n\r\n\r\nIce";
                break;
            case "SOPHIE":
                elementIcon[0].sprite = elementsSprites[2];
                elementIcon[1].sprite = elementsSprites[2];
                killText[0].text = "Alan gains a bonus:\r\n" +
                    "Strength: <color=#3B7D4F>+" + (2 * (LevelManager.level - 1)) + "</color>\r\n" +
                    "Magic Attack: <color=#3B7D4F>+0</color>\r\n" +
                    "Defense: <color=#3B7D4F>+" + (LevelManager.level) + "</color>\r\n" +
                    "Speed: <color=#3B7D4F>+" + (LevelManager.level) + "</color>\r\n" +
                    "Max Health: <color=#3B7D4F>+0</color>\r\n" +
                    "Max Mana: <color=#3B7D4F>+0</color>";
                killText[1].text = "New Magic Element!\r\n\r\n\r\n\r\nElectric";
                befriendText[0].text = "New ally!\r\n<color=#3B7D4F>Sophie</color>";
                befriendText[1].text = "\r\nSpecial Ability: <color=#cf752b>Thunderstorm</color>\r\nAttack all enemies with electric magic.";
                befriendText[2].text = "Sophie's Magic Element:\r\n\r\n\r\n\r\nElectric";
                break;
        }
    }

    public void exitKillFriendMenu()
    {
        //a choice has been made
        if (killingMaybe)
        {
            youWinMenu.killedPartyMember = true;
            alanAnimator.SetTrigger("kill"); // play kill anim
            partymembersFadeOut.DOFade(1, 0.5f).OnComplete(() => {
                finalWordsScript.playFinalWords(charInQuestion);
                alanAnimator.SetTrigger("return"); // returns to idle
                partymembersFadeOut.DOFade(0, 2f);
            });
            }
        else if (befriendingMaybe)
        {
            alanAnimator.SetTrigger("befriend");
            closeAreYouSure();
            tweenInObjects[0].GetComponent<RectTransform>().DOMove(locations[2].position, 1);
            tweenInObjects[1].GetComponent<RectTransform>().DOMove(locations[3].position, 1);
            tweenInObjects[2].GetComponent<RectTransform>().DOMove(locations[4].position, 1);
            //slowly fade out alan and the other party member
            partymembersFadeOut.DOFade(1,1f).OnComplete(() => { partymembersFadeOut.DOFade(0, 2f).OnComplete(() => {
                battleController.BefriendButton();
                truebgFade.GetComponent<killFriendBG>().fadeOut();
                finalWordsScript.openYouWin();
                alanAnimator.SetTrigger("return"); // returns to idle
            });
            });
            return;
        }


        truebgFade.GetComponent<killFriendBG>().fadeOut();
        closeAreYouSure();
        tweenInObjects[0].GetComponent<RectTransform>().DOMove(locations[2].position, 1);
        tweenInObjects[1].GetComponent<RectTransform>().DOMove(locations[3].position, 1);
        tweenInObjects[2].GetComponent<RectTransform>().DOMove(locations[4].position, 1);

        
    }

    public void openAreYouSure(bool isKilling)
    {
        tutorialBG.gameObject.SetActive(true);
        tutorialBG.DOFade(1, 1);
        if (isKilling)
        {
            areYouSureText.text = "You have chosen to <color=#AD2F45>kill</color> " + charInQuestion + ". Are you sure?";
            killingMaybe = true;
        }
        else
        {
            areYouSureText.text = "You have chosen to <color=#FF5277>befriend</color> " + charInQuestion + ". Are you sure?";
            befriendingMaybe = true;
        }
        areYouSureMenu.SetActive(true);
        areYouSureMenu.GetComponent<RectTransform>().DOMove(locations[0].position, 1);
    }

    public void closeAreYouSure()
    {
        tutorialBG.DOFade(0, 1).OnComplete(() => { tutorialBG.gameObject.SetActive(false);});
        areYouSureMenu.GetComponent<RectTransform>().DOMove(locations[1].position, 1).OnComplete(() => { areYouSureMenu.SetActive(false); });

        killingMaybe = false;
        befriendingMaybe = false;
    }

}
