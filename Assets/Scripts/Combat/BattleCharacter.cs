using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCharacter : MonoBehaviour
{
    //not using the stats for now
    static public int[] playerStats = {
        /*Strength*/ 2,
        /*Magic Attack*/ 1,
        /*Defense*/ 2, 
        /*Speed*/ 4, 
        /*Health*/ 4, 
        /*MaxHealth*/ 5,
        /*Mana*/ 6,
        /*MaxMana*/ 7,
        /*EXP*/ 0,
        /*LvlUpThreshold*/ 10 };
    private CharacterData characterData = new CharacterData("Bob", playerStats, "Fire", "Is a cube", true);

    static public int[] enemyStats = {
        /*Strength*/ 2,
        /*Magic Attack*/ 1,
        /*Defense*/ 2, 
        /*Speed*/ 4, 
        /*Health*/ 4, 
        /*MaxHealth*/ 5,
        /*Mana*/ 6,
        /*MaxMana*/ 7,
        /*EXP*/ 0,
        /*LvlUpThreshold*/ 10 };
    private CharacterData enemyData = new CharacterData("Bob", enemyStats, "Fire", "Is a cube", false);


    private State state;
    private Vector3 slideTargetPosition;
    private Action onSlideComplete;

    private bool GIsPlayerTeam;

    private GameObject selectionCircleObject;

    private HealthSystem healthSystem;

    private enum State
    {
        Idle, 
        Sliding, 
        Busy
    }

    private void Awake()
    {
        state = State.Idle;
        selectionCircleObject = transform.Find("Outline").gameObject;
        HideSelectionCircle();
    }

    private void Start()
    {
        
    }

    //Sprite 
    public void Setup(bool LIsPlayerTeam, Texture2D playerSprite, Texture2D enemySprite)
    {
        this.GIsPlayerTeam = LIsPlayerTeam;
        if (LIsPlayerTeam)
        {
            //Ally
            //textures and animations

        }
        else
        {
            //enemy
        }

        healthSystem = new HealthSystem(100);
    }

    private void PlayAnimIdle()
    {
        if (GIsPlayerTeam)
        {
            //idle animtion for allies
        }
        else 
        { 
            //idle animation for enemies
        }
    }

    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                break;
            case State.Busy:
                break;
            case State.Sliding:
                float slideSpeed = 4f;
                transform.position += (slideTargetPosition - GetPosition()) * slideSpeed * Time.deltaTime;

                float reachedDistance = .5f;
                if (Vector3.Distance(GetPosition(), slideTargetPosition) < reachedDistance)
                {
                    //Arrived at slide target position
                    transform.position = slideTargetPosition;
                    onSlideComplete();
                }
                break;
        }
    }


    //Code for face the target and attacking
    public Vector3 GetPosition()
    {
        return transform.position;
    }
    public void Attack(BattleCharacter targetCharacter, Action onAttackComplete)
    {
        Vector3 slideCloseToTargetPosition = targetCharacter.GetPosition() + (GetPosition() - targetCharacter.GetPosition()).normalized * 2f;
        Vector3 startingPosition = GetPosition();

        //Slide to target
        SlideToPosition(slideCloseToTargetPosition, () =>
        {
            //Arrived at target and face them
            state = State.Busy;
            Vector3 attackDir = (targetCharacter.GetPosition() - GetPosition()).normalized;

            targetCharacter.Damage(10);

            //Animation would go here, and then the attack would be marked as complete once the animation ends with onAttackComplete
            //For now, there is no delay between attacking and the attack ending

            //Attack Complete, slide back
            SlideToPosition(startingPosition, () =>
            {
                //slide back complete, back to idle
                state = State.Idle;
                //idle animation trigger would go here
                onAttackComplete();
            });
        });

        
    }

    //Code for taking damage
    public void Damage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);
        Debug.Log("Health: " + healthSystem.GetHealth());
    }

    //Code for checking if an enemy is dead
    public bool IsDead()
    {
        return healthSystem.IsDead();
    }

    private void SlideToPosition(Vector3 slideTargetPosition, Action onSlideComplete)
    {
        this.slideTargetPosition = slideTargetPosition;
        this.onSlideComplete = onSlideComplete;
        state = State.Sliding;

        //sliding animation
        if (slideTargetPosition.x > 0) 
        { 
            //players 
        }
        else
        {
            //enemies
        }
    }

    public void HideSelectionCircle()
    {
        selectionCircleObject.SetActive(false);
    }

    public void ShowSelectionCircle()
    {
        selectionCircleObject.SetActive(true);
    }
}
