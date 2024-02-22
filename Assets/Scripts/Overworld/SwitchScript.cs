using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchScript : MonoBehaviour
{

    public GameObject Door;
    //public RigidbodyConstraints2D pos;
    


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Switch")
        {
            Debug.Log("Collided");
            //pos = RigidbodyConstraints2D.FreezePosition;
            Destroy(Door);
        }
    }
}
