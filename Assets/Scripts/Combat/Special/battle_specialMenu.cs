using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class battle_specialMenu : MonoBehaviour
{
    private Dictionary<string, string[]> specialDescriptions;

    private void Awake()
    {
        //I have to conform to the naming convention but we know who is who
        specialDescriptions = new Dictionary<string, string[]> {
        { "0", alanSpecialDescriptions },
        { "1", kisaSpecialDescriptions },
        { "2", nicolSpecialDescriptions },
        { "3", sophieSpecialDescriptions }
    };
    }

    public int reorganizeName(string name)
    {
        switch (name)
        {
            case "tank guy":
                return 0;
            case "bard guy":
                return 1;
            case "monk guy":
                return 3;
            case "mage guy":
                return 2;
            default:
                return -1;
        }
    }

    //different arrays of text for different character's specials
    [SerializeField] private string[] alanSpecialDescriptions;
    [SerializeField] private string[] kisaSpecialDescriptions;
    [SerializeField] private string[] nicolSpecialDescriptions;
    [SerializeField] private string[] sophieSpecialDescriptions;

    //You need to pass this in
    public int WhoAreWeViewing;
    [SerializeField] private TextMeshProUGUI[] specialNameText;
    [SerializeField] private TextMeshProUGUI[] specialTXT_Desc;
    [SerializeField] private GameObject openSpecialMenu;

    [SerializeField] private GameObject[] buttonUI;
    [SerializeField] private GameObject[] allButtons;
    [SerializeField] private GameObject[] syncStrike;

    [SerializeField] private PlayerController playerController;

    [SerializeField] private updateSPOnScreen updateSP;

    //change back button to allow it to move down to special menu

    //Stuff for checking party level, who we're viewing, etc.
    private int level;

    public void setWhoAreWeViewing(string whoAreWeViewing)
    {
        WhoAreWeViewing = reorganizeName(whoAreWeViewing);
    }

    private void OnEnable()
    {
        //because it defaults to their first special = always is 1
        updateSP.setSliderGlow(1);
        level = LevelManager.level;
        //check party level
        if (level < 5)
        {
            //if party level < 5, only display first special. return
            DisplaySpecial(1);
        }
        else if (level < 10)
        {
            //if party level < 10, display first two specials. return
            DisplaySpecial(2);
        }
        else
        {
            //else, party is >=10. display all specials
            DisplaySpecial(3);
        }

        switch (WhoAreWeViewing)
        {
            case 0://ALAN
                buttonUI[0].SetActive(true);
                buttonUI[1].SetActive(false);
                buttonUI[2].SetActive(false);
                buttonUI[3].SetActive(false);
                break;
            case 1: //KISA
                buttonUI[0].SetActive(false);
                buttonUI[1].SetActive(true);
                buttonUI[2].SetActive(false);
                buttonUI[3].SetActive(false);
                break;
            case 2: //NICOL
                buttonUI[0].SetActive(false);
                buttonUI[1].SetActive(false);
                buttonUI[2].SetActive(true);
                buttonUI[3].SetActive(false);
                break;
            case 3://SOPHIE
                buttonUI[0].SetActive(false);
                buttonUI[1].SetActive(false);
                buttonUI[2].SetActive(false);
                buttonUI[3].SetActive(true);
                break;
        }

        if(playerController.getObtainedCharacters()>= 1)
        {
            syncStrike[WhoAreWeViewing].SetActive(true);
        } else
        {
            syncStrike[WhoAreWeViewing].SetActive(false);
        }
    }

    private void DisplaySpecial(int howMany)
    {
        //How many will never be higher than 3
        int counter = 0;
        if (specialDescriptions.TryGetValue(WhoAreWeViewing.ToString(), out string[] descriptions))
        {
            for (counter = 0; counter < howMany; counter++)
            {
                allButtons[counter + (3 * WhoAreWeViewing)].SetActive(true);
                specialNameText[counter].text = descriptions[counter];  // Set the title
                specialTXT_Desc[counter].text = descriptions[counter + 3];   // Set the description
            }
            if (counter < 3) // If we have still not displayed enough, that means that there are some skills that have not been unlocked. Make sure they are locked
            {
                //Might break, have no idea
                for (counter = howMany; counter < 3; counter++)
                {
                    allButtons[counter+(3*WhoAreWeViewing)].SetActive(false);
                    specialNameText[counter].text = " ???";
                    specialTXT_Desc[counter].text = "Locked until level "+(counter)*5;
                }
            }
            
        }
        else
        {
            Debug.LogError("Character not found: " + WhoAreWeViewing);
        }
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(allButtons[WhoAreWeViewing * 3]);
        // Manually trigger the OnSelect event for the first selected object
        ExecuteEvents.Execute(allButtons[WhoAreWeViewing*3], new BaseEventData(EventSystem.current), ExecuteEvents.selectHandler);

    }

    private void OnDisable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(openSpecialMenu);
    }

}
