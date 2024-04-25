using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Apple;

public class LevelTeleports : MonoBehaviour
{

    //public Transform Teleport1;
    //public Transform Teleport2;
    public GameObject portalParent1;
    public GameObject portalParent2;
    public GameObject portalParent3;
    public GameObject portalParent4;
    public Collider2D Player;
    public Transform destination1;
    public Transform destination2;
    public Transform destination3;
    public Transform destination4;

    public GameObject ContinueUI;
    public PlayerController PlayerController;

    public audioManager audioManager;

    public float distance = 0.2f;
    public int Level = 1;

    public void Update()
    {
        Level = PlayerController.Level;
    }

    public void OnTriggerEnter2D(Collider2D col)
    {

        if (Vector2.Distance(transform.position, col.transform.position) > distance)
        {
            PauseMenu.canOpenPause = false;
            ContinueUI.SetActive(true);
            PlayerController.isfrozen = true;
        }
        
    }

    public void Next()
    {
        audioManager.playSFX(23);
        PlayerController.isfrozen = false;
        PauseMenu.canOpenPause = true;
        if (Level == 1)
        {
            Player.transform.position = new Vector3(destination1.position.x, destination1.position.y);
            Destroy(portalParent1);
        }
        else if (Level == 2)
        {
            Player.transform.position = new Vector3(destination2.position.x, destination2.position.y);
            Destroy(portalParent2);
        }
        else if(Level == 3)
        {
            Player.transform.position = new Vector3(destination3.position.x, destination3.position.y);
            Destroy(portalParent3);
        }
        else
        {
            Player.transform.position = new Vector3(destination4.position.x, destination4.position.y);
            Destroy(portalParent4);
        }
    }

    public void Stay()
    {
        PauseMenu.canOpenPause = true;
        PlayerController.isfrozen = false;
        ContinueUI.SetActive(false);
        return;
    }
}
