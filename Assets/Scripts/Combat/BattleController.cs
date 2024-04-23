using DG.Tweening;
using DG.Tweening.Core.Easing;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BattleController : MonoBehaviour
{
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
            enemySpawning = SpawnCharacter(false, skeletonStats, "Skeleton Guy", 0, 4, 1);

            return enemySpawning;
        }
        else if (randomiser == 3)
        {
            enemySpawning = SpawnCharacter(false, ghostStats, "Ghost Guy", 0, 4, 3);

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

        SetStats();

        befriendOrAbsorbButton.SetActive(false);

        

        instance = this;

        //int howManyToSpawn = Random.Range(1, 5);

        //True for an ally, false for an enemy

        tankChar = SpawnCharacter(true, tankStats, "Tank Guy", 1, 0, 1);

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
            firstEnemy = SpawnCharacter(false, skeletonStats, "Skeleton Guy", 0, 4, 1);
        }
        else if (playerController.isWraith)
        {
            firstEnemy = SpawnCharacter(false, wraithStats, "Wraith Guy", 0, 4, 2);
        }
        else if (playerController.isInvisGuy)
        {
            firstEnemy = SpawnCharacter(false, ghostStats, "Ghost Guy", 0, 4, 3);
        }
        else if (playerController.KisaBoss)
        {
            firstEnemy = SpawnCharacter(false, evilBardStats, "Bard Guy", 3, 3, 0);
            partyBoss = true;
            //howManyToSpawn = 0;
        }
        else if (playerController.NicolBoss)
        {
            firstEnemy = SpawnCharacter(false, evilMageStats, "Mage Guy", 2, 1, 2);
            partyBoss = true;
            //howManyToSpawn = 0;
        }
        else if (playerController.SophieBoss)
        {
            firstEnemy = SpawnCharacter(false, evilMonkStats, "Monk Guy", 4, 2, 3);
            partyBoss = true;
            //howManyToSpawn = 0;
        }
        else if (playerController.LichBoss)
        {
            firstEnemy = SpawnCharacter(false, lichStats, "Lich Guy", 0, 5, 4);
        }

        if (partyBoss == false)
        {
            am.playBGM("T3");
        }
        else if (partyBoss == true)
        {
            //for right now im making it the normal music cause idk if i like this music w/ boss fights
            //am.playBGM("T6");
            am.playBGM("T3");
        }


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

    #region Stats

    //WHEN CHANGING STATS CHANGE THEM IN LEVEL MANAGER AS WELL


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
        /*Defense*/ 4, 
        /*Speed*/ 5, 
        /*Health*/ 8, 
        /*MaxHealth*/ 8,
        /*Mana*/ 9,
        /*MaxMana*/ 9};

    static public int[] evilMageStats = {
        /*Strength*/ 10,
        /*Magic Attack*/ 13,
        /*Defense*/ 4, 
        /*Speed*/ 5, 
        /*Health*/ 40, 
        /*MaxHealth*/ 40,
        /*Mana*/ 9,
        /*MaxMana*/ 9};

    //Monk Stats
    static public int[] monkStats = {
        /*Strength*/ 13,
        /*Magic Attack*/ 7,
        /*Defense*/ 4, 
        /*Speed*/ 6, 
        /*Health*/ 10, 
        /*MaxHealth*/ 10,
        /*Mana*/ 5,
        /*MaxMana*/ 5};

    static public int[] evilMonkStats = {
        /*Strength*/ 17,
        /*Magic Attack*/ 13,
        /*Defense*/ 8, 
        /*Speed*/ 6, 
        /*Health*/ 50, 
        /*MaxHealth*/ 50,
        /*Mana*/ 5,
        /*MaxMana*/ 5};

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
        /*Strength*/ 8,
        /*Magic Attack*/ 9,
        /*Defense*/ 5, 
        /*Speed*/ 4, 
        /*Health*/ 30, 
        /*MaxHealth*/ 30,
        /*Mana*/ 7,
        /*MaxMana*/ 7};

    //Slime Stats
    static public int[] slimeStats = {
        /*Strength*/ 7,
        /*Magic Attack*/ 1,
        /*Defense*/ 3, 
        /*Speed*/ 2, 
        /*Health*/ 17, 
        /*MaxHealth*/ 17,
        /*Mana*/ 6,
        /*MaxMana*/ 7};

    //Skeleton Stats
    static public int[] skeletonStats = {
        /*Strength*/ 7,
        /*Magic Attack*/ 1,
        /*Defense*/ 7, 
        /*Speed*/ 3, 
        /*Health*/ 10, 
        /*MaxHealth*/ 10,
        /*Mana*/ 6,
        /*MaxMana*/ 7};

    static public int[] wraithStats = {
        /*Strength*/ 9,
        /*Magic Attack*/ 1,
        /*Defense*/ 5, 
        /*Speed*/ 5, 
        /*Health*/ 17, 
        /*MaxHealth*/ 17,
        /*Mana*/ 6,
        /*MaxMana*/ 7};

    static public int[] ghostStats = {
        /*Strength*/ 9,
        /*Magic Attack*/ 1,
        /*Defense*/ 11, 
        /*Speed*/ 4, 
        /*Health*/ 7, 
        /*MaxHealth*/ 7,
        /*Mana*/ 6,
        /*MaxMana*/ 7};

    static public int[] lichStats = {
        /*Strength*/ 9,
        /*Magic Attack*/ 12,
        /*Defense*/ 7, 
        /*Speed*/ 4, 
        /*Health*/ 50, 
        /*MaxHealth*/ 50,
        /*Mana*/ 6,
        /*MaxMana*/ 7};

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
        /*Defense*/ 4, 
        /*Speed*/ 5, 
        /*Health*/ 8, 
        /*MaxHealth*/ 8,
        /*Mana*/ 9,
        /*MaxMana*/ 9};

        //Monk Stats
        int[] lMonkStats = {
        /*Strength*/ 13,
        /*Magic Attack*/ 7,
        /*Defense*/ 4, 
        /*Speed*/ 6, 
        /*Health*/ 10, 
        /*MaxHealth*/ 10,
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
        /*Defense*/ 7, 
        /*Speed*/ 3, 
        /*Health*/ 10, 
        /*MaxHealth*/ 10,
        /*Mana*/ 6,
        /*MaxMana*/ 7};

        int[] lWraithStats = {
        /*Strength*/ 9,
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
        /*Defense*/ 11, 
        /*Speed*/ 4, 
        /*Health*/ 7, 
        /*MaxHealth*/ 7,
        /*Mana*/ 6,
        /*MaxMana*/ 7};

        int[] lLichStats = {
        /*Strength*/ 9,
        /*Magic Attack*/ 12,
        /*Defense*/ 7, 
        /*Speed*/ 4, 
        /*Health*/ 50, 
        /*MaxHealth*/ 50,
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
            lichStats = lLichStats;
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

    public GameObject tutorialHandler;
    [SerializeField] private GameObject attackButtonBlocker;

    private bool partyBoss;

    public Animator battleFadeAnim;

    public audioManager am;

    [SerializeField] private GameObject specialObject;
    public UnityEngine.UI.Image specialButton;
    public Sprite specReady;
    public Sprite specNotReady;

    [SerializeField] private GameObject kisaHealthText;
    [SerializeField] private GameObject nicolHealthText;
    [SerializeField] private GameObject sophieHealthText;

    [SerializeField] private GameObject turnOffForKillBefriend;

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
        specialButton = specialObject.GetComponent<UnityEngine.UI.Image>();
    }

    private void Update()
    {

        if (state == State.WaitingForPlayer)
        {
            fightingButtons.SetActive(true);

            if (activeChar.specialAvailable)
            {
                specialButton.sprite = specReady;
            }
            else
            {
                specialButton.sprite = specNotReady;
            }
        }
        else
        {
            fightingButtons.SetActive(false);
        }  

        if (alanFireMagicButton.activeInHierarchy)
        {
            attackButtonBlocker.SetActive(false);
        }
        else
        {
            attackButtonBlocker.SetActive(true);
        }

        /* Testing how to instantiate the particle effects
         * if (Input.GetKeyDown(KeyCode.G))
        {
            Vector3 transform = activeChar.GetPosition();

            GameObject particle = Instantiate(particleManager, transform, Quaternion.identity, activeChar.transform);
        }*/

        
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
    }

    public void attackButton()
    {
        state = State.Busy;

        /*activeChar.Attack(secEnemyChar, activeChar, () =>
        {
            ChooseNextActiveChar();
        });*/

        StartCoroutine(AttackTargeting()); 
    }

    public void defendButton()
    {
        state = State.Busy;

        StartCoroutine(BlockConfirm());
    }

    public void magicButton()
    {
        /*activeChar.magAttack(enemyChar, activeChar, () =>
        {
            ChooseNextActiveChar();
        });*/


        if (activeChar.statSheet.name == "Tank Guy") //&& levelManager.kisaAbsorb || activeChar.statSheet.name == "Tank Guy" && levelManager.nicolAbsorb || activeChar.statSheet.name == "Tank Guy" && levelManager.sophieAbsorb)
        {
            state = State.Busy;

            backButton.SetActive(true);

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
        
        StartCoroutine(MagicTargeting());
    }

    public void nicolMagic()
    {
        activeChar.statSheet.magicElement = magicTypes[1];

        alanFireMagicButton.SetActive(false);
        nicolIceMagicButton.SetActive(false);
        kisaWindMagicButton.SetActive(false);
        sophieElectricMagicButton.SetActive(false);

        StartCoroutine(MagicTargeting());
    }

    public void sophieMagic()
    {
        activeChar.statSheet.magicElement = magicTypes[2];

        alanFireMagicButton.SetActive(false);
        nicolIceMagicButton.SetActive(false);
        kisaWindMagicButton.SetActive(false);
        sophieElectricMagicButton.SetActive(false);

        StartCoroutine(MagicTargeting());
    }

    public void kisaMagic()
    {
        activeChar.statSheet.magicElement = magicTypes[3];

        alanFireMagicButton.SetActive(false);
        nicolIceMagicButton.SetActive(false);
        kisaWindMagicButton.SetActive(false);
        sophieElectricMagicButton.SetActive(false);

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
            state = State.Busy;

            StartCoroutine(SpecialTargeting());
        }
    }

    public void Die()
    {
        SceneManager.LoadScene("GameOver");
    }

    #endregion

    #region Targeting Coroutines

    public KeyCode confirmKey = KeyCode.Space;

    //When these coroutines are called, the while loop while loop indefinitely until the enter or "return" key is pressed
    private IEnumerator AttackTargeting()
    {
        int enemyNum = 0;


        backButton.SetActive(true);

        enemyList[enemyNum].ShowTargetCircle();

        while (!Input.GetKeyDown(confirmKey))
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
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
            }

            yield return null;
        }

        activeChar.Attack(enemyList[enemyNum], activeChar, () =>
        {
            ChooseNextActiveChar();
        });

        enemyList[enemyNum].HideTargetCircle();


        backButton.SetActive(false);
    }

    private IEnumerator BlockConfirm()
    {
        backButton.SetActive(true);

        while (!Input.GetKeyDown(confirmKey))
        {
            yield return null;
        }

        activeChar.isBlocking = true;
        activeChar.animator.SetBool("Blocking", true);
        ChooseNextActiveChar();

        backButton.SetActive(false);
    }

    private IEnumerator MagicTargeting()
    {
        int enemyNum = 0;

        enemyList[enemyNum].ShowTargetCircle();

        while (!Input.GetKeyDown(confirmKey))
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
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
            }

            yield return null;
        }

        enemyList[enemyNum].HideTargetCircle();


        backButton.SetActive(false);

        activeChar.magAttack(enemyList[enemyNum], activeChar, () =>
        {
            ChooseNextActiveChar();
        });
    }

    private bool isTaunting = false;

    private IEnumerator SpecialTargeting()
    {
        int enemyNum = 0;

        //Tank
        if (activeChar.statSheet.specialMove == 1)
        {
            backButton.SetActive(true);

            while (!Input.GetKeyDown(confirmKey))
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
            //Showing weakness

            backButton.SetActive(true);

            enemyList[enemyNum].ShowTargetCircle();

            while (!Input.GetKeyDown(confirmKey))
            {
                if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
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
                }

                yield return null;
            }

            Vector3 position = enemyList[enemyNum].GetPosition();
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
                Debug.Log("No weakness");
            }

            enemyList[enemyNum].HideTargetCircle();


            backButton.SetActive(false);

            //ChooseNextActiveChar();
        }
        //Bard
        else if (activeChar.statSheet.specialMove == 3)
        {
            backButton.SetActive(true);

            while (!Input.GetKeyDown(confirmKey))
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
            backButton.SetActive(true);

            while (!Input.GetKeyDown(confirmKey))
            {
                yield return null;
            }

            backButton.SetActive(false);


            for (int i = 0; i < enemyList.Count; i++)
            {
                enemyList[i].GotDamaged(activeChar.statSheet.stats["Strength"], enemyList[i].statSheet.stats["Defense"]);
            }

            //ChooseNextActiveChar();
        }
        else
        {
            //If the special move is targeting a specific character, use this entire "else" code


            enemyList[enemyNum].ShowTargetCircle();

            while (!Input.GetKeyDown(confirmKey))
            {
                if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
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
                }

                yield return null;
            }

            //Put code you want to execute here

            enemyList[enemyNum].HideTargetCircle();
        }

        activeChar.specialAvailable = false;

        ChooseNextActiveChar();

    }

    #endregion

    private BattleCharacter SpawnCharacter(bool isPlayerTeam, int[] statsToUse, string LName, int lSpecial, int lMagicType, int lMagicWeakness)
    {
        Vector3 position;
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


        battleCharacter.Setup(isPlayerTeam, playerController.Level);
        
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
    private void ChooseNextActiveChar()
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
            if (enemyList[i].statSheet.stats["Health"] <= 0 || enemyList[i] == null)
            {
                //Can put death animation here
                /*Vector3 position = enemyList[i].GetPosition();
                ParticleManager particle = Instantiate(enemyList[i].particleManager, position, Quaternion.identity, enemyList[i].transform);
                particle.animator.SetBool("ElectricFX", true);*/

                if (enemyList[i].statSheet.name == "Slime Guy")
                {
                    levelManager.gainedEXP += Convert.ToInt32(((100*playerController.Level) / LevelManager.level) * (0.25f));
                    //levelManager.gainedEXP += 25;
                }
                else if (enemyList[i].statSheet.name == "Skeleton Guy")
                {
                    levelManager.gainedEXP += Convert.ToInt32(((100 * playerController.Level) / LevelManager.level) * (0.3f));
                    //levelManager.gainedEXP += 30;
                }
                else if (enemyList[i].statSheet.name == "Ghost Guy")
                {
                    levelManager.gainedEXP += Convert.ToInt32(((100 * playerController.Level) / LevelManager.level) * (0.35f));
                    //levelManager.gainedEXP += 35;
                }
                else if (enemyList[i].statSheet.name == "Wraith Guy")
                {
                    levelManager.gainedEXP += Convert.ToInt32(((100 * playerController.Level) / LevelManager.level) * (0.4f));
                    //levelManager.gainedEXP += 40;
                }
                else
                {
                    levelManager.gainedEXP += 100;
                }

                //enemyList[i].EnemyFadeOut();
                StartCoroutine(enemyList[i].EnemyFadeOut());
                enemyList.RemoveAt(i);

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
        //If the next character in the queue is on the player team
        else
        {
            //Debug.Log("ally " + characterQueue.Peek().statSheet.name);
            if (characterQueue.Peek().statSheet.specialMove == 1)
            {
                isTaunting = false;
            }

            SetActiveCharBattle(characterQueue.Peek());
            alreadyWent.Enqueue(characterQueue.Dequeue());
            activeChar.isBlocking = false;
            activeChar.animator.SetBool("Blocking", false);
            state = State.WaitingForPlayer;
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

            SceneManager.LoadScene("GameOver");
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
            playerController.LichBoss = false;

            if (playerController.KisaBoss || playerController.NicolBoss || playerController.SophieBoss)
            {
                //we should probably change the music here or smth
                am.playSFX(14);
                am.playBGM("T6");

                turnOffForKillBefriend.SetActive(false);
                befriendOrAbsorbButton.SetActive(true);
            }
            else
            {
                youWin.SetActive(true);
                //playerController.isfrozen = false;
                battleObject.SetActive(false);
            }

            /*while (levelManager.currentEXP >= levelManager.lvlUpThreshold)
            {
                levelManager.LevelUp();
            }*/

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

        playerController.joinParty();
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
