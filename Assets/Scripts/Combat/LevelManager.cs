using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LevelManager : MonoBehaviour
{
    public int currentEXP = 0;
    public int level = 1;
    public int lvlUpThreshold = 10;

    public bool kisaAbsorb;
    public bool nicolAbsorb;
    public bool sophieAbsorb;

    static LevelManager Instance;

    public BattleController battleController;

    #region Stats to Store

    static private int[] tankStoredStats = new int[8] {
        /*0 Strength*/ 10,
        /*1 Magic Attack*/ 4,
        /*2 Defense*/ 5, 
        /*3 Speed*/ 3, 
        /*4 Health*/ 18, 
        /*5 MaxHealth*/ 18,
        /*6 Mana*/ 4,
        /*7 MaxMana*/ 4
};

    static private int[] mageStoredStats = new int[8] {
        /*0 Strength*/ 7,
        /*1 Magic Attack*/ 13,
        /*2 Defense*/ 4, 
        /*3 Speed*/ 5, 
        /*4 Health*/ 8, 
        /*5 MaxHealth*/ 8,
        /*6 Mana*/ 9,
        /*7 MaxMana*/ 9
    };

    static private int[] monkStoredStats = new int[8] {
        /*0 Strength*/ 13,
        /*1 Magic Attack*/ 7,
        /*2 Defense*/ 4, 
        /*3 Speed*/ 6, 
        /*4 Health*/ 10, 
        /*5 MaxHealth*/ 10,
        /*6 Mana*/ 5,
        /*7 MaxMana*/ 5
    };

    static private int[] bardStoredStats = new int[8] {
        /*0 Strength*/ 6,
        /*1 Magic Attack*/ 5,
        /*2 Defense*/ 4, 
        /*3 Speed*/ 2, 
        /*4 Health*/ 15, 
        /*5 MaxHealth*/ 15,
        /*6 Mana*/ 7,
        /*7 MaxMana*/ 7
    };

    #endregion

    #region EnemyStats 
    static public int[] slimeStats = {
        /*0 Strength*/ 7,
        /*1 Magic Attack*/ 1,
        /*2 Defense*/ 3, 
        /*3 Speed*/ 2, 
        /*4 Health*/ 20, 
        /*5 MaxHealth*/ 20,
        /*6 Mana*/ 6,
        /*7 MaxMana*/ 7};

    //Skeleton Stats
    static public int[] skeletonStats = {
        /*0 Strength*/ 7,
        /*1 Magic Attack*/ 1,
        /*2 Defense*/ 7, 
        /*3 Speed*/ 3, 
        /*4 Health*/ 10, 
        /*5 MaxHealth*/ 10,
        /*6 Mana*/ 6,
        /*7 MaxMana*/ 7};

    static public int[] wraithStats = {
        /*0 Strength*/ 11,
        /*1 Magic Attack*/ 1,
        /*2 Defense*/ 5, 
        /*3 Speed*/ 5, 
        /*4 Health*/ 17, 
        /*5 MaxHealth*/ 17,
        /*6 Mana*/ 6,
        /*7 MaxMana*/ 7};

    static public int[] ghostStats = {
        /*0 Strength*/ 9,
        /*1 Magic Attack*/ 1,
        /*2 Defense*/ 11, 
        /*3 Speed*/ 4, 
        /*4 Health*/ 7, 
        /*5 MaxHealth*/ 7,
        /*6 Mana*/ 6,
        /*7 MaxMana*/ 7};

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        /*if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);*/

        kisaAbsorb = false;
        nicolAbsorb = false;
        sophieAbsorb = false;
        level = 1;
        lvlUpThreshold = 10;
        currentEXP = 0;
    }

    public void LevelUp()
    {
        level++;
        currentEXP -= lvlUpThreshold;


        /*0 Strength*/
        /*1 Magic Attack*/ 
        /*2 Defense*/ 
        /*3 Speed*/ 
        /*4 Health*/ 
        /*5 MaxHealth*/ 
        /*6 Mana*/ 
        /*7 MaxMana*/

        for (int i = 0; i < battleController.partyMembers.Count; i++)
        {
            if (battleController.partyMembers[i].statSheet.name == "Tank Guy")
            {
                battleController.partyMembers[i].statSheet.stats["Strength"] += 1;
                battleController.partyMembers[i].statSheet.stats["Magic Attack"] += 1;
                battleController.partyMembers[i].statSheet.stats["Defense"] += 1;
                battleController.partyMembers[i].statSheet.stats["Speed"] += 1;
                battleController.partyMembers[i].statSheet.stats["Health"] += 1;
                battleController.partyMembers[i].statSheet.stats["MaxHealth"] += 1;
                battleController.partyMembers[i].statSheet.stats["Mana"] += 1;
                battleController.partyMembers[i].statSheet.stats["MaxMana"] += 1;

                if (kisaAbsorb)
                {
                    battleController.partyMembers[i].statSheet.stats["MaxHealth"] += 1;
                    battleController.partyMembers[i].statSheet.stats["Health"] += 1;
                    battleController.partyMembers[i].statSheet.stats["MagicAttack"] += 1;
                }

                if (nicolAbsorb)
                {
                    battleController.partyMembers[i].statSheet.stats["Magic Attack"] += 1;
                    battleController.partyMembers[i].statSheet.stats["MaxMana"] += 1;
                    battleController.partyMembers[i].statSheet.stats["Mana"] += 1;
                }


                if (sophieAbsorb)
                {
                    battleController.partyMembers[i].statSheet.stats["Strength"] += 1;
                    battleController.partyMembers[i].statSheet.stats["Defense"] += 1;
                }
           

            }
            else if (battleController.partyMembers[i].statSheet.name == "Mage Guy")
            {
                battleController.partyMembers[i].statSheet.stats["Strength"] += 1;
                battleController.partyMembers[i].statSheet.stats["Magic Attack"] += 2;
                battleController.partyMembers[i].statSheet.stats["Defense"] += 1;
                battleController.partyMembers[i].statSheet.stats["Speed"] += 1;
                battleController.partyMembers[i].statSheet.stats["Health"] += 1;
                battleController.partyMembers[i].statSheet.stats["MaxHealth"] += 1;
                battleController.partyMembers[i].statSheet.stats["Mana"] += 2;
                battleController.partyMembers[i].statSheet.stats["MaxMana"] += 2;
            }
            else if (battleController.partyMembers[i].statSheet.name == "Bard Guy")
            {
                battleController.partyMembers[i].statSheet.stats["Strength"] += 1;
                battleController.partyMembers[i].statSheet.stats["Magic Attack"] += 2;
                battleController.partyMembers[i].statSheet.stats["Defense"] += 1;
                battleController.partyMembers[i].statSheet.stats["Speed"] += 1;
                battleController.partyMembers[i].statSheet.stats["Health"] += 2;
                battleController.partyMembers[i].statSheet.stats["MaxHealth"] += 2;
                battleController.partyMembers[i].statSheet.stats["Mana"] += 1;
                battleController.partyMembers[i].statSheet.stats["MaxMana"] += 1;

                if (battleController.partyMembers[i].IsDead() == false)
                {
                    battleController.partyMembers[i].statSheet.stats["Health"] += 1;
                }
            }
            else if (battleController.partyMembers[i].statSheet.name == "Monk Guy")
            {
                battleController.partyMembers[i].statSheet.stats["Strength"] += 2;
                battleController.partyMembers[i].statSheet.stats["Magic Attack"] += 1;
                battleController.partyMembers[i].statSheet.stats["Defense"] += 2;
                battleController.partyMembers[i].statSheet.stats["Speed"] += 1;
                battleController.partyMembers[i].statSheet.stats["Health"] += 1;
                battleController.partyMembers[i].statSheet.stats["MaxHealth"] += 1;
                battleController.partyMembers[i].statSheet.stats["Mana"] += 1;
                battleController.partyMembers[i].statSheet.stats["MaxMana"] += 1;
            }
        }
    }

    public void StoreStats()
    {
        for (int i = 0; i < battleController.partyMembers.Count; i++)
        {
            if (battleController.partyMembers[i].statSheet.name == "Tank Guy")
            {
                tankStoredStats[0] = battleController.partyMembers[i].statSheet.stats["Strength"];
                tankStoredStats[1] = battleController.partyMembers[i].statSheet.stats["Magic Attack"];
                tankStoredStats[2] = battleController.partyMembers[i].statSheet.stats["Defense"];
                tankStoredStats[3] = battleController.partyMembers[i].statSheet.stats["Speed"];
                tankStoredStats[4] = battleController.partyMembers[i].statSheet.stats["Health"];
                tankStoredStats[5] = battleController.partyMembers[i].statSheet.stats["MaxHealth"];
                tankStoredStats[6] = battleController.partyMembers[i].statSheet.stats["Mana"];
                tankStoredStats[7] = battleController.partyMembers[i].statSheet.stats["MaxMana"];
            }
            else if (battleController.partyMembers[i].statSheet.name == "Mage Guy")
            {
                mageStoredStats[0] = battleController.partyMembers[i].statSheet.stats["Strength"];
                mageStoredStats[1] = battleController.partyMembers[i].statSheet.stats["Magic Attack"];
                mageStoredStats[2] = battleController.partyMembers[i].statSheet.stats["Defense"];
                mageStoredStats[3] = battleController.partyMembers[i].statSheet.stats["Speed"];
                mageStoredStats[4] = battleController.partyMembers[i].statSheet.stats["Health"];
                mageStoredStats[5] = battleController.partyMembers[i].statSheet.stats["MaxHealth"];
                mageStoredStats[6] = battleController.partyMembers[i].statSheet.stats["Mana"];
                mageStoredStats[7] = battleController.partyMembers[i].statSheet.stats["MaxMana"];
            }
            else if (battleController.partyMembers[i].statSheet.name == "Bard Guy")
            {
                bardStoredStats[0] = battleController.partyMembers[i].statSheet.stats["Strength"];
                bardStoredStats[1] = battleController.partyMembers[i].statSheet.stats["Magic Attack"];
                bardStoredStats[2] = battleController.partyMembers[i].statSheet.stats["Defense"];
                bardStoredStats[3] = battleController.partyMembers[i].statSheet.stats["Speed"];
                bardStoredStats[4] = battleController.partyMembers[i].statSheet.stats["Health"];
                bardStoredStats[5] = battleController.partyMembers[i].statSheet.stats["MaxHealth"];
                bardStoredStats[6] = battleController.partyMembers[i].statSheet.stats["Mana"];
                bardStoredStats[7] = battleController.partyMembers[i].statSheet.stats["MaxMana"];
            }
            else if (battleController.partyMembers[i].statSheet.name == "Monk Guy")
            {
                monkStoredStats[0] = battleController.partyMembers[i].statSheet.stats["Strength"];
                monkStoredStats[1] = battleController.partyMembers[i].statSheet.stats["Magic Attack"];
                monkStoredStats[2] = battleController.partyMembers[i].statSheet.stats["Defense"];
                monkStoredStats[3] = battleController.partyMembers[i].statSheet.stats["Speed"];
                monkStoredStats[4] = battleController.partyMembers[i].statSheet.stats["Health"];
                monkStoredStats[5] = battleController.partyMembers[i].statSheet.stats["MaxHealth"];
                monkStoredStats[6] = battleController.partyMembers[i].statSheet.stats["Mana"];
                monkStoredStats[7] = battleController.partyMembers[i].statSheet.stats["MaxMana"];
            }
        }
    }

    public void NewPartyMember(string memberName)
    {
        if (memberName == "Kisa")
        {
            bardStoredStats[4] += (2 * (level - 1));
            bardStoredStats[5] += (2 * (level - 1));
        }
        else if (memberName == "Nicol")
        {
            mageStoredStats[1] += (2 * (level - 1));
        }
        else if (memberName == "Sophie")
        {
            monkStoredStats[0] += (2 * (level - 1));
        }
    }

    public void AbsorbPartyMember(string memberName)
    {
        if (memberName == "Kisa")
        {
            kisaAbsorb = true;

            battleController.partyMembers[0].statSheet.stats["Health"] += (2 * (level - 1));
            battleController.partyMembers[0].statSheet.stats["MaxHealth"] += (2 * (level - 1));
        }
        else if (memberName == "Nicol")
        {
            nicolAbsorb = true;

            battleController.partyMembers[0].statSheet.stats["Magic Attack"] += (2 * (level - 1));

        }
        else if (memberName == "Sophie")
        {
            sophieAbsorb = true;

            battleController.partyMembers[0].statSheet.stats["Strength"] += (2 * (level - 1));
        }
    }

    public int[] SetTankStats()
    {
        return tankStoredStats;
    }

    public int[] SetMonkStats()
    {
        return monkStoredStats;
    }

    public int[] SetMageStats()
    {
        return mageStoredStats;
    }

    public int[] SetBardStats()
    {
        return bardStoredStats;
    }

    public int[] SetSlimeStats(int[] lSlimeStats)
    {
        for (int i = 0; i < lSlimeStats.Length; i++)
        {
            lSlimeStats[i] += 1 * (level - 1);
        }

        lSlimeStats[4] += 2 * (level - 1);
        lSlimeStats[5] += 2 * (level - 1);

        return lSlimeStats;
    }

    public int[] SetSkeletonStats(int[] lSkeletonStats)
    {
        for (int i = 0; i < lSkeletonStats.Length; i++)
        {
            lSkeletonStats[i] += 1 * (level - 1);
        }
        lSkeletonStats[0] += 2 * (level - 1);

        return lSkeletonStats;
    }

    public int[] SetGhostStats(int[] lGhostStats)
    {
        for (int i = 0; i < lGhostStats.Length; i++)
        {
            lGhostStats[i] += 1 * (level - 1);
        }
        lGhostStats[2] += 2 * (level - 1);
        lGhostStats[0] += 1 * (level - 1);

        return lGhostStats;
    }

    public int[] SetWraithStats(int[] lWraithStats)
    {

        for (int i = 0; i < lWraithStats.Length; i++)
        {
            lWraithStats[i] += 1 * (level - 1);
        }
        lWraithStats[0] += 2 * (level - 1);
        lWraithStats[4] += 1 * (level - 1);
        lWraithStats[5] += 1 * (level - 1);

        return lWraithStats;
    }

    public int GetCharHealth(string charName)
    {
        if (charName == "Mage Guy")
        {
            return mageStoredStats[4];
        }
        else if (charName == "Bard Guy")
        {
            return bardStoredStats[4];
        }
        else if (charName == "Monk Guy")
        {
            return monkStoredStats[4];
        }
        else
        {
            Debug.Log("No character found");
            return 0;
        }
    }

    public void FullHeal()
    {
        bardStoredStats[4] = bardStoredStats[5];
        bardStoredStats[6] = bardStoredStats[7];

        monkStoredStats[4] = monkStoredStats[5];
        monkStoredStats[6] = monkStoredStats[7];

        tankStoredStats[4] = tankStoredStats[5];
        tankStoredStats[6] = tankStoredStats[7];

        mageStoredStats[4] = mageStoredStats[5];
        mageStoredStats[6] = mageStoredStats[7];
    }
}
