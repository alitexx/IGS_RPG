using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpButtons : MonoBehaviour
{
    public GameObject helpText;

    private void Start()
    {
        helpText.SetActive(false);
    }

    public void OpenHelp()
    {
        helpText.SetActive(true);
    }

    public void CloseHelp() 
    { 
        helpText.SetActive(false);
    }
}
