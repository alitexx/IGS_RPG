using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject MainMenuUI;
    public OpeningCutscene openingCutscene;

    public void Startgame()
    {
        MainMenuUI.SetActive(false);
        openingCutscene.dialogueSTART();
        //SceneManager.LoadScene("OpeningCutscene"); //loads main level
    }

    public void QuitGame()
    {
        Application.Quit(); //quits
    }

    public void Options()
    {
        //add code for options (maybe different scene?)
    }

    public void Controls()
    {
        //add code for controls (maybe different scene?)
    }
}
