using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class displaySupport : MonoBehaviour
{
    public CharSupportsData charSupportsData;

    [SerializeField] private Image[] charIcons, heartIcons;
    [SerializeField] private Sprite[] charSprites, heartSprites;
    [SerializeField] private Button[] supportBtn;

    [SerializeField] private PlayerController playerControl;
    private int deadCharacters;

    //ALAN = 0
    //KISA = 1
    //NICOL = 2
    //SOPHIE = 3

    public void updateSupportIcons(int character)
    {
        deadCharacters = playerControl.getDeadCharacters();
        switch (character) // passed in from character inspection script
        {
            case 0: // ALAN
                showCharSupport(0, 1, charSupportsData.alankisa_support);
                showCharSupport(1, 2, charSupportsData.alannico_support);
                showCharSupport(2, 3, charSupportsData.alansoph_support);

                break;
            case 1: //KISA
                showCharSupport(0, 0, charSupportsData.alankisa_support);
                showCharSupport(1, 2, charSupportsData.kisanico_support);
                showCharSupport(2, 3, charSupportsData.kisasoph_support);
                break;
            case 2: //NICOL
                showCharSupport(0, 0, charSupportsData.alannico_support);
                showCharSupport(1, 1, charSupportsData.kisanico_support);
                showCharSupport(2, 3, charSupportsData.nicosoph_support);
                break;
            case 3: //SOPHIE
                showCharSupport(0, 0, charSupportsData.alansoph_support);
                showCharSupport(1, 1, charSupportsData.nicosoph_support);
                showCharSupport(2, 2, charSupportsData.kisasoph_support);
                break;
        }
    }

    private int getSupportType(int character)
    {
        if (deadCharacters == 0)
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
        }

        // Convert the integer into a binary string representation.
        string binaryString = Convert.ToString(deadCharacters, 2).PadLeft(3, '0'); // Ensure the binary string is at least 3 digits long.

        // Loop through each character in the binary array.
        for (int i = 0; i < binaryString.Length; i++)
        {
            if (binaryString[i] == '1' && character == i) // If the character is dead
            {
                return heartSprites.Length - 1; // Return the broken heart sprite.
            }
        }

        // If neither condition is true, they are affected.
        return heartSprites.Length - 9; // Return default affected state.
    }



    private void showCharSupport(int position, int charName, int binarySupport)
    {
        charIcons[position].sprite = charSprites[charName + 4*CheckDeadCharacter(charName)];

        int supportType = getSupportType(charName);
        heartIcons[position].sprite = heartSprites[Convert.ToInt32(binarySupport)+supportType];

        //Check if there's a cutscene they should be able to watch
        if (checkIfSupportUnlocked(binarySupport))
        {
            supportBtn[position].gameObject.SetActive(true);
            //DO BUTTON BUSINESS HERE
        }
    }

    private bool checkIfSupportUnlocked(int supportLevel)
    {
        // Extract the last 4 bits for the player's support level
        int supportPoints = supportLevel & 0b1111; // Masking the last 4 bits
                                                   // Extract the first 3 bits to check if the player has seen the scenes
        int seenEvents = (supportLevel >> 4) & 0b111; // Shift right by 4 and mask the first 3 bits

        // Check for support level 3, 7, or 14 and corresponding event not seen
        if (supportPoints == 3 && (seenEvents & 0b001) == 0)
        {
            return true; // Event for support level 3 not seen yet
        }
        else if (supportPoints == 7 && (seenEvents & 0b010) == 0)
        {
            return true; // Event for support level 7 not seen yet
        }
        else if (supportPoints == 14 && (seenEvents & 0b100) == 0)
        {
            return true; // Event for support level 14 not seen yet
        }

        return false; // No event to unlock
    }


    int CheckDeadCharacter(int value)
    {
        // If the value is 0, return 0 since no action should be taken
        if (value == 0)
            return 0;

        // Adjust value to check the corresponding bit in deadCharacters
        int bitPosition = value - 1; // Since 1 corresponds to the 0th bit, 2 to the 1st, etc.

        // Use bitwise AND to check if the bit at the bitPosition is set
        int bitMask = 1 << bitPosition;

        // If the bit is set, return 1; otherwise, return 0
        return (deadCharacters & bitMask) != 0 ? 1 : 0;
    }

}
