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

    public GameObject fighterObject;

    public CharacterData statSheet;
    public bool isBlocking;
    public bool AlanGuarding;


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
    [SerializeField] private Transform weakPopup;

    private GameObject healthParent;
    private Transform healthObject;
    public TextMeshProUGUI healthText;

    public bool specialAvailable;

    //TempBuffs
    public bool tempBuffed = false;
    public int tempIncrease = 0;
    public Dictionary<string, bool> statsBuffed = new Dictionary<string, bool>() {
        {"Strength", false},
        {"Magic Attack", false},
        {"Defense", false},
        {"Speed", false},
        {"Health", false},
        {"MaxHealth", false},
        {"Mana", false},
        {"MaxMana", false},

    }; //dictionary showing what stats are buffed

    public Dictionary<string, int> valueOfBuff = new Dictionary<string, int>() {
        {"Strength", 0},
        {"Magic Attack", 0},
        {"Defense", 0},
        {"Speed", 0},
        {"Health", 0},
        {"MaxHealth", 0},
        {"Mana", 0},
        {"MaxMana", 0},

    }; //dictionary showing the value of each stat buff

    public int OvertimeHealTurnsLeft = 0;

    public int focusedTurnsLeft = 0;

    //TempDebuffs
    public bool Confused;
    public Dictionary<string, bool> statsDebuffed = new Dictionary<string, bool>() {
        {"Strength", false},
        {"Magic Attack", false},
        {"Defense", false},
        {"Speed", false},
        {"Health", false},
        {"MaxHealth", false},
        {"Mana", false},
        {"MaxMana", false},

    }; //dictionary showing what stats are debuffed

    public Dictionary<string, int> valueOfDebuff = new Dictionary<string, int>() {
        {"Strength", 0},
        {"Magic Attack", 0},
        {"Defense", 0},
        {"Speed", 0},
        {"Health", 0},
        {"MaxHealth", 0},
        {"Mana", 0},
        {"MaxMana", 0},

    }; //dictionary showing the value of each stat debuff

    // for when the player uses hp as mana
    [SerializeField] private CanvasGroup redBG;
    private bool isTutorial;

    private enum State
    {
        Idle, 
        Sliding, 
        Busy,
        Attacking
    }

    private void Awake()
    {
        Confused = false;
        amGameObject = GameObject.Find("/-- AUDIO --");
        am = amGameObject.GetComponent<audioManager>();

        state = State.Idle;
        specialAvailable = true;
        selectionCircleObject = transform.Find("Outline").gameObject;
        targetingCircleObject = transform.Find("TargetCircle").gameObject;
        weaknessObject = transform.Find("Weakness").gameObject;
        AlanGuarding = false;
        
        HideWeaknessObject();

        HideSelectionCircle();
        HideTargetCircle();
        isTutorial = GameObject.FindGameObjectWithTag("Tutorial").activeInHierarchy;
    }

    private void Start()
    {
        //Debug.Log(GIsPlayerTeam + ": Strength:" + statSheet.stats["Strength"]);
    }

    public void TempIncreaseStats(string stat, int amount)
    {
        if (statsBuffed[stat] == false)
        {
            valueOfBuff[stat] = amount;

            statsBuffed[stat] = true;

            this.statSheet.stats[stat] += valueOfBuff[stat];

            tempBuffed = true;
        }
    }

    public void TempDecraseStats(string stat, int amount)
    {
        if (statsDebuffed[stat] == false)
        {
            //Debug.Log("Debuffing " + stat + " by " + amount);

            valueOfDebuff[stat] = amount;

            statsDebuffed[stat] = true;

            this.statSheet.stats[stat] -= amount;
        }
    }

    public void UndoTempBuff()
    {
        //this.statSheet.stats[statIncreased] -= tempIncrease;

        foreach (KeyValuePair<string, bool> stat in statsBuffed)
        {
            //Debug.Log("Stat: " + stat.Key + " and isBuffed: " + stat.Value);

            if (stat.Value == true)
            {
                //Debug.Log("Removing " + valueOfBuff[stat.Key] + " from " + stat.Key);
                this.statSheet.stats[stat.Key] -= valueOfBuff[stat.Key];
            }
        }

        tempBuffed = false;
    }

    //Sprite 
    public void Setup(bool LIsPlayerTeam, int stageLevel, bool RightMostFighter)
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

            if (RightMostFighter)
            {
                charSprite.sortingOrder = 3;
            }

            healthBar = new World_Bar(transform, new Vector3(0, 1.4f), new Vector3(1, 0.2f), new Color(58 / 255f, 63 / 255f, 94 / 255f), new Color(99 / 255f, 171 / 255f, 63 / 255f), healthSystem.GetHealthPercent(), 100, new World_Bar.Outline { color = new Color(1 / 255f, 3 / 255f, 16 / 255f), size = 0.15f });
            manaBar = new World_Bar(transform, new Vector3(0, 1.6f), new Vector3(1, 0.2f), new Color(58 / 255f, 63 / 255f, 94 / 255f), new Color(79 / 255f, 164 / 255f, 184 / 255f), (float)statSheet.stats["Mana"] / statSheet.stats["MaxMana"], 100, new World_Bar.Outline { color = new Color(1 / 255f, 3 / 255f, 16 / 255f), size = 0.15f });
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

            healthBar = new World_Bar(transform, new Vector3(0, -0.8f), new Vector3(1, 0.2f), new Color(58 / 255f, 63 / 255f, 94 / 255f), new Color(230 / 255f, 69 / 255f, 94 / 255f), 1f, 100, new World_Bar.Outline { color = new Color(1 / 255f, 3 / 255f, 16 / 255f), size = 0.2f });
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

    private BattleCharacter gAttacker = null;
    private BattleCharacter gTarget = null;
    private Vector3 gPosition;

    public void Attack(BattleCharacter targetCharacter, BattleCharacter attacker, Action onAttackComplete)
    {
        Vector3 slideCloseToTargetPosition = targetCharacter.GetPosition() + (GetPosition() - targetCharacter.GetPosition()).normalized * 3f;
        Vector3 startingPosition = GetPosition();

        if (attacker.GIsPlayerTeam == true)
        {
            state = State.Busy;
            Vector3 attackDir = (targetCharacter.GetPosition() - GetPosition()).normalized;

            attacker.animator.SetBool("Attacking", true);
            state = State.Attacking;

            Vector3 position = targetCharacter.GetPosition();

            gPosition = position;
            gAttacker = attacker;
            gTarget = targetCharacter;

            StartCoroutine(WaitUntilAttackOver(targetCharacter, attacker, startingPosition, onAttackComplete));
        }
        else
        {
            //Slide to target
            SlideToPosition(slideCloseToTargetPosition, () =>
            {
                //Arrived at target and face them
                state = State.Busy;
                Vector3 attackDir = (targetCharacter.GetPosition() - GetPosition()).normalized;

                attacker.animator.SetBool("Attacking", true);
                state = State.Attacking;

                Vector3 position = targetCharacter.GetPosition();

                gPosition = position;
                gAttacker = attacker;
                gTarget = targetCharacter;

                StartCoroutine(WaitUntilAttackOver(targetCharacter, attacker, startingPosition, onAttackComplete));
            });
        }
    }

    public void AttAnim()
    {
        animator.SetBool("Attacking", true);

        state = State.Attacking;
    }

    private IEnumerator WaitUntilAttackOver(BattleCharacter targetCharacter, BattleCharacter attacker, Vector3 startingPosition, Action onAttackComplete)
    {
        int critOrMiss = 5;

        while (attacker.state == State.Attacking)
        {
            yield return null;
        }

        //This clump of code is how to get the particle manager to do stuff
        //Vector3 position = targetCharacter.GetPosition();
        //ParticleManager particle = Instantiate(particleManager, position, Quaternion.identity, targetCharacter.transform);

        //This was previously commented out, added it back in to play the sfx
        //if (attacker.statSheet.name == "Tank Guy" || attacker.statSheet.name == "Wraith Guy" || attacker.statSheet.name == "Skeleton Guy" || attacker.statSheet.name == "Mage Guy")
        //{
        //    am.playSFX(2);
        //    //particle.animator.SetBool("SlashFX", true);
        //}
        //else
        //{
        //    am.playSFX(3);
        //    //particle.animator.SetBool("PunchFX", true);
        //}

        

        critOrMiss = Random.Range(1, 21);

        //If this is the tutorial, overwrite whatever the critOrMiss is
        if (isTutorial)
        {
            critOrMiss = 10;
        }

        //critOrMiss = 20;
        if (focusedTurnsLeft <= 0)
        {
            if (critOrMiss == 1) //Miss
            {
                Transform damagePopupTransform = Instantiate(damagePopup, targetCharacter.transform.position, Quaternion.identity);
                DamagePopUp damPopScript = damagePopupTransform.GetComponent<DamagePopUp>();
                damPopScript.SetupString("MISS");
                am.playSFX(35);
            }
            else if (critOrMiss > 1 && critOrMiss < 20) //Regular Hit
            {
                targetCharacter.GotDamaged(attacker.statSheet.stats["Strength"], targetCharacter.statSheet.stats["Defense"]);
            }
            else if (critOrMiss == 20) //Critical Hit
            {
                int critDamage = attacker.statSheet.stats["Strength"] - targetCharacter.statSheet.stats["Defense"];

                critDamage = critDamage * 2;

                Vector3 critPosition = targetCharacter.transform.position;

                critPosition.y += 0.5f;

                Transform damagePopupTransform = Instantiate(damagePopup, critPosition, Quaternion.identity);
                DamagePopUp damPopScript = damagePopupTransform.GetComponent<DamagePopUp>();
                damPopScript.SetupString("CRITICAL HIT!");
                am.playSFX(34);

                targetCharacter.GotDamaged(critDamage, 0 /*No defense becausse defense has already been deducted*/);
            }
        }
        else //focusing
        {
            if (critOrMiss < 18)
            {
                targetCharacter.GotDamaged(attacker.statSheet.stats["Strength"], targetCharacter.statSheet.stats["Defense"]);
            }
            else
            {
                int critDamage = attacker.statSheet.stats["Strength"] - targetCharacter.statSheet.stats["Defense"];

                critDamage = critDamage * 2;

                Vector3 critPosition = targetCharacter.transform.position;

                critPosition.y += 0.5f;

                Transform damagePopupTransform = Instantiate(damagePopup, critPosition, Quaternion.identity);
                DamagePopUp damPopScript = damagePopupTransform.GetComponent<DamagePopUp>();
                damPopScript.SetupString("CRITICAL HIT!");
                am.playSFX(34);

                targetCharacter.GotDamaged(critDamage, 0 /*No defense becausse defense has already been deducted*/);
            }
        }

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

    // FOR ATTACKING ANIMATIONS!!
    public void SetGPosition(Vector3 inputPos)
    {
        gPosition = inputPos;
    }

    public void SetGTarget(BattleCharacter inputChar)
    {
        gTarget = inputChar;
    }

    public void slashHit()
    {
        am.playSFX(2);
        //Errors out here
        ParticleManager particle = Instantiate(particleManager, gPosition, Quaternion.identity, gTarget.transform);
        particle.animator.SetBool("SlashFX", true);
        //state = State.Busy;
        //animator.SetBool("Attacking", false);
    }

    public void punchHit()
    {

        am.playSFX(3);
        ParticleManager particle = Instantiate(particleManager, gPosition, Quaternion.identity, gTarget.transform);
        particle.animator.SetBool("PunchFX", true);
        //state = State.Busy;
        //animator.SetBool("Attacking", false);
    }

    //

    public void magAttack(BattleCharacter targetCharacter, BattleCharacter attacker, Action onAttackComplete)
    {
        state = State.Attacking;
        attacker.animator.SetBool("MagAttacking", true);


        if (attacker.GIsPlayerTeam)
        {
            if (attacker.statSheet.stats["Mana"] > 1)
            {
                attacker.statSheet.stats["Mana"] -= 2;
            }
            else if (attacker.statSheet.stats["Mana"] == 1)
            {
                attacker.statSheet.stats["Mana"] -= 1;
                redBG = GameObject.FindGameObjectWithTag("RedBG").GetComponent<CanvasGroup>();
                redBG.DOFade(0.5f, 0.5f).OnComplete(() => {
                    redBG.DOFade(0.5f, 0.5f).OnComplete(() => {
                        redBG.DOFade(0, 0.5f);
                    });
                });
                healthSystem.Damage(attacker.statSheet.stats["MaxHealth"] / 8);
                ChangeHealthText();
            }
            else
            {
                redBG = GameObject.FindGameObjectWithTag("RedBG").GetComponent<CanvasGroup>();
                redBG.DOFade(1f, 0.5f).OnComplete(() => {
                    redBG.DOFade(1f, 0.5f).OnComplete(() => {
                        redBG.DOFade(0, 0.5f);
                    });
                });
                healthSystem.Damage(attacker.statSheet.stats["MaxHealth"] / 4);
                ChangeHealthText();
            }
        }

        if (attacker.statSheet.stats["Mana"] < 0)
        {
            attacker.statSheet.stats["Mana"] = 0;
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

    public void ChangeManaBar(float fillPercent)
    {
        manaBar.SetSize(fillPercent);
    }

    private IEnumerator WaitUntilMagAttackOver(BattleCharacter targetCharacter, BattleCharacter attacker, Action onAttackComplete)
    {
        while (attacker.state == State.Attacking)
        {
            yield return null;
        }

        int critOrMiss = 5;

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

        critOrMiss = Random.Range(1, 21);

        if (isTutorial)
        {
            critOrMiss = 10;
        }

        //critOrMiss = 4;

        if (critOrMiss <= 3) //Miss
        {
            Transform damagePopupTransform = Instantiate(damagePopup, targetCharacter.transform.position, Quaternion.identity);
            DamagePopUp damPopScript = damagePopupTransform.GetComponent<DamagePopUp>();
            damPopScript.SetupString("MISS");
        }
        else if (critOrMiss > 3)
        {

            if (targetCharacter.statSheet.weakness == attacker.statSheet.magicElement)
            {
                targetCharacter.GotDamaged(attacker.statSheet.stats["Magic Attack"] * 2, 0 /*Placeholder because magic ignores defense*/);
                Vector3 weaknessPosition = targetCharacter.GetPosition();
                weaknessPosition.x -= 1.5f;
                Transform weakPopUpTransform = Instantiate(weakPopup, weaknessPosition, Quaternion.identity);
            }
            else
            {
                targetCharacter.GotDamaged(attacker.statSheet.stats["Magic Attack"], 0 /*Placeholder because magic ignores defense*/ );
            }
        }

        yield return new WaitForSeconds(.9f);

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

            damageMinusDefense -= damageMinusDefense / 2;

            //Damage PopUp
            Transform damagePopupTransform = Instantiate(damagePopup, transform.position, Quaternion.identity);
            DamagePopUp damPopScript = damagePopupTransform.GetComponent<DamagePopUp>();
            damPopScript.SetupInt(damageMinusDefense);

            //Debug.Log("Defender Health: " + healthSystem.GetHealth());
        }
        else
        {
            if (GIsPlayerTeam)
            {
                am.playSFX(11);
            }

            //Damage PopUp
            Transform damagePopupTransform = Instantiate(damagePopup, transform.position, Quaternion.identity);
            DamagePopUp damPopScript = damagePopupTransform.GetComponent<DamagePopUp>();
            damPopScript.SetupInt(damageMinusDefense);

            //Debug.Log("Defender Health: " + healthSystem.GetHealth());
        }

        if (AlanGuarding)
        {
            damageMinusDefense -= damageMinusDefense / 4;
        }

        TrueDamage(damageMinusDefense);

        StartCoroutine(WaitUntilHurtOver(damageMinusDefense));

        //Debug.Log("Final Damage: " + damageMinusDefense);
    }

    public void TrueDamage(int damage) //Damage function
    {
        Transform damagePopupTransform = Instantiate(damagePopup, transform.position, Quaternion.identity);
        DamagePopUp damPopScript = damagePopupTransform.GetComponent<DamagePopUp>();
        damPopScript.SetupInt(damage);

        healthSystem.Damage(damage);

        statSheet.stats["Health"] = healthSystem.GetHealth();

        if (GIsPlayerTeam)
        {
            ChangeHealthText();
        }
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

    #region EnemyFade

    static public bool FadeOneRunning;
    static public bool FadeTwoRunning;


    public IEnumerator EnemyFadeOut()
    {
        FadeOneRunning = true;

        for (float f = 1; f >= 0; f -= 0.05f)
        {
            if (charSprite == null)
            {
                continue;
            }

            Color c = charSprite.material.color;
            c.a = f;
            c.r = Random.Range(0, 1f);
            c.g = Random.Range(0, 1f);
            c.b = Random.Range(0, 1f);


            charSprite.color = c;

            yield return new WaitForSeconds(0.05f);
        }

        FadeOneRunning = false;

        Destroy(fighterObject);
    }

    public IEnumerator SecondEnemyFadeOut()
    {
        FadeTwoRunning = true;

        for (float f = 1; f >= 0; f -= 0.05f)
        {
            if (charSprite == null)
            {
                continue;
            }

            Color c = charSprite.material.color;
            c.a = f;
            c.r = Random.Range(0, 1f);
            c.g = Random.Range(0, 1f);
            c.b = Random.Range(0, 1f);


            charSprite.color = c;

            yield return new WaitForSeconds(0.05f);
        }

        FadeTwoRunning = false;

        Destroy(fighterObject);
    }

    public IEnumerator ThirdEnemyFadeOut()
    {
        for (float f = 1; f >= 0; f -= 0.05f)
        {
            if (charSprite == null)
            {
                continue;
            }

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

    #endregion

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
