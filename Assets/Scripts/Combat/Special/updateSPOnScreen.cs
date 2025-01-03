using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class updateSPOnScreen : MonoBehaviour
{
    [SerializeField] private GameObject spObject;
    [SerializeField] private Slider spSlider;
    [SerializeField] private Image spGlowSlider;
    [SerializeField] private TextMeshProUGUI spText;


    [SerializeField] private Sprite nospecialicon, specialicon;
    [SerializeField] private Image[] imagesToUpdate;
    [SerializeField] private audioManager am;
    private int associatedCharacter;
    private int[] currentSpecials = new int[4];

    public void addSpecial(string who, int howManySpecials)
    {
        switch (who.ToLower())
        {
            case "tank guy":
                associatedCharacter = 0;
                break;
            case "bard guy":
                associatedCharacter = 4;
                break;
            case "mage guy":
                associatedCharacter = 8;
                break;
            case "monk guy":
                associatedCharacter = 12;
                break;
        }

        if(howManySpecials > 0 && howManySpecials <= 4)
        {
            imagesToUpdate[associatedCharacter+ currentSpecials[associatedCharacter / 4]].sprite = specialicon;
            currentSpecials[associatedCharacter / 4]++;
            am.playSFX(40);
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
            case "tank guy":
                associatedCharacter = 0;
                break;
            case "bard guy":
                associatedCharacter = 4;
                break;
            case "mage guy":
                associatedCharacter = 8;
                break;
            case "monk guy":
                associatedCharacter = 12;
                break;
        }

        if (howManySpecials >= 0 && howManySpecials <= 4)
        {
            currentSpecials[associatedCharacter / 4]--;
            imagesToUpdate[associatedCharacter + currentSpecials[associatedCharacter / 4]].sprite = nospecialicon;
            while (currentSpecials[associatedCharacter / 4] > howManySpecials)
            {
                currentSpecials[associatedCharacter / 4]--;
                imagesToUpdate[associatedCharacter + currentSpecials[associatedCharacter / 4]].sprite = nospecialicon;
            }
        }
        else
        {
            Debug.Log("The special isnt the right number IDIOT. ITS " + howManySpecials);
        }
    }


    //slider nonsense
    public void setSliderVal(string who)
    {
        int localAssociatedCharacter = 0;
        switch (who.ToLower())
        {
            //if Alan, do nothing, as he is 0
            case "bard guy":
                localAssociatedCharacter = 1;
                break;
            case "mage guy":
                localAssociatedCharacter = 2;
                break;
            case "monk guy":
                localAssociatedCharacter = 3;
                break;
        }
        spSlider.value = (float)((float)currentSpecials[localAssociatedCharacter] / 4);
        spText.text = (currentSpecials[localAssociatedCharacter].ToString()) + "/4";
        Debug.Log(spText);
    }

    public void setSliderGlow(int skillCost)
    {
        spGlowSlider.fillAmount = (float)((float)skillCost / 4);
    }

    //Checks if the character using the special has enough special points to use the special
    public bool canUseSpecial(int who, int specialLevel)
    {
        //If their current special points are greater than the number of points they want to use, let them.
        if ((float)currentSpecials[who] >= specialLevel)
        {
            return true;
        } else // else, tell them NO. play the music here because i dont want to bring audioManager to useSpecial
        {
            am.playSFX(29);
            return false;
        }
    }
}
