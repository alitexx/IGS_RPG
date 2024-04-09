using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{
    public GameObject MainMenuUI;
    public OpeningCutscene openingCutscene;

    public void Startgame()
    {
        //ADD SOMETHING HERE ABOUT LOADING SAVE DATA!!

        MainMenuUI.GetComponent<CanvasGroup>().DOFade(0,1.5f).OnComplete(() => { MainMenuUI.SetActive(false); });
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
