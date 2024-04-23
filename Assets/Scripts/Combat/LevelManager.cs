using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LevelManager : MonoBehaviour
{
    public int gainedEXP = 0;
    public int currentEXP = 0;
    public static int level = 1;

    public bool kisaAbsorb;
    public bool nicolAbsorb;
    public bool sophieAbsorb;

    [SerializeField] private pauseMenuManager pauseMenuManager;
    [SerializeField] private mainDialogueManager mainDialogueManager;
    [SerializeField] private PlayerController playerController;

    static LevelManager Instance;

    public audioManager am;

    public BattleController battleController;

    #region Stats to Store

    static private int[] tankStoredStats = new int[8] {
        /*Strength*/ 10,
        /*Magic Attack*/ 5,
        /*Defense*/ 5, 
        /*Speed*/ 3, 
        /*Health*/ 18, 
        /*MaxHealth*/ 18,
        /*Mana*/ 4,
        /*MaxMana*/ 4
};

    static private int[] mageStoredStats = new int[8] {
        /*0 Strength*/ 7,
        /*1 Magic Attack*/ 8,
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
        /*1 Magic Attack*/ 6,
        /*2 Defense*/ 4, 
        /*3 Speed*/ 4, 
        /*4 Health*/ 15, 
        /*5 MaxHealth*/ 15,
        /*6 Mana*/ 7,
        /*7 MaxMana*/ 7
    };

    #endregion

    #region Starting Stats

    static private int[] tankStartStats = new int[8] {
        /*Strength*/ 10,
        /*Magic Attack*/ 5,
        /*Defense*/ 5, 
        /*Speed*/ 3, 
        /*Health*/ 18, 
        /*MaxHealth*/ 18,
        /*Mana*/ 4,
        /*MaxMana*/ 4
};

    static private int[] mageStartStats = new int[8] {
        /*0 Strength*/ 7,
        /*1 Magic Attack*/ 8,
        /*2 Defense*/ 4, 
        /*3 Speed*/ 5, 
        /*4 Health*/ 8, 
        /*5 MaxHealth*/ 8,
        /*6 Mana*/ 9,
        /*7 MaxMana*/ 9
    };

    static private int[] monkStartStats = new int[8] {
        /*0 Strength*/ 13,
        /*1 Magic Attack*/ 7,
        /*2 Defense*/ 4, 
        /*3 Speed*/ 6, 
        /*4 Health*/ 10, 
        /*5 MaxHealth*/ 10,
        /*6 Mana*/ 5,
        /*7 MaxMana*/ 5
    };

    static private int[] bardStartStats = new int[8] {
        /*0 Strength*/ 6,
        /*1 Magic Attack*/ 6,
        /*2 Defense*/ 4, 
        /*3 Speed*/ 4, 
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
        /*4 Health*/ 17, 
        /*5 MaxHealth*/ 17,
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
        /*0 Strength*/ 9,
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
        level = playerController.partyLevel;

        /*if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);*/

        //kisaAbsorb = false;
        //nicolAbsorb = false;
        //sophieAbsorb = false;
        //level = 1;
        //currentEXP = 0;

        FullHeal();
    }

    public void LoadStats(int loadedLevel)
    {
        level = loadedLevel;

        Debug.Log(level);

        if (playerController.BattleTutorialCleared == 1)
        {
            for (int i = 0; i < 8; i++)
            {
                tankStoredStats[i] = tankStartStats[i];
                mageStoredStats[i] = mageStartStats[i];
                monkStoredStats[i] = monkStartStats[i];
                bardStoredStats[i] = bardStartStats[i];
            }
        }
        else
        {
            for (int i = 0; i < 8; i++)
            {
                tankStoredStats[i] += (1 * (level - 1));
                mageStoredStats[i] += (1 * (level - 1));
                bardStoredStats[i] += (1 * (level - 1));
                monkStoredStats[i] += (1 * (level - 1));
            }

            tankStoredStats[0] += (1 * (level - 1));
            tankStoredStats[4] += (1 * (level - 1));
            tankStoredStats[5] += (1 * (level - 1));
        }

        Debug.Log(tankStoredStats[5]);
    }

    public void LevelUp()
    {

        level++;

        //Debug.Log(level);

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
                battleController.partyMembers[i].statSheet.stats["Strength"] += 2;
                battleController.partyMembers[i].statSheet.stats["Magic Attack"] += 1;
                battleController.partyMembers[i].statSheet.stats["Defense"] += 1;
                battleController.partyMembers[i].statSheet.stats["Speed"] += 1;
                battleController.partyMembers[i].statSheet.stats["Health"] += 2;
                battleController.partyMembers[i].statSheet.stats["MaxHealth"] += 2;
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
            //Nicol
            if (battleController.partyMembers[i].statSheet.name == "Mage Guy")
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
            //Kisa
            if (battleController.partyMembers[i].statSheet.name == "Bard Guy")
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
            //Sophie
            if (battleController.partyMembers[i].statSheet.name == "Monk Guy")
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
            
            if (battleController.partyMembers[i].statSheet.name == "Mage Guy")
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
            
            if (battleController.partyMembers[i].statSheet.name == "Bard Guy")
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
            
            if (battleController.partyMembers[i].statSheet.name == "Monk Guy")
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
            bardStoredStats[0] += (1 * (level - 1));
            bardStoredStats[1] += (2 * (level - 1));
            bardStoredStats[2] += (1 * (level - 1));
            bardStoredStats[3] += (1 * (level - 1));
            bardStoredStats[4] += (2 * (level - 1));
            bardStoredStats[5] += (2 * (level - 1));
            bardStoredStats[6] += (1 * (level - 1));
            bardStoredStats[7] += (1 * (level - 1));

            //the only time you gain a new party member will be after a boss fight, so I can put these here
            youWinMenu.loadedDialogue = "kisaPostFight_k";
            //mainDialogueManager.dialogueSTART("kisaPostFight_k");
            pauseMenuManager.partyMemberAdded("KISA");
        }
        else if (memberName == "Nicol")
        {
            mageStoredStats[0] += (1 * (level - 1));
            mageStoredStats[1] += (2 * (level - 1));
            mageStoredStats[2] += (1 * (level - 1));
            mageStoredStats[3] += (1 * (level - 1));
            mageStoredStats[4] += (1 * (level - 1));
            mageStoredStats[5] += (1 * (level - 1));
            mageStoredStats[6] += (2 * (level - 1));
            mageStoredStats[7] += (2 * (level - 1));

            if (kisaAbsorb)
            {
                youWinMenu.loadedDialogue = "nicolPostFight_xn";
                //mainDialogueManager.dialogueSTART("nicolPostFight_xn");
            } else
            {
                youWinMenu.loadedDialogue = "nicolPostFight_kn";
                //mainDialogueManager.dialogueSTART("nicolPostFight_kn");
            }
            pauseMenuManager.partyMemberAdded("NICOL");
        }
        else if (memberName == "Sophie")
        {
            monkStoredStats[0] += (2 * (level - 1));
            monkStoredStats[1] += (1 * (level - 1));
            monkStoredStats[2] += (2 * (level - 1));
            monkStoredStats[3] += (1 * (level - 1));
            monkStoredStats[4] += (1 * (level - 1));
            monkStoredStats[5] += (1 * (level - 1));
            monkStoredStats[6] += (1 * (level - 1));
            monkStoredStats[7] += (1 * (level - 1));

            if (kisaAbsorb)
            {
                youWinMenu.loadedDialogue = "sophiePostFight_xns";
                //mainDialogueManager.dialogueSTART("sophiePostFight_xns");
            } else if (nicolAbsorb)
            {
                youWinMenu.loadedDialogue = "sophiePostFight_kxs";
                //mainDialogueManager.dialogueSTART("sophiePostFight_kxs");
            }
            else
            {
                youWinMenu.loadedDialogue = "sophiePostFight_kns";
                //mainDialogueManager.dialogueSTART("sophiePostFight_kns");
            }
            pauseMenuManager.partyMemberAdded("SOPHIE");
        }
    }

    public void AbsorbPartyMember(string memberName)
    {
        if (memberName == "Kisa")
        {
            //before: health increased by 2 * level -1
            //new: health increased by 2 * level -1, max mana increased by level, magic attack increased by level
            kisaAbsorb = true;
            battleController.partyMembers[0].statSheet.stats["Health"] += (2 * (level - 1));
            battleController.partyMembers[0].statSheet.stats["MaxHealth"] += (2 * (level - 1));
            battleController.partyMembers[0].statSheet.stats["Mana"] += (level);
            battleController.partyMembers[0].statSheet.stats["MaxMana"] += (level);
            battleController.partyMembers[0].statSheet.stats["Magic Attack"] += (level);
            // will always be the same dialogue
            youWinMenu.loadedDialogue = "kisaPostFight_x";
            //mainDialogueManager.dialogueSTART("kisaPostFight_x");
            pauseMenuManager.partyMemberKilled("KISA");
        }
        else if (memberName == "Nicol")
        {
            nicolAbsorb = true;
            //before: magic attack increased by 2 * level -1
            //new: magic attack increaed by 2 * level -1, speed increased by level, max mana increased by level
            battleController.partyMembers[0].statSheet.stats["Magic Attack"] += (2 * (level - 1));
            battleController.partyMembers[0].statSheet.stats["Speed"] += (level);
            battleController.partyMembers[0].statSheet.stats["Mana"] += (level);
            battleController.partyMembers[0].statSheet.stats["MaxMana"] += (level);
            //determine what dialogue shoule be played
            if (kisaAbsorb)
            {
                youWinMenu.loadedDialogue = "nicolPostFight_xx";
                mainDialogueManager.dialogueSTART("nicolPostFight_xx");
            }
            else
            {
                youWinMenu.loadedDialogue = "nicolPostFight_kx";
                mainDialogueManager.dialogueSTART("nicolPostFight_kx");
            }
            pauseMenuManager.partyMemberKilled("NICOL");
        }
        else if (memberName == "Sophie")
        {
            sophieAbsorb = true;
            //before: strength increased by 2 * level -1
            //new: strength increased by 2 * level -1, speed increased by level, defense increased by level
            battleController.partyMembers[0].statSheet.stats["Strength"] += (2 * (level - 1));
            battleController.partyMembers[0].statSheet.stats["Speed"] += (level);
            battleController.partyMembers[0].statSheet.stats["Defense"] += (level);
            //determine what dialogue shoule be played
            if (kisaAbsorb && nicolAbsorb)
            {
                youWinMenu.loadedDialogue = "sophiePostFight_xxx";
                //mainDialogueManager.dialogueSTART("sophiePostFight_xxx");
            }
            else if (kisaAbsorb)
            {
                youWinMenu.loadedDialogue = "sophiePostFight_xnx";
                //mainDialogueManager.dialogueSTART("sophiePostFight_xnx");
            }
            else if (nicolAbsorb)
            {
                youWinMenu.loadedDialogue = "sophiePostFight_kxx";
                //mainDialogueManager.dialogueSTART("sophiePostFight_kxx");
            }
            else
            {
                youWinMenu.loadedDialogue = "sophiePostFight_knx";
                //mainDialogueManager.dialogueSTART("sophiePostFight_knx");
            }
            pauseMenuManager.partyMemberKilled("SOPHIE");
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
            lSlimeStats[i] += 1 * (playerController.Level - 1);
        }

        lSlimeStats[4] += 2 * (playerController.Level - 1);
        lSlimeStats[5] += 2 * (playerController.Level - 1);

        return lSlimeStats;
    }

    public int[] SetSkeletonStats(int[] lSkeletonStats)
    {
        for (int i = 0; i < lSkeletonStats.Length; i++)
        {
            lSkeletonStats[i] += 1 * (playerController.Level - 1);
        }
        lSkeletonStats[0] += 2 * (playerController.Level - 1);

        return lSkeletonStats;
    }

    public int[] SetGhostStats(int[] lGhostStats)
    {
        for (int i = 0; i < lGhostStats.Length; i++)
        {
            lGhostStats[i] += 1 * (playerController.Level - 1);
        }
        lGhostStats[2] += 2 * (playerController.Level - 1);
        lGhostStats[0] += 1 * (playerController.Level - 1);

        return lGhostStats;
    }

    public int[] SetWraithStats(int[] lWraithStats)
    {

        for (int i = 0; i < lWraithStats.Length; i++)
        {
            lWraithStats[i] += 1 * (playerController.Level - 1);
        }
        lWraithStats[0] += 2 * (playerController.Level - 1);
        lWraithStats[4] += 1 * (playerController.Level - 1);
        lWraithStats[5] += 1 * (playerController.Level - 1);

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
