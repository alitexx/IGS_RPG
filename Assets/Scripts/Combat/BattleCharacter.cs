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
using TMPro;
using DG.Tweening;
using Random = UnityEngine.Random;

public class BattleCharacter : MonoBehaviour
{
    //not using the stats for now
    //stats used on BattleController
    //private CharacterData characterData = new CharacterData("Bob", playerStats, "Fire", "Is a cube", true);
    //private CharacterData enemyData = new CharacterData("Bob", enemyStats, "Fire", "Is a cube", false);

    [SerializeField] private GameObject fighterObject;

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
    public GameObject spriteObject;

    public HealthSystem healthSystem;
    //temporary health bar
    private World_Bar healthBar;
    private World_Bar manaBar;

    public Animator animator;

    public audioManager am;
    public GameObject amGameObject;

    public ParticleManager particleManager;

    [SerializeField] private Transform damagePopup;

    private GameObject healthParent;
    private Transform healthObject;
    public TextMeshProUGUI healthText;

    public bool specialAvailable;

    private enum State
    {
        Idle, 
        Sliding, 
        Busy,
        Attacking
    }

    private void Awake()
    {
        
        amGameObject = GameObject.Find("/-- AUDIO --");
        am = amGameObject.GetComponent<audioManager>();

        state = State.Idle;
        specialAvailable = true;
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
    public void Setup(bool LIsPlayerTeam, int stageLevel)
    {
        this.GIsPlayerTeam = LIsPlayerTeam;

        healthSystem = new HealthSystem(statSheet.stats["MaxHealth"], statSheet.stats["Health"]);

        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;

        healthParent = GameObject.Find("/Battle");

        if (LIsPlayerTeam)
        {
            //Ally
            //textures and animations
            if (statSheet.name == "Tank Guy")
            {
                healthObject = healthParent.transform.Find("Canvas/HP&Sides/HPAmounts/Alan/AlanBG/AlanHP");
                healthText = healthObject.GetComponent<TextMeshProUGUI>();
                //charSprite.color = Color.magenta;
                animator.SetBool("isTank", true);
            }
            else if (statSheet.name == "Mage Guy")
            {
                healthObject = healthParent.transform.Find("Canvas/HP&Sides/HPAmounts/Nicol/BG/NicolHP");
                healthText = healthObject.GetComponent<TextMeshProUGUI>();
                //charSprite.color = Color.red;
                animator.SetBool("isMage", true);
            }
            else if (statSheet.name == "Monk Guy")
            {
                healthObject = healthParent.transform.Find("Canvas/HP&Sides/HPAmounts/Sophie/BG/SophieHP");
                healthText = healthObject.GetComponent<TextMeshProUGUI>();
                //charSprite.color = Color.yellow;
                animator.SetBool("isMonk", true);
            }
            else if (statSheet.name == "Bard Guy")
            {
                healthObject = healthParent.transform.Find("Canvas/HP&Sides/HPAmounts/Kisa/BG/KisaHP");
                healthText = healthObject.GetComponent<TextMeshProUGUI>();
                //charSprite.color = Color.green;
                animator.SetBool("isBard", true);
            }

            healthBar = new World_Bar(transform, new Vector3(0, 1.4f), new Vector3(1, 0.2f), Color.grey, Color.green, healthSystem.GetHealthPercent(), 100, new World_Bar.Outline { color = Color.black, size = 0.15f });
            manaBar = new World_Bar(transform, new Vector3(0, 1.6f), new Vector3(1, 0.2f), Color.grey, Color.blue, (float)statSheet.stats["Mana"] / statSheet.stats["MaxMana"], 100, new World_Bar.Outline { color = Color.black, size = 0.15f });
        }
        else
        {
            animator.SetInteger("StageLevel", stageLevel);

            //enemy
            if (statSheet.name == "Slime Guy")
            {
                //charSprite.color = Color.cyan;
                animator.SetBool("isSlime", true);
                spriteObject.transform.position = new Vector3(spriteObject.transform.position.x, 0.2f, spriteObject.transform.position.z);
                spriteObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }
            else if (statSheet.name == "Skeleton Guy")
            {
                //charSprite.color = Color.white;
                animator.SetBool("isSkeleton", true);
                spriteObject.transform.position = new Vector3(spriteObject.transform.position.x, 0.1f, spriteObject.transform.position.z);
                spriteObject.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            }
            else if (statSheet.name == "Wraith Guy")
            {
                //charSprite.color = Color.black;
                animator.SetBool("isWraith", true);
                spriteObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }
            else if (statSheet.name == "Ghost Guy")
            {
                //charSprite.color = Color.grey;
                animator.SetBool("isGhost", true);
                spriteObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }
            else if (statSheet.name == "Mage Guy")
            {
                animator.SetBool("isBadNicol", true);
            }
            else if (statSheet.name == "Monk Guy")
            {
                animator.SetBool("isBadSophie", true);
            }
            else if (statSheet.name == "Bard Guy")
            {
                animator.SetBool("isBadKisa", true);
            }
            else if (statSheet.name == "Lich Guy")
            {
                animator.SetBool("isLich", true);
            }

            healthBar = new World_Bar(transform, new Vector3(0, -0.8f), new Vector3(1, 0.2f), Color.grey, Color.red, 1f, 100, new World_Bar.Outline { color = Color.black, size = 0.2f });
        }

        if (LIsPlayerTeam) 
        { 
            healthText.text = statSheet.stats["Health"].ToString() + "/" + statSheet.stats["MaxHealth"].ToString();
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

    #region Attacks, Hurt, and Animations
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
            am.playSFX(2);
            particle.animator.SetBool("SlashFX", true);
        }
        else
        {
            am.playSFX(3);
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

        animator.SetBool("Attacking", false);
    }

    public void magAttack(BattleCharacter targetCharacter, BattleCharacter attacker, Action onAttackComplete)
    {
        state = State.Attacking;
        attacker.animator.SetBool("MagAttacking", true);


        if (attacker.GIsPlayerTeam)
        {
            if (attacker.statSheet.stats["Mana"] > 0)
            {
                attacker.statSheet.stats["Mana"]--;
            }
            else
            {
                healthSystem.Damage(attacker.statSheet.stats["MaxHealth"] / 4);
            }
        }

        if (attacker.GIsPlayerTeam)
        {
            manaBar.SetSize((float)attacker.statSheet.stats["Mana"] / attacker.statSheet.stats["MaxMana"]);
        }

        if (attacker.statSheet.magicElement == "Bone")
        {
            LichAttack(targetCharacter);
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
            am.playSFX(4);
            particle.animator.SetBool("FireFX", true);
            yield return new WaitForSeconds(.6f);
        }
        else if (attacker.statSheet.magicElement == "Ice")
        {
            am.playSFX(5);
            particle.animator.SetBool("IceFX", true);
            yield return new WaitForSeconds(1.4f);
        }
        else if (attacker.statSheet.magicElement == "Wind")
        {
            am.playSFX(6);
            particle.animator.SetBool("WindFX", true);
            yield return new WaitForSeconds(.6f);
        }
        else if (attacker.statSheet.magicElement == "Electric")
        {
            am.playSFX(7);
            particle.animator.SetBool("ElectricFX", true);
            yield return new WaitForSeconds(.6f);
        }

        if (targetCharacter.statSheet.weakness == attacker.statSheet.magicElement)
        {
            targetCharacter.GotDamaged(attacker.statSheet.stats["Magic Attack"] * 2, 0 /*Placeholder because magic ignores defense*/);
        }
        else
        {
            targetCharacter.GotDamaged(attacker.statSheet.stats["Magic Attack"], 0 /*Placeholder because magic ignores defense*/ );
        }

        yield return new WaitForSeconds(.5f);

        state = State.Busy;

        onAttackComplete();
    }

    public void magAttackOver()
    {
        state = State.Busy;

        animator.SetBool("MagAttacking", false);
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
            if (GIsPlayerTeam)
            {
                am.playSFX(13);
            }

            Transform damagePopupTransform = Instantiate(damagePopup, transform.position, Quaternion.identity);
            DamagePopUp damPopScript = damagePopupTransform.GetComponent<DamagePopUp>();
            damPopScript.Setup(damageMinusDefense / 2);

            healthSystem.Damage(damageMinusDefense / 2);
            //Debug.Log("Defender Health: " + healthSystem.GetHealth());
        }
        else
        {
            if (GIsPlayerTeam)
            {
                am.playSFX(11);
            }

            Transform damagePopupTransform = Instantiate(damagePopup, transform.position, Quaternion.identity);
            DamagePopUp damPopScript = damagePopupTransform.GetComponent<DamagePopUp>();
            damPopScript.Setup(damageMinusDefense);

            healthSystem.Damage(damageMinusDefense);
            //Debug.Log("Defender Health: " + healthSystem.GetHealth());
        }

        statSheet.stats["Health"] = healthSystem.GetHealth();

        if (GIsPlayerTeam)
        {
            ChangeHealthText();
        }

        StartCoroutine(WaitUntilHurtOver(damageMinusDefense));

        //Debug.Log("Final Damage: " + damageMinusDefense);
    }

    private IEnumerator WaitUntilHurtOver(int damageMinusDefense)
    {
        while (animator.GetBool("Hurt") == true)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);

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

    public void LichAttack(BattleCharacter target)
    {
        Vector3 position = target.GetPosition();
        ParticleManager particle = Instantiate(particleManager, position, Quaternion.identity, target.transform);
        particle.animator.SetBool("LichFX", true);
    }

    #endregion

    public void ChangeHealthText()
    {
        statSheet.stats["Health"] = healthSystem.GetHealth();
        healthText.text = statSheet.stats["Health"].ToString() + "/" + statSheet.stats["MaxHealth"].ToString();
    }

    //Code for checking if an enemy is dead
    public bool IsDead()
    {
        return healthSystem.IsDead();
    }

    public void AllyFadeOut()
    {
        Transform position = fighterObject.transform;
        
        fighterObject.transform.DOMoveY(transform.position.y - 6, 0.7f);
    }

    public IEnumerator EnemyFadeOut()
    {
        for (float f = 1; f >= 0; f -= 0.05f)
        {
            Color c = charSprite.material.color;
            c.a = f;
            c.r = Random.Range(0, 1f);
            c.g = Random.Range(0, 1f);
            c.b = Random.Range(0, 1f);


            charSprite.color = c;

            yield return new WaitForSeconds(0.05f);
        }

        Destroy(fighterObject);
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
