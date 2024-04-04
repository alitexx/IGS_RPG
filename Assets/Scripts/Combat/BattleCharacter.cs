using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine.U2D;
using CodeMonkey;
using UnityEngine.UIElements;

public class BattleCharacter : MonoBehaviour
{
    //not using the stats for now
    //stats used on BattleController
    //private CharacterData characterData = new CharacterData("Bob", playerStats, "Fire", "Is a cube", true);
    //private CharacterData enemyData = new CharacterData("Bob", enemyStats, "Fire", "Is a cube", false);

    public CharacterData statSheet;
    public bool isBlocking;

    public GameObject weaknessObject;
    public SpriteRenderer weaknessImage;

    private State state;
    private Vector3 slideTargetPosition;
    private Action onSlideComplete;

    //Global "which team" bool
    public bool GIsPlayerTeam;

    //highlighted circle on whose turn it is
    private GameObject selectionCircleObject;
    private GameObject targetingCircleObject;

    //temporarily telling who it is
    public SpriteRenderer charSprite;

    public HealthSystem healthSystem;
    //temporary health bar
    private World_Bar healthBar;
    private World_Bar manaBar;

    public Animator animator;

    public ParticleManager particleManager;
    private enum State
    {
        Idle, 
        Sliding, 
        Busy,
        Attacking
    }

    private void Awake()
    {
        state = State.Idle;
        selectionCircleObject = transform.Find("Outline").gameObject;
        targetingCircleObject = transform.Find("TargetCircle").gameObject;
        weaknessObject = transform.Find("Weakness").gameObject;
        
        HideWeaknessObject();

        HideSelectionCircle();
        HideTargetCircle();
    }

    private void Start()
    {
        //Debug.Log(GIsPlayerTeam + ": Strength:" + statSheet.stats["Strength"]);
    }

    //Sprite 
    public void Setup(bool LIsPlayerTeam)
    {
        this.GIsPlayerTeam = LIsPlayerTeam;

        healthSystem = new HealthSystem(statSheet.stats["MaxHealth"], statSheet.stats["Health"]);

        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;

        if (LIsPlayerTeam)
        {
            //Ally
            //textures and animations
            if (statSheet.name == "Tank Guy")
            {
                charSprite.color = Color.magenta;
                animator.SetBool("isTank", true);
            }
            else if (statSheet.name == "Mage Guy")
            {
                //charSprite.color = Color.red;
                animator.SetBool("isMage", true);
            }
            else if (statSheet.name == "Monk Guy")
            {
                //charSprite.color = Color.yellow;
                animator.SetBool("isMonk", true);
            }
            else if (statSheet.name == "Bard Guy")
            {
                //charSprite.color = Color.green;
                animator.SetBool("isBard", true);
            }

            healthBar = new World_Bar(transform, new Vector3(0, 0.8f), new Vector3(1, 0.2f), Color.grey, Color.green, healthSystem.GetHealthPercent(), 100, new World_Bar.Outline { color = Color.black, size = 0.2f });
            manaBar = new World_Bar(transform, new Vector3(0, 1f), new Vector3(1, 0.2f), Color.grey, Color.blue, (float)statSheet.stats["Mana"] / statSheet.stats["MaxMana"], 100, new World_Bar.Outline { color = Color.black, size = 0.2f });
        }
        else
        {
            //enemy
            if (statSheet.name == "Slime Guy")
            {
                //charSprite.color = Color.cyan;
                animator.SetBool("isSlime", true);
            }
            else if (statSheet.name == "Skeleton Guy")
            {
                //charSprite.color = Color.white;
                animator.SetBool("isSkeleton", true);
            }
            else if (statSheet.name == "Wraith Guy")
            {
                //charSprite.color = Color.black;
                animator.SetBool("isWraith", true);
            }
            else if (statSheet.name == "Ghost Guy")
            {
                //charSprite.color = Color.grey;
                animator.SetBool("isGhost", true);
            }

            healthBar = new World_Bar(transform, new Vector3(0, 0.8f), new Vector3(1, 0.2f), Color.grey, Color.red, 1f, 100, new World_Bar.Outline { color = Color.black, size = 0.2f });
        }


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
        Vector3 slideCloseToTargetPosition = targetCharacter.GetPosition() + (GetPosition() - targetCharacter.GetPosition()).normalized * 3f;
        Vector3 startingPosition = GetPosition();

        //Slide to target
        SlideToPosition(slideCloseToTargetPosition, () =>
        {
            //Arrived at target and face them
            state = State.Busy;
            Vector3 attackDir = (targetCharacter.GetPosition() - GetPosition()).normalized;

            attacker.animator.SetBool("Attacking", true);
            state = State.Attacking;

            StartCoroutine(WaitUntilAttackOver(targetCharacter, attacker, startingPosition, onAttackComplete));
        });
    }

    private IEnumerator WaitUntilAttackOver(BattleCharacter targetCharacter, BattleCharacter attacker, Vector3 startingPosition, Action onAttackComplete)
    {
        while (attacker.state == State.Attacking)
        {
            yield return null;
        }

        //This clump of code is how to get the particle manager to do stuff
        Vector3 position = targetCharacter.GetPosition();
        ParticleManager particle = Instantiate(particleManager, position, Quaternion.identity, targetCharacter.transform);


        if (attacker.statSheet.name == "Tank Guy" || attacker.statSheet.name == "Wraith Guy" || attacker.statSheet.name == "Skeleton Guy" || attacker.statSheet.name == "Mage Guy")
        {
            particle.animator.SetBool("SlashFX", true);
        }
        else
        {
            particle.animator.SetBool("PunchFX", true);
        }


        targetCharacter.GotDamaged(attacker.statSheet.stats["Strength"], targetCharacter.statSheet.stats["Defense"]);

        attacker.animator.SetBool("Attacking", false);

        SlideToPosition(startingPosition, () =>
        {
            //slide back complete, back to idle
            state = State.Idle;
            //idle animation trigger would go here
            onAttackComplete();
        });
    }

    public void AttackOver()
    {
        state = State.Busy;
    }

    public void magAttack(BattleCharacter targetCharacter, BattleCharacter attacker, Action onAttackComplete)
    {
        state = State.Attacking;
        attacker.animator.SetBool("MagAttacking", true);

        if (attacker.statSheet.stats["Mana"] > 0)
        {
            attacker.statSheet.stats["Mana"]--;
        }
        else
        {
            healthSystem.Damage(attacker.statSheet.stats["MaxHealth"] / 4);
        }

        if (attacker.GIsPlayerTeam)
        {
            manaBar.SetSize((float)attacker.statSheet.stats["Mana"] / attacker.statSheet.stats["MaxMana"]);
        }

        StartCoroutine(WaitUntilMagAttackOver(targetCharacter, attacker, onAttackComplete));
    }

    private IEnumerator WaitUntilMagAttackOver(BattleCharacter targetCharacter, BattleCharacter attacker, Action onAttackComplete)
    {
        while (attacker.state == State.Attacking)
        {
            yield return null;
        }

        animator.SetBool("MagAttacking", false);

        Vector3 position = targetCharacter.GetPosition();
        ParticleManager particle = Instantiate(particleManager, position, Quaternion.identity, targetCharacter.transform);

        if (attacker.statSheet.magicElement == "Fire")
        {
            particle.animator.SetBool("FireFX", true);
        }
        else if (attacker.statSheet.magicElement == "Ice")
        {
            particle.animator.SetBool("IceFX", true);
        }
        else if (attacker.statSheet.magicElement == "Wind")
        {
            particle.animator.SetBool("WindFX", true);
        }
        else if (attacker.statSheet.magicElement == "Electric")
        {
            particle.animator.SetBool("ElectricFX", true);
        }

        yield return new WaitForSeconds(.6f);

        if (targetCharacter.statSheet.weakness == attacker.statSheet.magicElement)
        {
            targetCharacter.GotDamaged(attacker.statSheet.stats["Magic Attack"] * 2, 0 /*Placeholder because magic ignores defense*/);
        }
        else
        {
            targetCharacter.GotDamaged(attacker.statSheet.stats["Magic Attack"], 0 /*Placeholder because magic ignores defense*/ );
        }


        state = State.Busy;

        onAttackComplete();
    }

    public void magAttackOver()
    {
        state = State.Busy;
    }

    /*public void specialMove(BattleCharacter targetCharacter, BattleCharacter attacker, Action onAttackComplete)
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

            targetCharacter.weaknessObject.SetActive(true);

            if (targetCharacter.statSheet.weakness == "Fire")
            {
                targetCharacter.weaknessImage.sprite = fire;
            }
            else if (targetCharacter.statSheet.weakness == "Ice")
            {
                targetCharacter.weaknessImage.sprite = ice;
            }
            else if (targetCharacter.statSheet.weakness == "Electric")
            {
                targetCharacter.weaknessImage.sprite = electric;
            }
            else if (targetCharacter.statSheet.weakness == "Wind")
            {
                targetCharacter.weaknessImage.sprite = wind;
            }
            else
            {
                Debug.Log("No weakness");
            }
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
    }*/

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

        animator.SetBool("Hurt", true);

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

        StartCoroutine(WaitUntilHurtOver(damageMinusDefense));

        //Debug.Log("Final Damage: " + damageMinusDefense);
    }

    private IEnumerator WaitUntilHurtOver(int damageMinusDefense)
    {
        while (animator.GetBool("Hurt") == true)
        {
            yield return null;
        }

        /*if (isBlocking)
        {
            healthSystem.Damage(damageMinusDefense / 2);
            //Debug.Log("Defender Health: " + healthSystem.GetHealth());
        }
        else
        {
            healthSystem.Damage(damageMinusDefense);
            //Debug.Log("Defender Health: " + healthSystem.GetHealth());
        }

        statSheet.stats["Health"] = healthSystem.GetHealth();*/
    }

    public void HurtOver()
    {
        animator.SetBool("Hurt", false);
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

    public void HideWeaknessObject()
    {
        weaknessObject.SetActive(false);
    }

    public void ShowWeaknessObject()
    {
        weaknessObject.SetActive(true);
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
