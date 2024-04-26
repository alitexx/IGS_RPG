using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float Speed = 3f;
    public Rigidbody2D TargetRB;
    public Rigidbody2D EnemyRB;
    public GameObject Target;
    public float maxDistance = 5f;
    public float fleeDistanceThreshold = 10f; // Adjust this value as needed
    float DistanceBetweenObjects;
    private Animator animator;
    private Vector2 input;
    public PlayerController PlayerController;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", 0);

    }


        // Update is called once per frame
    void Update()
    {
        if (DistanceBetweenObjects > maxDistance)
        {
            animator.SetBool("isMoving", false);
        }

        //Movement
        DistanceBetweenObjects = Vector3.Distance(transform.position, Target.transform.position);
        if(PlayerController.isfrozen == false)
        {
            if ((DistanceBetweenObjects <= maxDistance) /*&& (PlayerController.isfrozen == false)*/)
            {
                if (PlayerController.partyLevel >3*PlayerController.Level)
                {
                    Vector3 moveDirection = (transform.position - TargetRB.transform.position).normalized;
                    // Player is high level, so flee from the player
                    EnemyRB.transform.position = Vector2.MoveTowards(EnemyRB.transform.position, transform.position + moveDirection * fleeDistanceThreshold, (Speed * Time.deltaTime/1.25f));
                }
                else
                {
                    // Player is not high level or too close, so approach the player
                    EnemyRB.transform.position = Vector2.MoveTowards(EnemyRB.transform.position, TargetRB.transform.position, Speed * Time.deltaTime);
                }

                //EnemyRB.transform.position = Vector2.MoveTowards(EnemyRB.transform.position, TargetRB.transform.position, Speed * Time.deltaTime);

                //Animations
                animator.SetBool("isMoving", true);

                if (Target.transform.position.x > transform.position.x)
                {
                    input.x = 1;
                }
                else
                {
                    input.x = -1;
                }

                if (Target.transform.position.y > transform.position.y)
                {
                    input.y = 1;
                }
                else
                {
                    input.y = -1;
                }

                if (input.x > .5) input.y = 0;
                if (input.y > .5) input.x = 0;

                if (input != Vector2.zero)
                {
                    animator.SetFloat("moveX", input.x);
                    animator.SetFloat("moveY", input.y);
                }

                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);
                animator.SetFloat("speed", input.sqrMagnitude);
            }
        }
        
    }
}