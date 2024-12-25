using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;
using UnityEngine.UI;

public class miguelConversation : MonoBehaviour
{
    [SerializeField] private CanvasGroup saveMenu;
    [SerializeField] private mainDialogueManager mainDialogueManager;

    //This is passed in
    public string savePointName;

    //for finding dead characters
    [SerializeField] private PlayerController playerControl;
    private int deadCharacters;
    private int obtainedCharacters;

    //Determines if this menu has been opened before
    //Add to save data
    private bool hasBeenGreeted;

    //saving previous save point
    float previousSave = -1;

    //For the button/event nonsense
    //miguel btn
    [SerializeField] private GameObject miguelBtn, saveButton, returnButton;

    /* New problem: What if they go back and try to talk to Miguel after obtaining a character? This is only an issue on few dialogues:
     * Before Kisa
     * Before Nicol
     * Before Sophie
     * These dialogues, if the character has been replaced, should be replaced with "stop trying to go back and keep moving" dialogue.
     * Check if the player has killed/obtained these characters and are trying to access this specific dialogue. if so, give them the ban hammer
     * 
     * 
     * update kns system because rn it probably doesnt print right
     * 
     * Horrible fix to kns system: keep names as kns for everything. if they dont have someone, just pretend that they do. idk.
     * 
     */

    //Uncomment out when we're ready to activate this. Could be annoying during testing

    private void OnEnable()
    {
        //If you have not been greeted, play the greeting. Depends on the floor we're on
        if (!hasBeenGreeted)
        {
            hasBeenGreeted = true;
            switch (playerControl.Level)
            {
                //A cutscene should play, but I haven't finished this part yet. 
                case 1:
                    mainDialogueManager.dialogueSTART("SaveConvo/firstSaveInteract_f1");
                    break;
                case 2:
                case 3:
                    mainDialogueManager.dialogueSTART("SaveConvo/firstSaveInteract_midFloor");
                    break;
                case 4:
                    mainDialogueManager.dialogueSTART("SaveConvo/firstSaveInteract_f4");
                    break;
            }
            PauseMenu.GamePaused = false;
            this.gameObject.SetActive(false);
        }
        Debug.Log(ExtractFloorNumber(savePointName));
        Debug.Log(previousSave);
        if (ExtractFloorNumber(savePointName) > previousSave)
        {
            miguelBtn.SetActive(true);
        }
    }


    public void startConversation()
    {
        EventSystem.current.SetSelectedGameObject(null);
        //used to determine if someone is dead
        deadCharacters = playerControl.getDeadCharacters();
        //used to determine if someone is unobtained
        obtainedCharacters = playerControl.getObtainedCharacters();
        //start dialogue based on save point name

        //should special dialogue play?
        if (GetSpecialDialogue(savePointName))
        {
            mainDialogueManager.dialogueSTART("SaveConvo/longtimenosee_"+SumBinaryDigits(deadCharacters).ToString());
        }
        else
        {
            //check if character has been obtained, and check if you are at a specific location where the dialogue should turn off.
            if (DoneBossFight(savePointName))
            {
                //They cannot see this dialogue, as they have already done the boss fight.
                mainDialogueManager.dialogueSTART("SaveConvo/goback");
            } else
            {
                //turn dead characters into kns naming system
                mainDialogueManager.dialogueSTART("SaveConvo/" + savePointName + binaryToKNS(deadCharacters));
            }
        }


        //turn off menu background, have it fade out
        saveMenu.DOFade(0, 1f).OnComplete(() =>
        {
            saveMenu.DOKill();
            //turn off pause UI
            this.gameObject.SetActive(false);
        });

        PauseMenu.GamePaused = false;
    }
    public void endConversation(bool shouldButtonBeActive = false)
    {
        this.gameObject.SetActive(true);
        miguelBtn.SetActive(shouldButtonBeActive);
        //sets button to nothing, waits 1 sec, then makes it the resume button
        assignButtonAfterSave();
        saveMenu.DOFade(1, 0.5f).OnComplete(() =>
        {
            saveMenu.DOKill();
        });

    }

    //I call this the kns naming system!!!!! x = they're dead, k = kisa alive, n = nicol alive, s = sophie alive!!
    private string binaryToKNS(int binary)
    {
        // Start with the preexisting string
        string returnedString = "_";

        // Convert the integer to a binary string
        string binaryString = Convert.ToString(binary, 2).PadLeft(3, '0'); // Ensure it's 3 bits long

        int floorNumber = playerControl.Level;
        if (floorNumber > binaryString.Length)
        {
            floorNumber = binaryString.Length;
        }

        // Loop through each character in the binary string
        for (int i = 0; i < floorNumber; i++)
        {
            if (binaryString[i] == '0')
            {
                // Add 'k', 'n', or 's' depending on the position of the 0
                switch (i)
                {
                    case 0:
                        returnedString += "k";
                        break;
                    case 1:
                        returnedString += "n";
                        break;
                    case 2:
                        returnedString += "s";
                        break;
                }
            }
            else if (binaryString[i] == '1')
            {
                // Add 'x' for 1
                returnedString += "x";
            }
        }

        return returnedString;
    }


    private int ExtractFloorNumber(string floorString)
    {
        // Define a mapping of save point names to numbers
        var savePointMapping = new Dictionary<string, int>
            {
                { "floor1start", 0 },
                { "floor1end", 1 },
                { "floor2start", 2 },
                { "floor2end", 3 },
                { "floor3start", 4 },
                { "floor3midpoint", 5 },
                { "floor3end", 6 },
                { "floor4start", 7 },
                { "floor4end", 8 }
            };

        // Check if the input string exists in the dictionary
        if (savePointMapping.TryGetValue(floorString, out int floorNumber))
        {
            return floorNumber;
        }

        // Return -1 if the save point name is not recognized
        return -1;
    }


    private bool GetSpecialDialogue(string lastFloor)
    {
        // Extract the numbers from the floor strings
        float lastNumber = ExtractFloorNumber(lastFloor);

        // Check if the current floor is 2 or more levels ahead of the last floor
        if (previousSave >= lastNumber + 2)
        {
            // Trigger special dialogue
            previousSave = lastNumber;
            return true;
        }
        previousSave = lastNumber;
        // Otherwise, return null
        return false;
    }

    private int SumBinaryDigits(int binary)
    {
        int sum = 0;

        // Convert the binary number to a string
        string binaryString = binary.ToString();

        // Iterate over each character in the string
        foreach (char digit in binaryString)
        {
            // Convert the character to an integer and add to the sum
            sum += int.Parse(digit.ToString());
        }

        return sum;
    }

    private bool DoneBossFight(string savePointName)
    {
        // Convert the binary integers to strings
        string binaryStr1 = deadCharacters.ToString();
        string binaryStr2 = obtainedCharacters.ToString();

        // Pad the binary strings with leading zeros to ensure they have the same length
        int maxLength = Mathf.Max(binaryStr1.Length, binaryStr2.Length);
        binaryStr1 = binaryStr1.PadLeft(maxLength, '0');
        binaryStr2 = binaryStr2.PadLeft(maxLength, '0');

        // Determine the position to check based on the save point
        int positionToCheck = -1;

        switch (savePointName)
        {
            case "floor1end":
            case "floor1start":
                positionToCheck = 0; // Check the first bit
                break;
            case "floor2end":
            case "floor2start":
                positionToCheck = 1; // Check the second bit
                break;
            case "floor3midpoint":
            case "floor3start":
                positionToCheck = 2; // Check the third bit
                break;
            default:
                return false; // Invalid save point name
        }

        // Check if the position is valid and both values have '1' in that position
        if (positionToCheck >= 0 && positionToCheck < binaryStr1.Length && positionToCheck < binaryStr2.Length)
        {
            return binaryStr1[positionToCheck] == '1' && binaryStr2[positionToCheck] == '1';
        }

        return false;
    }

    public void assignButtonAfterSave()
    {
        EventSystem.current.SetSelectedGameObject(null);
        StartCoroutine(SetReturnButtonAfterDelay(returnButton));
    }

    // The coroutine to wait for 1 second and set the return button
    private IEnumerator SetReturnButtonAfterDelay(GameObject button)
    {
        yield return new WaitForSeconds(1f); // Wait for 1 second
        if (button != null)
        {
            EventSystem.current.SetSelectedGameObject(button);
            StopAllCoroutines(); // Stop the coroutine if hovering ends
        }
    }
}
