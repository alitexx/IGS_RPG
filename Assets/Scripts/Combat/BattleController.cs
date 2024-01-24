using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private State state;


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
    }

    private void Update()
    {
        if (state == State.WaitingForPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                state = State.Busy;
                playerChar.Attack(enemyChar, () =>
                {
                    ChooseNextActiveChar();
                });
            }
        }
    }

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

            enemyChar.Attack(playerChar, () =>
            {
                ChooseNextActiveChar();
            });

        }
        else
        {
            SetActiveCharBattle(playerChar);
            state = State.WaitingForPlayer;
        }
    }

    private bool TestBattleOver()
    {
        if (playerChar.IsDead())
        {
            return true;
        }
        else if (enemyChar.IsDead())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
