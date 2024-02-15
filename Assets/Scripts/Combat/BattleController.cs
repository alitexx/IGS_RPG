using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class BattleController : MonoBehaviour
{
    public static BattleController GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;

        //True for an ally, false for an enemy
        tankChar = SpawnCharacter(true, tankStats, "Tank Guy", 1, 0, 1);
        mageChar = SpawnCharacter(true, mageStats, "Mage Guy", 2, 1, 2);
        bardChar = SpawnCharacter(true, bardStats, "Bard Guy", 3, 3, 0);
        monkChar = SpawnCharacter(true, monkStats, "Monk Guy", 4, 2, 3);

        slimeChar = SpawnCharacter(false, slimeStats, "Slime Guy", 0 /*0 Because enemies don't have specials*/, 4, 0);
        skeletonChar = SpawnCharacter(false, skeletonStats, "Skeleton Guy", 0, 4, 1);
        WraithChar = SpawnCharacter(false, wraithStats, "Wraith Guy", 0, 4, 2);
        GhostChar = SpawnCharacter(false, wraithStats, "Ghost Guy", 0, 4, 3);
        

        /*
        if (playerChar.statSheet.stats["Speed"] < enemyChar.statSheet.stats["Speed"])
        {
            state = State.Busy;
            characterQueue.Enqueue(enemyChar);
            characterQueue.Enqueue(secondPlayerChar);
            characterQueue.Enqueue(playerChar);
        }
        else
        {
            state = State.WaitingForPlayer;
            characterQueue.Enqueue(playerChar);
            characterQueue.Enqueue(secondPlayerChar);
            characterQueue.Enqueue(enemyChar);
        }*/

        SortTurnOrder();

        //Reading the queue
        /*while (characterQueue.Count != 0)
        {
            Debug.Log(characterQueue.Peek().statSheet.name);
            alreadyWent.Enqueue(characterQueue.Dequeue());
        }

        while (alreadyWent.Count != 0)
        {
            characterQueue.Enqueue(alreadyWent.Dequeue());
        }*/

        SetActiveCharBattle(characterQueue.Peek());
        ChooseNextActiveChar();

        targetText.SetActive(false);
        playerWinText.SetActive(false);
        enemyWinText.SetActive(false);
    }

    #region Variables

    [SerializeField] private Transform playerCharacterTransform;

    //For use with sprites later
    public Texture2D playerSpriteSheet;
    public Texture2D enemySpriteSheet;

    private static BattleController instance;

    //Players
    private BattleCharacter tankChar;
    private BattleCharacter mageChar;
    private BattleCharacter monkChar;
    private BattleCharacter bardChar;

    //Enemies
    private BattleCharacter slimeChar;
    private BattleCharacter skeletonChar;
    private BattleCharacter WraithChar;
    private BattleCharacter GhostChar;

    private BattleCharacter activeChar;

    //Testing turn order
    private Queue<BattleCharacter> characterQueue = new Queue<BattleCharacter>();
    private Queue<BattleCharacter> alreadyWent = new Queue<BattleCharacter>();

    #region Stats

    //Tank Stats
    static public int[] tankStats = {
        /*Strength*/ 13,
        /*Magic Attack*/ 4,
        /*Defense*/ 8, 
        /*Speed*/ 3, 
        /*Health*/ 13, 
        /*MaxHealth*/ 13,
        /*Mana*/ 4,
        /*MaxMana*/ 4,
        /*EXP*/ 0,
        /*LvlUpThreshold*/ 10 };

    //Mage Stats
    static public int[] mageStats = {
        /*Strength*/ 7,
        /*Magic Attack*/ 13,
        /*Defense*/ 5, 
        /*Speed*/ 5, 
        /*Health*/ 6, 
        /*MaxHealth*/ 6,
        /*Mana*/ 9,
        /*MaxMana*/ 9,
        /*EXP*/ 0,
        /*LvlUpThreshold*/ 10 };

    //Monk Stats
    static public int[] monkStats = {
        /*Strength*/ 16,
        /*Magic Attack*/ 7,
        /*Defense*/ 4, 
        /*Speed*/ 6, 
        /*Health*/ 8, 
        /*MaxHealth*/ 8,
        /*Mana*/ 5,
        /*MaxMana*/ 5,
        /*EXP*/ 0,
        /*LvlUpThreshold*/ 10 };

    //Bard Stats
    static public int[] bardStats = {
        /*Strength*/ 5,
        /*Magic Attack*/ 5,
        /*Defense*/ 4, 
        /*Speed*/ 2, 
        /*Health*/ 15, 
        /*MaxHealth*/ 15,
        /*Mana*/ 7,
        /*MaxMana*/ 7,
        /*EXP*/ 0,
        /*LvlUpThreshold*/ 10 };

    //Slime Stats
    static public int[] slimeStats = {
        /*Strength*/ 10,
        /*Magic Attack*/ 1,
        /*Defense*/ 9, 
        /*Speed*/ 4, 
        /*Health*/ 15, 
        /*MaxHealth*/ 15,
        /*Mana*/ 6,
        /*MaxMana*/ 7,
        /*EXP*/ 0,
        /*LvlUpThreshold*/ 10 };

    //Skeleton Stats
    static public int[] skeletonStats = {
        /*Strength*/ 8,
        /*Magic Attack*/ 1,
        /*Defense*/ 11, 
        /*Speed*/ 3, 
        /*Health*/ 8, 
        /*MaxHealth*/ 8,
        /*Mana*/ 6,
        /*MaxMana*/ 7,
        /*EXP*/ 0,
        /*LvlUpThreshold*/ 10 };

    static public int[] wraithStats = {
        /*Strength*/ 14,
        /*Magic Attack*/ 1,
        /*Defense*/ 6, 
        /*Speed*/ 5, 
        /*Health*/ 17, 
        /*MaxHealth*/ 17,
        /*Mana*/ 6,
        /*MaxMana*/ 7,
        /*EXP*/ 0,
        /*LvlUpThreshold*/ 10 };

    static public int[] ghostStats = {
        /*Strength*/ 12,
        /*Magic Attack*/ 1,
        /*Defense*/ 8, 
        /*Speed*/ 4, 
        /*Health*/ 13, 
        /*MaxHealth*/ 13,
        /*Mana*/ 6,
        /*MaxMana*/ 7,
        /*EXP*/ 0,
        /*LvlUpThreshold*/ 10 };

    #endregion

    //Magic Types
    public Dictionary<int ,string> magicTypes = new Dictionary<int, string>()
    {
        {0, "Fire"},
        {1 , "Ice"},
        {2 , "Electric"},
        {3 , "Wind"},
        {4 , "No Magic"}
    };

    public Sprite fire;
    public Sprite ice;
    public Sprite wind;
    public Sprite electric;

    private List<BattleCharacter> playerList = new List<BattleCharacter>();

    private List<BattleCharacter> enemyList = new List<BattleCharacter>();

    private State state;

    public GameObject fightingButtons;

    #endregion

    private enum State
    {
        WaitingForPlayer,
        Busy,
    }

    //Changes the queue into an array, sorts the array, and then turns it back into a queue
    private void SortTurnOrder()
    {
        BattleCharacter[] tempArray = characterQueue.ToArray();

        
        for (int i = 0; i < tempArray.Length - 1; i++)
        {
            if (tempArray[i].statSheet.stats["Speed"] /*+ Random.Range(-2, 2)*/ < tempArray[i + 1].statSheet.stats["Speed"] /*+ Random.Range(-2, 2)*/)
            {
                BattleCharacter tempChar = tempArray[i];
                tempArray[i] = tempArray[i + 1];
                tempArray[i + 1] = tempChar;

                i = -1;
            }
        }

        characterQueue.Clear();

        for (int i = 0; i < tempArray.Length; i++)
        {
            //Debug.Log(tempArray[i].statSheet.name + " has the speed " + tempArray[i].statSheet.stats["Speed"]);

            characterQueue.Enqueue(tempArray[i]);
        }
    }

    private void Start()
    {
        //True for an ally, false for an enemy
        //Moved all the start code to awake
        /*playerChar = SpawnCharacter(true, playerStats);
        secondPlayerChar = SpawnCharacter(true, secondPlayerStats);
        enemyChar = SpawnCharacter(false, enemyStats);

        if (playerChar.statSheet.stats["Speed"] < enemyChar.statSheet.stats["Speed"])
        {
            state = State.Busy;
            characterQueue.Enqueue(enemyChar);
            characterQueue.Enqueue(secondPlayerChar);
            characterQueue.Enqueue(playerChar);
        }
        else
        {
            state = State.WaitingForPlayer;
            characterQueue.Enqueue(playerChar);
            characterQueue.Enqueue(secondPlayerChar);
            characterQueue.Enqueue(enemyChar);
        }

        //state = State.WaitingForPlayer;

        SetActiveCharBattle(characterQueue.Peek());
        ChooseNextActiveChar();

        playerWinText.SetActive(false);
        enemyWinText.SetActive(false);*/
    }

    private void Update()
    {
        if (state == State.WaitingForPlayer)
        {
            //Testing if fighting even works
            /*
            if (Input.GetKeyDown(KeyCode.Space))
            {
                state = State.Busy;
                playerChar.Attack(enemyChar, () =>
                {
                    ChooseNextActiveChar();
                });
            }*/

            //Testing if the player list works
            /*if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log(playerList[1].statSheet.stats["Strength"]);
            }*/

            fightingButtons.SetActive(true);
        }
        else
        {
            fightingButtons.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("RPG_World");
        }       
    }

    #region Buttons

    public void BeanButton()
    {
        Debug.Log("beans");
    }

    public void BackButton()
    {
        StopAllCoroutines();

        for (int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].HideTargetCircle();
        }

        for (int i = 0; i < playerList.Count; i++)
        {
            playerList[i].HideTargetCircle();
        }

        state = State.WaitingForPlayer;
        fightingButtons.SetActive(true);
        targetText.SetActive(false);
    }

    public void attackButton()
    {
        state = State.Busy;

        /*activeChar.Attack(secEnemyChar, activeChar, () =>
        {
            ChooseNextActiveChar();
        });*/

        StartCoroutine(AttackTargeting()); 
    }

    public void defendButton()
    {
        state = State.Busy;

        StartCoroutine(BlockConfirm());
    }

    public void magicButton()
    {
        state = State.Busy;

        /*activeChar.magAttack(enemyChar, activeChar, () =>
        {
            ChooseNextActiveChar();
        });*/

        StartCoroutine(MagicTargeting());
    }

    public void specialButton()
    {
        state = State.Busy;

        /*activeChar.specialMove(enemyChar, activeChar, () =>
        {
            ChooseNextActiveChar();
        });*/

        StartCoroutine(SpecialTargeting());
    }

    public void Die()
    {
        SceneManager.LoadScene("GameOver");
    }

    #endregion

    public GameObject targetText;

    #region Targeting Coroutines

    //When these coroutines are called, the while loop while loop indefinitely until the enter or "return" key is pressed
    private IEnumerator AttackTargeting()
    {
        int enemyNum = 0;

        targetText.SetActive(true);

        enemyList[enemyNum].ShowTargetCircle();

        while (!Input.GetKeyDown(KeyCode.Return))
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                enemyList[enemyNum].HideTargetCircle();
                if (enemyNum == enemyList.Count - 1)
                {
                    enemyNum = 0;
                }
                else
                {
                    enemyNum++;
                }
                enemyList[enemyNum].ShowTargetCircle();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                enemyList[enemyNum].HideTargetCircle();
                if (enemyNum == 0)
                {
                    enemyNum = (enemyList.Count - 1);
                }
                else
                {
                    enemyNum--;
                }
                enemyList[enemyNum].ShowTargetCircle();
            }

            yield return null;
        }

        activeChar.Attack(enemyList[enemyNum], activeChar, () =>
        {
            ChooseNextActiveChar();
        });

        enemyList[enemyNum].HideTargetCircle();

        targetText.SetActive(false);
    }

    private IEnumerator BlockConfirm()
    {
        targetText.SetActive(true);

        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }

        activeChar.isBlocking = true;
        ChooseNextActiveChar();

        targetText.SetActive(false);
    }

    private IEnumerator MagicTargeting()
    {
        int enemyNum = 0;

        targetText.SetActive(true);

        enemyList[enemyNum].ShowTargetCircle();

        while (!Input.GetKeyDown(KeyCode.Return))
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                enemyList[enemyNum].HideTargetCircle();
                if (enemyNum == enemyList.Count - 1)
                {
                    enemyNum = 0;
                }
                else
                {
                    enemyNum++;
                }
                enemyList[enemyNum].ShowTargetCircle();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                enemyList[enemyNum].HideTargetCircle();
                if (enemyNum == 0)
                {
                    enemyNum = (enemyList.Count - 1);
                }
                else
                {
                    enemyNum--;
                }
                enemyList[enemyNum].ShowTargetCircle();
            }

            yield return null;
        }

        enemyList[enemyNum].HideTargetCircle();

        targetText.SetActive(false);

        activeChar.magAttack(enemyList[enemyNum], activeChar, () =>
        {
            ChooseNextActiveChar();
        });
    }

    private bool isTaunting = false;

    private IEnumerator SpecialTargeting()
    {
        int enemyNum = 0;

        //Tank
        if (activeChar.statSheet.specialMove == 1)
        {
            targetText.SetActive(true);

            while (!Input.GetKeyDown(KeyCode.Return))
            {
                yield return null;
            }

            isTaunting = true;

            activeChar.healthSystem.Heal(activeChar.statSheet.stats["MaxHealth"] / 2);

            ChooseNextActiveChar();

            targetText.SetActive(false);
        }
        //Mage
        else if (activeChar.statSheet.specialMove == 2)
        {
            //Showing weakness
            targetText.SetActive(true);

            enemyList[enemyNum].ShowTargetCircle();

            while (!Input.GetKeyDown(KeyCode.Return))
            {
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    enemyList[enemyNum].HideTargetCircle();
                    if (enemyNum == enemyList.Count - 1)
                    {
                        enemyNum = 0;
                    }
                    else
                    {
                        enemyNum++;
                    }
                    enemyList[enemyNum].ShowTargetCircle();
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    enemyList[enemyNum].HideTargetCircle();
                    if (enemyNum == 0)
                    {
                        enemyNum = (enemyList.Count - 1);
                    }
                    else
                    {
                        enemyNum--;
                    }
                    enemyList[enemyNum].ShowTargetCircle();
                }

                yield return null;
            }

            enemyList[enemyNum].weaknessObject.SetActive(true);

            if (enemyList[enemyNum].statSheet.weakness == "Fire")
            {
                enemyList[enemyNum].weaknessImage.sprite = fire;
            }
            else if (enemyList[enemyNum].statSheet.weakness == "Ice")
            {
                enemyList[enemyNum].weaknessImage.sprite = ice;
            }
            else if (enemyList[enemyNum].statSheet.weakness == "Electric")
            {
                enemyList[enemyNum].weaknessImage.sprite = electric;
            }
            else if (enemyList[enemyNum].statSheet.weakness == "Wind")
            {
                enemyList[enemyNum].weaknessImage.sprite = wind;
            }
            else
            {
                Debug.Log("No weakness");
            }

            enemyList[enemyNum].HideTargetCircle();

            targetText.SetActive(false);

            ChooseNextActiveChar();
        }
        //Bard
        else if (activeChar.statSheet.specialMove == 3)
        {
            targetText.SetActive(true);

            while (!Input.GetKeyDown(KeyCode.Return))
            {
                yield return null;
            }

            targetText.SetActive(false);

            for (int i = 0; i < playerList.Count; i++)
            {
                playerList[i].healthSystem.Heal(playerList[i].statSheet.stats["MaxHealth"] / 2);
            }

            ChooseNextActiveChar();
        }
        //Monk
        else if (activeChar.statSheet.specialMove == 4)
        {
            targetText.SetActive(true);

            while (!Input.GetKeyDown(KeyCode.Return))
            {
                yield return null;
            }

            targetText.SetActive(false);


            for (int i = 0; i < enemyList.Count; i++)
            {
                enemyList[i].GotDamaged(activeChar.statSheet.stats["Strength"], enemyList[i].statSheet.stats["Defense"]);
            }

            ChooseNextActiveChar();
        }
        else
        {
            //If the special move is targeting a specific character, use this entire "else" code
            targetText.SetActive(true);

            enemyList[enemyNum].ShowTargetCircle();

            while (!Input.GetKeyDown(KeyCode.Return))
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    enemyList[enemyNum].HideTargetCircle();
                    if (enemyNum == enemyList.Count - 1)
                    {
                        enemyNum = 0;
                    }
                    else
                    {
                        enemyNum++;
                    }
                    enemyList[enemyNum].ShowTargetCircle();
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    enemyList[enemyNum].HideTargetCircle();
                    if (enemyNum == 0)
                    {
                        enemyNum = (enemyList.Count - 1);
                    }
                    else
                    {
                        enemyNum--;
                    }
                    enemyList[enemyNum].ShowTargetCircle();
                }

                yield return null;
            }

            //Put code you want to execute here

            enemyList[enemyNum].HideTargetCircle();

            targetText.SetActive(false);
        }
    }

    #endregion

    private BattleCharacter SpawnCharacter(bool isPlayerTeam, int[] statsToUse, string LName, int lSpecial, int lMagicType, int lMagicWeakness)
    {
        Vector3 position;
        if (isPlayerTeam)
        {
            if (playerList.Count == 0)
            {
                position = new Vector3(-5, 4);
            }
            else if (playerList.Count == 1)
            {
                position = new Vector3(-5, 2);
            }
            else if (playerList.Count == 2)
            {
                position = new Vector3(-5, 0);
            }
            else if (playerList.Count == 3)
            {
                position = new Vector3(-5, -2);
            }
            else
            {
                position = new Vector3(-5, 0);
            }
        }
        else
        {
            if (enemyList.Count == 0)
            {
                position = new Vector3(5, 4);
            }
            else if (enemyList.Count == 1)
            {
                position = new Vector3(5, 2);
            }
            else if (enemyList.Count == 2)
            {
                position = new Vector3(5, 0);
            }
            else if (enemyList.Count == 3)
            {
                position = new Vector3(5, -2);
            }
            else
            {
                position = new Vector3(5, 0);
            }
        }
        Transform characterTransform =  Instantiate(playerCharacterTransform, position, Quaternion.identity);
        BattleCharacter battleCharacter = characterTransform.GetComponent<BattleCharacter>();

        //Setting stats
        if (isPlayerTeam)
        {
                                                         //Name    Stats     Magic Type    Description  player Team   Magic Weakness
            battleCharacter.statSheet = new CharacterData(LName, statsToUse, magicTypes[lMagicType], "Is a cube", true, magicTypes[lMagicWeakness], lSpecial);
            playerList.Add(battleCharacter);
        }
        else
        {
            battleCharacter.statSheet = new CharacterData(LName, statsToUse, magicTypes[lMagicType], "Isn't a cube", false, magicTypes[lMagicWeakness], lSpecial);
            enemyList.Add(battleCharacter);
        }


        battleCharacter.Setup(isPlayerTeam, playerSpriteSheet, enemySpriteSheet);
        
        characterQueue.Enqueue(battleCharacter);

        return battleCharacter;
    }

    private void SetActiveCharBattle(BattleCharacter battleCharacter)
    {
        if (activeChar != null)
        {
            activeChar.HideSelectionCircle();
        }

        activeChar = battleCharacter;
        activeChar.ShowSelectionCircle();
    }

    //changes turns
    private void ChooseNextActiveChar()
    {
        if (TestBattleOver())
        {
            return;
        }

        /*if (activeChar == playerChar)
        {
            SetActiveCharBattle(enemyChar);
            state = State.Busy;

            enemyChar.Attack(playerChar, enemyChar, () =>
            {
                ChooseNextActiveChar();
            });

        }*/


        //Restarting the queue
        if (characterQueue.Count == 0)
        {
            //Debug.Log("beans");
            while (alreadyWent.Count != 0)
            {
                characterQueue.Enqueue(alreadyWent.Dequeue());
            };
        }

        //Removing dead characters from the queue
        if (characterQueue.Peek().healthSystem.GetHealth() == 0)
        {
            //if the dead character is an enemy
            /*if (characterQueue.Peek().GIsPlayerTeam == false)
            {
                enemyList.Remove(characterQueue.Peek());
            }
            //if the dead character is a player
            else
            {
                playerList.Remove(characterQueue.Peek());
            }*/

            characterQueue.Dequeue();

            while (alreadyWent.Count != 0)
            {
                characterQueue.Enqueue(alreadyWent.Dequeue());
            };

            while (characterQueue.Count != 0)
            {
                alreadyWent.Enqueue(characterQueue.Dequeue());
            };

            while (alreadyWent.Count != 0)
            {
                characterQueue.Enqueue(alreadyWent.Dequeue());
            };
        }

        //If the next character in the queue is on the enemy team
        if (characterQueue.Peek().GIsPlayerTeam == false)
        {
            //Debug.Log("enemy " + characterQueue.Peek().statSheet.name);
            SetActiveCharBattle(characterQueue.Peek());
            alreadyWent.Enqueue(characterQueue.Dequeue());

            state = State.Busy;

            /*
            activeChar.Attack(playerChar, activeChar, () =>
            {
                ChooseNextActiveChar();
            });*/

            int enemyTarget;

            if (isTaunting == false)
            {
                //Debug.Log("Not taunted");
                enemyTarget = Random.Range(0, playerList.Count);

                activeChar.Attack(playerList[enemyTarget], activeChar, () =>
                {
                    ChooseNextActiveChar();
                });
            }
            else
            {
                //Debug.Log("Taunted");
                activeChar.Attack(tankChar, activeChar, () =>
                {
                    ChooseNextActiveChar();
                });
            }

            //Debug.Log("Target: " +  enemyTarget);

            //Make enemy focus one target
            //enemyTarget = 1;



        }
        //If the next character in the queue is on the player team
        else
        {
            //Debug.Log("ally " + characterQueue.Peek().statSheet.name);
            if (characterQueue.Peek().statSheet.specialMove == 1)
            {
                isTaunting = false;
            }

            SetActiveCharBattle(characterQueue.Peek());
            alreadyWent.Enqueue(characterQueue.Dequeue());
            activeChar.isBlocking = false;
            state = State.WaitingForPlayer;
        }

        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i].statSheet.stats["Health"] <= 0)
            {
                playerList.RemoveAt(i);
            }
        }

        for (int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList[i].statSheet.stats["Health"] <= 0)
            {
                enemyList.RemoveAt(i);
            }
        }
    }

    //Who died
    public GameObject playerWinText;
    public GameObject enemyWinText;
    private bool TestBattleOver()
    {
        if (tankChar.IsDead() && mageChar.IsDead())
        {
            enemyWinText.SetActive(true);
            SceneManager.LoadScene("GameOver");
            return true;
        }
        else if (slimeChar.IsDead() && skeletonChar.IsDead())
        {
            playerWinText.SetActive(true);
            return true;
        }
        else
        {
            return false;
        }
    }
}
