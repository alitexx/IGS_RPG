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
    [SerializeField] private CanvasGroup fadeIn;
    [SerializeField] private PlayerController playerController;

    private void Start()
    {
        Time.timeScale = 1f;
        am.playBGM("T1");
        fadeIn.alpha = 1;
        fadeIn.DOFade(0, 1f);
    }

    public void Startgame()
    {
        if (PlayerPrefs.GetInt("BattleTutorialCleared") == 0)
        {
            MainMenuUI.GetComponent<CanvasGroup>().DOFade(0, 1.5f).OnComplete(() => { MainMenuUI.SetActive(false); });
            openingCutscene.dialogueSTART();
        }
        else
        {
            fadeToRPGWorld();
        }
        //playerController.loadGame();
        //SceneManager.LoadScene("OpeningCutscene"); //loads main level
    }

    public void fadeToRPGWorld()
    {
        fadeIn.blocksRaycasts = true;
        fadeIn.DOFade(1, 1f).OnComplete(() =>
        {
            fadeIn.DOKill();
            SceneManager.LoadScene("RPG_World");
        });
    }
    public void fadeToMainMenu()
    {
        fadeIn.blocksRaycasts = true;
        fadeIn.DOFade(1, 1f).OnComplete(() =>
        {
            fadeIn.DOKill();
            SceneManager.LoadScene("TitleScreen");
        });
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
