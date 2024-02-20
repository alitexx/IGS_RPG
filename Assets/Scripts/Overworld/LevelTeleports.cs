using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Apple;

public class LevelTeleports : MonoBehaviour
{

    public Transform Teleport1;
    public Transform Teleport2;
    public GameObject portalParent;
    private Transform destination;

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

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (Vector2.Distance(transform.position, col.transform.position) > distance)
        {
            col.transform.position = new Vector3(destination.position.x, destination.position.y);
            Destroy(portalParent);
        }
        
    }



}
