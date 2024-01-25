using System.Collections;
using System.Collections.Generic;
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
    }


    [SerializeField] private Transform playerCharacterTransform;

    //For use with sprites later
    public Texture2D playerSpriteSheet;
    public Texture2D enemySpriteSheet;

    private static BattleController instance;


    private BattleCharacter playerChar;
    private BattleCharacter enemyChar;
    private BattleCharacter activeChar;

    //Stats
    static public int[] playerStats = {
        /*Strength*/ 2,
        /*Magic Attack*/ 1,
        /*Defense*/ 2, 
        /*Speed*/ 4, 
        /*Health*/ 5, 
        /*MaxHealth*/ 5,
        /*Mana*/ 6,
        /*MaxMana*/ 7,
        /*EXP*/ 0,
        /*LvlUpThreshold*/ 10 };
    static public int[] enemyStats = {
        /*Strength*/ 2,
        /*Magic Attack*/ 1,
        /*Defense*/ 2, 
        /*Speed*/ 4, 
        /*Health*/ 3, 
        /*MaxHealth*/ 3,
        /*Mana*/ 6,
        /*MaxMana*/ 7,
        /*EXP*/ 0,
        /*LvlUpThreshold*/ 10 };

    private State state;

    public GameObject fightingButtons;

   private enum State
    {
        WaitingForPlayer,
        Busy,
    }

    private void Start()
    {
        //True for an ally, false for an enemy
        playerChar = SpawnCharacter(true);
        enemyChar = SpawnCharacter(false);


        state = State.WaitingForPlayer;

        SetActiveCharBattle(playerChar);

        playerWinText.SetActive(false);
        enemyWinText.SetActive(false);
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
        playerChar.Attack(enemyChar, playerChar, () =>
        {
            ChooseNextActiveChar();
        });
    }

    public void defendButton()
    {
        playerChar.isBlocking = true;
        ChooseNextActiveChar();
    }

    #endregion

    private BattleCharacter SpawnCharacter(bool isPlayerTeam)
    {
        Vector3 position;
        if (isPlayerTeam)
        {
            position = new Vector3(-5, 0);
            
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
            battleCharacter.statSheet = new CharacterData("Bob", playerStats, "Fire", "Is a cube", true);
        }
        else
        {
            battleCharacter.statSheet = new CharacterData("Evil Bob", enemyStats, "Ice", "Isn't a cube", false);
        }


        battleCharacter.Setup(isPlayerTeam, playerSpriteSheet, enemySpriteSheet);
        

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

        if (activeChar == playerChar)
        {
            SetActiveCharBattle(enemyChar);
            state = State.Busy;

            enemyChar.Attack(playerChar, enemyChar, () =>
            {
                ChooseNextActiveChar();
            });

        }
        else
        {
            SetActiveCharBattle(playerChar);
            playerChar.isBlocking = false;
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
            //SceneManager.LoadScene("GameOver");
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
