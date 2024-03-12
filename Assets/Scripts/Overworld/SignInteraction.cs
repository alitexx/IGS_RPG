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
        DistanceBetweenObjects = Vector3.Distance(transform.position, Target.transform.position);

        if (DistanceBetweenObjects <= maxDistance)
        {
            StartCoroutine(SignMessage());
        }
    }

    private IEnumerator SignMessage()
    {
        SignUI.SetActive(true);
        yield return new WaitForSeconds(5);
        SignUI.SetActive(false);
    }

}
