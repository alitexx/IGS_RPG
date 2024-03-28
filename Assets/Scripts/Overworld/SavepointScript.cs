using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavepointScript : MonoBehaviour
{

    public GameObject Target;
    float maxDistance = 1.5f;
    float DistanceBetweenObjects;
    public LevelManager levelManager;

    // Start is called before the first frame update
    private void Update()
    {

        DistanceBetweenObjects = Vector3.Distance(transform.position, Target.transform.position);

        if (DistanceBetweenObjects <= maxDistance && Input.GetKeyDown("space"))
        {
            //call function to heal, and eventually save.
            levelManager.FullHeal();
            //confirm save/heal as a menu or something (name.setactive(true)
            Debug.Log("YIPEEEE");
        }

    }
}
