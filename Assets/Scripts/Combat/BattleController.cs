using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

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
        playerChar = SpawnCharacter(true, playerStats, "Magic Guy");
        secondPlayerChar = SpawnCharacter(true, secondPlayerStats, "Tank Guy");
        enemyChar = SpawnCharacter(false, enemyStats, "Slime Guy");

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
        //state = State.WaitingForPlayer;

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

        playerWinText.SetActive(false);
        enemyWinText.SetActive(false);
    }

    #region Variables

    [SerializeField] private Transform playerCharacterTransform;

    //For use with sprites later
    public Texture2D playerSpriteSheet;
    public Texture2D enemySpriteSheet;

    private static BattleController instance;


    private BattleCharacter playerChar;
    private BattleCharacter secondPlayerChar;
    private BattleCharacter enemyChar;
    private BattleCharacter activeChar;

    //Testing turn order
    private Queue<BattleCharacter> characterQueue = new Queue<BattleCharacter>();
    private Queue<BattleCharacter> alreadyWent = new Queue<BattleCharacter>();

    //Stats
    static public int[] playerStats = {
        /*Strength*/ 5,
        /*Magic Attack*/ 6,
        /*Defense*/ 3, 
        /*Speed*/ 4, 
        /*Health*/ 10, 
        /*MaxHealth*/ 10,
        /*Mana*/ 6,
        /*MaxMana*/ 7,
        /*EXP*/ 0,
        /*LvlUpThreshold*/ 10 };

    static public int[] secondPlayerStats = {
        /*Strength*/ 8,
        /*Magic Attack*/ 2,
        /*Defense*/ 5, 
        /*Speed*/ 2, 
        /*Health*/ 15, 
        /*MaxHealth*/ 15,
        /*Mana*/ 6,
        /*MaxMana*/ 7,
        /*EXP*/ 0,
        /*LvlUpThreshold*/ 10 };

    static public int[] enemyStats = {
        /*Strength*/ 7,
        /*Magic Attack*/ 1,
        /*Defense*/ 2, 
        /*Speed*/ 3, 
        /*Health*/ 20, 
        /*MaxHealth*/ 20,
        /*Mana*/ 6,
        /*MaxMana*/ 7,
        /*EXP*/ 0,
        /*LvlUpThreshold*/ 10 };
    
    //Magic Types
    public Dictionary<int ,string> magicTypes = new Dictionary<int, string>()
    {
        {0, "Fire"},
        {1 , "Ice"},
        {2 , "Electric"},
        {3 , "Wind"}
    };

    private List<BattleCharacter> playerList = new List<BattleCharacter>();

    private State state;

    private int playerCount;

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
            if (tempArray[i].statSheet.stats["Speed"] < tempArray[i + 1].statSheet.stats["Speed"])
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

    public void attackButton()
    {
        state = State.Busy;
        activeChar.Attack(enemyChar, activeChar, () =>
        {
            ChooseNextActiveChar();
        });
    }

    public void defendButton()
    {
        activeChar.isBlocking = true;
        ChooseNextActiveChar();
    }

    public void magicButton()
    {
        state = State.Busy;
        activeChar.magAttack(enemyChar, activeChar, () =>
        {
            ChooseNextActiveChar();
        });
    }

    #endregion

    private BattleCharacter SpawnCharacter(bool isPlayerTeam, int[] statsToUse, string LName)
    {
        Vector3 position;
        if (isPlayerTeam)
        {
            if (playerCount == 0)
            {
                position = new Vector3(-5, 0);
            }
            else if (playerCount == 1)
            {
                position = new Vector3(-5, 3);
            }
            else
            {
                position = new Vector3(-5, 0);
            }

            playerCount++;
        }
        else
        {
            position = new Vector3(5, 0);
        }
        Transform characterTransform =  Instantiate(playerCharacterTransform, position, Quaternion.identity);
        BattleCharacter battleCharacter = characterTransform.GetComponent<BattleCharacter>();

        //Setting stats
        if (isPlayerTeam)
        {
                                                         //Name    Stats     Magic Type    Description  player Team   Magic Weakness
            battleCharacter.statSheet = new CharacterData(LName, statsToUse, magicTypes[0], "Is a cube", true, magicTypes[1]);
            playerList.Add(battleCharacter);
        }
        else
        {
            battleCharacter.statSheet = new CharacterData(LName, statsToUse, magicTypes[1], "Isn't a cube", false, magicTypes[1]);
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


        //Testing Queues
        if (characterQueue.Count == 0)
        {
            while (alreadyWent.Count != 0)
            {
                characterQueue.Enqueue(alreadyWent.Dequeue());
            };
        }

        //If the next character in the queue is on the enemy team
        if (characterQueue.Peek().GIsPlayerTeam == false)
        {
            SetActiveCharBattle(characterQueue.Peek());
            alreadyWent.Enqueue(characterQueue.Dequeue());

            state = State.Busy;

            /*
            activeChar.Attack(playerChar, activeChar, () =>
            {
                ChooseNextActiveChar();
            });*/
            int enemyTarget = Random.Range(0, 2);

            //Debug.Log("Target: " +  enemyTarget);

            activeChar.Attack(playerList[enemyTarget], activeChar, () =>
            {
                ChooseNextActiveChar();
            });

        }
        //If the next character in the queue is on the player team
        else
        {
            SetActiveCharBattle(characterQueue.Peek());
            alreadyWent.Enqueue(characterQueue.Dequeue());
            activeChar.isBlocking = false;
            state = State.WaitingForPlayer;
        }
    }


    //Who died
    public GameObject playerWinText;
    public GameObject enemyWinText;
    private bool TestBattleOver()
    {
        if (playerChar.IsDead())
        {
            enemyWinText.SetActive(true);
            SceneManager.LoadScene("GameOver");
            return true;
        }
        else if (enemyChar.IsDead())
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
