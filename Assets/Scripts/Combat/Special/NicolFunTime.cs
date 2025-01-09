using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class NicolFunTime : MonoBehaviour
{
    private string text1; // First text shown
    private string text2; // Second text shown

    //side A is shown first, then side B
    [SerializeField] private RectTransform sidea, sideb;
    [SerializeField] private TextMeshProUGUI sideaText, sideBtext;

    [SerializeField] private RectTransform[] locations;
    // NICOL FUN TIME

    //This is for his trial and error special, i just wanted to make a new script to managing the effects

    //roll is the number rolled, i.e. how good or bad this is going to be for the party. rolls should go as follows:
    // - 1: STRONG NEGATIVE.
    // - 2-6: weak negative
    // - 7-11: nothing happens
    // - 12-18: weak positive
    // - 19-20: STRONG POSITIVE

    //genre is what is Nicol trying to do? there's a few different genres:
    //FIREBALL (code: 0) : Nicol tries to nuke the enemies with a fireball.
    //  - on a strong positive, all enemies are set to 1 HP.
    //  - on a weak positive, all enemies take minor damage (probably about 25% of their max hp)
    //  - on a neutral, nothing happens
    //  - on a weak negative, all allies take minor damage (i would say 25% of their max like on a weak positive, but that's mean. make it 5-10 hp each)
    //  - on a strong negative, all allies are set to 1 HP. womp womp

    //HEALING (code: 1) : Nicol tries to heal his allies.
    //  - on a strong positive, all allies are set to max hp
    //  - on a weak positive, all allies are healed by 50%
    //  - on a neutral, nothing happens
    //  - on a weak negative, all monsters gain 25% HP
    //  - on a strong negative, all non-defeated monsters are restored to full HP

    // MP RESTORATION (code: 2) : Nicol tries to restore everyone's MP.
    //  - on a strong positive, all allies are set to max MP
    //  - on a weak positive, all allies gain 50% MP.
    //  - on a neutral, nothing happens
    //  - on a weak negative, all allies lose 2 MP, if applicable
    //  - on a strong negative, all allies lose all MP.

    // RALLYING THE PARTY (code: 3) : Nicol tries to bolster the party and gain more SP
    //  - on a strong all allies are set to max SP
    //  - on a weak positive, all allies gain 1 SP
    //  - on a neutral, nothing happens
    //  - on a weak negative, all allies lose 1 SP.
    //  - on a strong negative, all allies lose all SP.

    // NOTE: NONE OF THESE CALCULATIONS ARE DONE HERE. THIS ONLY DISPLAYS THE EFFECTS/TEXT

    public void manageTextBubbles(int genre, int roll)
    {
        // Find the correct text that should be displayed
        text1 = findText1(genre, roll);
        sideaText.text = text1;
        sideBtext.text = text2;

        sidea.gameObject.SetActive(true);
        sideb.gameObject.SetActive(true);

        // Create a sequence for smoother control
        Sequence sequence = DOTween.Sequence();

        // YOU'RE TELLING ME I COULD HAVE BEEN DOING IT LIKE THIS THE **WHOLE TIME**?!?!??!?!?!??!?!? BRO
        sequence.Append(sidea.DOMove(locations[1].position, 0.5f))
                .AppendInterval(1f) // Wait 1 second
                .Append(sideb.DOMove(locations[4].position, 0.5f))
                .AppendInterval(1f) // Wait 1 second
                .Append(sidea.DOMove(locations[2].position, 0.5f))
                .Join(sideb.DOMove(locations[5].position, 0.5f)) // Move both simultaneously
                .OnComplete(() => {
                    //reset positions and turn off game objects
                    sidea.gameObject.SetActive(false);
                    sideb.gameObject.SetActive(false);
                    sidea.position = locations[0].position;
                    sideb.position = locations[3].position;
                });
    }


    private string findText1(int genre, int roll)
    {
        switch (genre)
        {
            case 0: // Fireball
                //Alr now how bad is it
                text2 = findText2(roll,
                    "...the party barely survives.",
                    "...and it goes awry.",
                    "It fizzles out before it can form.",
                    "...and it lands!",
                    "...the flames engulf the monsters!"
                    );

                return "Nicol tries a fireball...";
            case 1: // HP
                //Alr now how bad is it
                text2 = findText2(roll,
                    "...and gets REALLY confused.",
                    "...and gets confused.",
                    "...but cannot concentrate.",
                    "...and somewhat succeeds.",
                    "...and succeeds!"
                    );
                return "Nicol tries to heal his allies...";
            case 2: // MP
                //Alr now how bad is it
                text2 = findText2(roll,
                    "...and gets REALLY confused.",
                    "...and gets confused.",
                    "...but cannot concentrate.",
                    "...and somewhat succeeds.",
                    "...and succeeds!"
                    );
                return "Nicol tries to restore his allies...";
            case 3: // SP
                //Alr now how bad is it
                text2 = findText2(roll,
                    "...but makes matters FAR worse.",
                    "...but makes matters worse.",
                    "...but they aren't listening.",
                    "...and somewhat succeeds.",
                    "...and succeeds!"
                    );
                return "Nicol tries to rally the party...";
        }
        return "I think you put the wrong numbers in";
    }

    //Ordered on worst to best. text1 is bad and text5 is good
    private string findText2(int roll, string input1, string input2, string input3, string input4, string input5)
    {
        //You can reuse this code for determining the effects of the special, or u can put it here. whatever is easier :)

        if (roll == 0)
        {
            return input1;
        }
        else if (roll >= 1 && roll <= 5)
        {
            return input2;
        }
        else if (roll >= 6 && roll <= 10)
        {
            return input3;
        }
        else if (roll >= 11 && roll <= 17)
        {
            return input4;
        }
        else if (roll >= 18 && roll <= 19)
        {
            return input5;
        }
        else // Shouldn't be possible to get here
        {
            return "...and does a cool kickflip instead.";
        }
    }
}
