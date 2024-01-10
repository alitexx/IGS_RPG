using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public string name; // the character's name
    public Dictionary<string, int> stats = new Dictionary<string, int>() {
        {"Strength", 0},
        {"Magic Attack", 0},
        {"Defense", 0},
        {"Speed", 0},
        {"Health", 0},
        {"Mana", 0},
        {"EXP", 0}

    }; //dictionary containing every character's stats
    public string magicType; // PLACEHOLDER
    public string description; // a description of the character to be shown on the pause menu

    public Character(string name, Dictionary<string, int> stats, string magicType, string description)
    {
        this.name = name;
        this.stats = stats;
        this.magicType = magicType;
        this.description = description;
    }

    public void IncreaseStats(bool isLevelUp)
    {
        // check if this is because of a level up or not. If it is not because of a level up, 
    }


}
