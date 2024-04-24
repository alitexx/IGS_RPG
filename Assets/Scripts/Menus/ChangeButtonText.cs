using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeButtonText : MonoBehaviour
{

    public TMP_Text buttonText;
    public PlayerController playerController;

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt("BattleTutorialCleared") == 1)
        {
            buttonText.text = "Continue";
        }
        else
        {
            buttonText.text = "Start";
        }
    }
}
