using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadifSaved : MonoBehaviour
{

    public PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("BattleTutorialCleared") == 1)
        {
            //called
            playerController.loadGame();
        }
    }

}
