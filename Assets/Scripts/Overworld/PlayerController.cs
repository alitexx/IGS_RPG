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
    public bool tutorialFight = false;
    //bosses
    public bool KisaBoss = false;
    public bool NicolBoss = false;
    public bool SophieBoss = false;
    public bool LichBoss = false;

    //Level Number
    public int Level = 1;

    //Camera Movement Detecter
    public CamMovementDetect cameraMovementDetecter;


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

        BattleUI.SetActive(false);
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

        if (Vector3.Distance(rb.transform.position, waypointTrail[0].position) >= followGap)
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

            if (collision.gameObject.name == "1")
            {
                KisaBoss = true;
            }
            else if (collision.gameObject.name == "2")
            {
                NicolBoss = true;
            }
            else if (collision.gameObject.name == "3")
            {
                SophieBoss = true;
            }
            else
            {
                LichBoss = true;
            }

            //do anything else (like party members)

            battleFade.SetBool("BattleStarting", true);
            Destroy(collision.gameObject);
        }

        if (collision.tag == "DialogueFreeze")
        {
            //do something with dialogue idk lmao
        }

        
        if (collision.gameObject.name == "level2")
        {
            Level = 2;
        }

        if (collision.gameObject.name == "level3")
        {
            Level = 3;
        }

        if (collision.gameObject.name == "level4")
        {
            Level = 4;
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

}
