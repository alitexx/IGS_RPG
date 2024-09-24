using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class specialPauseMenu : MonoBehaviour
{


    private Dictionary<string, string[]> specialDescriptions;

    private void Awake()
    {
        specialDescriptions = new Dictionary<string, string[]> {
        { "alan", alanSpecialDescriptions },
        { "kisa", kisaSpecialDescriptions },
        { "nicol", nicolSpecialDescriptions },
        { "sophie", sophieSpecialDescriptions }
    };
    }

    //different arrays of text for different character's specials
    [SerializeField] private string[] alanSpecialDescriptions;
    [SerializeField] private string[] kisaSpecialDescriptions;
    [SerializeField] private string[] nicolSpecialDescriptions;
    [SerializeField] private string[] sophieSpecialDescriptions;

    //array of all images for all specials
    [SerializeField] private Sprite[] specialSprites;
    //text we should be changing
    [SerializeField] private TextMeshProUGUI[] specialTXT_Title;
    [SerializeField] private TextMeshProUGUI[] specialTXT_Desc;
    //images we should be updating
    [SerializeField] private Image[] specialIcons;

    //Locked images
    [SerializeField] private GameObject[] LockedImages;

    //SpecialCharges
    [SerializeField] private GameObject[] SpecialCharges;

    //Stuff for checking party level, who we're viewing, etc.
    private int level;
    //This is sent to us via the pause_characterInspection script
    public string WhoAreWeViewing;

    private void OnEnable()
    {
        //So we aren't wasting processing time grabbing the same variable over and over again
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
            for (counter = 0; counter < howMany; counter++)
            {
                specialTXT_Title[counter].text = descriptions[counter];  // Set the title
                specialTXT_Desc[counter].text = descriptions[counter + 3];   // Set the description
                specialIcons[counter].sprite = specialSprites[counter + int.Parse(descriptions[6])];  // Set sprite
                specialIcons[counter].color = new Color(255, 255, 255, 255);
                LockedImages[counter].SetActive(false);
                SpecialCharges[counter].SetActive(true);
            }
            if (counter < 3) // If we have still not displayed enough, that means that there are some skills that have not been unlocked. Make sure they are locked
            {
                //Might break, have no idea
                for (counter = howMany; counter < 3; counter++)
                {
                    specialTXT_Title[counter].text = "LOCKED";
                    specialTXT_Desc[counter].text = "";

                    //do something with this sprite
                    specialIcons[counter].color = new Color(255, 255, 255, 0);
                    LockedImages[counter].SetActive(true);
                    SpecialCharges[counter].SetActive(false);
                }
            }
        }
        else
        {
            Debug.LogError("Character not found: " + WhoAreWeViewing);
        }
    }

}
