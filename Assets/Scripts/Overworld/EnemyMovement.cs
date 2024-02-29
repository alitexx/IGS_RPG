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
    float maxDistance = 5f;
    float DistanceBetweenObjects;


    // Update is called once per frame
    void Update()
    {
        DistanceBetweenObjects = Vector3.Distance(transform.position, Target.transform.position);

        if(DistanceBetweenObjects <= maxDistance)
        {
            EnemyRB.transform.position = Vector2.MoveTowards(EnemyRB.transform.position, TargetRB.transform.position, Speed * Time.deltaTime);
        }
    }
}
