using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;

public class PartyBehavior : MonoBehaviour
{
    public enum FollowPosition { FIRST, SECOND, THIRD};

    public FollowPosition myPos;
    public Rigidbody2D rb;

    public float movementSpeed = 5f;
    public GameObject Player;
    public Transform followSlot;
    public int stepsBehind;
    public Queue<Vector3> record = new Queue<Vector3>();
    private Vector3 lastRecord;



    // Update is called once per frame
    void Update()
    {
        if (followSlot == null)
        {
            switch (myPos)
            {
                case FollowPosition.FIRST:
                    followSlot = Player.GetComponent<PlayerController>().waypointTrail[1];
                    break;
                case FollowPosition.SECOND:
                    followSlot = Player.GetComponent<PlayerController>().waypointTrail[2];
                    break;
                case FollowPosition.THIRD:
                    followSlot = Player.GetComponent<PlayerController>().waypointTrail[3];
                    break;
            }
        }


        if (Vector3.Distance(lastRecord, rb.position) > stepsBehind)
        {
            rb.position = Vector2.MoveTowards(rb.position, followSlot.transform.position, movementSpeed * Time.deltaTime);
        }
    }
}
