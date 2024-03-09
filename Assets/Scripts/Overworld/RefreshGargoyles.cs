using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshGargoyles : MonoBehaviour
{
    public GameObject Gargoyle;
    public Transform startSpot;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Gargoyle.transform.position = new Vector3(startSpot.position.x, startSpot.position.y);
    }




}
