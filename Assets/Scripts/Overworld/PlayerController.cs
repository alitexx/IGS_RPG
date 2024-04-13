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
    public float[] playerPosition = new float[2];
    int KisainParty = 0;
    int NicolinParty = 0;
    int SophieinParty = 0;
    int KisaAbsorbed = 0;
    int NicolAbsorbed = 0;
    int SophieAbsorbed = 0;
    int HasBeenThruTutorial = 0;

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


    private void Start()
    {
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

        partyLevel = LevelManager.level;

        playerPosition[0] = transform.position.x;
        playerPosition[1] = transform.position.y;

        if(hasKisa == true) 
        {
            KisainParty = 1;
        }

        if (hasNicol == true)
        {
            NicolinParty = 1;
        }

        if (hasSophie == true)
        {
            SophieinParty = 1;
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
                    else if (hasKisa)
                    {
                        mainDialogueManager.dialogueSTART("sophieEncounter_xn");
                    }
                    else
                    {
                        mainDialogueManager.dialogueSTART("sophieEncounter_xx");
                    }
                    break;
                default:
                    //i dont have these done yet
                    LichBoss = true;
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

        
        if (collision.gameObject.name == "level2")
        {
            Level = 2;
            if (hasKisa)
            {
                mainDialogueManager.dialogueSTART("secondFloor_k");
            }
            //Do anything else if needed
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.name == "level3")
        {
            Level = 3;
            if (hasKisa && hasNicol)
            {
                mainDialogueManager.dialogueSTART("thirdFloor_kn");
            }
            //Do anything else if needed
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.name == "level4")
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
            }

            if (collision.gameObject.tag == "TutorialSlime")
            {
                Destroy(collision.gameObject);
                isfrozen = true;
                isSlime = true;
                tutorialFight = true;
                tutorialHandler.SetActive(true);
                battleFade.SetBool("BattleStarting", true);
            }

            if (collision.gameObject.tag == "Wraith")
            {
                Destroy(collision.gameObject);
                isfrozen = true;
                isWraith = true;
                battleFade.SetBool("BattleStarting", true);
            }

            if (collision.gameObject.tag == "InvisGuy")
            {
                Destroy(collision.gameObject);
                isfrozen = true;
                isInvisGuy = true;
                battleFade.SetBool("BattleStarting", true);
            }

            if (collision.gameObject.tag == "Skeleton")
            {
                Destroy(collision.gameObject);
                isfrozen = true;
                isSkeleton = true;
                battleFade.SetBool("BattleStarting", true);
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
        PlayerPrefs.SetInt("PartyLevel", partyLevel);
        PlayerPrefs.SetFloat("PlayerPositionX", playerPosition[0]);
        PlayerPrefs.SetFloat("PlauerPositionY", playerPosition[1]);
        PlayerPrefs.SetInt("hasKisa", KisainParty);
        PlayerPrefs.SetInt("hasNicol", NicolinParty);
        PlayerPrefs.SetInt("hasSophie", SophieinParty);
        PlayerPrefs.SetInt("AbsorbedKisa", KisaAbsorbed);
        PlayerPrefs.SetInt("AbsorbedNicol", NicolAbsorbed);
        PlayerPrefs.SetInt("AbsorbedSophie", SophieAbsorbed);
        PlayerPrefs.SetInt("FloorLevel", Level);
        PlayerPrefs.SetInt("HasBeenThruTutorial", HasBeenThruTutorial);
        PlayerPrefs.Save();
    }

    public void DeleteSave() 
    {
        PlayerPrefs.DeleteAll();
    }

    public void loadGame() 
    {
        LevelManager.level = PlayerPrefs.GetInt("PartyLevel");
        playerPosition[0] = PlayerPrefs.GetFloat("PlayerPositionX");
        playerPosition[1] = PlayerPrefs.GetFloat("PlayerPositiony");
        transform.position = new Vector2(playerPosition[0], playerPosition[1]);
        KisainParty = PlayerPrefs.GetInt("hasKisa");
        if (KisainParty == 1)
        {
            hasKisa = true;
        }
        else
        {
            hasKisa = false;
        }

        NicolinParty = PlayerPrefs.GetInt("hasNicol");
        if (NicolinParty == 1)
        {
            hasNicol = true;
        }
        else
        {
            hasNicol = false;
        }

        SophieinParty = PlayerPrefs.GetInt("hasSophie");
        if (SophieinParty == 1)
        {
            hasSophie = true;
        }
        else
        {
            hasSophie = false;
        }

        KisaAbsorbed = PlayerPrefs.GetInt("AbsorbedKisa");
        NicolAbsorbed = PlayerPrefs.GetInt("AbsorbedNicol");
        SophieAbsorbed = PlayerPrefs.GetInt("AbsorbedSophie");

        Level = PlayerPrefs.GetInt("FloorLevel");
        HasBeenThruTutorial = PlayerPrefs.GetInt("HasBeenThruTutorial");


    }

}
