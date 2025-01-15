using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class CharSupportsData : MonoBehaviour
{
    // Initialize support data
    public int alankisa_support, alannico_support, alansoph_support, kisanico_support, kisasoph_support, nicosoph_support;

    // Method to increase support
    public void increaseSupport(string character1, string character2, int howMuch = 1)
    {
        character1 = character1.ToLower();
        character2 = character2.ToLower();

        // Get the current support value based on character combination
        int supportValue = GetSupportValue(character1.Substring(0, 4).ToLower(), character2.Substring(0, 4).ToLower());

        // Isolate the last 4 bits (the support level) using a bitmask
        int supportLevel = supportValue & 0b1111; // Extract the last 4 bits

        //check if this number is 3, 8, or 16.
        switch (supportLevel)
        {
            case 3:
            case 8:
            case 14:
                return;
        }
        // Increase support level
        supportLevel += howMuch;

        // Make sure support level doesn't exceed 15 (1111 in binary)
        supportLevel = Mathf.Clamp(supportLevel, 0, 15);

        // Combine the modified support level back with the first 3 bits (seen events)
        supportValue = (supportValue & 0b1110000) | supportLevel;

        // Update the support value for the correct character pair
        SetSupportValue(character1.Substring(0, 4).ToLower(), character2.Substring(0, 4).ToLower(), supportValue);
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
        else if ((character1 == "alan" && character2 == "nico") || (character1 == "nico" && character2 == "alan"))
            return alannico_support;
        else if ((character1 == "alan" && character2 == "soph") || (character1 == "soph" && character2 == "alan"))
            return alansoph_support;
        else if ((character1 == "kisa" && character2 == "nico") || (character1 == "nico" && character2 == "kisa"))
            return kisanico_support;
        else if ((character1 == "kisa" && character2 == "soph") || (character1 == "soph" && character2 == "kisa"))
            return kisasoph_support;
        else if ((character1 == "nico" && character2 == "soph") || (character1 == "soph" && character2 == "nico"))
            return nicosoph_support;
        else
        {
            Debug.Log("No pair with that name found");
            return 0; // Default if no match found
        }
    }

    // Helper method to set the support value for a given character pair
    private void SetSupportValue(string character1, string character2, int value)
    {
             if ((character1 == "alan" && character2 == "kisa") || (character1 == "kisa" && character2 == "alan"))
            alankisa_support = value;
        else if ((character1 == "alan" && character2 == "nico") || (character1 == "nico" && character2 == "alan"))
            alannico_support = value;
        else if ((character1 == "alan" && character2 == "soph") || (character1 == "soph" && character2 == "alan"))
            alansoph_support = value;
        else if ((character1 == "kisa" && character2 == "nico") || (character1 == "nico" && character2 == "kisa"))
            kisanico_support = value;
        else if ((character1 == "kisa" && character2 == "soph") || (character1 == "soph" && character2 == "kisa"))
            kisasoph_support = value;
        else if ((character1 == "nico" && character2 == "soph") || (character1 == "soph" && character2 == "nico"))
            nicosoph_support = value;
    }

    public void seenEvent(string character1, string character2, int eventNumber)
    {
        // Ensure the event number is within a valid range (1 to 3)
        if (eventNumber < 1 || eventNumber > 4)
        {
            Debug.LogError("Invalid event number! Must be between 1 and 4.");
            return;
        } else if (eventNumber == 4)
        {
            SetSupportValue(character1.ToLower(), character2.ToLower(), 0101110);
        }

        // Get the current support value for the character pair
        int supportValue = GetSupportValue(character1.ToLower(), character2.ToLower());

        // Calculate the event bit to modify (1st, 2nd, or 3rd event)
        int eventBit = 1 << (3 - eventNumber); // e.g., for event 1: bit at position 3, event 2: bit at position 2, etc.

        // Mark the event as seen (set the corresponding bit to 1 in the first 3 bits)
        supportValue |= eventBit; // Use bitwise OR to set the specific event bit to 1

        // Extract the last 4 bits (special points) and increment them by 1
        int specialPoints = supportValue & 0b1111; // Mask out only the last 4 bits
        specialPoints = Mathf.Min(specialPoints + 1, 15); // Increment, ensuring it doesn't exceed 15

        // Combine the updated first 3 bits and the new special points back into a single int
        supportValue = (supportValue & ~0b1111) | specialPoints; // Clear the last 4 bits, then OR with updated points

        // Update the support value for the character pair
        SetSupportValue(character1.ToLower(), character2.ToLower(), supportValue);
    }

    //What if you already have hearts and a character becomes affected? fun.
    //I have no idea if this works pretend that it does
    public void becomeAffected()
    {
        // Get all fields of the current class
        FieldInfo[] fields = this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

        foreach (FieldInfo field in fields)
        {
            // Check if the field is one of the integer values we care about
            if (field.FieldType == typeof(int) && field.Name.EndsWith("_support"))
            {
                // Get the current value of the field
                int value = (int)field.GetValue(this);

                // If the integer is 0, skip processing
                if (value == 0)
                {
                    return;
                }

                // Extract the seen events
                int firstThreeBits = (value >> 4) & 0b00000111; // Shift right to isolate the three bits
                if(firstThreeBits == 000)
                {
                    return;
                }

                // Determine the new value based on the first three bits
                int newValue = 0;
                switch (firstThreeBits)
                {
                    case 1: // Only the first bit is 1
                        newValue = 0b00000001; // Binary for 1
                        break;
                    case 2:
                        //This is if they are already affected. Just skip processing for them
                        return;
                    case 3: // First and second bits are 1 (binary 11)
                        newValue = 0b00000010; // Binary for 2
                        break;
                    case 7: // All three bits are 1 (binary 111)
                        newValue = 0b00000011; // Binary for 3
                        break;
                    default:
                        // No change for other patterns
                        break;
                }

                // Update the field with the new value
                field.SetValue(this, newValue);
            }
        }
    }
    //this returns a multiplier to damage
    //BRANDON!!! CALL THIS FUNCTION!!!!!!11 THIS ONE!!!!!!
    public int getSyncStrikeMultiplier(string character1, string character2)
    {
        int supportValue = GetSupportValue(character1.ToLower(), character2.ToLower());

        // Extract the first 3 bits by shifting right 4 times
        int eventBits = supportValue >> 4;

        // Mask out any extra bits beyond the first 3
        eventBits &= 0b111; // Keep only the last 3 bits (00000111)

        // Count the number of 1s in the 3 bits
        int seenEventCount = 0;
        for (int i = 0; i < 3; i++)
        {
            if ((eventBits & (1 << i)) != 0) // Check if each bit is 1
            {
                seenEventCount++;
            }
        }

        return seenEventCount+1; // Must always be above 1
    }
}
