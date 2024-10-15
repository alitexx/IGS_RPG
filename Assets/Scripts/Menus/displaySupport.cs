using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class displaySupport : MonoBehaviour
{
    public CharSupportsData charSupportsData;

    [SerializeField] private Image[] charIcons, heartIcons;
    [SerializeField] private Sprite[] charSprites, heartSprites;

    [SerializeField] private PlayerController playerControl;

    //ALAN = 0
    //KISA = 1
    //NICOL = 2
    //SOPHIE = 3

    private void updateSupportIcons(string charName)
    {
        switch (charName.ToUpper()) // passed in from character inspection script
        {
            case "ALAN":
                showCharSupport(0, 1, charSupportsData.alankisa_support);
                showCharSupport(1, 2, charSupportsData.alannico_support);
                showCharSupport(2, 3, charSupportsData.alansoph_support);
                break;
            case "KISA":
                showCharSupport(0, 0, charSupportsData.alankisa_support);
                showCharSupport(1, 2, charSupportsData.kisanico_support);
                showCharSupport(2, 3, charSupportsData.kisasoph_support);
                break;
            case "NICOL":
                showCharSupport(0, 0, charSupportsData.alannico_support);
                showCharSupport(1, 1, charSupportsData.kisanico_support);
                showCharSupport(2, 3, charSupportsData.nicosoph_support);
                break;
            case "SOPHIE":
                showCharSupport(0, 0, charSupportsData.alansoph_support);
                showCharSupport(1, 1, charSupportsData.nicosoph_support);
                showCharSupport(2, 2, charSupportsData.kisasoph_support);
                break;
        }
    }






















    private void showCharSupport(int position, int charName, int binarySupport)
    {
        charIcons[position].sprite = charSprites[charName];
        heartIcons[position].sprite = heartSprites[Convert.ToInt32(binarySupport)];

        if ((playerControl.KisaAbsorbed == 1 && charName == 1) || (playerControl.NicolAbsorbed == 1 && charName == 1) || (playerControl.SophieAbsorbed == 1 && charName == 3))
        {

        }

            //changes that need to happen


            //nicol hearts disappear when he is dead
            //nicol hearts go down if kisa or sophie is dead

            //sophie hearts disappear when she is dead
            //sophie hearts go down when kisa or nicol is killed


            switch (charName)
        {
            case 1: //KISA
                if (playerControl.KisaAbsorbed == 1) // If Kisa is dead, change her sprite to be the dead one
                {
                    heartIcons[position].sprite = heartSprites[0]; // set as 0 for now, change this when importing sprites
                }
                else if (playerControl.SophieAbsorbed == 1)
                {
                    heartIcons[position].sprite = heartSprites[0]; // set as 0 for now, change this when importing sprites
                }
                break;
            case 2: //NICOL
                break;
            case 3: //SOPHIE
                break;
            default: //ALAN
                heartIcons[position].sprite = heartSprites[Convert.ToInt32(binarySupport)];
                break;
        }
    }
}
