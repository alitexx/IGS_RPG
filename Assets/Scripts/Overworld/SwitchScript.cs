using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchScript : MonoBehaviour
{

    public GameObject Door;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Switch")
        {
            Debug.Log("Collided");
            Destroy(Door);
        }
    }
}
