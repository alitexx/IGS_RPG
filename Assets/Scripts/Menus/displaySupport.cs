using System;
using UnityEngine;
using UnityEngine.UI;

public class displaySupport : MonoBehaviour
{

    [SerializeField] private Button[] bottomButtons;
    public Button[] previousUpKeys = new Button[3];


    public CharSupportsData charSupportsData;

    [SerializeField] private mainDialogueManager mainDialogueManager;

    [SerializeField] private Image[] charIcons, heartIcons;
    [SerializeField] private Sprite[] charSprites, heartSprites;
    [SerializeField] private Button[] supportBtn;
    [SerializeField] private Button[] partyMemberButtons;
    [SerializeField] private Button specialButton;
    [SerializeField] private Button specialButton2;

    [SerializeField] private PlayerController playerControl;
    private int deadCharacters;
    private int unobtainedCharacters;
    private Button previousRight;

    private string[] supportNames = new string[3];
    private int[] localSupportData = new int[3];

    //ALAN = 0
    //KISA = 1
    //NICOL = 2
    //SOPHIE = 3

    public void updateSupportIcons(int character)
    {
        //used to determine broken hearts
        deadCharacters = playerControl.getDeadCharacters();
        //used to determine character icons in the pause menu
        unobtainedCharacters = playerControl.getUnobtainedCharacters();
        switch (character) // passed in from character inspection script
        {
            case 0: // ALAN
                showCharSupport(0, 1, charSupportsData.alankisa_support, "alankisa");
                showCharSupport(1, 2, charSupportsData.alannico_support, "alannico");
                showCharSupport(2, 3, charSupportsData.alansoph_support, "alansoph");
                break;
            case 1: //KISA
                showCharSupport(0, 0, charSupportsData.alankisa_support, "alankisa");
                showCharSupport(1, 2, charSupportsData.kisanico_support, "kisanico");
                showCharSupport(2, 3, charSupportsData.kisasoph_support, "kisasoph");
                break;
            case 2: //NICOL
                showCharSupport(0, 0, charSupportsData.alannico_support, "alannico");
                showCharSupport(1, 1, charSupportsData.kisanico_support, "kisanico");
                showCharSupport(2, 3, charSupportsData.nicosoph_support, "nicosoph");
                break;
            case 3: //SOPHIE
                showCharSupport(0, 0, charSupportsData.alansoph_support, "alansoph");
                showCharSupport(1, 1, charSupportsData.nicosoph_support, "nicosoph");
                showCharSupport(2, 2, charSupportsData.kisasoph_support, "kisasoph");
                break;
        }

        int supportNum = isSupportActive();
        if (supportNum != -1)
        {
            //There is a support button on the screen. Find this button and set that as the character's right button and vice versa
            updateButtonRight(partyMemberButtons[character], supportBtn[supportNum]);
            updateButton(supportBtn[supportNum], partyMemberButtons[character]);
        }
        else
        {
            //There is not a support up. set right to the special button
            updateButtonRight(partyMemberButtons[character], specialButton);
            updateButton(specialButton, partyMemberButtons[character]);
            updateButton(specialButton2, partyMemberButtons[character]);
        }

        // Update pause, options, etc.

        changeUpKeyOnButton(bottomButtons[0], partyMemberButtons[character]);
        changeUpKeyOnButton(bottomButtons[1], partyMemberButtons[character]);
        changeUpKeyOnButton(bottomButtons[2], partyMemberButtons[character]);

    }

    public void closeMenu()
    {
        if (previousUpKeys[0] == null)// if they have not been assigned, assign them now
        {
            previousUpKeys[0] = bottomButtons[0].navigation.selectOnUp as Button;
            previousUpKeys[1] = bottomButtons[1].navigation.selectOnUp as Button;
            previousUpKeys[2] = bottomButtons[2].navigation.selectOnUp as Button;
        } else
        {
            changeUpKeyOnButton(bottomButtons[0], previousUpKeys[0]);
            changeUpKeyOnButton(bottomButtons[1], previousUpKeys[1]);
            changeUpKeyOnButton(bottomButtons[2], previousUpKeys[2]);
        }
    }

    private int isSupportActive()
    {
        // Loop through the supportBtn array to check if any other button is active
        for (int i = 0; i < supportBtn.Length; i++)
        {
            if (supportBtn[i].gameObject.activeSelf) // Ignore the current position
            {
                updateButton(specialButton, supportBtn[i]);
                return i;
            }
        }
        return -1;
    }

    private int getSupportType(int character)
    {
        if (deadCharacters == 0) // Only runs if no one is dead
        {
            // Check the first two bits of deadCharacters to see which events they have watched.
            int firstBit = (deadCharacters & 0b001); // Extract the first bit.
            int secondBit = (deadCharacters & 0b010); // Extract the second bit.

            if (secondBit != 0)
            {
                return 2; // If the second bit is set, return 2 (means they've seen both the first and second events).
            }
            else if (firstBit != 0)
            {
                return 1; // If only the first bit is set, return 1 (means they've seen the first event).
            }
            return 0; // No events watched.
        } else if (deadCharacters == 111) //This is if everyone's dead, put in as a failsafe for a nicol issue I was having
        {
            return heartSprites.Length - 1; // Return the broken heart sprite.
        }

        // Convert the integer into a binary string representation.
        string binaryString = Convert.ToString(deadCharacters, 2).PadLeft(3, '0'); // Ensure the binary string is at least 3 digits long.

        // Loop through each character in the binary array.
        for (int i = 0; i < binaryString.Length; i++)
        {
            if (binaryString[i] == '1' && character == i) // If the character is dead
            {
                Debug.Log(binaryString[i] + " is dead.");
                return heartSprites.Length - 1; // Return the broken heart sprite.
            }
        }

        // If neither condition is true, they are affected.
        Debug.Log("The character is not dead. They are affected.");
        return heartSprites.Length - 9; // Return default affected state.
    }



    private void showCharSupport(int position, int charName, int binarySupport, string supportName)
    {
        charIcons[position].sprite = charSprites[charName + 4 * CheckUnobtainedCharacter(charName)];

        int supportType = getSupportType(charName);

        //Debug.Log(binarySupport);
        
        if(binarySupport == 0101110) //If they have only seen the second event and they're on supportType14, this is an affected character. send them back to 7, don't let them view another event.
        {
            heartIcons[position].sprite = heartSprites[Convert.ToInt32(binarySupport & 0b1111) + 7];
            supportBtn[position].gameObject.SetActive(false);
        }
        else
        {
            heartIcons[position].sprite = heartSprites[Convert.ToInt32(binarySupport & 0b1111) + supportType];
            //Check if there's a cutscene they should be able to watch
            if (checkIfSupportUnlocked(binarySupport))
            {
                supportBtn[position].gameObject.SetActive(true);
                localSupportData[position] = binarySupport & 0b1111;
                //DO BUTTON BUSINESS HERE
            }
            else
            {
                supportBtn[position].gameObject.SetActive(false);
            }
        }
        supportNames[position] = supportName;
    }

    private bool checkIfSupportUnlocked(int supportLevel)
    {
        // Extract the last 4 bits for the player's support level
        int supportPoints = supportLevel & 0b1111; // Masking the last 4 bits
                                                   // Extract the first 3 bits to check if the player has seen the scenes
        int seenEvents = (supportLevel >> 4) & 0b111; // Shift right by 4 and mask the first 3 bits

        if(deadCharacters != 0) // It doesn't matter who's dead, but it only checks for supports at lvl 7 here.
        {
            if (supportPoints == 7 && (seenEvents & 0b010) == 0)
            {
                return true; // Event for support level 7 not seen yet
            } else
            {
                return false;
            }
        }
        // Check for support level 3, 7, or 14 and corresponding event not seen
        if (supportPoints == 3 && playerControl.Level >= 1 && (seenEvents & 0b001) == 0)
        {
            return true; // Event for support level 3 not seen yet
        }
        else if (supportPoints == 8 && playerControl.Level >= 2 && (seenEvents & 0b010) == 0)
        {
            return true; // Event for support level 7 not seen yet
        }
        else if (supportPoints == 14 && playerControl.Level >= 3 && playerControl.hasSophie) // 14 heart requires all party members to be living // Removed needing to see support events, but if I need to add it back:  && (seenEvents & 0b100) == 0
        {
            return true; // Event for support level 14 not seen yet
        }
        Debug.Log("Seen Events: " + (seenEvents & 0b100));

        return false; // No event to unlock
    }
    int CheckUnobtainedCharacter(int value)
    {
        // If the value is 0, return 0 since no action should be taken
        if (value == 0)
            return 0;

        // Reverse the bit position: value 1 checks the most significant bit in a 3-bit number.
        int bitPosition = 3 - value; // 1 -> 2nd bit, 2 -> 1st bit, 3 -> 0th bit

        // Use bitwise AND to check if the bit at the bitPosition is set
        int bitMask = 1 << bitPosition;

        // If the bit is set, return 1; otherwise, return 0
        return (unobtainedCharacters & bitMask) != 0 ? 1 : 0;
    }

    public void runSupportDialogue(int supportIcon)
    {
        supportBtn[supportIcon].gameObject.SetActive(false);
        //run support. Pass in support name + local support 
        Debug.Log(supportNames[supportIcon] + localSupportData[supportIcon].ToString());
        
        //What if someone is dead?
        if (deadCharacters == 0)// if someone is not dead
        {
            //for example, if this was alan and kisa's first support, the file name would be alankisa3
            mainDialogueManager.dialogueSTART("Supports/" + supportNames[supportIcon] + localSupportData[supportIcon].ToString());
        }
        else
        {
            //Kisa dead = 100
            //Nicol dead = 010
            //Sophie dead = 001
            //These can be mixed and matched
            //THIS NEEDS PADDING!! EDIT!!!! HELLO!!! HELLLOOOOOOOOO!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!1 HEY YOU!!!!!!!!!!!1 ADD PADDING TO THE DEAD CHARACTERS!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            mainDialogueManager.dialogueSTART("Supports/DeadAllyConvo/" + supportNames[supportIcon] + deadCharacters.ToString("D3"));
        }
    }

    //private void showCharSupport(int position, int charName, int binarySupport)
    //{
    //    charIcons[position].sprite = charSprites[charName + 4 * CheckUnobtainedCharacter(charName)];

    //    int supportType = getSupportType(charName);
    //    heartIcons[position].sprite = heartSprites[Convert.ToInt32(binarySupport) + supportType];

    //    //Check if there's a cutscene they should be able to watch
    //    if (checkIfSupportUnlocked(binarySupport))
    //    {
    //        supportBtn[position].gameObject.SetActive(true);

    //        //button fun time
    //        Debug.Log("i should be updating the button but i am not");
    //        updateButton(supportBtn[position], partyMemberButtons[charName]);
    //    }
    //}


    private void updateButton(Button buttonUpdated, Button left)
    {
        
        if (buttonUpdated != null)
        {
            // Get the current navigation settings of the target button
            Navigation navigation = buttonUpdated.navigation;

            // Update only the 'selectOnLeft' value, keeping the others as they are
            if (left != null)
            {
                navigation.selectOnLeft = left;
            }
            // Apply the modified navigation back to the button
            buttonUpdated.navigation = navigation;
        }
    }

    private void updateButtonRight(Button buttonUpdated, Button left = null)
    {
        if (buttonUpdated != null)
        {
            // Get the current navigation settings of the target button
            Navigation navigation = buttonUpdated.navigation;
            previousRight = navigation.selectOnRight as Button;
            // Update only the 'selectOnLeft' value, keeping the others as they are
            if (left != null)
            {
                navigation.selectOnRight = left;
            } else
            {
                navigation.selectOnRight = previousRight;
            }

            // Apply the modified navigation back to the button
            buttonUpdated.navigation = navigation;
        }
    }


    private void changeUpKeyOnButton(Button buttonUpdated, Button newButton = null)
    {
        if (buttonUpdated != null)
        {
            // Get the current navigation settings of the target button
            Navigation navigation = buttonUpdated.navigation;
            
            // Update only the 'selectOnLeft' value, keeping the others as they are
            if (newButton != null)
            {
                navigation.selectOnUp = newButton;
                // Apply the modified navigation back to the button
                buttonUpdated.navigation = navigation;
                return;
            }
        }
    }
}
