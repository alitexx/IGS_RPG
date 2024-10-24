using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 5f;
    public Rigidbody2D rb;
    public bool isfrozen = false;

    private Vector2 input;

    private Animator animator;

    public LayerMask solidObjectsLayer;

    //party following 
    public Transform[] waypointTrail = new Transform[4];
    public int followGap;

    //Battle stuff
    public GameObject battleUI;
    public Animator battleFade;
    //who in party
    public bool hasKisa = false;
    public bool hasNicol = false;
    public bool hasSophie = false;
    //enemies
    public bool isSlime = false;
    public bool isWraith = false;
    public bool isInvisGuy = false;
    public bool isSkeleton = false;
    //Tutorial
    public bool tutorialFight = false;
    public GameObject tutorialHandler;
    //bosses
    public bool KisaBoss = false;
    public bool NicolBoss = false;
    public bool SophieBoss = false;
    public bool LichBoss = false;

    //Save Data vars
    public int Level = 1;
    public int partyLevel = 1;
    public int currentEXP = 0;
    public float[] playerPosition = new float[2];
    int KisainParty = 0;
    int NicolinParty = 0;
    int SophieinParty = 0;
    public int KisaAbsorbed = 0;
    public int NicolAbsorbed = 0;
    public int SophieAbsorbed = 0;
    public int HasBeenThruTutorial = 0;
    public int BattleTutorialCleared = 0;
    public int DoorsOpened = 0;
    public GameObject[] Doors;
    public GameObject slime;

    public mapManager MapManager;
    public int floor1RoomsDiscovered;
    public int floor2RoomsDiscovered;
    public int floor3RoomsDiscovered;
    public int floor4RoomsDiscovered;

    //Camera Movement Detecter
    public CamMovementDetect cameraMovementDetecter;

    //gameObjects and Sprites
    public GameObject Kisa;
    public GameObject Nicol;
    public GameObject Sophie;

    public SpriteRenderer KisaRenderer;
    public Sprite absorbKisa;

    public SpriteRenderer NicolRenderer;
    public Sprite absorbNicol;

    public SpriteRenderer SophieRenderer;
    public Sprite absorbSophie;

    // for starting dialogue
    [SerializeField] private mainDialogueManager mainDialogueManager;

    public LevelManager levelManager;


    [SerializeField] private pauseMenuManager pauseMenuManager;

    [SerializeField] private GameObject[] cutscenes;
    private void Start()
    {
        Time.timeScale = 1.0f;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        waypointTrail = new Transform[4] {
            rb.transform,
            //This works, they just need to have their transform relative to parent adjusted to not overlap
            rb.transform.Find("FollowTrail1"),
            rb.transform.Find("FollowTrail2"),
            rb.transform.Find("FollowTrail3")
        };

        tutorialHandler.SetActive(false);

        battleUI.SetActive(false);

    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        if(isfrozen != true)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");
            if (input.x != 0) input.y = 0;

            if (input != Vector2.zero)
            {
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);
            }

            animator.SetFloat("moveX", input.x);
            animator.SetFloat("moveY", input.y);
            animator.SetFloat("speed", input.sqrMagnitude);
        }

        if (isfrozen == true) 
        {
            animator.SetFloat("moveX", 0);
            animator.SetFloat("moveY", 0);
            animator.SetFloat("speed", 0);
        }

        /*if (Vector3.Distance(rb.transform.position, waypointTrail[0].position) >= followGap)
        {
            waypointTrail[1].position = waypointTrail[0].position;
        }
        if (Vector3.Distance(rb.transform.position, waypointTrail[1].position) >= 2 * followGap)
        {
            waypointTrail[2].position = waypointTrail[1].position;
        }
        if (Vector3.Distance(rb.transform.position, waypointTrail[2].position) >= 3 * followGap)
        {
            waypointTrail[3].position = waypointTrail[2].position;
        }
        */
        
        //Save stuff
        partyLevel = LevelManager.level;

        playerPosition[0] = transform.position.x;
        playerPosition[1] = transform.position.y;

        if(hasKisa == true) 
        {
            KisainParty = 1;
        }
        else
        {
            KisainParty = 0;
        }

        if (hasNicol == true)
        {
            NicolinParty = 1;
        }
        else
        {
            NicolinParty = 0;
        }

        if (hasSophie == true)
        {
            SophieinParty = 1;
        }
        else
        {
            SophieinParty = 0;
        }
    }

    void FixedUpdate()
    {
        if(isfrozen == false)
        {
            rb.MovePosition(rb.position + input * movementSpeed * Time.fixedDeltaTime);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        KisaBoss = false;
        NicolBoss = false;
        SophieBoss = false;
        LichBoss = false;

        if (collision.tag == "BossFreeze")
        {
            isfrozen = true;

            switch (collision.gameObject.name)
            {
                case "0":
                    //start pre-tutorial dialogue
                    //start tutorial fight
                    isSlime = true;
                    tutorialFight = true;
                    tutorialHandler.SetActive(true);
                    mainDialogueManager.dialogueSTART("tutorialEncounter");
                    youWinMenu.loadedDialogue = "partySplit";
                    break;
                case "1":
                    KisaBoss = true;
                    mainDialogueManager.dialogueSTART("kisaEncounter");
                    break;
                case "2":
                    NicolBoss = true;
                    if (hasKisa)
                    {
                        mainDialogueManager.dialogueSTART("nicolEncounter_k");
                    } else
                    {
                        mainDialogueManager.dialogueSTART("nicolEncounter_x");
                    }
                    break;
                case "3":
                    SophieBoss = true;
                    if (hasKisa && hasNicol)
                    {
                        mainDialogueManager.dialogueSTART("sophieEncounter_kn");
                    }
                    else if (hasKisa)
                    {
                        mainDialogueManager.dialogueSTART("sophieEncounter_kx");
                    }
                    else if (hasNicol)
                    {
                        mainDialogueManager.dialogueSTART("sophieEncounter_xn");
                    }
                    else
                    {
                        mainDialogueManager.dialogueSTART("sophieEncounter_xx");
                    }
                    break;
                default:
                    LichBoss = true;
                    if (hasKisa && hasNicol && hasSophie)
                    {
                        mainDialogueManager.dialogueSTART("lichEncounter_kns");
                    }
                    else if (hasKisa && hasNicol)
                    {
                        mainDialogueManager.dialogueSTART("lichEncounter_knx");
                    }
                    else if (hasKisa && hasSophie)
                    {
                        mainDialogueManager.dialogueSTART("lichEncounter_kxs");
                    }
                    else if (hasNicol && hasSophie)
                    {
                        mainDialogueManager.dialogueSTART("lichEncounter_xns");
                    }
                    else if (hasKisa)
                    {
                        mainDialogueManager.dialogueSTART("lichEncounter_kxx");
                    }
                    else if (hasNicol)
                    {
                        mainDialogueManager.dialogueSTART("lichEncounter_xnx");
                    }
                    else
                    {
                        mainDialogueManager.dialogueSTART("end_genocide");
                    }
                    break;
            }
            //starting battle is now determined by the dialogue system! haha!
            //battleFade.SetBool("BattleStarting", true);
            Destroy(collision.gameObject);
        }

        if (collision.tag == "DialogueFreeze")
        {
            //do something with dialogue idk lmao
        }

        if (collision.gameObject.name == "level1")
        {
            //start dialogue
            hasKisa = false;
            hasNicol = false;
            hasSophie = false;
        }
        
        if (collision.gameObject.name == "level2" && Level != 2)
        {
            Level = 2;
            if (hasKisa)
            {
                mainDialogueManager.dialogueSTART("secondFloor_k");
            }
            //Do anything else if needed
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.name == "level3" && Level != 3)
        {
            Level = 3;
            if (hasKisa && hasNicol)
            {
                mainDialogueManager.dialogueSTART("thirdFloor_kn");
            }
            //Do anything else if needed
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.name == "level4" && Level != 4)
        {
            Level = 4;
            if (hasKisa && hasNicol && hasSophie)
            {
                mainDialogueManager.dialogueSTART("fourthFloor_kns");
            }
            //Do anything else if needed
            Destroy(collision.gameObject);
        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
            isSlime = false;
            isWraith = false;
            isInvisGuy = false;
            isSkeleton = false;
            tutorialFight = false;


            if (collision.gameObject.tag == "Slime")
            {

                Destroy(collision.gameObject);
                isfrozen = true;
                isSlime = true;
                battleFade.SetBool("BattleStarting", true);
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }

            if (collision.gameObject.tag == "TutorialSlime")
            {
                Destroy(collision.gameObject);
                isfrozen = true;
                isSlime = true;
                tutorialFight = true;
                tutorialHandler.SetActive(true);
                battleFade.SetBool("BattleStarting", true);
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }

            if (collision.gameObject.tag == "Wraith")
            {
                Destroy(collision.gameObject);
                isfrozen = true;
                isWraith = true;
                battleFade.SetBool("BattleStarting", true);
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }

            if (collision.gameObject.tag == "InvisGuy")
            {
                Destroy(collision.gameObject);
                isfrozen = true;
                isInvisGuy = true;
                battleFade.SetBool("BattleStarting", true);
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }

            if (collision.gameObject.tag == "Skeleton")
            {
                Destroy(collision.gameObject);
                isfrozen = true;
                isSkeleton = true;
                battleFade.SetBool("BattleStarting", true);
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
    }


    public void Absorb()
    {
        if (KisaBoss == true)
        {
            Destroy(Kisa);
            //KisaRenderer.sprite = absorbKisa;
            KisaAbsorbed = 1;
            HasBeenThruTutorial = 1;
        }
        else if (NicolBoss == true)
        {
            Destroy(Nicol);
            //NicolRenderer.sprite = absorbNicol;
            NicolAbsorbed = 1;
        }
        else if (SophieBoss == true)
        {
            Destroy(Sophie);
            //SophieRenderer.sprite = absorbSophie;
            SophieAbsorbed = 1;
        }
    }

    public void joinParty()
    {
        if (KisaBoss == true)
        {
            Destroy(Kisa);
            //delete or do whatever we want 
            HasBeenThruTutorial = 1;
        }
        else if (NicolBoss == true)
        {
            Destroy(Nicol);
            //delete or do whatever we want
        }
        else if (SophieBoss == true)
        {
            Destroy(Sophie);
            //delete or do whatever we want
        }
    }

    public void saveGame()
    {
        BattleTutorialCleared = 1;
        PlayerPrefs.SetInt("DoorsOpened", DoorsOpened);
        PlayerPrefs.SetInt("PartyLevel", partyLevel);
        PlayerPrefs.SetInt("CurrentEXP", levelManager.currentEXP);
        PlayerPrefs.SetFloat("PlayerPositionX", playerPosition[0]);
        PlayerPrefs.SetFloat("PlayerPositionY", playerPosition[1]);
        PlayerPrefs.SetInt("hasKisa", KisainParty);
        PlayerPrefs.SetInt("hasNicol", NicolinParty);
        PlayerPrefs.SetInt("hasSophie", SophieinParty);
        PlayerPrefs.SetInt("AbsorbedKisa", KisaAbsorbed);
        PlayerPrefs.SetInt("AbsorbedNicol", NicolAbsorbed);
        PlayerPrefs.SetInt("AbsorbedSophie", SophieAbsorbed);
        PlayerPrefs.SetInt("FloorLevel", Level);
        PlayerPrefs.SetInt("HasBeenThruTutorial", HasBeenThruTutorial);
        PlayerPrefs.SetInt("BattleTutorialCleared", BattleTutorialCleared);
        PlayerPrefs.Save();
        Debug.Log("Saved stuff?");
        //Debug.Log(PlayerPrefs.GetFloat("PlayerPositionX"));
        //Debug.Log(PlayerPrefs.GetFloat("PlayerPositionY"));
        //Debug.Log(PlayerPrefs.GetInt("CurrentEXP"));
        //Debug.Log(PlayerPrefs.GetInt("FloorLevel"));
        //Debug.Log(PlayerPrefs.GetInt("PartyLevel"));
        Debug.Log(PlayerPrefs.GetInt("hasKisa"));


        floor1RoomsDiscovered = MapManager.getRoomsDiscovered(1);
        floor2RoomsDiscovered = MapManager.getRoomsDiscovered(2);
        floor3RoomsDiscovered = MapManager.getRoomsDiscovered(3);
        floor4RoomsDiscovered = MapManager.getRoomsDiscovered(4);
        PlayerPrefs.SetInt("Floor1Explored", floor1RoomsDiscovered);
        PlayerPrefs.SetInt("Floor2Explored", floor2RoomsDiscovered);
        PlayerPrefs.SetInt("Floor3Explored", floor3RoomsDiscovered);
        PlayerPrefs.SetInt("Floor4Explored", floor4RoomsDiscovered);

    }

    public void DeleteSave() 
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Deleted save");
    }

    public void loadGame() 
    {
        Debug.Log("loading...");
        //LevelManager.level = PlayerPrefs.GetInt("PartyLevel");
        levelManager.currentEXP = PlayerPrefs.GetInt("CurrentEXP");
        playerPosition[0] = PlayerPrefs.GetFloat("PlayerPositionX");
        playerPosition[1] = PlayerPrefs.GetFloat("PlayerPositionY");
        transform.position = new Vector2(playerPosition[0], playerPosition[1]);
        KisainParty = PlayerPrefs.GetInt("hasKisa");

        Debug.Log("HasKisa: " + KisainParty);

        if (KisainParty == 1)
        {
            hasKisa = true;
            pauseMenuManager.partyMemberAdded("Kisa");
            
        }
        else
        {
            hasKisa = false;
        }

        NicolinParty = PlayerPrefs.GetInt("hasNicol");
        if (NicolinParty == 1)
        {
            hasNicol = true;
            pauseMenuManager.partyMemberAdded("NICOL");
        }
        else
        {
            hasNicol = false;
        }

        SophieinParty = PlayerPrefs.GetInt("hasSophie");
        if (SophieinParty == 1)
        {
            hasSophie = true;
            pauseMenuManager.partyMemberAdded("SOPHIE");
        }
        else
        {
            hasSophie = false;
        }

        KisaAbsorbed = PlayerPrefs.GetInt("AbsorbedKisa");
        if (KisaAbsorbed == 1)
        {
            levelManager.kisaAbsorb = true;
            pauseMenuManager.partyMemberKilled("KISA");
        }
        NicolAbsorbed = PlayerPrefs.GetInt("AbsorbedNicol");
        if(NicolAbsorbed == 1)
        {
            levelManager.nicolAbsorb = true;
            pauseMenuManager.partyMemberKilled("NICOL");
        }
        SophieAbsorbed = PlayerPrefs.GetInt("AbsorbedSophie");
        if(SophieAbsorbed == 1)
        {
            levelManager.sophieAbsorb = true;
            pauseMenuManager.partyMemberKilled("SOPHIE");
        }

        Level = PlayerPrefs.GetInt("FloorLevel");
        HasBeenThruTutorial = PlayerPrefs.GetInt("HasBeenThruTutorial");
        if(HasBeenThruTutorial == 1)
        {
            slime.gameObject.SetActive(false);
        }
        BattleTutorialCleared = PlayerPrefs.GetInt("BattleTutorialCleared");
        Debug.Log("Loaded save?");


        Debug.Log(Level);
        if (SophieAbsorbed == 1 && NicolAbsorbed == 1 && KisaAbsorbed == 1)// if they've killed everyone
        {
            mainDialogueManager.dialogueSTART("load_genocide"); // play dialogue when player loads in
        } else if (Level >= 1 && Level <= 4) // just a quick check so dialogue doesn't break. this should be true, but it doesnt hurt to double check
        {
            Debug.Log("load_floor" + Level.ToString());
            mainDialogueManager.dialogueSTART("load_floor" + Level.ToString()); // play dialogue when player loads in. changes based on floor
        }

        levelManager.LoadStats(PlayerPrefs.GetInt("PartyLevel"));

        //God awful door saves
        DoorsOpened = PlayerPrefs.GetInt("DoorsOpened");
        for (int i = 0; i < DoorsOpened;i++)
        {
            Doors[i].gameObject.SetActive(false);
        }

        //delete objects depending on level

        switch (Level)
        {
            case 1:
                //make this collidable
                
                cutscenes[0].SetActive(true);
                if (KisaAbsorbed == 1 || KisainParty == 1)
                {
                    //delete models too
                    cutscenes[5].SetActive(false);
                    cutscenes[9].SetActive(false);
                }
                break;
            case 2:
                cutscenes[1].SetActive(false);
                if (NicolAbsorbed == 1 || NicolinParty == 1)
                {
                    cutscenes[6].SetActive(false);
                    cutscenes[10].SetActive(false);
                }
                break;
            case 3:
                cutscenes[2].SetActive(false);
                if (SophieAbsorbed == 1 || SophieinParty == 1)
                {
                    cutscenes[7].SetActive(false);
                    cutscenes[11].SetActive(false);
                }
                break;
            case 4:
                cutscenes[3].SetActive(false);
                break;
            default:
                break;
        }

        //Map saves
        MapManager.setRoomsDiscovered(1, PlayerPrefs.GetInt("Floor1Explored"));
        MapManager.setRoomsDiscovered(2, PlayerPrefs.GetInt("Floor2Explored"));
        MapManager.setRoomsDiscovered(3, PlayerPrefs.GetInt("Floor3Explored"));
        MapManager.setRoomsDiscovered(4, PlayerPrefs.GetInt("Floor4Explored"));

    }

    public int getDeadCharacters()
    {
        return ((KisaAbsorbed * 100) + (NicolAbsorbed*10) + (SophieAbsorbed));
    }
    public int getUnobtainedCharacters()
    {
        //Passes back the characters you DONT have
        return ((Convert.ToInt32(!hasKisa) * 100) + (Convert.ToInt32(!hasNicol) * 10) + (Convert.ToInt32(!hasSophie)));
    }
    public int getObtainedCharacters()
    {
        //Passes back the characters you have
        return ((Convert.ToInt32(hasKisa) * 100) + (Convert.ToInt32(hasNicol) * 10) + (Convert.ToInt32(hasSophie)));
    }
}
