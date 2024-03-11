using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class whatishappening : MonoBehaviour
{

    private void OnDisable()
    {
        Debug.Log("WHO IS DISABLING ME", this);
    }
    private void OnEnable()
    {
        Debug.Log("I just got enabled!", this);
    }
}
