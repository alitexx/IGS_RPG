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
    [SerializeField] private updateSPOnScreen updateSP;
    [SerializeField] private CharSupportsData charSupportsData;

    //For Trial and Error
    [SerializeField] private NicolFunTime nicolFunTime;

    //For alan whichLevel
    [SerializeField] private PlayerController playerController;

    //When special button is clicked
    //Currently, this does no targetting. Organize this code as you see fit!
    public void useSpecialBtn(int whichLevel)
    {

        WhoAreWeViewing = b_sm.WhoAreWeViewing;
        //Temporarily commented out to test specials
        
        if(WhoAreWeViewing == 0 && whichLevel == 3) // If they're trying to use Tenacity, re-calculate whichLevel
        {
            
            int tempwhichLevel = SumDigits(playerController.getDeadCharacters());
            switch (tempwhichLevel)
            {
                case 0: // No one dead
                    tempwhichLevel = 3; // This really doesn't change anything but leaving it blank could be confusing
                    break;
                case 1:
                case 2: //  1-2 people dead
                    tempwhichLevel = 2;
                    break;
                case 3: // everyone (minus Alan) dead
                    tempwhichLevel = 1;
                    break;
            }
            if (updateSP.canUseSpecial(WhoAreWeViewing, tempwhichLevel) == false)
            {
                Debug.Log("WOMP WOMP YOU DON'T HAVE ENOUGH SPECIAL POINTS (Tenacity)");
                return;
            }
            alanSpecial(whichLevel);
            updateSP.removeSpecial("tank guy", tempwhichLevel - 1);
            return;
        }
        





        /*if (updateSP.canUseSpecial(WhoAreWeViewing, whichLevel) == false)
        {
            Debug.Log("WOMP WOMP YOU DON'T HAVE ENOUGH SPECIAL POINTS");
            return;
        }*/

        WhoAreWeViewing = b_sm.WhoAreWeViewing;
        switch(WhoAreWeViewing)
        {
            case 0: //ALAN
                alanSpecial(whichLevel);
                updateSP.removeSpecial("tank guy", whichLevel-1);
                break;
            case 1: //KISA
                kisaSpecial(whichLevel);
                updateSP.removeSpecial("bard guy", whichLevel-1);
                break;
            case 2: //NICOL
                nicolSpecial(whichLevel);
                updateSP.removeSpecial("mage guy", whichLevel-1);
                break;
            case 3:// SOPHIE
                sophieSpecial(whichLevel);
                updateSP.removeSpecial("monk guy", whichLevel-1);
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
            case 0:
                syncStrike("alan");
                break;
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
            case 0:
                syncStrike("kisa");
                break;
            case 1:
                //use Kisa's first special, Sing
                battControl.KisaSing();

                StartCoroutine(battControl.WaitBeforeChoosingNext(1.5f));

                battControl.backButton.SetActive(false);
                break;
            case 2:
                //use Kisa's second special, Distract
                battControl.backButton.SetActive(true);
                StartCoroutine(battControl.ConfusedTargeting());
                break;
            case 3:
                //use Kisa's third special, Revive. If we can't revive, change this to Performance.
                battControl.KisaPerform();

                StartCoroutine(battControl.WaitBeforeChoosingNext(1.5f));

                battControl.backButton.SetActive(false);
                break;
        }
        b_sm.gameObject.SetActive(false);
    }

    public void nicolSpecial(int whichLevel)
    {
        switch (whichLevel)
        {
            case 0:
                syncStrike("nicol");
                break;
            case 1:
                //use Nicol's first special, Mockery
                battControl.NicolMockery();

                battControl.backButton.SetActive(false);

                StartCoroutine(battControl.WaitBeforeChoosingNext(1f));
                break;
            case 2:
                //use Nicol's second special, Trial and Error

                //How good/bad the effect is
                int roll = Random.Range(0, 20); // 0-19
                //What the effect is
                int genre = Random.Range(0, 4); // 0-3

                //This is the text box aspect of trial and error. Look in the script for more info!
                //Currently does not process any of the actual effects, just tells players what they are.
                nicolFunTime.manageTextBubbles(genre, roll);

                break;
            case 3:
                //use Nicol's third special, Encourage
                Debug.Log("beans");
                battControl.NicolMotivate();

                battControl.backButton.SetActive(false);

                StartCoroutine(battControl.WaitBeforeChoosingNext(1.5f));

                break;
        }

        b_sm.gameObject.SetActive(false);
    }

    public void sophieSpecial(int whichLevel)
    {
        switch (whichLevel)
        {
            case 0:
                syncStrike("sophie");
                break;
            case 1:
                //use Sophie's first special, Earthquake
                //Can we make the screen shake here? I don't have an animation for this, thinking more rock tomb in pokemon for the effects

                break;
            case 2:
                //use Sophie's second special, Focus
                break;
            case 3:
                //use Sophie's third special, Thunderstorm
                break;
        }

        battControl.backButton.SetActive(false);
        b_sm.gameObject.SetActive(false);
    }

    private void syncStrike(string whoIsInitiating)
    {
        //Do similar targetting to Nicol's final special, where you have to select an ally and that is the ally you attack with
        //Have both allies attack
        //      - specifically, whoIsInitiating's animation plays first, then 0.75 seconds later the other ally's attack goes. these are physical attacks
        //      - use this function to find the damage multiplier:

        //      charSupportsData.getSyncStrikeMultiplier(whoIsInitiating, otherAlly);

        //      - ^ this returns a value to be multiplied to damage. it always increases damage dealt by 4 (or another set value), and that value is multipied by the number returned from this function
        // REMEMBER TO PLAY HEART ANIMATIONS ABOVE BOTH ALLIES WHO USE THIS SPECIAL!
        // REMEMBER TO RUN THIS FUNCTION TO ADD TO THEIR FRIENDSHIP BONUS/SUPPORT LEVEL/WHATEVER YOU WANT TO CALL IT

        //      charSupportData.increaseSupport((whoIsInitiating, otherAlly);
    }

    //This is for one function and thats it
    // Function to calculate the sum of digits of a three-digit number
    public int SumDigits(int number)
    {
        // Ensure the number is positive
        number = Mathf.Abs(number);

        // Extract each digit and calculate the sum
        int hundreds = number / 100;        // Get the hundreds place
        int tens = (number / 10) % 10;     // Get the tens place
        int ones = number % 10;            // Get the ones place

        return hundreds + tens + ones;
    }
}
