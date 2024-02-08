using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[System.Serializable]
public class CharacterData
{
    public int level;
    public string name; // the character's name
    public Dictionary<string, int> stats = new Dictionary<string, int>() {
        {"Strength", 5},
        {"Magic Attack", 0},
        {"Defense", 0},
        {"Speed", 0},
        {"Health", 0},
        {"MaxHealth", 0},
        {"Mana", 0},
        {"MaxMana", 0},
        {"EXP", 0},
        {"LvlUpThreshold", 10}

    }; //dictionary containing every character's stats
    public string magicElement; // PLACEHOLDER
    public string description; // a description of the character to be shown on the pause menu

    public int[] valuesForStats;

    public bool isPlayerTeam;

    public string weakness;

    public int specialMove;

    //1 = Tank, 2 = Mage, 3 = Bard, 4 = Monk

    public CharacterData(string name, int[] valuesToUse, string magicType, string description, bool playerTeam, string lWeakness, int lSpecial)
    {
        this.name = name;
        this.stats["Strength"] = valuesToUse[0];
        this.stats["Magic Attack"] = valuesToUse[1];
        this.stats["Defense"] = valuesToUse[2];
        this.stats["Speed"] = valuesToUse[3];
        this.stats["Health"] = valuesToUse[4];
        this.stats["MaxHealth"] = valuesToUse[5];
        this.stats["Mana"] = valuesToUse[6];
        this.stats["MaxMana"] = valuesToUse[7];
        this.stats["EXP"] = valuesToUse[8];
        this.stats["LvlUpThreshold"] = valuesToUse[9];
        this.magicElement = magicType;
        this.description = description;
        this.isPlayerTeam = playerTeam;
        this.weakness = lWeakness;
        this.specialMove = lSpecial;
    }



    
    public void IncreaseStats(bool isLevelUp)
    {
        // check if this is because of a level up or not. If it is not because of a level up, 
    }
}
