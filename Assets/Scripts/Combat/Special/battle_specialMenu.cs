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
        { "alan", alanSpecialDescriptions },
        { "kisa", kisaSpecialDescriptions },
        { "nicol", nicolSpecialDescriptions },
        { "sophie", sophieSpecialDescriptions }
    };
    }

    public string reorganizeName(string name)
    {
        switch (name)
        {
            case "tank guy":
                return "alan";
            case "bard guy":
                return "kisa";
            case "monk guy":
                return "sophie";
            case "mage guy":
                return "nicol";
            default:
                return name;
        }
    }

    //different arrays of text for different character's specials
    [SerializeField] private string[] alanSpecialDescriptions;
    [SerializeField] private string[] kisaSpecialDescriptions;
    [SerializeField] private string[] nicolSpecialDescriptions;
    [SerializeField] private string[] sophieSpecialDescriptions;

    //You need to pass this in
    public string WhoAreWeViewing;
    [SerializeField] private TextMeshProUGUI[] specialNameText;
    [SerializeField] private TextMeshProUGUI[] specialTXT_Desc;
    [SerializeField] private GameObject[] firstspecialButton;
    [SerializeField] private GameObject openSpecialMenu;

    [SerializeField] private GameObject[] buttonUI;

    //Stuff for checking party level, who we're viewing, etc.
    private int level;

    public void setWhoAreWeViewing(string whoAreWeViewing)
    {
        WhoAreWeViewing = reorganizeName(whoAreWeViewing);
    }

    private void OnEnable()
    {

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
    }

    private void DisplaySpecial(int howMany)
    {
        //How many will never be higher than 3
        int counter = 0;
        if (specialDescriptions.TryGetValue(WhoAreWeViewing.ToLower(), out string[] descriptions))
        {
            switch (WhoAreWeViewing)
            {
                case "alan":
                    buttonUI[0].SetActive(true);
                    buttonUI[1].SetActive(false);
                    buttonUI[2].SetActive(false);
                    buttonUI[3].SetActive(false);
                    //So we aren't wasting processing time grabbing the same variable over and over again
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(firstspecialButton[0]);
                    // Manually trigger the OnSelect event for the first selected object
                    ExecuteEvents.Execute(firstspecialButton[0], new BaseEventData(EventSystem.current), ExecuteEvents.selectHandler);
                    break;
                case "kisa":
                    buttonUI[0].SetActive(false);
                    buttonUI[1].SetActive(true);
                    buttonUI[2].SetActive(false);
                    buttonUI[3].SetActive(false);//So we aren't wasting processing time grabbing the same variable over and over again
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(firstspecialButton[1]);
                    // Manually trigger the OnSelect event for the first selected object
                    ExecuteEvents.Execute(firstspecialButton[1], new BaseEventData(EventSystem.current), ExecuteEvents.selectHandler);
                    break;
                case "sophie":
                    buttonUI[0].SetActive(false);
                    buttonUI[1].SetActive(false);
                    buttonUI[2].SetActive(true);
                    buttonUI[3].SetActive(false);//So we aren't wasting processing time grabbing the same variable over and over again
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(firstspecialButton[2]);
                    // Manually trigger the OnSelect event for the first selected object
                    ExecuteEvents.Execute(firstspecialButton[2], new BaseEventData(EventSystem.current), ExecuteEvents.selectHandler);
                    break;
                case "nicol":
                    buttonUI[0].SetActive(false);
                    buttonUI[1].SetActive(false);
                    buttonUI[2].SetActive(false);
                    buttonUI[3].SetActive(true);//So we aren't wasting processing time grabbing the same variable over and over again
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(firstspecialButton[3]);
                    // Manually trigger the OnSelect event for the first selected object
                    ExecuteEvents.Execute(firstspecialButton[3], new BaseEventData(EventSystem.current), ExecuteEvents.selectHandler);
                    break;
            }
            for (counter = 0; counter < howMany; counter++)
            {
                specialNameText[counter].text = descriptions[counter];  // Set the title
                specialTXT_Desc[counter].text = descriptions[counter + 3];   // Set the description
            }
            if (counter < 3) // If we have still not displayed enough, that means that there are some skills that have not been unlocked. Make sure they are locked
            {
                //Might break, have no idea
                for (counter = howMany; counter < 3; counter++)
                {
                    specialNameText[counter].text = " ???";
                    specialTXT_Desc[counter].text = "Locked until level "+(counter)*5;
                }
            }
        }
        else
        {
            Debug.LogError("Character not found: " + WhoAreWeViewing);
        }

        // Manually trigger the OnSelect event for the first selected object
        //ExecuteEvents.Execute(firstspecialButton, new BaseEventData(EventSystem.current), ExecuteEvents.selectHandler);

    }

    private void OnDisable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(openSpecialMenu);
    }

}
