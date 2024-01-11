using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Enemy
{
    public int level;
    public string name; // the character's name
    public Dictionary<string, int> stats = new Dictionary<string, int>() {
        {"Strength", 0},
        {"Magic Attack", 0},
        {"Defense", 0},
        {"Speed", 0},
        {"Health", 0},
        {"MaxHealth", 0},
        {"Mana", 0},
        {"MaxMana", 0},
    };

    public string MagicType;

    public Enemy(string name, int[] valuesToUse, string magicType)
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
        this.MagicType = magicType;
    }

    public EnemyState enemyState;
}

public enum EnemyState
{
    Idle,
    Casting,
    Attacking,
    Hurt,
    Dead
}