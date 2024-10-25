using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class useSpecial : MonoBehaviour
{
    //Get who we are viewing
    public int WhoAreWeViewing;
    public battle_specialMenu b_sm;

    public BattleController battControl;
    private BattleCharacter currentChar;

    //When special button is clicked
    //Currently, this does no targetting. Organize this code as you see fit!
    public void useSpecialBtn(int whichLevel)
    {
        WhoAreWeViewing = b_sm.WhoAreWeViewing;
        switch(WhoAreWeViewing)
        {
            case 0: //ALAN
                alanSpecial(whichLevel);
                break;
            case 1: //KISA
                kisaSpecial(whichLevel);
                break;
            case 2: //NICOL
                nicolSpecial(whichLevel);
                break;
            case 3:// SOPHIE
                sophieSpecial(whichLevel);
                break;
            default:
                Debug.LogError("Cannot find character with ID: " + WhoAreWeViewing);
                return;
        }
    }

    public void alanSpecial(int whichLevel)
    {
        switch(whichLevel)
        {
            case 1:
                //use Alan's first special, Guard
                battControl.AlanGuardStatIncrease();

                StartCoroutine(battControl.WaitBeforeChoosingNext(1.5f));
                break;
            case 2:
                //use Alan's second special, Taunt

                battControl.AlanTaunt();

                StartCoroutine(battControl.WaitBeforeChoosingNext(1f));

                break;
            case 3:
                //use Alan's third special, Tenacity

                battControl.AlanTenacity();

                StartCoroutine(battControl.WaitBeforeChoosingNext(2f));
                break;
        }

        battControl.backButton.SetActive(false);
        b_sm.gameObject.SetActive(false);
    }

    public void kisaSpecial(int whichLevel)
    {
        switch (whichLevel)
        {
            case 1:
                //use Kisa's first special, Sing
                break;
            case 2:
                //use Kisa's second special, Distract
                break;
            case 3:
                //use Kisa's third special, Revive. If we can't revive, change this to Performance.
                break;
        }
    }

    public void nicolSpecial(int whichLevel)
    {
        switch (whichLevel)
        {
            case 1:
                //use Nicol's first special, Preeminence
                break;
            case 2:
                //use Nicol's second special, Enchant
                break;
            case 3:
                //use Nicol's third special, Encourage
                break;
        }
    }

    public void sophieSpecial(int whichLevel)
    {
        switch (whichLevel)
        {
            case 1:
                //use Sophie's first special, Earthquake
                break;
            case 2:
                //use Sophie's second special, Focus
                break;
            case 3:
                //use Sophie's third special, Starstorm
                break;
        }
    }
}
