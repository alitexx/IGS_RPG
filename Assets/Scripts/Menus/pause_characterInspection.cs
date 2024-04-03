using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class pause_characterInspection : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private TextMeshProUGUI characterInfo;
    [SerializeField] private TextMeshProUGUI stats_1;
    [SerializeField] private TextMeshProUGUI stats_2;
    [SerializeField] private TextMeshProUGUI specialInfo;
    [SerializeField] private TextMeshProUGUI elementInfo;
    [SerializeField] private Image elementIcon;
    [SerializeField] private LevelManager levelManager;

    private int[] selectedCharacterStats;

    [SerializeField] private Sprite[] elementsIcons;


    public void characterInspection(int character)
    {
        switch(character)
        {
            case 0: // alan
                selectedCharacterStats = levelManager.SetTankStats();
                characterName.text = "Alan";
                characterInfo.text = "A knight sworn to protect Isen. He is searching for a way to bring back his fallen mentor.";
                stats_1.text = $"Strength: {selectedCharacterStats[0]}\r\nMagic Attack: {selectedCharacterStats[1]}\r\nDefense: {selectedCharacterStats[2]}\r\nSpeed: {selectedCharacterStats[3]}";
                stats_2.text = $"Health: {selectedCharacterStats[4]}/{selectedCharacterStats[5]}\r\nMana: {selectedCharacterStats[6]}/{selectedCharacterStats[7]}\r\nSpecial Uses: x/y";
                specialInfo.text = "Special\r\nTaunt\r\n\r\n\r\n\r\nHeal yourself by 50%. Attract attacks from enemies.";
                elementInfo.text = "Magical Element\r\n\r\n\r\n\r\n\r\nFire";
                elementIcon.sprite = elementsIcons[0];
                break;
            case 1: // kisa
                selectedCharacterStats = levelManager.SetBardStats();
                characterName.text = "Kisa";
                characterInfo.text = "An elven bard. Her family doubts her adventuring capabilities due to her pampered upbringing and pompous demeanor, but she wishes to prove them wrong.";
                stats_1.text = $"Strength: {selectedCharacterStats[0]}\r\nMagic Attack: {selectedCharacterStats[1]}\r\nDefense: {selectedCharacterStats[2]}\r\nSpeed: {selectedCharacterStats[3]}";
                stats_2.text = $"Health: {selectedCharacterStats[4]}/{selectedCharacterStats[5]}\r\nMana: {selectedCharacterStats[6]}/{selectedCharacterStats[7]}\r\nSpecial Uses: x/y";
                specialInfo.text = "Special\r\nHealing\r\n\r\n\r\n\r\nHeal the party by 50%.";
                elementInfo.text = "Magical Element\r\n\r\n\r\n\r\n\r\nWind";
                elementIcon.sprite = elementsIcons[1];
                break;
            case 2: // nicol
                selectedCharacterStats = levelManager.SetMageStats();
                characterName.text = "Nicol";
                characterInfo.text = "A cat-like red magician. Raised by magicians, he now wanders the globe for the sake of exploration, frequently calling himself an \"aristocat\".";
                stats_1.text = $"Strength: {selectedCharacterStats[0]}\r\nMagic Attack: {selectedCharacterStats[1]}\r\nDefense: {selectedCharacterStats[2]}\r\nSpeed: {selectedCharacterStats[3]}";
                stats_2.text = $"Health: {selectedCharacterStats[4]}/{selectedCharacterStats[5]}\r\nMana: {selectedCharacterStats[6]}/{selectedCharacterStats[7]}\r\nSpecial Uses: x/y";
                specialInfo.text = "Special\r\nExamine\r\n\r\n\r\n\r\nView an enemy's weakness.";
                elementInfo.text = "Magical Element\r\n\r\n\r\n\r\n\r\nIce";
                elementIcon.sprite = elementsIcons[2];
                break;
            case 3: // sophie
                selectedCharacterStats = levelManager.SetMonkStats();
                characterName.text = "Sophie";
                characterInfo.text = "A monk from a far-off land. She abandoned her royal title to live a simpler life in Isen. She owns a renowned dojo in the heart of Isen and set out on this journey to find her missing students.";
                stats_1.text = $"Strength: {selectedCharacterStats[0]}\r\nMagic Attack: {selectedCharacterStats[1]}\r\nDefense: {selectedCharacterStats[2]}\r\nSpeed: {selectedCharacterStats[3]}";
                stats_2.text = $"Health: {selectedCharacterStats[4]}/{selectedCharacterStats[5]}\r\nMana: {selectedCharacterStats[6]}/{selectedCharacterStats[7]}\r\nSpecial Uses: x/y";
                specialInfo.text = "Special\r\nThunderstorm\r\n\r\n\r\n\r\nAttack all enemies with an electric attack.";
                elementInfo.text = "Magical Element\r\n\r\n\r\n\r\n\r\nElectric";
                elementIcon.sprite = elementsIcons[3];
                break;
        }
        selectedCharacterStats = null;
    }
}
