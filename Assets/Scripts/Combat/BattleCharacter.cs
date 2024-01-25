using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using JetBrains.Annotations;
using Unity.VisualScripting;

public class BattleCharacter : MonoBehaviour
{
    //not using the stats for now
    //stats used on BattleController
    //private CharacterData characterData = new CharacterData("Bob", playerStats, "Fire", "Is a cube", true);
    //private CharacterData enemyData = new CharacterData("Bob", enemyStats, "Fire", "Is a cube", false);

    public CharacterData statSheet;
    public bool isBlocking;

    private State state;
    private Vector3 slideTargetPosition;
    private Action onSlideComplete;

    //Global "which team" bool
    private bool GIsPlayerTeam;

    //highlighted circle on whose turn it is
    private GameObject selectionCircleObject;

    private HealthSystem healthSystem;
    //temporary health bar
    private World_Bar healthBar;

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
        //Debug.Log(GIsPlayerTeam + ": Strength:" + statSheet.stats["Strength"]);
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

        healthSystem = new HealthSystem(statSheet.stats["MaxHealth"]);

        //temporary health bar
        healthBar = new World_Bar(transform, new Vector3(0, 1), new Vector3(1, 0.2f), Color.grey, Color.red, 1f, 100, new World_Bar.Outline { color = Color.black, size = 0.2f });
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;

        PlayAnimIdle();
    }

    private void HealthSystem_OnHealthChanged(object sender, EventArgs e)
    {
        healthBar.SetSize(healthSystem.GetHealthPercent());
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
        //checking states and preventing spam
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
    public void Attack(BattleCharacter targetCharacter, BattleCharacter attacker, Action onAttackComplete)
    {
        Vector3 slideCloseToTargetPosition = targetCharacter.GetPosition() + (GetPosition() - targetCharacter.GetPosition()).normalized * 2f;
        Vector3 startingPosition = GetPosition();

        //Slide to target
        SlideToPosition(slideCloseToTargetPosition, () =>
        {
            //Arrived at target and face them
            state = State.Busy;
            Vector3 attackDir = (targetCharacter.GetPosition() - GetPosition()).normalized;

            Debug.Log(attacker.statSheet.stats["Strength"]);
            targetCharacter.GotDamaged(attacker.statSheet.stats["Strength"]);

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

    public void magAttack(BattleCharacter targetCharacter, BattleCharacter attacker, Action onAttackComplete)
    {
        if (targetCharacter.statSheet.weakness == attacker.statSheet.magicElement)
        {
            targetCharacter.GotDamaged(attacker.statSheet.stats["Magic Attack"] * 2);
        }
        else
        {
            targetCharacter.GotDamaged(attacker.statSheet.stats["Magic Attack"]);
        }

        onAttackComplete();
    }
    //Code for taking damage
    public void GotDamaged(int strength)
    {
        if (isBlocking)
        {
            healthSystem.Damage(strength / 2);
            Debug.Log("Health: " + healthSystem.GetHealth());
        }
        else
        {
            healthSystem.Damage(strength);
            Debug.Log("Health: " + healthSystem.GetHealth());
        }
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
