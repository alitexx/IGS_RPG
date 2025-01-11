using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LevelManager : MonoBehaviour
{
    public int gainedEXP = 0;
    public int currentEXP = 0;
    public static int level = 1;

    //Used for level up of enemies, determines how difficult they should be.
    public float enemyDifficultyScale = 1.15f;

    public bool kisaAbsorb;
    public bool nicolAbsorb;
    public bool sophieAbsorb;

    [SerializeField] private pauseMenuManager pauseMenuManager;
    [SerializeField] private mainDialogueManager mainDialogueManager;
    [SerializeField] private PlayerController playerController;

    static LevelManager Instance;

    public audioManager am;

    public BattleController battleController;

    #region Stat Increase Per level
    //This is so I can manage how much they gain per level/modify stats easily
    private int[] alanIncreasedStats = new int[8] {
        /*Strength*/ 4,
        /*Magic Attack*/ 1,
        /*Defense*/ 2, 
        /*Speed*/ 0, 
        /*Health*/ 2, 
        /*MaxHealth*/ 2,
        /*Mana*/ 1,
        /*MaxMana*/ 1
};
    private int[] kisaIncreasedStats = new int[8] {
        /*Strength*/ 2,
        /*Magic Attack*/ 4,
        /*Defense*/ 1, 
        /*Speed*/ 1, 
        /*Health*/ 5, 
        /*MaxHealth*/ 5,
        /*Mana*/ 2,
        /*MaxMana*/ 2
};
    private int[] nicolIncreasedStats = new int[8] {
        /*Strength*/ 3,
        /*Magic Attack*/ 3,
        /*Defense*/ 1, 
        /*Speed*/ 1, 
        /*Health*/ 4, 
        /*MaxHealth*/ 4,
        /*Mana*/ 2,
        /*MaxMana*/ 2
};
    private int[] sophieIncreasedStats = new int[8] {
        /*Strength*/ 5,
        /*Magic Attack*/ 3,
        /*Defense*/ 2, 
        /*Speed*/ 1, 
        /*Health*/ 1, 
        /*MaxHealth*/ 1,
        /*Mana*/ 2,
        /*MaxMana*/ 2
};
    #endregion

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
        /*2 Defense*/ 8, 
        /*3 Speed*/ 5, 
        /*4 Health*/ 11, 
        /*5 MaxHealth*/ 11,
        /*6 Mana*/ 9,
        /*7 MaxMana*/ 9
    };

    static private int[] monkStoredStats = new int[8] {
        /*0 Strength*/ 13,
        /*1 Magic Attack*/ 7,
        /*2 Defense*/ 4, 
        /*3 Speed*/ 8, 
        /*4 Health*/ 13, 
        /*5 MaxHealth*/ 13,
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
        /*2 Defense*/ 6, 
        /*3 Speed*/ 5, 
        /*4 Health*/ 11, 
        /*5 MaxHealth*/ 11,
        /*6 Mana*/ 9,
        /*7 MaxMana*/ 9
    };

    static private int[] monkStartStats = new int[8] {
        /*0 Strength*/ 13,
        /*1 Magic Attack*/ 7,
        /*2 Defense*/ 2, 
        /*3 Speed*/ 8, 
        /*4 Health*/ 13, 
        /*5 MaxHealth*/ 13,
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
    //static public int[] slimeStats = {
    //    /*0 Strength*/ 7,
    //    /*1 Magic Attack*/ 1,
    //    /*2 Defense*/ 3, 
    //    /*3 Speed*/ 2, 
    //    /*4 Health*/ 17, 
    //    /*5 MaxHealth*/ 17,
    //    /*6 Mana*/ 6,
    //    /*7 MaxMana*/ 7};

    ////Skeleton Stats
    //static public int[] skeletonStats = {
    //    /*0 Strength*/ 7,
    //    /*1 Magic Attack*/ 1,
    //    /*2 Defense*/ 7, 
    //    /*3 Speed*/ 3, 
    //    /*4 Health*/ 10, 
    //    /*5 MaxHealth*/ 10,
    //    /*6 Mana*/ 6,
    //    /*7 MaxMana*/ 7};

    //static public int[] wraithStats = {
    //    /*0 Strength*/ 9,
    //    /*1 Magic Attack*/ 1,
    //    /*2 Defense*/ 5, 
    //    /*3 Speed*/ 5, 
    //    /*4 Health*/ 17, 
    //    /*5 MaxHealth*/ 17,
    //    /*6 Mana*/ 6,
    //    /*7 MaxMana*/ 7};

    //static public int[] ghostStats = {
    //    /*0 Strength*/ 9,
    //    /*1 Magic Attack*/ 1,
    //    /*2 Defense*/ 11, 
    //    /*3 Speed*/ 4, 
    //    /*4 Health*/ 7, 
    //    /*5 MaxHealth*/ 7,
    //    /*6 Mana*/ 6,
    //    /*7 MaxMana*/ 7};

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        level = playerController.partyLevel;

        if (playerController.BattleTutorialCleared != 1)
        {
            LoadStats(1);
        }

        //Debug.Log(tankStoredStats[5]);

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

    private void Update()
    {
        //Increasing level by 5
        if (Input.GetKeyDown(KeyCode.L) && Input.GetKeyDown(KeyCode.E) && Input.GetKeyDown(KeyCode.V))
        {
            level += 5;
        }
    }

    public void LoadStats(int loadedLevel)
    {
        level = loadedLevel;

        //Debug.Log(level);



        if (playerController.BattleTutorialCleared != 1)
        {
            for (int i = 0; i < 8; i++)
            {
                if (i != 3)
                {
                    tankStoredStats[i] = tankStartStats[i];
                    mageStoredStats[i] = mageStartStats[i];
                    monkStoredStats[i] = monkStartStats[i];
                    bardStoredStats[i] = bardStartStats[i];
                }
            }

            //Setting previous stats for level up UI
            for (int i = 0; i < 4; i++)
            {
                if (i == 0)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        levelUpUI.previousStats[i][j] = tankStoredStats[j];
                    }
                }
                else if (i == 1)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        levelUpUI.previousStats[i][j] = bardStoredStats[j];
                    }
                }
                else if (i == 2)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        levelUpUI.previousStats[i][j] = mageStoredStats[j];
                    }
                }
                else if (i == 3)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        levelUpUI.previousStats[i][j] = monkStoredStats[j];
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < 8; i++)
            {
                if (i != 3)
                {
                    tankStoredStats[i] = tankStartStats[i] + (alanIncreasedStats[i] * (level - 1));
                    mageStoredStats[i] = mageStartStats[i] + (nicolIncreasedStats[i] * (level - 1));
                    bardStoredStats[i] = bardStartStats[i] + (kisaIncreasedStats[i] * (level - 1));
                    monkStoredStats[i] = monkStartStats[i] + (sophieIncreasedStats[i] * (level - 1));
                }
            }

            //Tank
            //tankStoredStats[0] += (1 * (level - 1));
            ////tankStoredStats[2] += (1 * (level - 1));
            //tankStoredStats[4] += (1 * (level - 1));
            //tankStoredStats[5] += (1 * (level - 1));

            if (kisaAbsorb)
            {
                tankStoredStats[1] += (1 * (level - 1));
                tankStoredStats[4] += (4 * (level - 1));
                tankStoredStats[5] += (4 * (level - 1));
                //tankStoredStats[6] += (1 * (level - 1));
                //tankStoredStats[7] += (1 * (level - 1));
            }

            if (nicolAbsorb)
            {
                tankStoredStats[0] += (1 * (level - 1));
                tankStoredStats[1] += (1 * (level - 1));
                //tankStoredStats[3] += (1 * (level - 1));
                tankStoredStats[6] += (2 * (level - 1));
                tankStoredStats[7] += (2 * (level - 1));
            }

            if (sophieAbsorb)
            {
                tankStoredStats[0] += (3 * (level - 1));
                tankStoredStats[1] += (1 * (level - 1));
                //tankStoredStats[3] += (1 * (level - 1));
            }


            ////Mage
            //mageStoredStats[0] += (1 * (level - 1));
            //mageStoredStats[1] += (1 * (level - 1));
            ////mageStoredStats[3] += (1 * (level - 1));
            ////mageStoredStats[6] += (1 * (level - 1));
            ////mageStoredStats[7] += (1 * (level - 1));

            ////Bard
            //bardStoredStats[1] += (1 * (level - 1));
            //bardStoredStats[4] += (2 * (level - 1));
            ////bardStoredStats[5] += (2 * (level - 1));
            ////bardStoredStats[6] += (1 * (level - 1));
            ////bardStoredStats[7] += (1 * (level - 1));

            ////Monk
            //monkStoredStats[0] += (2 * (level - 1));
            //monkStoredStats[1] += (1 * (level - 1));
            ////monkStoredStats[3] += (1 * (level - 1));
            ///
        }

        //Debug.Log(tankStoredStats[5]);

        //Setting previous stats for level up UI
        for (int i = 0; i < 4; i++)
        {
            if (i == 0)
            {
                for (int j = 0; j < 8; j++)
                {
                    levelUpUI.previousStats[i][j] = tankStoredStats[j];
                }
            }
            else if (i == 1)
            {
                for (int j = 0; j < 8; j++)
                {
                    levelUpUI.previousStats[i][j] = bardStoredStats[j];
                }
            }
            else if (i == 2)
            {
                for (int j = 0; j < 8; j++)
                {
                    levelUpUI.previousStats[i][j] = mageStoredStats[j];
                }
            }
            else if (i == 3)
            {
                for (int j = 0; j < 8; j++)
                {
                    levelUpUI.previousStats[i][j] = monkStoredStats[j];
                }
            }
        }
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
                battleController.partyMembers[i].statSheet.stats["Strength"] += alanIncreasedStats[0];
                battleController.partyMembers[i].statSheet.stats["Magic Attack"] += alanIncreasedStats[1];
                battleController.partyMembers[i].statSheet.stats["Defense"] += alanIncreasedStats[2];
                //battleController.partyMembers[i].statSheet.stats["Speed"] += alanIncreasedStats[3];
                battleController.partyMembers[i].statSheet.stats["Health"] += alanIncreasedStats[4];
                battleController.partyMembers[i].statSheet.stats["MaxHealth"] += alanIncreasedStats[5];
                battleController.partyMembers[i].statSheet.stats["Mana"] += alanIncreasedStats[6];
                battleController.partyMembers[i].statSheet.stats["MaxMana"] += alanIncreasedStats[7];

                if (kisaAbsorb)
                {
                    battleController.partyMembers[i].statSheet.stats["MaxHealth"] += 4;
                    battleController.partyMembers[i].statSheet.stats["Health"] += 4;
                    battleController.partyMembers[i].statSheet.stats["Magic Attack"] += 1;
                    //battleController.partyMembers[i].statSheet.stats["MaxMana"] += 1;
                    //battleController.partyMembers[i].statSheet.stats["Mana"] += 1;
                }
                if (nicolAbsorb)
                {
                    battleController.partyMembers[i].statSheet.stats["Strength"] += 1;
                    battleController.partyMembers[i].statSheet.stats["Magic Attack"] += 1;
                    //battleController.partyMembers[i].statSheet.stats["Speed"] += 1;
                    battleController.partyMembers[i].statSheet.stats["MaxMana"] += 2;
                    battleController.partyMembers[i].statSheet.stats["Mana"] += 2;
                }
                if (sophieAbsorb)
                {
                    battleController.partyMembers[i].statSheet.stats["Strength"] += 3;
                    battleController.partyMembers[i].statSheet.stats["Magic Attack"] += 1;
                    //battleController.partyMembers[i].statSheet.stats["Speed"] += 1;
                }
            }
            //Nicol
            if (battleController.partyMembers[i].statSheet.name == "Mage Guy")
            {
                battleController.partyMembers[i].statSheet.stats["Strength"] += nicolIncreasedStats[0];
                battleController.partyMembers[i].statSheet.stats["Magic Attack"] += nicolIncreasedStats[1];
                battleController.partyMembers[i].statSheet.stats["Defense"] += nicolIncreasedStats[2];
                //battleController.partyMembers[i].statSheet.stats["Speed"] += nicolIncreasedStats[3];
                battleController.partyMembers[i].statSheet.stats["MaxHealth"] += nicolIncreasedStats[5];
                battleController.partyMembers[i].statSheet.stats["Mana"] += nicolIncreasedStats[6];
                battleController.partyMembers[i].statSheet.stats["MaxMana"] += nicolIncreasedStats[7];

                if (battleController.partyMembers[i].IsDead() == false)
                {
                    battleController.partyMembers[i].statSheet.stats["Health"] += nicolIncreasedStats[4];
                }
            }
            //Kisa
            if (battleController.partyMembers[i].statSheet.name == "Bard Guy")
            {
                battleController.partyMembers[i].statSheet.stats["Strength"] += kisaIncreasedStats[0];
                battleController.partyMembers[i].statSheet.stats["Magic Attack"] += kisaIncreasedStats[1];
                battleController.partyMembers[i].statSheet.stats["Defense"] += kisaIncreasedStats[2];
                //battleController.partyMembers[i].statSheet.stats["Speed"] += kisaIncreasedStats[3];
                battleController.partyMembers[i].statSheet.stats["MaxHealth"] += kisaIncreasedStats[5];
                battleController.partyMembers[i].statSheet.stats["Mana"] += kisaIncreasedStats[6];
                battleController.partyMembers[i].statSheet.stats["MaxMana"] += kisaIncreasedStats[7];

                if (battleController.partyMembers[i].IsDead() == false)
                {
                    battleController.partyMembers[i].statSheet.stats["Health"] += kisaIncreasedStats[4];
                }
            }
            //Sophie
            if (battleController.partyMembers[i].statSheet.name == "Monk Guy")
            {
                battleController.partyMembers[i].statSheet.stats["Strength"] += sophieIncreasedStats[0];
                battleController.partyMembers[i].statSheet.stats["Magic Attack"] += sophieIncreasedStats[1];
                battleController.partyMembers[i].statSheet.stats["Defense"] += sophieIncreasedStats[2];
                //battleController.partyMembers[i].statSheet.stats["Speed"] += sophieIncreasedStats[3];
                battleController.partyMembers[i].statSheet.stats["MaxHealth"] += sophieIncreasedStats[5];
                battleController.partyMembers[i].statSheet.stats["Mana"] += sophieIncreasedStats[6];
                battleController.partyMembers[i].statSheet.stats["MaxMana"] += sophieIncreasedStats[7];

                if (battleController.partyMembers[i].IsDead() == false)
                {
                    battleController.partyMembers[i].statSheet.stats["Health"] += sophieIncreasedStats[4];
                }
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
            bardStoredStats[0] = bardStartStats[0] + (kisaIncreasedStats[0] * (level - 1));
            bardStoredStats[1] = bardStartStats[1] + (kisaIncreasedStats[1] * (level - 1));
            bardStoredStats[2] = bardStartStats[2] + (kisaIncreasedStats[2] * (level - 1));
            //bardStoredStats[3] = bardStartStats[3] + (kisaIncreasedStats[3] * (level - 1));
            bardStoredStats[4] = bardStartStats[4] + (kisaIncreasedStats[4] * (level - 1));
            bardStoredStats[5] = bardStartStats[5] + (kisaIncreasedStats[5] * (level - 1));
            bardStoredStats[6] = bardStartStats[6] + (kisaIncreasedStats[6] * (level - 1));
            bardStoredStats[7] = bardStartStats[7] + (kisaIncreasedStats[7] * (level - 1));

            //the only time you gain a new party member will be after a boss fight, so I can put these here
            youWinMenu.loadedDialogue = "kisaPostFight_k";
            //mainDialogueManager.dialogueSTART("kisaPostFight_k");
            pauseMenuManager.partyMemberAdded("KISA");
        }
        else if (memberName == "Nicol")
        {
            mageStoredStats[0] = mageStartStats[0] + (nicolIncreasedStats[0] * (level - 1));
            mageStoredStats[1] = mageStartStats[1] + (nicolIncreasedStats[1] * (level - 1));
            mageStoredStats[2] = mageStartStats[2] + (nicolIncreasedStats[2] * (level - 1));
            //mageStoredStats[3] = mageStartStats[3] + (nicolIncreasedStats[3] * (level - 1));
            mageStoredStats[4] = mageStartStats[4] + (nicolIncreasedStats[4] * (level - 1));
            mageStoredStats[5] = mageStartStats[5] + (nicolIncreasedStats[5] * (level - 1));
            mageStoredStats[6] = mageStartStats[6] + (nicolIncreasedStats[6] * (level - 1));
            mageStoredStats[7] = mageStartStats[7] + (nicolIncreasedStats[7] * (level - 1));

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
            monkStoredStats[0] = monkStartStats[0] + (sophieIncreasedStats[0] * (level - 1));
            monkStoredStats[1] = monkStartStats[1] + (sophieIncreasedStats[1] * (level - 1));
            monkStoredStats[2] = monkStartStats[2] + (sophieIncreasedStats[2] * (level - 1));
            //monkStoredStats[3] = monkStartStats[3] + (sophieIncreasedStats[3] * (level - 1));
            monkStoredStats[4] = monkStartStats[4] + (sophieIncreasedStats[4] * (level - 1));
            monkStoredStats[5] = monkStartStats[5] + (sophieIncreasedStats[5] * (level - 1));
            monkStoredStats[6] = monkStartStats[6] + (sophieIncreasedStats[6] * (level - 1));
            monkStoredStats[7] = monkStartStats[7] + (sophieIncreasedStats[7] * (level - 1));

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

        StoreStats();
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
            battleController.partyMembers[0].statSheet.stats["Strength"] += (level);
            battleController.partyMembers[0].statSheet.stats["Magic Attack"] += (level);
            //battleController.partyMembers[0].statSheet.stats["Speed"] += (level);
            battleController.partyMembers[0].statSheet.stats["Mana"] += (level);
            battleController.partyMembers[0].statSheet.stats["MaxMana"] += (level);
            //determine what dialogue shoule be played
            if (kisaAbsorb)
            {
                youWinMenu.loadedDialogue = "nicolPostFight_xx";
                //mainDialogueManager.dialogueSTART("nicolPostFight_xx");
            }
            else
            {
                youWinMenu.loadedDialogue = "nicolPostFight_kx";
                //mainDialogueManager.dialogueSTART("nicolPostFight_kx");
            }
            pauseMenuManager.partyMemberKilled("NICOL");
        }
        else if (memberName == "Sophie")
        {
            sophieAbsorb = true;
            //before: strength increased by 2 * level -1
            //new: strength increased by 2 * level -1, speed increased by level, defense increased by level
            battleController.partyMembers[0].statSheet.stats["Strength"] += (2 * (level - 1));
            //battleController.partyMembers[0].statSheet.stats["Speed"] += (level);
            battleController.partyMembers[0].statSheet.stats["Magic Attack"] += (level);
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

        StoreStats();
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

    //For Katie reference

    /*0 Strength*/
    /*1 Magic Attack*/
    /*2 Defense*/
    /*3 Speed*/
    /*4 Health*/
    /*5 MaxHealth*/
    /*6 Mana*/
    /*7 MaxMana*/

    //If we need to update defense (would be nice so magic makes more sense at later levels)
    //lSlimeStats[2] += 1 * (playerController.Level - 1);

    public int[] SetSlimeStats(int[] lSlimeStats)
    {
        for (int i = 0; i < lSlimeStats.Length; i++)
        {
            if (i != 3) // Exclude speed from the main scaling
            {
                // Exponential scaling for levels 3 and above
                lSlimeStats[i] += (int)(4 * Math.Pow(playerController.Level, enemyDifficultyScale) - 4);
            }
        }

        // Adjust Strength with increased scaling
        lSlimeStats[0] += (int)(1.75 * (Math.Pow(playerController.Level, enemyDifficultyScale)));

        // Adjust Health and MaxHealth with increased scaling
        lSlimeStats[4] += (int)(1.5 * (Math.Pow(playerController.Level, enemyDifficultyScale)));
        lSlimeStats[5] += (int)(1.5 * (Math.Pow(playerController.Level, enemyDifficultyScale)));

        return lSlimeStats;
    }


    public int[] SetSkeletonStats(int[] lSkeletonStats)
    {
        for (int i = 0; i < lSkeletonStats.Length; i++)
        {
            if (i != 3) // Exclude Speed
            {
                // Exponential scaling for levels 3 and above
                lSkeletonStats[i] += (int)(4 * Math.Pow(playerController.Level, enemyDifficultyScale) - 4);
            }
        }

        // Adjust Strength + Def with slightly higher scaling
        lSkeletonStats[0] += (int)(2.25 * (Math.Pow(playerController.Level, enemyDifficultyScale)));
        lSkeletonStats[2] += (int)(1.25 * (Math.Pow(playerController.Level, enemyDifficultyScale)));

        return lSkeletonStats;
    }


    public int[] SetGhostStats(int[] lGhostStats)
    {
        for (int i = 0; i < lGhostStats.Length; i++)
        {
            if (i != 3) // Exclude Speed
            {
                // General stats scaling
                lGhostStats[i] += (int)(4 * Math.Pow(playerController.Level, enemyDifficultyScale) - 4);
            }
        }

        //// Defense-specific scaling
        //lGhostStats[2] += (int)(1.5 * (Math.Pow(playerController.Level, enemyDifficultyScale)));

        // Strength-specific scaling
        lGhostStats[0] += (int)(1.5 * (Math.Pow(playerController.Level, enemyDifficultyScale)));

        return lGhostStats;
    }

    public int[] SetWraithStats(int[] lWraithStats)
    {
        for (int i = 0; i < lWraithStats.Length; i++)
        {
            if (i != 3) // Exclude Speed
            {
                // General stats scaling
                lWraithStats[i] += (int)(4 * Math.Pow(playerController.Level, enemyDifficultyScale) - 4);
            }
        }

        // Strength-specific scaling
        lWraithStats[0] += (int)(2 * (Math.Pow(playerController.Level, enemyDifficultyScale)));

        // Health and MaxHealth scaling
        lWraithStats[4] += (int)(1.5 * (Math.Pow(playerController.Level, enemyDifficultyScale)));
        lWraithStats[5] += (int)(1.5 * (Math.Pow(playerController.Level, enemyDifficultyScale)));

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
