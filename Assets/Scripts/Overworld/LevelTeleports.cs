using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Apple;

public class LevelTeleports : MonoBehaviour
{

    public Transform Teleport1;
    public Transform Teleport2;
    public GameObject portalParent;
    public Collider2D Player;
    private Transform destination;

    public GameObject ContinueUI;
    public PlayerController PlayerController;

    public bool is2;
    public float distance = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        if (is2 == false)
        {
            destination = Teleport2;
        }
        else
        {
            destination = Teleport1;
        }
    }

    public void OnTriggerEnter2D(Collider2D col)
    {

        if (Vector2.Distance(transform.position, col.transform.position) > distance)
        {
            ContinueUI.SetActive(true);
            PlayerController.isfrozen = true;
        }
        
    }

    public void Next()
    {
        PlayerController.isfrozen = false;
        ContinueUI.SetActive(false);
        Player.transform.position = new Vector3(destination.position.x, destination.position.y);
        Destroy(portalParent);

    }

    public void Stay()
    {
        PlayerController.isfrozen = false;
        ContinueUI.SetActive(false);
        return;
    }
}
