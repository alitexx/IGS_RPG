using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverScript : MonoBehaviour
{
    public GameObject Door;
    public GameObject Target;
    float maxDistance = 1.5f;
    float DistanceBetweenObjects;

    private void Update()
    {
        
        DistanceBetweenObjects = Vector3.Distance(transform.position, Target.transform.position);

        if (DistanceBetweenObjects <= maxDistance && Input.GetKeyDown("space"))
        {
            Debug.Log(":D");
            Destroy(Door);
        }
    
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && Input.GetKeyDown("space"))
        {
            Debug.Log(":D");
            Destroy(Door);
        }
    }
}
