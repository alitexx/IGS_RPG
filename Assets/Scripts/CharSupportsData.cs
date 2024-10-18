using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSupportsData : MonoBehaviour
{
    // Initialize support data
    public int alankisa_support, alannico_support, alansoph_support, kisanico_support, kisasoph_support, nicosoph_support;

    // Method to increase support
    public void increaseSupport(string character1, string character2, int howMuch = 1)
    {
        // Get the current support value based on character combination
        int supportValue = GetSupportValue(character1, character2);

        // Isolate the last 4 bits (the support level) using a bitmask
        int supportLevel = supportValue & 0b1111; // Extract the last 4 bits

        // Increase support level
        supportLevel += howMuch;

        // Make sure support level doesn't exceed 15 (1111 in binary)
        supportLevel = Mathf.Clamp(supportLevel, 0, 15);

        // Combine the modified support level back with the first 3 bits (seen events)
        supportValue = (supportValue & 0b1110000) | supportLevel;

        // Update the support value for the correct character pair
        SetSupportValue(character1, character2, supportValue);
    }

    // Method to decrease support
    public void decreaseSupport(string character1, string character2, int howMuch = 1)
    {
        // Get the current support value based on character combination
        int supportValue = GetSupportValue(character1.ToLower(), character2.ToLower());

        // Isolate the last 4 bits (the support level) using a bitmask
        int supportLevel = supportValue & 0b1111; // Extract the last 4 bits

        // Decrease support level
        supportLevel -= howMuch;

        // Ensure support level doesn't go below 0
        supportLevel = Mathf.Clamp(supportLevel, 0, 15);

        // Combine the modified support level back with the first 3 bits (seen events)
        supportValue = (supportValue & 0b1110000) | supportLevel;

        // Update the support value for the correct character pair
        SetSupportValue(character1.ToLower(), character2.ToLower(), supportValue);
    }

    // Helper method to get the current support value for a given character pair
    private int GetSupportValue(string character1, string character2)
    {
        if ((character1 == "alan" && character2 == "kisa") || (character1 == "kisa" && character2 == "alan"))
            return alankisa_support;
        else if ((character1 == "alan" && character2 == "nicol") || (character1 == "nicol" && character2 == "alan"))
            return alannico_support;
        else if ((character1 == "alan" && character2 == "sophie") || (character1 == "sophie" && character2 == "alan"))
            return alansoph_support;
        else if ((character1 == "kisa" && character2 == "nicol") || (character1 == "nicol" && character2 == "kisa"))
            return kisanico_support;
        else if ((character1 == "kisa" && character2 == "sophie") || (character1 == "sophie" && character2 == "kisa"))
            return kisasoph_support;
        else if ((character1 == "nicol" && character2 == "sophie") || (character1 == "sophie" && character2 == "nicol"))
            return nicosoph_support;
        else
            return 0; // Default if no match found
    }

    // Helper method to set the support value for a given character pair
    private void SetSupportValue(string character1, string character2, int value)
    {
        if ((character1 == "alan" && character2 == "kisa") || (character1 == "kisa" && character2 == "alan"))
            alankisa_support = value;
        else if ((character1 == "alan" && character2 == "nicol") || (character1 == "nicol" && character2 == "alan"))
            alannico_support = value;
        else if ((character1 == "alan" && character2 == "sophie") || (character1 == "sophie" && character2 == "alan"))
            alansoph_support = value;
        else if ((character1 == "kisa" && character2 == "nicol") || (character1 == "nicol" && character2 == "kisa"))
            kisanico_support = value;
        else if ((character1 == "kisa" && character2 == "sophie") || (character1 == "sophie" && character2 == "kisa"))
            kisasoph_support = value;
        else if ((character1 == "nicol" && character2 == "sophie") || (character1 == "sophie" && character2 == "nicol"))
            nicosoph_support = value;
    }

    // Helper function to mark an event as seen
    public void seenEvent(string character1, string character2, int eventNumber)
    {
        // Ensure the event number is within a valid range (1 to 3)
        if (eventNumber < 1 || eventNumber > 3)
        {
            Debug.LogError("Invalid event number! Must be between 1 and 3.");
            return;
        }

        // Get the current support value for the character pair
        int supportValue = GetSupportValue(character1, character2);

        // Shift 1 by (eventNumber - 1) to set the correct bit (1st, 2nd, or 3rd)
        int eventBit = 1 << (3 - eventNumber); // (3 - eventNumber) because the first event is the leftmost bit

        // Mark the event as seen (set the corresponding bit to 1)
        supportValue |= eventBit; // OR the current value with the event bit to set it

        // Update the support value for the correct character pair
        SetSupportValue(character1.ToLower(), character2.ToLower(), supportValue);
    }
}
