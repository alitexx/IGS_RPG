using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class killFriendManager : MonoBehaviour
{
    public GameObject tutorialbutton, killbutton, areyousurebutton, previousbutton;

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
    [SerializeField] private TextMeshProUGUI[] SpecialDesc;

    [SerializeField] private GameObject syncStrikeInfo;

    //For supports after killing
    [SerializeField] private CharSupportsData supportsData;

    public int partyLevel;

    private string charInQuestion;

    private bool killingMaybe;
    private bool befriendingMaybe;

    public BattleController battleController;
    
    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null);
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
            if (!playerController.hasKisa && !playerController.hasNicol) // if you don't have either nicol or kisa in the party, then it's a genocide run
            {
                charInQuestion = "Sophie";
                PartyMemberInQuestion.sprite = partyMembersAvailable[2];
                killingMaybe = true;
                exitKillFriendMenu(5);
                truebgFade.SetActive(true);
                return;
            }
            Debug.Log("sophie");
            charInQuestion = "Sophie";
            PartyMemberInQuestion.sprite = partyMembersAvailable[2];
        }
        EditTextInformation(charInQuestion);
        if (!playerController.hasKisa || playerController.KisaAbsorbed == 1) // replace this with if they've seen the tutorial
        {
            tutorialBG.DOFade(1, 1);
            tutorialMenu.SetActive(true);
            tutorialMenu.GetComponent<RectTransform>().DOMove(locations[0].position, 1);
            EventSystem.current.SetSelectedGameObject(tutorialbutton);
        }
        else
        {
            truebgFade.SetActive(true);
            tweenInObjects[2].GetComponent<RectTransform>().DOMove(locations[0].position, 1).OnComplete(() =>
            {
                tweenInObjects[0].GetComponent<RectTransform>().DOMove(locations[0].position, 1);
                tweenInObjects[1].GetComponent<RectTransform>().DOMove(locations[0].position, 1);
            });
            tutorialBG.DOFade(0, 1).OnComplete(() => { tutorialBG.gameObject.SetActive(false); truebgFade.SetActive(true); });
            EventSystem.current.SetSelectedGameObject(killbutton);
        }

        // If Alan has not taken another party member, show text for gaining sync strike.
        if (playerController.getObtainedCharacters() == 0)
        {
            syncStrikeInfo.SetActive(true);
        } else // They have someone. Don't show sync strike
        {
            syncStrikeInfo.SetActive(false);
        }
    }

    IEnumerator alanKillPartyAnim(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        alanAnimator.SetTrigger("kill"); // play kill anim
        yield return new WaitForSeconds(0.7f);
        partymembersFadeOut.DOFade(1, 0.5f).OnComplete(() => {
            finalWordsScript.playFinalWords(charInQuestion);
            alanAnimator.SetTrigger("return"); // returns to idle
            partymembersFadeOut.DOFade(0, 2f);
        });
        StopCoroutine(alanKillPartyAnim(timeToWait));
    }

    public void closeKillFriendTutorial()
    {
        EventSystem.current.SetSelectedGameObject(null);
        tutorialMenu.GetComponent<RectTransform>().DOMove(locations[1].position, 1).OnComplete(() => { tutorialMenu.SetActive(false); });
        tweenInObjects[2].GetComponent<RectTransform>().DOMove(locations[0].position, 1).OnComplete(() => {
            tweenInObjects[0].GetComponent<RectTransform>().DOMove(locations[0].position, 1);
            tweenInObjects[1].GetComponent<RectTransform>().DOMove(locations[0].position, 1);
        });
        tutorialBG.DOFade(0, 1).OnComplete(() => { 
            tutorialBG.gameObject.SetActive(false); 
            truebgFade.SetActive(true); 
            EventSystem.current.SetSelectedGameObject(killbutton); 
        });
    }

    ///NOTE!!!! THIS IS DONE!!!! You just need to call the function and pass in the boss's name, you MUST pass in the boss name as either "Kisa", "Nicol", or "Sophie"!!! please lol
    public void EditTextInformation(string bossName)
    {
        partyLevel = LevelManager.level;
        switch (bossName.ToUpper())
        {
            //placeholders right now because my computer is stupid
            case "KISA":
                elementIcon[0].sprite = elementsSprites[0];
                elementIcon[1].sprite = elementsSprites[0];
                killText[0].text = "Alan gains a bonus:\r\n" +
                    "Magic Attack: <color=#3B7D4F>+"+ ((LevelManager.level-1)) +"</color>\r\n" +
                    "Max Health: <color=#3B7D4F>+"+ (4*(LevelManager.level-1)) +"</color>\r\n" +
                    "Alan gains bonuses to these stats for all future level-ups.";
                killText[1].text = "New Magic Element!\r\n\r\n\r\n\r\nWind";
                befriendText[0].text = "New ally!\r\n<color=#3B7D4F>Kisa</color>";
                SpecialDesc[0].text = "Using your voice, heal all allies by 25% of their maximum health.";
                SpecialDesc[1].text = "Confuse the target, distracting them and skipping their next turn.";
                SpecialDesc[2].text = "Using your voice, heal all allies to their maximum health.";
                elementIcon[2].sprite = elementsSprites[3];
                elementIcon[3].sprite = elementsSprites[4];
                elementIcon[4].sprite = elementsSprites[5];
                if (partyLevel < 5)
                {
                    befriendText[1].text = " Sing\r\n\r\n ???\r\n\r\n ???";
                    SpecialDesc[1].text = "Locked until LVL 5";
                    SpecialDesc[2].text = "Locked until LVL 1O";
                    elementIcon[3].sprite = elementsSprites[12];
                    elementIcon[4].sprite = elementsSprites[12];
                } else if (partyLevel < 10)
                {
                    befriendText[1].text = " Sing\r\n\r\n Confuse\r\n\r\n ???";
                    SpecialDesc[2].text = "Locked until LVL 1O";
                    elementIcon[4].sprite = elementsSprites[12];
                } else
                {
                    befriendText[1].text = " Sing\r\n\r\n Confuse\r\n\r\n Performance";
                }
                
                befriendText[2].text = "Kisa's Magic Element:\r\n\r\n\r\n\r\nWind";
                break;
            case "NICOL":
                elementIcon[0].sprite = elementsSprites[1];
                elementIcon[1].sprite = elementsSprites[1];
                killText[0].text = "Alan gains a bonus:\r\n" +
                    "Strength: <color=#3B7D4F>+" + (LevelManager.level - 1) + "</color>\r\n" +
                    "Magic Attack: <color=#3B7D4F>+" + ((LevelManager.level - 1)) + "</color>\r\n" +
                    "Max Mana: <color=#3B7D4F>+" + (2 * (LevelManager.level - 1)) + "</color>\r\n" +
                    "Alan gains bonuses to these stats for all future level-ups.";
                killText[1].text = "New Magic Element!\r\n\r\n\r\n\r\nIce";
                befriendText[0].text = "New ally!\r\n<color=#3B7D4F>Nicol</color>";
                SpecialDesc[0].text = "Decrease the enemy's attack and defense.";
                SpecialDesc[1].text = "Something... happens.";
                SpecialDesc[2].text = "Cheer your allies on! All allies regain health for the next 3-5 turns.";
                elementIcon[2].sprite = elementsSprites[6];
                elementIcon[3].sprite = elementsSprites[7];
                elementIcon[4].sprite = elementsSprites[8];

                if (partyLevel < 5)
                {
                    befriendText[1].text = " Mockery\r\n\r\n ???\r\n\r\n ???";
                    elementIcon[3].sprite = elementsSprites[12];
                    elementIcon[4].sprite = elementsSprites[12];
                    SpecialDesc[1].text = "Locked until LVL 5";
                    SpecialDesc[2].text = "Locked until LVL 1O";
                } else if (partyLevel < 10)
                {
                    befriendText[1].text = " Mockery\r\n\r\n Trial and Error\r\n\r\n ???";
                    SpecialDesc[2].text = "Locked until LVL 1O";
                    elementIcon[4].sprite = elementsSprites[12];
                }
                else
                {
                    befriendText[1].text = " Mockery\r\n\r\n Trial and Error\r\n\r\n Motivate";
                }

                befriendText[2].text = "Nicol's Magic Element:\r\n\r\n\r\n\r\nIce";
                break;
            case "SOPHIE":
                elementIcon[0].sprite = elementsSprites[2];
                elementIcon[1].sprite = elementsSprites[2];
                killText[0].text = "Alan gains a bonus:\r\n" +
                    "Strength: <color=#3B7D4F>+" + (3 * (LevelManager.level - 1)) + "</color>\r\n" +
                    "Magic Attack: <color=#3B7D4F>+" + (LevelManager.level) + "</color>\r\n" +
                    "Alan gains bonuses to these stats for all future level-ups.";
                killText[1].text = "New Magic Element!\r\n\r\n\r\n\r\nElectric";
                befriendText[0].text = "New ally!\r\n<color=#3B7D4F>Sophie</color>";
                SpecialDesc[0].text = "A physical attack that targets all enemies. Has a high chance of missing.";
                SpecialDesc[1].text = "Your attacks always hit, and your critical hit chance is higher for the next 3 turns.";
                SpecialDesc[2].text = "Strike all enemies, dealing 30 damage to each hit. Has a high chance of missing.";
                elementIcon[2].sprite = elementsSprites[9];
                elementIcon[3].sprite = elementsSprites[10];
                elementIcon[4].sprite = elementsSprites[11];
                if (partyLevel < 5)
                {
                    befriendText[1].text = " Earthquake\r\n\r\n ???\r\n\r\n ???";
                    SpecialDesc[1].text = "Locked until LVL 5";
                    SpecialDesc[2].text = "Locked until LVL 1O";
                    elementIcon[3].sprite = elementsSprites[12];
                    elementIcon[4].sprite = elementsSprites[12];
                } else if (partyLevel < 10)
                {
                    befriendText[1].text = " Earthquake\r\n\r\n Focus\r\n\r\n ???";
                    SpecialDesc[2].text = "Locked until LVL 1O";
                    elementIcon[4].sprite = elementsSprites[12];
                }
                else
                {
                    befriendText[1].text = " Earthquake\r\n\r\n Focus\r\n\r\n Thunderstorm";
                }

                befriendText[2].text = "Sophie's Magic Element:\r\n\r\n\r\n\r\nElectric";
                break;
        }
    }

    public void exitKillFriendMenu(float killSpeed = 0.25f)
    {
        //a choice has been made
        EventSystem.current.SetSelectedGameObject(null);
        if (killingMaybe)
        {
            //For audio
            youWinMenu.killedPartyMember = true;
            //Change supports, if applicable
            supportsData.becomeAffected();
            StartCoroutine(alanKillPartyAnim(killSpeed));
            
            SteamIntegrations steamInt = FindObjectOfType<SteamIntegrations>();

            //Achievements
            switch (charInQuestion)
            {
                case "Kisa":
                    steamInt.UnlockAchievement("ACH_K_kisa");
                    break;
                case "Nicol":
                    steamInt.UnlockAchievement("ACH_K_nicol");
                    break;
                case "Sophie":
                    steamInt.UnlockAchievement("ACH_K_sophie");
                    break;
                default:
                    Debug.Log("not supposed to be here");
                    break;
            }

            }
        else if (befriendingMaybe)
        {
            SteamIntegrations steamInt = FindObjectOfType<SteamIntegrations>();

            //Achievements
            switch (charInQuestion)
            {
                case "Kisa":
                    steamInt.UnlockAchievement("ACH_S_kisa");
                    break;
                case "Nicol":
                    steamInt.UnlockAchievement("ACH_S_nicol");
                    break;
                case "Sophie":
                    steamInt.UnlockAchievement("ACH_S_sophie");
                    break;
                default:
                    Debug.Log("not supposed to be here");
                    break;
            }

            alanAnimator.SetTrigger("befriend");
            closeAreYouSure();
            tweenInObjects[0].GetComponent<RectTransform>().DOMove(locations[2].position, 1);
            tweenInObjects[1].GetComponent<RectTransform>().DOMove(locations[3].position, 1);
            tweenInObjects[2].GetComponent<RectTransform>().DOMove(locations[4].position, 1);
            //slowly fade out alan and the other party member
            partymembersFadeOut.DOFade(1,2.5f).OnComplete(() => { partymembersFadeOut.DOFade(0, 2f).OnComplete(() => {
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
        EventSystem.current.SetSelectedGameObject(null);
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
        EventSystem.current.SetSelectedGameObject(areyousurebutton);
    }

    public void closeAreYouSure()
    {
        EventSystem.current.SetSelectedGameObject(null);
        tutorialBG.DOFade(0, 1).OnComplete(() => { tutorialBG.gameObject.SetActive(false);});
        areYouSureMenu.GetComponent<RectTransform>().DOMove(locations[1].position, 1).OnComplete(() => { areYouSureMenu.SetActive(false); });

        if(killingMaybe)
        {
            EventSystem.current.SetSelectedGameObject(killbutton);
        } else
        {
            EventSystem.current.SetSelectedGameObject(previousbutton);
        }

        killingMaybe = false;
        befriendingMaybe = false;
    }

}
