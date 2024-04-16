using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{
    public GameObject MainMenuUI;
    public CanvasGroup fadedbg;
    public OpeningCutscene openingCutscene;
    [SerializeField] private audioManager am;
    [SerializeField] private PlayerController playerController;

    private void Start()
    {
        am.playBGM("T1");
    }

    public void Startgame()
    {
        //ADD SOMETHING HERE ABOUT LOADING SAVE DATA!!
        if (playerController.BattleTutorialCleared == 1)
        {
            playerController.loadGame();
        }
        else
        {
            MainMenuUI.GetComponent<CanvasGroup>().DOFade(0, 1.5f).OnComplete(() => { MainMenuUI.SetActive(false); });
            openingCutscene.dialogueSTART();
            //SceneManager.LoadScene("OpeningCutscene"); //loads main level
        }

    }

    public void QuitGame()
    {
        Application.Quit(); //quits
    }

    public void OptionsOpen()
    {
        fadedbg.gameObject.SetActive(true);
        fadedbg.DOFade(1, 1);
    }

    public void OptionsClose()
    {
        fadedbg.DOFade(0, 1).OnComplete(() => { fadedbg.gameObject.SetActive(false); });
    }
    public void Controls()
    {
        //add code for controls (maybe different scene?)
    }
}
