using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
public class pause_characterInspection : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI characterInfo;
    [SerializeField] private TextMeshProUGUI stats_1;
    [SerializeField] private TextMeshProUGUI stats_2;
    [SerializeField] private TextMeshProUGUI elementInfo;
    [SerializeField] private Image elementIcon;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private specialPauseMenu spm;
    [SerializeField] private Slider[] HPMPSliders;

    [SerializeField] private PlayerController playerControl;

    private int[] selectedCharacterStats;

    [SerializeField] private Sprite[] elementsIcons;
    [SerializeField] private displaySupport displaySupport;

    /*0 Strength*/
    /*1 Magic Attack*/
    /*2 Defense*/
    /*3 Speed*/
    /*4 Health*/
    /*5 MaxHealth*/
    /*6 Mana*/
    /*7 MaxMana*/
    public void characterInspection(int character)
    {
        displaySupport.updateSupportIcons(character);
        switch (character)
        {
            case 0: // alan
                spm.WhoAreWeViewing = "alan";
                selectedCharacterStats = levelManager.SetTankStats();
                characterInfo.text = "A knight sworn to protect Isen. He is searching for a way to bring back his fallen mentor.";
                
                if (playerControl.KisaAbsorbed == 1)
                {
                    elementInfo.text = "Magical Element\r\n\r\n\r\n\r\n\r\nFire, Wind";
                    elementIcon.sprite = elementsIcons[4];
                    if (playerControl.SophieAbsorbed == 1)
                    {
                        elementInfo.text = "Magical Element\r\n\r\n\r\n\r\n\r\nFire, Wind, Electric";
                        elementIcon.sprite = elementsIcons[7];
                    }
                    if (playerControl.NicolAbsorbed == 1)
                    {
                        elementInfo.text = "Magical Element\r\n\r\n\r\n\r\n\r\nFire, Wind, Ice";
                        elementIcon.sprite = elementsIcons[5];
                        if (playerControl.SophieAbsorbed == 1)
                        {
                            elementInfo.text = "Magical Element\r\n\r\n\r\n\r\n\r\nFire, Wind, Ice, Electric";
                            elementIcon.sprite = elementsIcons[6];
                        }
                    }
                } else if (playerControl.NicolAbsorbed == 1)
                {
                    elementInfo.text = "Magical Element\r\n\r\n\r\n\r\n\r\nFire, Ice";
                    elementIcon.sprite = elementsIcons[8];
                    if (playerControl.SophieAbsorbed == 1)
                    {
                        elementInfo.text = "Magical Element\r\n\r\n\r\n\r\n\r\nFire, Ice, Electric";
                        elementIcon.sprite = elementsIcons[9];
                    }
                } else if (playerControl.SophieAbsorbed == 1)
                {
                    elementInfo.text = "Magical Element\r\n\r\n\r\n\r\n\r\nFire, Electric";
                    elementIcon.sprite = elementsIcons[10];
                } else
                {
                    //if no one is absorbed
                    elementInfo.text = "Magical Element\r\n\r\n\r\n\r\n\r\nFire";
                    elementIcon.sprite = elementsIcons[0];
                }
                break;
            case 1: // kisa
                spm.WhoAreWeViewing = "kisa";
                selectedCharacterStats = levelManager.SetBardStats();
                characterInfo.text = "An elven bard. Her family doubts her adventuring capabilities due to her pampered upbringing and pompous demeanor, but she wishes to prove them wrong.";
                elementInfo.text = "Magical Element\r\n\r\n\r\n\r\n\r\nWind";
                elementIcon.sprite = elementsIcons[1];
                break;
            case 2: // nicol
                spm.WhoAreWeViewing = "nicol";
                selectedCharacterStats = levelManager.SetMageStats();
                characterInfo.text = "A cat-like red magician. Raised by magicians, he now wanders the globe for the sake of exploration, calling himself an \"aristocat\".";
                elementInfo.text = "Magical Element\r\n\r\n\r\n\r\n\r\nIce";
                elementIcon.sprite = elementsIcons[2];
                break;
            case 3: // sophie
                spm.WhoAreWeViewing = "sophie";
                selectedCharacterStats = levelManager.SetMonkStats();
                characterInfo.text = "A monk from a far-off land. She owns a renowned dojo in the heart of Isen and set out on this journey to find her missing students.";
                elementInfo.text = "Magical Element\r\n\r\n\r\n\r\n\r\nElectric";
                elementIcon.sprite = elementsIcons[3];
                break;
        }
        stats_1.text = $"Strength: {selectedCharacterStats[0]}\r\nMagic Attack: {selectedCharacterStats[1]}\r\nDefense: {selectedCharacterStats[2]}\r\nSpeed: {selectedCharacterStats[3]}";
        stats_2.text = $"Health: {selectedCharacterStats[4]}/{selectedCharacterStats[5]}\r\nMana: {selectedCharacterStats[6]}/{selectedCharacterStats[7]}";
        HPMPSliders[0].value = (float)selectedCharacterStats[4] / selectedCharacterStats[5];
        HPMPSliders[1].value = (float)selectedCharacterStats[6] / selectedCharacterStats[7];
        selectedCharacterStats = null;
    }
}
