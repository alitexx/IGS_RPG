using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchScript : MonoBehaviour
{

    public GameObject Door;
    public Rigidbody2D RB;
    public bool isOnSwitch;
    public audioManager audioManager;
    public GameObject Switch;
    public PlayerController playerController;

    [SerializeField] private mapManager mapManager;
    [SerializeField] private GameObject associatedExclamation;


    private void Start()
    {
        RB = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && isOnSwitch == false)
        {
            RB.isKinematic = false;
            RB.velocity = Vector3.zero;
            audioManager.playSFX(21);
        }
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && isOnSwitch == false)
        {
            RB.isKinematic = true;
            RB.velocity = Vector3.zero;
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "Switch")
        {
            gameObject.transform.position = Switch.transform.position;
            playerController.DoorsOpened += 1;
            audioManager.playSFX(20);
            Debug.Log("Collided");
            RB.velocity = Vector3.zero;
            RB.isKinematic = true;
            isOnSwitch = true;
            audioManager.playSFX(22);
            Destroy(Door);
            mapManager.ForceOpenMap(associatedExclamation);
        }
    }
}
