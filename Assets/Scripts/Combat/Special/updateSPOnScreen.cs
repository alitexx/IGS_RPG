using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class updateSPOnScreen : MonoBehaviour
{
    [SerializeField] private Sprite nospecialicon, specialicon;
    [SerializeField] private Image[] imagesToUpdate;
    private int associatedCharacter;
    private int[] currentSpecials = new int[4];

    public void addSpecial(string who, int howManySpecials)
    {
        switch (who.ToLower())
        {
            case "alan":
                associatedCharacter = 0;
                break;
            case "kisa":
                associatedCharacter = 4;
                break;
            case "nicol":
                associatedCharacter = 8;
                break;
            case "sophie":
                associatedCharacter = 12;
                break;
        }

        if(howManySpecials > 0 && howManySpecials <= 4)
        {
            currentSpecials[associatedCharacter / 4]++;
            imagesToUpdate[associatedCharacter+ currentSpecials[associatedCharacter / 4]].sprite = specialicon;
            while (currentSpecials[associatedCharacter / 4] < howManySpecials)
            {
                imagesToUpdate[associatedCharacter + currentSpecials[associatedCharacter / 4]].sprite = specialicon;
                currentSpecials[associatedCharacter / 4]++;
            }
        }
    }
    public void removeSpecial(string who, int howManySpecials)
    {
        switch (who.ToLower())
        {
            case "alan":
                associatedCharacter = 0;
                break;
            case "kisa":
                associatedCharacter = 4;
                break;
            case "nicol":
                associatedCharacter = 8;
                break;
            case "sophie":
                associatedCharacter = 12;
                break;
        }

        if (howManySpecials > 0 && howManySpecials <= 4)
        {
            currentSpecials[associatedCharacter / 4]--;
            imagesToUpdate[associatedCharacter + currentSpecials[associatedCharacter / 4]].sprite = nospecialicon;
            while (currentSpecials[associatedCharacter / 4] > howManySpecials)
            {
                imagesToUpdate[associatedCharacter + currentSpecials[associatedCharacter / 4]].sprite = nospecialicon;
                currentSpecials[associatedCharacter / 4]--;
            }
        }
    }
}
