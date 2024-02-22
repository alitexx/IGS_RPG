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
            Vector2 distance = new Vector2(TargetRB.position.x - EnemyRB.position.x, TargetRB.position.y - EnemyRB.position.y);
            distance = distance.normalized;
            EnemyRB.AddForce(distance * Speed);
        }
    }
}
