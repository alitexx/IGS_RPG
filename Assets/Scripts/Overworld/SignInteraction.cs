using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignInteraction : MonoBehaviour
{
    public GameObject SignUI;
    public GameObject Target;
    float maxDistance = 5f;
    float DistanceBetweenObjects;

    void Update()
    {
        SignUI.SetActive(false);

        DistanceBetweenObjects = Vector3.Distance(transform.position, Target.transform.position);

        if (DistanceBetweenObjects <= maxDistance)
        {
            SignUI.SetActive(true);
        }
           
        if(DistanceBetweenObjects > maxDistance)
        {
            SignUI.SetActive(false);
        }
    }
}
