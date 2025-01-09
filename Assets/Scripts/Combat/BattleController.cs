using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System.Linq;
using UnityEditor;

public class BattleController : MonoBehaviour
{
    public bool hasContemplatedKilling = false;

    public void TestButton()
    {
        Debug.Log("Beans");
    }

    //for displaying the you win menu
    [SerializeField] private GameObject youWin;
    public static BattleController GetInstance()
    {
        return instance;
    }

    public LevelManager levelManager;

    BattleCharacter SpawningEnemy(/*BattleCharacter enemySpawning*/)
    {
        int randomiser = 1;

        if (playerController.Level == 1)
        {
            randomiser = 1;
        }
        else if (playerController.Level == 2)
        {
            randomiser = Random.Range(1, 3);
        }
        else if (playerController.Level == 3)
        {
            randomiser = Random.Range(1, 4);
        }
        else if (playerController.Level == 4)
        {
            randomiser = Random.Range(1, 5);
        }

        BattleCharacter enemySpawning;

        if (randomiser == 1)
        {
            enemySpawning = SpawnCharacter(false, slimeStats, "Slime Guy", 0 /*0 Because enemies don't have specials*/, 4, 0);

            return enemySpawning;
        }
        else if(randomiser == 2)
        {
            enemySpawning = SpawnCharacter(false, skeletonStats, "Skeleton Guy", 0, 4, 3);

            return enemySpawning;
        }
        else if (randomiser == 3)
        {
            enemySpawning = SpawnCharacter(false, ghostStats, "Ghost Guy", 0, 4, 1);

            return enemySpawning;
        }
        else
        {
            enemySpawning = SpawnCharacter(false, wraithStats, "Wraith Guy", 0, 4, 2);

            return enemySpawning;
        }
    }

    void SetStats()
    {
        tankStats = levelManager.SetTankStats();

        monkStats = levelManager.SetMonkStats();
        bardStats = levelManager.SetBardStats();
        mageStats = levelManager.SetMageStats();

        slimeStats = levelManager.SetSlimeStats(slimeStats);
        ghostStats = levelManager.SetGhostStats(ghostStats);
        skeletonStats = levelManager.SetSkeletonStats(skeletonStats);
        wraithStats = levelManager.SetWraithStats(wraithStats);
    }

    private void OnEnable()
    {
        nicolBuffed = false;
        amountBuffed = 0;

        manaObject.SetActive(false);

        if (playerController.hasKisa)
        {
            kisaHealthText.SetActive(true);
        }
        else 
        {
            kisaHealthText.SetActive(false);
        }

        if (playerController.hasSophie)
        {
            sophieHealthText.SetActive(true);
        }
        else
        {
            sophieHealthText.SetActive(false);
        }

        if (playerController.hasNicol)
        {
            nicolHealthText.SetActive(true);
        }
        else
        {
            nicolHealthText.SetActive(false);
        }

        ResetStats(true, false);

        levelManager.gainedEXP = 0;

        partyBoss = false;

        BattleCharacter.FadeOneRunning = false;
        BattleCharacter.FadeTwoRunning = false;

        SetStats();

        befriendOrAbsorbButton.SetActive(false);

        

        instance = this;

        //int howManyToSpawn = Random.Range(1, 5);

        //True for an ally, false for an enemy

        tankChar = SpawnCharacter(true, tankStats, "Tank Guy", 1, 0, 0);

        if (playerController.hasNicol && levelManager.GetCharHealth("Mage Guy") != 0)
        {
            mageChar = SpawnCharacter(true, mageStats, "Mage Guy", 2, 1, 2);
        }

        if (playerController.hasKisa && levelManager.GetCharHealth("Bard Guy") != 0)
        {
            bardChar = SpawnCharacter(true, bardStats, "Bard Guy", 3, 3, 0);
        }

        if (playerController.hasSophie && levelManager.GetCharHealth("Monk Guy") != 0)
        {
            monkChar = SpawnCharacter(true, monkStats, "Monk Guy", 4, 2, 3);
        }


        //First Enemy Spawn
        if (playerController.isSlime)
        {
            firstEnemy = SpawnCharacter(false, slimeStats, "Slime Guy", 0 /*0 Because enemies don't have specials*/, 4, 0);
        }
        else if (playerController.isSkeleton) 
        { 
            firstEnemy = SpawnCharacter(false, skeletonStats, "Skeleton Guy", 0, 4, 3);
        }
        else if (playerController.isWraith)
        {
            firstEnemy = SpawnCharacter(false, wraithStats, "Wraith Guy", 0, 4, 2);
        }
        else if (playerController.isInvisGuy)
        {
            firstEnemy = SpawnCharacter(false, ghostStats, "Ghost Guy", 0, 4, 1);
        }
        else if (playerController.KisaBoss)
        {
            firstEnemy = SpawnCharacter(false, evilBardStats, "Bard Guy", 3, 3, 0);
            partyBoss = true;
            //howManyToSpawn = 0;
        }
        else if (playerController.NicolBoss)
        {
            //Scale down Nicol
            if (playerController.KisaAbsorbed == 1)
            {
                evilMageStats = evilMageStats.Select(x => (int)(x * 0.8f)).ToArray();
            }
            firstEnemy = SpawnCharacter(false, evilMageStats, "Mage Guy", 2, 1, 2);
            partyBoss = true;
            //howManyToSpawn = 0;
        }
        else if (playerController.SophieBoss)
        {
            //Scale down Sophie
            if (playerController.KisaAbsorbed == 1) //Makes it easier if u dont have Kisa, either in genocide or because you only have Nicol
            {
                evilMonkStats = evilMonkStats.Select(x => (int)(x * 0.8f)).ToArray();
            }
            firstEnemy = SpawnCharacter(false, evilMonkStats, "Monk Guy", 4, 2, 3);
            partyBoss = true;
            //howManyToSpawn = 0;
        }
        else if (playerController.LichBoss)
        {
            firstEnemy = SpawnCharacter(false, lichStats, "Lich Guy", 0, 5, 4);
        }


        //

        // AUDIO MANAGEMENT

        //

        //If we aren't fighting the lich, play the normal music.
        if (!playerController.LichBoss)
        {
            am.playBGM("T3");
        }
        else // If this is the lich, play Lich music
        {
            am.playBGM("T7");
        }


        //Previous Code
        //if (partyBoss == false)
        //{
        //    am.playBGM("T3");
        //}
        //else if (partyBoss == true)
        //{
        //    //for right now im making it the normal music cause idk if i like this music w/ boss fights
        //    //am.playBGM("T6");
        //    am.playBGM("T3");
        //}


        if (playerController.Level >= 2 && partyBoss == false)
        {
            secondEnemy = SpawningEnemy();
        }

        if (playerController.Level >= 3 && partyBoss == false)
        {
            thirdEnemy = SpawningEnemy();
        }

        if (playerController.Level >= 4 && partyBoss == false)
        {
            FourthEnemy = SpawningEnemy();
        }

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


        backButton.SetActive(false);

        nicolIceMagicButton.SetActive(false);
        sophieElectricMagicButton.SetActive(false);
        kisaWindMagicButton.SetActive(false);
        alanFireMagicButton.SetActive(false);

        //Set the first button to be the fight button
        if(tutorialHandler.activeInHierarchy == false)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(attackButtonOBJ);
        }
    }

    #region Variables

    [SerializeField] private Transform playerCharacterTransform;

    private static BattleController instance;

    public bool fightingMage;

    //Players
    private BattleCharacter tankChar;
    private BattleCharacter mageChar;
    private BattleCharacter monkChar;
    private BattleCharacter bardChar;

    //Enemies
    private BattleCharacter firstEnemy;
    private BattleCharacter secondEnemy;
    private BattleCharacter thirdEnemy;
    private BattleCharacter FourthEnemy;

    private BattleCharacter activeChar;

    private Queue<BattleCharacter> characterQueue = new Queue<BattleCharacter>();
    private Queue<BattleCharacter> alreadyWent = new Queue<BattleCharacter>();
    [SerializeField] private updateSPOnScreen updateSP;

    #region Stats

    //WHEN CHANGING STATS CHANGE THEM IN LEVEL MANAGER AND LEVELUPUI AS WELL


    //Tank Stats
    static public int[] tankStats = {
        /*Strength*/ 10,
        /*Magic Attack*/ 5,
        /*Defense*/ 5, 
        /*Speed*/ 3, 
        /*Health*/ 18, 
        /*MaxHealth*/ 18,
        /*Mana*/ 4,
        /*MaxMana*/ 4};

    //Mage Stats
    static public int[] mageStats = {
        /*Strength*/ 7,
        /*Magic Attack*/ 8,
        /*Defense*/ 8, 
        /*Speed*/ 5, 
        /*Health*/ 11, 
        /*MaxHealth*/ 11,
        /*Mana*/ 9,
        /*MaxMana*/ 9};

    static public int[] evilMageStats = {
        /*Strength*/ 30,
        /*Magic Attack*/ 15,
        /*Defense*/ 8, 
        /*Speed*/ 3, 
        /*Health*/ 150, 
        /*MaxHealth*/ 150,
        /*Mana*/ 9,
        /*MaxMana*/ 9};

    //Monk Stats
    static public int[] monkStats = {
        /*Strength*/ 13,
        /*Magic Attack*/ 7,
        /*Defense*/ 4, 
        /*Speed*/ 8, 
        /*Health*/ 13, 
        /*MaxHealth*/ 13,
        /*Mana*/ 5,
        /*MaxMana*/ 5};

    static public int[] evilMonkStats = {
        /*Strength*/ 35,
        /*Magic Attack*/ 18,
        /*Defense*/ 10, 
        /*Speed*/ 3, 
        /*Health*/ 300, 
        /*MaxHealth*/ 300,
        /*Mana*/ 6,
        /*MaxMana*/ 6};

    //Bard Stats
    static public int[] bardStats = {
        /*Strength*/ 6,
        /*Magic Attack*/ 6,
        /*Defense*/ 4, 
        /*Speed*/ 4, 
        /*Health*/ 15, 
        /*MaxHealth*/ 15,
        /*Mana*/ 7,
        /*MaxMana*/ 7};

    static public int[] evilBardStats = {
        /*Strength*/ 10,
        /*Magic Attack*/ 7,
        /*Defense*/ 5, 
        /*Speed*/ 2, 
        /*Health*/ 45, 
        /*MaxHealth*/ 45,
        /*Mana*/ 7,
        /*MaxMana*/ 7};

    //Slime Stats
    static public int[] slimeStats = {
        /*Strength*/ 14,
        /*Magic Attack*/ 1,
        /*Defense*/ 3, 
        /*Speed*/ 2, 
        /*Health*/ 15, 
        /*MaxHealth*/ 15,
        /*Mana*/ 6,
        /*MaxMana*/ 7};

    //Skeleton Stats
    static public int[] skeletonStats = {
        /*Strength*/ 10,
        /*Magic Attack*/ 1,
        /*Defense*/ 4, 
        /*Speed*/ 3, 
        /*Health*/ 25, 
        /*MaxHealth*/ 25,
        /*Mana*/ 6,
        /*MaxMana*/ 7};

    static public int[] wraithStats = {
        /*Strength*/ 10,
        /*Magic Attack*/ 1,
        /*Defense*/ 5, 
        /*Speed*/ 5, 
        /*Health*/ 17, 
        /*MaxHealth*/ 17,
        /*Mana*/ 6,
        /*MaxMana*/ 7};

    static public int[] ghostStats = {
        /*Strength*/ 11,
        /*Magic Attack*/ 1,
        /*Defense*/ 30, 
        /*Speed*/ 4, 
        /*Health*/ 7, 
        /*MaxHealth*/ 7,
        /*Mana*/ 6,
        /*MaxMana*/ 7};

    //Katie note: I jacked the lich up like crazy! Have fun!

    static public int[] lichStats = {
        /*Strength*/ 55,
        /*Magic Attack*/ 20,
        /*Defense*/ 10, 
        /*Speed*/ 4, 
        /*Health*/ 500, 
        /*MaxHealth*/ 500,
        /*Mana*/ 30,
        /*MaxMana*/ 30};

    void ResetStats(bool resetEnemy, bool resetPlayer)
    {
        #region local Stats

        //Tank Stats
        int[] lTankStats = {
        /*Strength*/ 10,
        /*Magic Attack*/ 5,
        /*Defense*/ 5, 
        /*Speed*/ 3, 
        /*Health*/ 18, 
        /*MaxHealth*/ 18,
        /*Mana*/ 4,
        /*MaxMana*/ 4};

        //Mage Stats
        int[] lMageStats = {
        /*Strength*/ 7,
        /*Magic Attack*/ 8,
        /*Defense*/ 8, 
        /*Speed*/ 5, 
        /*Health*/ 11, 
        /*MaxHealth*/ 11,
        /*Mana*/ 9,
        /*MaxMana*/ 9};

        //Monk Stats
        int[] lMonkStats = {
        /*Strength*/ 13,
        /*Magic Attack*/ 7,
        /*Defense*/ 4, 
        /*Speed*/ 8, 
        /*Health*/ 13, 
        /*MaxHealth*/ 13,
        /*Mana*/ 5,
        /*MaxMana*/ 5};

        //Bard Stats
        int[] lBardStats = {
        /*Strength*/ 6,
        /*Magic Attack*/ 6,
        /*Defense*/ 4, 
        /*Speed*/ 4, 
        /*Health*/ 15, 
        /*MaxHealth*/ 15,
        /*Mana*/ 7,
        /*MaxMana*/ 7};

        //Slime Stats
        int[] lSlimeStats = {
        /*Strength*/ 7,
        /*Magic Attack*/ 1,
        /*Defense*/ 3, 
        /*Speed*/ 2, 
        /*Health*/ 17, 
        /*MaxHealth*/ 17,
        /*Mana*/ 6,
        /*MaxMana*/ 7};

        //Skeleton Stats
        int[] lSkeletonStats = {
        /*Strength*/ 7,
        /*Magic Attack*/ 1,
        /*Defense*/ 4, 
        /*Speed*/ 3, 
        /*Health*/ 10, 
        /*MaxHealth*/ 10,
        /*Mana*/ 6,
        /*MaxMana*/ 7};

        int[] lWraithStats = {
        /*Strength*/ 10,
        /*Magic Attack*/ 1,
        /*Defense*/ 5, 
        /*Speed*/ 5, 
        /*Health*/ 17, 
        /*MaxHealth*/ 17,
        /*Mana*/ 6,
        /*MaxMana*/ 7};

        int[] lGhostStats = {
        /*Strength*/ 9,
        /*Magic Attack*/ 1,
        /*Defense*/ 99, 
        /*Speed*/ 4, 
        /*Health*/ 7, 
        /*MaxHealth*/ 7,
        /*Mana*/ 6,
        /*MaxMana*/ 7};

        #endregion

        if (resetPlayer)
        {
            tankStats = lTankStats;
            mageStats = lMageStats;
            monkStats = lMonkStats;
            bardStats = lBardStats;
        }

        if (resetEnemy)
        {
            slimeStats = lSlimeStats;
            ghostStats = lGhostStats;
            wraithStats = lWraithStats;
            skeletonStats = lSkeletonStats;
        }
    }

   

    #endregion

    //Magic Types
    public Dictionary<int ,string> magicTypes = new Dictionary<int, string>()
    {
        {0 , "Fire"},
        {1 , "Ice"},
        {2 , "Electric"},
        {3 , "Wind"},
        {4 , "No Magic"},
        {5 , "Bone" }
    };

    public Sprite fire;
    public Sprite ice;
    public Sprite wind;
    public Sprite electric;

    private List<BattleCharacter> playerList = new List<BattleCharacter>();

    public List<BattleCharacter> partyMembers = new List<BattleCharacter>();

    private List<BattleCharacter> enemyList = new List<BattleCharacter>();

    public State state;
    
    //UI

    public GameObject fightingButtons;
    public GameObject backButton;

    public Camera mainCamera;

    public GameObject fighterObject;

    public GameObject battleObject;

    public PlayerController playerController;

    public GameObject befriendOrAbsorbButton;

    public GameObject nicolIceMagicButton;
    public GameObject sophieElectricMagicButton;
    public GameObject kisaWindMagicButton;
    public GameObject alanFireMagicButton;

    public GameObject attackButtonOBJ, defendButtonOBJ, specialButtonOBJ, magicButtonOBJ;

    private bool nicolBuffed;
    private int amountBuffed;

    public GameObject tutorialHandler;
    [SerializeField] TutorialHandler tutH;
    public bool coroutineRunning;

    //[SerializeField] private GameObject attackButtonBlocker;

    private bool partyBoss;

    public Animator battleFadeAnim;

    public audioManager am;

    [SerializeField] private UnityEngine.UI.Button specialObject;
    [SerializeField] private UnityEngine.UI.Image specialSprite;
    private SpriteState specialSpriteState;
    public Sprite specReady;
    public Sprite specReady2;
    public Sprite specNotReady;

    [SerializeField] private GameObject kisaHealthText;
    [SerializeField] private GameObject nicolHealthText;
    [SerializeField] private GameObject sophieHealthText;

    [SerializeField] private GameObject manaObject;
    [SerializeField] private UnityEngine.UI.Slider manaSlider;
    [SerializeField] private TextMeshProUGUI manaText;


    [SerializeField] private GameObject turnOffForKillBefriend;

    [SerializeField] private battle_specialMenu battle_specialmenu;

    public float timeBetweenSelectAndConfirm;

    [SerializeField] private GameObject tenacityPosition;

    //Keys

    public KeyCode attackKey = KeyCode.W;
    public KeyCode blockKey = KeyCode.S; 
    public KeyCode specialKey = KeyCode.A;
    public KeyCode magicKey = KeyCode.D;
    public KeyCode backKey = KeyCode.Backspace;
    #endregion

    public GameObject particleManager;

    public enum State
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
        if (playerController.BattleTutorialCleared != 1)
        {
            ResetStats(true, true);
        }
        specialSpriteState = specialObject.spriteState;
        coroutineRunning = false;
    }

    private void Update()
    {
        //Command for killing all but one enemy in combat
        /*if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            for (int i = 0; i < enemyList.Count - 1; i++)
            {
                enemyList[i].GotDamaged(99999, 0);
            }
        }*/

        if (state == State.WaitingForPlayer)
        {
            fightingButtons.SetActive(true);

            if (activeChar.specialAvailable) // this now changes the highlighted color too
            {
                specialSprite.sprite = specReady;
                specialSpriteState.highlightedSprite = specReady2;
                specialSpriteState.selectedSprite = specReady2;
            }
            else
            {
                specialSprite.sprite = specNotReady;
                specialSpriteState.highlightedSprite = specNotReady;
                specialSpriteState.selectedSprite = specNotReady;
            }
            specialObject.spriteState = specialSpriteState;
        }
        else
        {
            fightingButtons.SetActive(false);
        }  

        //if (alanFireMagicButton.activeInHierarchy)
        //{
        //    attackButtonBlocker.SetActive(false);
        //}
        //else
        //{
        //    attackButtonBlocker.SetActive(true);
        //}

         //Testing how to instantiate the particle effects
        if (Input.GetKeyDown(KeyCode.G))
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                Vector3 tauntedPosition = enemyList[i].GetPosition();
                ParticleManager tauntedParticle = Instantiate(enemyList[i].particleManager, tauntedPosition, Quaternion.identity, enemyList[i].transform);
                tauntedParticle.animator.SetBool("EnemyTauntedFX", true);
            }
        }

        
        //Keybinds
        /*if (fightingButtons.activeInHierarchy == true)
        {
            if (Input.GetKeyDown(attackKey))
            {
                attackButton();
            }

            if (Input.GetKeyDown(blockKey))
            {
                defendButton();
            }

            if (Input.GetKeyDown(specialKey))
            {
                specialButton();
            }

            if (Input.GetKeyDown(magicKey))
            {
                magicButton();
            }
        }

        if (backButton.activeInHierarchy == true)
        {
            if (Input.GetKeyDown(backKey))
            {
                BackButton();
            }
        }*/
    }

    #region Buttons

    public void BeanButton()
    {
        Debug.Log("beans");
    }

    public void BackButton()
    {
        StopAllCoroutines();

        coroutineRunning = false;

        manaObject.SetActive(false);
        battle_specialmenu.gameObject.SetActive(false);

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
        backButton.SetActive(false);

        nicolIceMagicButton.SetActive(false);
        sophieElectricMagicButton.SetActive(false);
        kisaWindMagicButton.SetActive(false);
        alanFireMagicButton.SetActive(false);

        //Set the first button to be the fight button
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(attackButtonOBJ);
    }

    public void attackButton()
    {
        state = State.Busy;

        /*activeChar.Attack(secEnemyChar, activeChar, () =>
        {
            ChooseNextActiveChar();
        });*/

        backButton.SetActive(true);

        

        StartCoroutine(AttackTargeting()); 
    }

    public void defendButton()
    {
        state = State.Busy;

        backButton.SetActive(true);

        StartCoroutine(BlockConfirm());
    }

    public void magicButton()
    {
        /*activeChar.magAttack(enemyChar, activeChar, () =>
        {
            ChooseNextActiveChar();
        });*/

        manaSlider.value = (float)((float)activeChar.statSheet.stats["Mana"] / activeChar.statSheet.stats["MaxMana"]);
        manaText.text = activeChar.statSheet.stats["Mana"] + "/" + activeChar.statSheet.stats["MaxMana"];


        manaObject.SetActive(true);

        if (activeChar.statSheet.name == "Tank Guy") //&& levelManager.kisaAbsorb || activeChar.statSheet.name == "Tank Guy" && levelManager.nicolAbsorb || activeChar.statSheet.name == "Tank Guy" && levelManager.sophieAbsorb)
        {
            if (tutH.tutorialCounter == 8)
            {
                tutH.continueTutorial();
            }
            else
            {
                backButton.SetActive(true);
            }

            state = State.Busy;

            alanFireMagicButton.SetActive(true);

            if (levelManager.nicolAbsorb == true)
            {
                nicolIceMagicButton.SetActive(true);
            }

            if (levelManager.sophieAbsorb == true)
            {
                sophieElectricMagicButton.SetActive(true);
            }

            if (levelManager.kisaAbsorb == true)
            {
                kisaWindMagicButton.SetActive(true);
            }
            //Set the first button to be the fire button if this is alan button
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(alanFireMagicButton);
        }

        else
        {
            state = State.Busy;

            backButton.SetActive(true);

            StartCoroutine(MagicTargeting());
        }
        

    }

    #region Magic Buttons

    //0 fire
    //1 ice
    //2 elec
    //3 wind

    public void alanMagic()
    {
        activeChar.statSheet.magicElement = magicTypes[0];

        alanFireMagicButton.SetActive(false);
        nicolIceMagicButton.SetActive(false);
        kisaWindMagicButton.SetActive(false);
        sophieElectricMagicButton.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(magicButtonOBJ);

        StartCoroutine(MagicTargeting());
    }

    public void nicolMagic()
    {
        activeChar.statSheet.magicElement = magicTypes[1];

        alanFireMagicButton.SetActive(false);
        nicolIceMagicButton.SetActive(false);
        kisaWindMagicButton.SetActive(false);
        sophieElectricMagicButton.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(magicButtonOBJ);

        StartCoroutine(MagicTargeting());
    }

    public void sophieMagic()
    {
        activeChar.statSheet.magicElement = magicTypes[2];

        alanFireMagicButton.SetActive(false);
        nicolIceMagicButton.SetActive(false);
        kisaWindMagicButton.SetActive(false);
        sophieElectricMagicButton.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(magicButtonOBJ);

        StartCoroutine(MagicTargeting());
    }

    public void kisaMagic()
    {
        activeChar.statSheet.magicElement = magicTypes[3];

        alanFireMagicButton.SetActive(false);
        nicolIceMagicButton.SetActive(false);
        kisaWindMagicButton.SetActive(false);
        sophieElectricMagicButton.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(magicButtonOBJ);

        StartCoroutine(MagicTargeting());
    }

    #endregion

    public void SpecialButton()
    {
        /*activeChar.specialMove(enemyChar, activeChar, () =>
        {
            ChooseNextActiveChar();
        });*/


        if (activeChar.specialAvailable == true)
        {
            // ADDED CODE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! CHANGE AS NEEDED 
            battle_specialmenu.setWhoAreWeViewing(activeChar.statSheet.name.ToLower());
            battle_specialmenu.gameObject.SetActive(true);
            //Set the slider and text of the special menu
            updateSP.setSliderVal(activeChar.statSheet.name.ToLower());



            //// End of added code
            backButton.SetActive(true);
            state = State.Busy;

            //StartCoroutine(SpecialTargeting());
            am.playSFX(26);
        } else
        {
            am.playSFX(29);
        }
    }

    public void Die()
    {
        SceneManager.LoadScene("GameOver");
    }

    #endregion

    public IEnumerator WaitBeforeChoosingNext(float timeToWait)
    {
        coroutineRunning = true;

        yield return new WaitForSeconds(timeToWait);

        ChooseNextActiveChar();

        coroutineRunning = false;
    }

    #region Targeting Coroutines


    //When these coroutines are called, the while loop while loop indefinitely until the enter or "return" key is pressed
    private IEnumerator AttackTargeting()
    {

        #region Targeting

        int enemyNum = 0;

        coroutineRunning = true;

        backButton.SetActive(true);

        enemyList[enemyNum].ShowTargetCircle();

        yield return new WaitForSeconds(timeBetweenSelectAndConfirm);

        while (!(Input.GetKeyDown(audioStatics.keycodeInterractButton) || Input.GetKeyDown(KeyCode.Return)))
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                //They do not want to select the back button. Unselect it.
                EventSystem.current.SetSelectedGameObject(null);
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
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                //They do not want to select the back button. Unselect it.
                EventSystem.current.SetSelectedGameObject(null);
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
            } else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                //They want to select the back button.
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(backButton);
                enemyList[enemyNum].HideTargetCircle();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                //They want to go back to selecting an enemy. Show Target Circle again.
                EventSystem.current.SetSelectedGameObject(null);
                enemyList[enemyNum].ShowTargetCircle();
            }

            yield return null;
        }

        #endregion


        activeChar.Attack(enemyList[enemyNum], activeChar, () =>
        {
            ChooseNextActiveChar();
        });

        enemyList[enemyNum].HideTargetCircle();

        backButton.SetActive(false);

        coroutineRunning = false;
    }

    private IEnumerator BlockConfirm()
    {
        coroutineRunning = true;

        backButton.SetActive(true);

        yield return new WaitForSeconds(timeBetweenSelectAndConfirm);

        while (!(Input.GetKeyDown(audioStatics.keycodeInterractButton) || Input.GetKeyDown(KeyCode.Return)))
        {
            yield return null;
        }

        activeChar.isBlocking = true;
        activeChar.healthSystem.Heal(5);
        activeChar.ChangeHealthText();

        Vector3 position = activeChar.GetPosition();
        ParticleManager healParticle = Instantiate(activeChar.particleManager, position, Quaternion.identity, activeChar.transform);
        healParticle.animator.SetBool("HealFX", true);

        activeChar.animator.SetBool("Blocking", true);
        ChooseNextActiveChar();

        backButton.SetActive(false);

        coroutineRunning = false;
    }

    private IEnumerator MagicTargeting()
    {
        coroutineRunning = true;

        int enemyNum = 0;

        enemyList[enemyNum].ShowTargetCircle();

        yield return new WaitForSeconds(timeBetweenSelectAndConfirm);

        while (!(Input.GetKeyDown(audioStatics.keycodeInterractButton) || Input.GetKeyDown(KeyCode.Return)))
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                //They do not want to select the back button. Unselect it.
                EventSystem.current.SetSelectedGameObject(null);
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
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                //They do not want to select the back button. Unselect it.
                EventSystem.current.SetSelectedGameObject(null);
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
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                //They want to select the back button.
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(backButton);
                enemyList[enemyNum].HideTargetCircle();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                //They want to go back to selecting an enemy. Show Target Circle again.
                EventSystem.current.SetSelectedGameObject(null);
                enemyList[enemyNum].ShowTargetCircle();
            }

            yield return null;
        }

        enemyList[enemyNum].HideTargetCircle();

        manaObject.SetActive(false);

        backButton.SetActive(false);

        activeChar.magAttack(enemyList[enemyNum], activeChar, () =>
        {
            ChooseNextActiveChar();
        });

        coroutineRunning = false;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(magicButtonOBJ);
    }

    private bool isTaunting = false;

    private IEnumerator SpecialTargeting()
    {
        coroutineRunning = true;

        int enemyNum = 0;

        backButton.SetActive(true);

        yield return new WaitForSeconds(timeBetweenSelectAndConfirm);

        //Tank
        if (activeChar.statSheet.specialMove == 1)
        {
            

            while (!(Input.GetKeyDown(audioStatics.keycodeInterractButton) || Input.GetKeyDown(KeyCode.Return)))
            {
                yield return null;
            }

            activeChar.animator.SetBool("MagAttacking", true);

            Vector3 position = activeChar.GetPosition();
            ParticleManager healParticle = Instantiate(activeChar.particleManager, position, Quaternion.identity, activeChar.transform);
            healParticle.animator.SetBool("HealFX", true);

            am.playSFX(12);

            am.playSFX(8);


            ParticleManager tauntParticle = Instantiate(activeChar.particleManager, position, Quaternion.identity, activeChar.transform);
            tauntParticle.animator.SetBool("TauntFX", true);

            isTaunting = true;

            activeChar.healthSystem.Heal(activeChar.statSheet.stats["MaxHealth"] / 2);

            activeChar.ChangeHealthText();

            backButton.SetActive(false);

            //ChooseNextActiveChar();
            
        }
        //Mage
        else if (activeChar.statSheet.specialMove == 2)
        {
            //Debuff to enemies and self-buff

            

            //enemyList[enemyNum].ShowTargetCircle();

            while (!(Input.GetKeyDown(audioStatics.keycodeInterractButton) || Input.GetKeyDown(KeyCode.Return)))
            {
                /*if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
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
                else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
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
                }*/

                yield return null;
            }

            //Revealing Weakness
            /*Vector3 position = enemyList[enemyNum].GetPosition();
            ParticleManager weaknessParticle = Instantiate(activeChar.particleManager, position, Quaternion.identity, activeChar.transform);
            weaknessParticle.animator.SetBool("WeaknessFX", true);

            am.playSFX(9);

            yield return new WaitForSeconds(1.5f);

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
                enemyList[enemyNum].weaknessImage.sprite = null;
                Debug.Log("No weakness");
            }

            enemyList[enemyNum].HideTargetCircle();*/

            backButton.SetActive(false);

            //Buffing
            Vector3 buffPosition = activeChar.GetPosition();
            ParticleManager buffParticle = Instantiate(activeChar.particleManager, buffPosition, Quaternion.identity, activeChar.transform);
            buffParticle.animator.SetBool("TauntFX", true);
            am.playSFX(8);

            nicolBuffed = true;
            amountBuffed = (int)(activeChar.statSheet.stats["Strength"] * 0.35);

            activeChar.statSheet.stats["Strength"] += amountBuffed;

            //Debuffing
            for (int i = 0; i < enemyList.Count; i++)
            {
                Vector3 debuffPosition = enemyList[i].GetPosition();
                ParticleManager debuffParticle = Instantiate(enemyList[i].particleManager, debuffPosition, Quaternion.identity, enemyList[i].transform);
                debuffParticle.animator.SetBool("DebuffFX", true);

                enemyList[i].statSheet.stats["Defense"] -= (int)(enemyList[i].statSheet.stats["Defense"] * (0.35f));
            }


            //ChooseNextActiveChar();
        }
        //Bard
        else if (activeChar.statSheet.specialMove == 3)
        {
           

            while (!(Input.GetKeyDown(audioStatics.keycodeInterractButton) || Input.GetKeyDown(KeyCode.Return)))
            {
                yield return null;
            }

            activeChar.animator.SetBool("MagAttacking", true);

            Vector3 singPosition = activeChar.GetPosition();
            ParticleManager singParticle = Instantiate(activeChar.particleManager, singPosition, Quaternion.identity, activeChar.transform);
            singParticle.animator.SetBool("KisaSingFX", true);
            am.playSFX(10);

            backButton.SetActive(false);

            for (int i = 0; i < playerList.Count; i++)
            {
                ParticleManager healParticle;

                Vector3 position = playerList[i].GetPosition();

                healParticle = Instantiate(playerList[i].particleManager, position, Quaternion.identity, playerList[i].transform);
                healParticle.animator.SetBool("HealFX", true);

                playerList[i].healthSystem.Heal(playerList[i].statSheet.stats["MaxHealth"] / 2);

                playerList[i].ChangeHealthText();

                am.playSFX(12);
                
            }

            //ChooseNextActiveChar();
        }
        //Monk
        else if (activeChar.statSheet.specialMove == 4)
        {
            

            while (!(Input.GetKeyDown(audioStatics.keycodeInterractButton) || Input.GetKeyDown(KeyCode.Return)))
            {
                yield return null;
            }

            backButton.SetActive(false);

            activeChar.animator.SetBool("MagAttacking", true);

            yield return new WaitForSeconds(1.7f);



            for (int i = 0; i < enemyList.Count; i++)
            {
                ParticleManager lightningParticle;

                Vector3 position = enemyList[i].GetPosition();

                lightningParticle = Instantiate(enemyList[i].particleManager, position, Quaternion.identity, enemyList[i].transform);
                lightningParticle.animator.SetBool("ElectricFX", true);

                enemyList[i].GotDamaged(activeChar.statSheet.stats["Magic Attack"], 0);//enemyList[i].statSheet.stats["Defense"]);
            }

            //ChooseNextActiveChar();
        }
        else
        {
            //If the special move is targeting a specific character, use this entire "else" code


            enemyList[enemyNum].ShowTargetCircle();

            while (!(Input.GetKeyDown(audioStatics.keycodeInterractButton) || Input.GetKeyDown(KeyCode.Return))) 
            {
                if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                {
                    //They do not want to select the back button.
                    EventSystem.current.SetSelectedGameObject(null);
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
                else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
                {
                    //They do not want to select the back button.
                    EventSystem.current.SetSelectedGameObject(null);
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
                else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
                {
                    //They want to select the back button.
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(backButton);
                    enemyList[enemyNum].HideTargetCircle();
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
                {
                    //They want to go back to selecting an enemy. Show Target Circle again.
                    EventSystem.current.SetSelectedGameObject(null);
                    enemyList[enemyNum].ShowTargetCircle();
                }

                yield return null;
            }

            //Put code you want to execute here

            enemyList[enemyNum].HideTargetCircle();
        }

        activeChar.specialAvailable = false;

        ChooseNextActiveChar();

        coroutineRunning = false;
    }

    #endregion

    private BattleCharacter SpawnCharacter(bool isPlayerTeam, int[] statsToUse, string LName, int lSpecial, int lMagicType, int lMagicWeakness)
    {
        Vector3 position;

        bool isRightMostFighter = false;

        if (isPlayerTeam)
        {
            if (playerList.Count == 0)
            {
                position = new Vector3(mainCamera.transform.position.x - 4, mainCamera.transform.position.y - 5.5f);
            }
            else if (playerList.Count == 1)
            {
                position = new Vector3(mainCamera.transform.position.x - 7, mainCamera.transform.position.y - 5.5f);
            }
            else if (playerList.Count == 2)
            {
                position = new Vector3(mainCamera.transform.position.x - 1, mainCamera.transform.position.y - 5.5f);
                isRightMostFighter = true;
            }
            else if (playerList.Count == 3)
            {
                position = new Vector3(mainCamera.transform.position.x - 10, mainCamera.transform.position.y - 5.5f);
            }
            else
            {
                position = new Vector3(mainCamera.transform.position.x - 6, mainCamera.transform.position.y - 5.5f);
            }
        }
        else
        {
            if (enemyList.Count == 0)
            {
                position = new Vector3(mainCamera.transform.position.x + 1, mainCamera.transform.position.y + 3);
            }
            else if (enemyList.Count == 1)
            {
                position = new Vector3(mainCamera.transform.position.x + 4, mainCamera.transform.position.y + 3);
            }
            else if (enemyList.Count == 2)
            {
                position = new Vector3(mainCamera.transform.position.x + 7, mainCamera.transform.position.y + 3);
            }
            else if (enemyList.Count == 3)
            {
                position = new Vector3(mainCamera.transform.position.x + 10, mainCamera.transform.position.y + 3);
            }
            else
            {
                position = new Vector3(mainCamera.transform.position.x + 1, mainCamera.transform.position.y + 3);
            }
        }
        Transform characterTransform =  Instantiate(playerCharacterTransform, position, Quaternion.identity, fighterObject.transform);
        BattleCharacter battleCharacter = characterTransform.GetComponent<BattleCharacter>();

        //Setting stats
        if (isPlayerTeam)
        {
                                                         //Name    Stats     Magic Type    Description  player Team   Magic Weakness
            battleCharacter.statSheet = new CharacterData(LName, statsToUse, magicTypes[lMagicType], "Is a cube", true, magicTypes[lMagicWeakness], lSpecial);
            playerList.Add(battleCharacter);
            partyMembers.Add(battleCharacter);
        }
        else
        {
            battleCharacter.statSheet = new CharacterData(LName, statsToUse, magicTypes[lMagicType], "Isn't a cube", false, magicTypes[lMagicWeakness], lSpecial);
            enemyList.Add(battleCharacter);
        }


        battleCharacter.Setup(isPlayerTeam, playerController.Level, isRightMostFighter);
        
        characterQueue.Enqueue(battleCharacter);

        return battleCharacter;
    }



    IEnumerator FadeOut(BattleCharacter deadGuy)
    {
        for (float f = 1; f >= 0; f -= 0.05f)
        {
            Color c = deadGuy.charSprite.material.color;
            c.a = f;
            c.r = Random.Range(0, 1f);
            c.g = Random.Range(0, 1f);
            c.b = Random.Range(0, 1f);


            deadGuy.charSprite.color = c;

            yield return new WaitForSeconds(0.05f);
        }

        Destroy(deadGuy.gameObject);
    }

    private void SetActiveCharBattle(BattleCharacter battleCharacter)
    {
        if (activeChar != null)
        {
            activeChar.HideSelectionCircle();
        }

        activeChar = battleCharacter;

        if (activeChar.GIsPlayerTeam)
        {
            activeChar.ShowSelectionCircle();
        }
    }

    //changes turns
    public void ChooseNextActiveChar()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i].statSheet.stats["Health"] <= 0)
            {
                playerList[i].AllyFadeOut();
                playerList.RemoveAt(i);
                am.playSFX(18);
            }
        }

        for (int i = 0; i < enemyList.Count; i++)
        {
            if (i >= enemyList.Count)
            {
                continue;
            }

            if (enemyList[i].statSheet.stats["Health"] <= 0 || enemyList[i] == null)
            {
                //Can put death animation here
                /*Vector3 position = enemyList[i].GetPosition();
                ParticleManager particle = Instantiate(enemyList[i].particleManager, position, Quaternion.identity, enemyList[i].transform);
                particle.animator.SetBool("ElectricFX", true);*/

                if (enemyList[i].statSheet.name == "Slime Guy")
                {
                    levelManager.gainedEXP += Convert.ToInt32((((100*playerController.Level) / LevelManager.level) * (0.25f)) / enemyList.Count);
                    //levelManager.gainedEXP += 25;
                }
                else if (enemyList[i].statSheet.name == "Skeleton Guy")
                {
                    levelManager.gainedEXP += Convert.ToInt32((((100 * playerController.Level) / LevelManager.level) * (0.45f)) / enemyList.Count);
                    //levelManager.gainedEXP += 30;
                }
                else if (enemyList[i].statSheet.name == "Ghost Guy")
                {
                    levelManager.gainedEXP += Convert.ToInt32((((100 * playerController.Level) / LevelManager.level) * (0.6f)) / enemyList.Count);
                    //levelManager.gainedEXP += 35;
                }
                else if (enemyList[i].statSheet.name == "Wraith Guy")
                {
                    levelManager.gainedEXP += Convert.ToInt32((((100 * playerController.Level) / LevelManager.level) * (0.75f)) / enemyList.Count);
                    //levelManager.gainedEXP += 40;
                }
                else
                {
                    levelManager.gainedEXP += 100;
                }

                //enemyList[i].EnemyFadeOut();
                //Debug.Log(i + " " + BattleCharacter.FadeOneRunning);
                
                //Checking if another enemy is dying, and playing the another coroutine if it is
                if (BattleCharacter.FadeOneRunning == true && BattleCharacter.FadeTwoRunning == false)
                {
                    StartCoroutine(enemyList[i].SecondEnemyFadeOut());
                    //Debug.Log(enemyList[i].statSheet.name + i + " Fade two");
                }

                if (BattleCharacter.FadeOneRunning == false && BattleCharacter.FadeTwoRunning == false)
                {
                    StartCoroutine(enemyList[i].EnemyFadeOut());
                    //Debug.Log(enemyList[i].statSheet.name + i + "Fade One");
                }

                if (BattleCharacter.FadeOneRunning == true && BattleCharacter.FadeTwoRunning == true) //&& BattleCharacter.FadeThreeRunning == false)
                {
                    StartCoroutine(enemyList[i].ThirdEnemyFadeOut());
                    //Debug.Log(enemyList[i].statSheet.name + i + "Fade three");
                }


                enemyList.RemoveAt(i);
                i--;

                am.playSFX(16);

                //Works, but game doesn't give time for enemy to fade out, and that can cause issues
                //StartCoroutine(FadeOut(enemyList[i]));
            }
        }

        if (TestBattleOver())
        {
            return;
        }

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

            characterQueue.Dequeue();

            /*while (alreadyWent.Count != 0)
            {
                characterQueue.Enqueue(alreadyWent.Dequeue());
            };*/
            
            while (alreadyWent.Count != 0)
            {
                characterQueue.Enqueue(alreadyWent.Dequeue());
            }

            while (characterQueue.Count != 0)
            {
                alreadyWent.Enqueue(characterQueue.Dequeue());
            };

            while (alreadyWent.Count != 0)
            {
                if (alreadyWent.Peek().IsDead() == true)
                {
                    alreadyWent.Dequeue();
                }
                else
                {
                    characterQueue.Enqueue(alreadyWent.Dequeue());
                }
            };
        }

        //If the next character in the queue is on the enemy team
        if (characterQueue.Peek().GIsPlayerTeam == false)
        {

            if (characterQueue.Peek().Confused == false)
            {
                //Debug.Log("enemy " + characterQueue.Peek().statSheet.name);
                SetActiveCharBattle(characterQueue.Peek());
                alreadyWent.Enqueue(characterQueue.Dequeue());

                state = State.Busy;

                int enemyTarget;

                if (isTaunting == false)
                {
                    //Debug.Log("Not taunted");
                    enemyTarget = Random.Range(0, playerList.Count);

                    if (activeChar.statSheet.magicElement == "No Magic")
                    {
                        activeChar.Attack(playerList[enemyTarget], activeChar, () =>
                        {
                            ChooseNextActiveChar();
                        });
                    }
                    else if (activeChar.statSheet.magicElement == "Bone")
                    {
                        activeChar.magAttack(playerList[enemyTarget], activeChar, () =>
                        {
                            ChooseNextActiveChar();
                        });
                    }
                    else
                    {
                        int whichAttack = Random.Range(1, 3);

                        if (whichAttack == 1)
                        {
                            activeChar.Attack(playerList[enemyTarget], activeChar, () =>
                            {
                                ChooseNextActiveChar();
                            });
                        }
                        else if (whichAttack == 2)
                        {
                            activeChar.magAttack(playerList[enemyTarget], activeChar, () =>
                            {
                                ChooseNextActiveChar();
                            });
                        }
                    }
                }
                else
                {
                    //Debug.Log("Taunted");

                    if (activeChar.statSheet.magicElement == "No Magic")
                    {
                        activeChar.Attack(tankChar, activeChar, () =>
                        {
                            ChooseNextActiveChar();
                        });
                    }
                    else if (activeChar.statSheet.magicElement == "Bone")
                    {
                        activeChar.magAttack(tankChar, activeChar, () =>
                        {
                            ChooseNextActiveChar();
                        });
                    }
                    else
                    {
                        int whichAttack = Random.Range(1, 3);

                        if (whichAttack == 1)
                        {
                            activeChar.Attack(tankChar, activeChar, () =>
                            {
                                ChooseNextActiveChar();
                            });
                        }
                        else if (whichAttack == 2)
                        {
                            activeChar.magAttack(tankChar, activeChar, () =>
                            {
                                ChooseNextActiveChar();
                            });
                        }
                    }
                }

                //Debug.Log("Target: " +  enemyTarget);

                //Make enemy focus one target
                //enemyTarget = 1;


            }
            else
            {
                characterQueue.Peek().Confused = false;

                alreadyWent.Enqueue(characterQueue.Dequeue());

                ChooseNextActiveChar();
            }
        }
        //If the next character in the queue is on the player team
        else
        {
            //Debug.Log("ally " + characterQueue.Peek().statSheet.name);
            //EDIT THIS!!! COME BACK TO THIS!!!!!!!!!!
            // NOTE: CHANGE 1 TO WHATEVER THE ALLY'S SPECIAL METER IS (should probably make a variable for this)
            updateSP.addSpecial(characterQueue.Peek().statSheet.name, 1);
            if (characterQueue.Peek() == tankChar)
            {
                isTaunting = false;
            }

            SetActiveCharBattle(characterQueue.Peek());
            alreadyWent.Enqueue(characterQueue.Dequeue());
            activeChar.isBlocking = false;
            activeChar.animator.SetBool("Blocking", false);
            state = State.WaitingForPlayer;

            if (activeChar.OvertimeHealTurnsLeft > 0 )
            {
                activeChar.OvertimeHealTurnsLeft--;

                ParticleManager healParticle;

                Vector3 position = activeChar.GetPosition();

                healParticle = Instantiate(activeChar.particleManager, position, Quaternion.identity, activeChar.transform);
                healParticle.animator.SetBool("HealFX", true);

                activeChar.healthSystem.Heal(activeChar.statSheet.stats["MaxHealth"] / 6 );
                Debug.Log(activeChar.statSheet.stats["MaxHealth"] / 6);

                activeChar.ChangeHealthText();

                am.playSFX(12);
            }

            if (activeChar.focusedTurnsLeft > 0)
            {
                //Debug.Log("Is focused rn");
                activeChar.focusedTurnsLeft--;
            }
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

        //Debug.Log("Enemycount: " + enemyList.Count);
    }

    #region Special Functions

    #region Alan
    public void AlanGuardStatIncrease()
    {
        Vector3 guardPosition = tankChar.GetPosition();

        guardPosition.x += 0.2f;
        guardPosition.y += 1f;

        ParticleManager guardParticle = Instantiate(tankChar.particleManager, guardPosition, Quaternion.identity, tankChar.transform);
        guardParticle.animator.SetBool("GuardFX", true);

        tankChar.TempIncreaseStats("Defense", tankChar.statSheet.stats["Defense"] / 5);
        am.playSFX(36);
    }

    public void AlanTaunt()
    {
        isTaunting = true;

        Vector3 tauntPosition = tankChar.GetPosition();

        tauntPosition.x += 0.2f;
        tauntPosition.y += 1f;

        ParticleManager tauntParticle = Instantiate(tankChar.particleManager, tauntPosition, Quaternion.identity, tankChar.transform);
        tauntParticle.animator.SetBool("TauntFX", true);

        for (int i = 0; i < enemyList.Count; i++)
        {
            Vector3 tauntedPosition = enemyList[i].GetPosition();
            ParticleManager tauntedParticle = Instantiate(enemyList[i].particleManager, tauntedPosition, Quaternion.identity, enemyList[i].transform);
            tauntedParticle.animator.SetBool("EnemyTauntedFX", true);
        }
    }

    public void AlanTenacity()
    {
        StartCoroutine(TenacityParticle());

        activeChar.AttAnim();
        //am.playSFX(37);
    }

    public IEnumerator TenacityParticle()
    {
        yield return new WaitForSeconds(1.2f);

        for (int i = 0; i < enemyList.Count; i++)
        {
            ParticleManager tenacityParticle;

            Vector3 position = tenacityPosition.transform.position;

            activeChar.SetGPosition(enemyList[i].GetPosition());
            activeChar.SetGTarget(enemyList[i]);

            tenacityParticle = Instantiate(enemyList[i].particleManager, position, Quaternion.identity, enemyList[i].transform);
            tenacityParticle.animator.SetBool("TenacityFX", true);

            enemyList[i].GotDamaged(activeChar.statSheet.stats["Strength"], 0);//enemyList[i].statSheet.stats["Defense"]);
            activeChar.slashHit();
        }
    }

    #endregion

    #region Kisa

    public void KisaSing()
    {
        activeChar.animator.SetBool("MagAttacking", true);

        Vector3 singPosition = activeChar.GetPosition();
        ParticleManager singParticle = Instantiate(activeChar.particleManager, singPosition, Quaternion.identity, activeChar.transform);
        singParticle.animator.SetBool("KisaSingFX", true);
        am.playSFX(10);

        backButton.SetActive(false);

        for (int i = 0; i < playerList.Count; i++)
        {
            ParticleManager healParticle;

            Vector3 position = playerList[i].GetPosition();

            healParticle = Instantiate(playerList[i].particleManager, position, Quaternion.identity, playerList[i].transform);
            healParticle.animator.SetBool("HealFX", true);

            playerList[i].healthSystem.Heal(playerList[i].statSheet.stats["MaxHealth"] / 4);

            playerList[i].ChangeHealthText();

            am.playSFX(12);

        }
    }

    public void KisaConfused()
    {
        //StartCoroutine()
    }

    public IEnumerator ConfusedTargeting()
    {
        int enemyNum = 0;

        coroutineRunning = true;

        backButton.SetActive(true);

        enemyList[enemyNum].ShowTargetCircle();

        yield return new WaitForSeconds(timeBetweenSelectAndConfirm);

        while (!(Input.GetKeyDown(audioStatics.keycodeInterractButton) || Input.GetKeyDown(KeyCode.Return)))
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                //They do not want to select the back button. Unselect it.
                EventSystem.current.SetSelectedGameObject(null);
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
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                //They do not want to select the back button. Unselect it.
                EventSystem.current.SetSelectedGameObject(null);
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
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                //They want to select the back button.
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(backButton);
                enemyList[enemyNum].HideTargetCircle();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                //They want to go back to selecting an enemy. Show Target Circle again.
                EventSystem.current.SetSelectedGameObject(null);
                enemyList[enemyNum].ShowTargetCircle();
            }
            yield return null;
        }

        if (coroutineRunning == true)
        {
            backButton.SetActive(false);

            enemyList[enemyNum].HideTargetCircle();

            enemyList[enemyNum].Confused = true;
            enemyList[enemyNum].animator.SetBool("ConfusedFX", true);
            ParticleManager confuseParticle;

            Vector3 position = enemyList[enemyNum].GetPosition();

            am.playSFX(42);

            confuseParticle = Instantiate(enemyList[enemyNum].particleManager, position, Quaternion.identity, enemyList[enemyNum].transform);
            confuseParticle.animator.SetBool("ConfuseFX", true);

            //Remove special points since it has been used
            updateSP.removeSpecial("bard guy", 2);

            StartCoroutine(WaitBeforeChoosingNext(1.8f));
        }
    }

    public void KisaPerform()
    {
        activeChar.animator.SetBool("MagAttacking", true);

        Vector3 singPosition = activeChar.GetPosition();
        ParticleManager singParticle = Instantiate(activeChar.particleManager, singPosition, Quaternion.identity, activeChar.transform);
        singParticle.animator.SetBool("KisaSingFX", true);
        am.playSFX(10);

        backButton.SetActive(false);

        for (int i = 0; i < playerList.Count; i++)
        {
            ParticleManager healParticle;

            Vector3 position = playerList[i].GetPosition();

            healParticle = Instantiate(playerList[i].particleManager, position, Quaternion.identity, playerList[i].transform);
            healParticle.animator.SetBool("HealFX", true);

            playerList[i].healthSystem.Heal(playerList[i].statSheet.stats["MaxHealth"]);

            playerList[i].ChangeHealthText();

            am.playSFX(12);

        }
    }
    #endregion

    #region Nicol

    public void NicolMockery()
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            ParticleManager debuffParticle;
            Vector3 position = enemyList[i].GetPosition();

            debuffParticle = Instantiate(enemyList[i].particleManager, position, Quaternion.identity, enemyList[i].transform);
            debuffParticle.animator.SetBool("DebuffFX", true);

            enemyList[i].TempDecraseStats("Strength", enemyList[i].statSheet.stats["Strength"] / 4);
            enemyList[i].TempDecraseStats("Defense", enemyList[i].statSheet.stats["Defense"] / 4);
        }
    }

    public void NicolMotivate()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            ParticleManager motivateParticle;

            Vector3 position = activeChar.GetPosition();

            motivateParticle = Instantiate(playerList[i].particleManager, position, Quaternion.identity, playerList[i].transform);
            motivateParticle.animator.SetBool("TauntFX", true);

            playerList[i].OvertimeHealTurnsLeft = 3;
        }
    }

    #endregion

    #region Sophie

    public IEnumerator SophieQuake()
    {
        activeChar.animator.SetBool("Attacking", true);

        int damageToDeal = 0;

        //Debug.Log("Strength: " + activeChar.statSheet.stats["Strength"]);

        damageToDeal = activeChar.statSheet.stats["Strength"] / enemyList.Count();

        //Debug.Log("Damage: " +  damageToDeal);

        yield return new WaitForSeconds(1);

        for (int i = 0; i < enemyList.Count; i++)
        {
            activeChar.SetGPosition(enemyList[i].GetPosition());
            activeChar.SetGTarget(enemyList[i]);

            enemyList[i].GotDamaged(damageToDeal, enemyList[i].statSheet.stats["Defense"]);
            activeChar.punchHit();
        }

        ChooseNextActiveChar();
    }

    public void SophieFocus()
    {
        Vector3 position = activeChar.GetPosition();

        ParticleManager focusParticle = Instantiate(activeChar.particleManager, position, Quaternion.identity, activeChar.transform);

        focusParticle.animator.SetBool("FocusFX", true);

        activeChar.focusedTurnsLeft = 4;
    }

    public IEnumerator SophieStorm()
    {
        activeChar.animator.SetBool("MagAttacking", true);

        yield return new WaitForSeconds(1.5f);

        for (int i = 0; i < enemyList.Count; i++)
        {
            Vector3 position = enemyList[i].GetPosition();

            ParticleManager thunderParticle = Instantiate(enemyList[i].particleManager, position, Quaternion.identity, enemyList[i].transform);
            thunderParticle.animator.SetBool("ElectricFX", true);
        }

        yield return new WaitForSeconds(0.7f);

        for (int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].GotDamaged(30, 0);
        }

        ChooseNextActiveChar();
    }

    #endregion

    #endregion

    private IEnumerator AlanDied()
    {
        Transform position = tankChar.fighterObject.transform;

        tankChar.fighterObject.transform.DOMoveY(tankChar.fighterObject.transform.position.y - 6, 0.7f);

        yield return new WaitForSeconds(.8f);

        SceneManager.LoadScene("GameOver");
    }
    //Who died
    private bool TestBattleOver()
    {
        bool AllEnemyDead = true;

        for (int i = 0; i < enemyList.Count; i ++)
        {
            if (enemyList[i].IsDead() == false)
            {
                AllEnemyDead = false;
            }
        }

        //if main character dies
        if (tankChar.IsDead()) //&& mageChar.IsDead())
        {

            playerController.isSlime = false;
            playerController.isSkeleton = false;
            playerController.isWraith = false;
            playerController.isInvisGuy = false;
            playerController.KisaBoss = false;
            playerController.NicolBoss = false;
            playerController.SophieBoss = false;
            playerController.LichBoss = false;

            StartCoroutine(AlanDied());
            return true;
        }
        else if (enemyList.Count == 0 || AllEnemyDead == true)
        {
            if (tutorialHandler.activeInHierarchy)
            {
                tutorialHandler.SetActive(false);
            }
            
            playerController.isSlime = false;
            playerController.isSkeleton = false;
            playerController.isWraith = false;
            playerController.isInvisGuy = false;
            if (playerController.LichBoss)
            {

                youWinMenu.loadedDialogue = "Cutscene";
            }

            //remove lich boss
            playerController.LichBoss = false;

           
            if (playerController.KisaBoss || playerController.NicolBoss || playerController.SophieBoss)
            {
                //we should probably change the music here or smth
                am.playSFX(14);
                am.playBGM("T6");

                hasContemplatedKilling = true; // can no longer contemplate killing for this floor
                turnOffForKillBefriend.SetActive(false);
                befriendOrAbsorbButton.SetActive(true);
                playerController.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                //NOTE!! ASK BRANDON ABT PLAYER LIST!!!
                if (playerList[0].statSheet.stats["Health"] <= playerList[0].statSheet.stats["MaxHealth"]/4 && !hasContemplatedKilling && playerController.getObtainedCharacters() == 0)
                {
                    hasContemplatedKilling = true; // can no longer contemplate killing for this floor
                    if(playerController.getDeadCharacters() == 111 && playerController.Level == 3)
                    {
                        //trigger special dialogue if u get this with everyone dead on floor 3
                        youWinMenu.loadedDialogue = "contemplateKillingf3.5";
                    } else
                    {
                        youWinMenu.loadedDialogue = "contemplateKillingf" + playerController.Level.ToString();
                    }
                }
                youWin.SetActive(true);
                //playerController.isfrozen = false;
                battleObject.SetActive(false);
                playerController.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }

            /*while (levelManager.currentEXP >= levelManager.lvlUpThreshold)
            {
                levelManager.LevelUp();
            }*/

            for (int i = 0; i < partyMembers.Count; i++)
            {
                /*if (partyMembers[i].statSheet.name == "Mage Guy")
                {
                    partyMembers[i].statSheet.stats["Strength"] -= amountBuffed;
                }*/

                //Undos any temporary buffs
                if (partyMembers[i].tempBuffed)
                {
                    partyMembers[i].UndoTempBuff();
                }
            }

            if ((levelManager.gainedEXP + levelManager.currentEXP >= 100))
            {
                levelManager.LevelUp();
            }

            levelManager.StoreStats();

            #region Destroy Existing Char

            if (befriendOrAbsorbButton.activeInHierarchy == false)
            {
                partyMembers.Clear();
            }

            playerList.Clear();

            characterQueue.Clear();

            alreadyWent.Clear();

            enemyList.Clear();

            if (tankChar != null)
            {
                Destroy(tankChar.gameObject);
            }

            if (monkChar != null)
            {
                Destroy(monkChar.gameObject);
            }

            if (bardChar != null)
            {
                Destroy(bardChar.gameObject);
            }

            if (mageChar != null)
            {
                Destroy(mageChar.gameObject);
            }

            if (firstEnemy != null)
            {
                Destroy(firstEnemy.gameObject);
            }

            if (secondEnemy != null)
            {
                Destroy(secondEnemy.gameObject);
            }

            if (thirdEnemy != null)
            {
                Destroy(thirdEnemy.gameObject);
            }

            if (FourthEnemy != null)
            {
                Destroy(FourthEnemy.gameObject);
            }
            #endregion


            //battleFadeAnim.SetBool("BattleOver", true);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void BefriendButton()
    {

        playerController.joinParty();

        if (playerController.KisaBoss == true)
        {
            //Bard
            playerController.KisaBoss = false;

            levelManager.NewPartyMember("Kisa");
            playerController.hasKisa = true;
            befriendOrAbsorbButton.SetActive(false);
        }
        else if (playerController.NicolBoss == true)
        {
            //Mage
            playerController.NicolBoss = false;

            levelManager.NewPartyMember("Nicol");
            playerController.hasNicol = true;
            befriendOrAbsorbButton.SetActive(false);
        }
        else if (playerController.SophieBoss == true)
        {
            //Monk
            playerController.SophieBoss = false;

            levelManager.NewPartyMember("Sophie");
            playerController.hasSophie = true;
            befriendOrAbsorbButton.SetActive(false);
        }

        turnOffForKillBefriend.SetActive(true);
        partyMembers.Clear();
        //playerController.isfrozen = false;
        battleObject.SetActive(false);
    }

    public void AbsorbButton()
    {
        playerController.Absorb();

        if (playerController.KisaBoss == true)
        {
            //Bard
            playerController.KisaBoss = false;

            levelManager.AbsorbPartyMember("Kisa");

            befriendOrAbsorbButton.SetActive(false);
        }
        else if (playerController.NicolBoss == true)
        {
            //Mage
            playerController.NicolBoss = false;

            levelManager.AbsorbPartyMember("Nicol");

            befriendOrAbsorbButton.SetActive(false);
        }
        else if (playerController.SophieBoss == true)
        {
            //Monk
            playerController.SophieBoss = false;

            levelManager.AbsorbPartyMember("Sophie");

            befriendOrAbsorbButton.SetActive(false);
        }


        turnOffForKillBefriend.SetActive(true);
        partyMembers.Clear();
        //playerController.isfrozen = false;
        battleObject.SetActive(false);
    }
}
