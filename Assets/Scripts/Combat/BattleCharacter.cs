using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine.U2D;

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
    public bool GIsPlayerTeam;

    //highlighted circle on whose turn it is
    private GameObject selectionCircleObject;
    private GameObject targetingCircleObject;

    //temporarily telling who it is
    public SpriteRenderer colorCircle;

    public HealthSystem healthSystem;
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
        targetingCircleObject = transform.Find("TargetCircle").gameObject;
        HideSelectionCircle();
        HideTargetCircle();
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
            if (statSheet.name == "Tank Guy")
            {
                colorCircle.color = Color.magenta;
            }
            else if (statSheet.name == "Mage Guy")
            {
                colorCircle.color = Color.red;
            }
            else if (statSheet.name == "Monk Guy")
            {
                colorCircle.color = Color.yellow;
            }
            else if (statSheet.name == "Bard Guy")
            {
                colorCircle.color = Color.green;
            }

            healthBar = new World_Bar(transform, new Vector3(0, 1), new Vector3(1, 0.2f), Color.grey, Color.green, 1f, 100, new World_Bar.Outline { color = Color.black, size = 0.2f });
        }
        else
        {
            //enemy
            if (statSheet.name == "Slime Guy")
            {
                colorCircle.color = Color.cyan;
            }
            else if (statSheet.name == "Skeleton Guy")
            {
                colorCircle.color = Color.white;
            }

            healthBar = new World_Bar(transform, new Vector3(0, 1), new Vector3(1, 0.2f), Color.grey, Color.red, 1f, 100, new World_Bar.Outline { color = Color.black, size = 0.2f });
        }

        healthSystem = new HealthSystem(statSheet.stats["MaxHealth"]);

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

            //Debug.Log(attacker.statSheet.stats["Strength"]);
            targetCharacter.GotDamaged(attacker.statSheet.stats["Strength"], targetCharacter.statSheet.stats["Defense"]);

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
        state = State.Busy;

        if (targetCharacter.statSheet.weakness == attacker.statSheet.magicElement)
        {
            targetCharacter.GotDamaged(attacker.statSheet.stats["Magic Attack"] * 2, 0 /*Placeholder because magic ignores defense*/);
        }
        else
        {
            targetCharacter.GotDamaged(attacker.statSheet.stats["Magic Attack"], 0 /*Placeholder because magic ignores defense*/ );
        }

        state = State.Idle;

        onAttackComplete();
    }

    public void specialMove(BattleCharacter targetCharcter, BattleCharacter attacker, Action onAttackComplete)
    {
        //Debug.Log("Special Move");

        //Tank Move
        if (attacker.statSheet.specialMove == 1)
        {
            Debug.Log("Tank Special");
            attacker.healthSystem.Heal(attacker.statSheet.stats["MaxHealth"] / 2);
        }
        //Mage Move
        else if (attacker.statSheet.specialMove == 2)
        {
            Debug.Log("Mage Special");

        }
        //Bard
        else if (attacker.statSheet.specialMove == 3)
        {
            Debug.Log("Bard Special");
        }
        //Monk
        else if (attacker.statSheet.specialMove == 4)
        {
            Debug.Log("Monk Special");
        }
        //Not assigned
        else
        {
            Debug.Log("Character does not have an assigned special");
        }

        onAttackComplete();
    }

    //Code for taking damage
    public void GotDamaged(int damageSource, int defenseStat)
    {
        int damageMinusDefense = damageSource - defenseStat;

        //Debug.Log("Attacker Strength: " + damageSource);
        //Debug.Log("Defender Defense: " + defenseStat);

        if (damageMinusDefense <= 0)
        {
            damageMinusDefense = 0;
        }

        //Debug.Log("Final Damage: " + damageMinusDefense);

        if (isBlocking)
        {
            healthSystem.Damage(damageMinusDefense / 2);
            //Debug.Log("Defender Health: " + healthSystem.GetHealth());
        }
        else
        {
            healthSystem.Damage(damageMinusDefense);
            //Debug.Log("Defender Health: " + healthSystem.GetHealth());
        }

        statSheet.stats["Health"] = healthSystem.GetHealth();
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

    public void HideTargetCircle()
    {
        targetingCircleObject.SetActive(false);
    }

    public void ShowTargetCircle()
    {
        targetingCircleObject.SetActive(true);
    }
}
