using System.Collections;
using System.Collections.Generic;
//using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 5f;
    public Rigidbody2D rb;
    private bool isfrozen;

    private Vector2 input;

    private Animator animator;

    public LayerMask solidObjectsLayer;

    //party following 
    public Transform[] waypointTrail = new Transform[4];
    public int followGap;

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
        rb.MovePosition(rb.position + input * movementSpeed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);
            SceneManager.LoadScene("BattleCoding"); 
        }
    
    }

}
