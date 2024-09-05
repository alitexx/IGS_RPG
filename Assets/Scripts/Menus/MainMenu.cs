using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;

public class MainMenu : MonoBehaviour
{
    //Used for displaying the version of lone labyrinth
    public TextMeshProUGUI versionText;
    
    //For controller/WASD accessibility
    public GameObject onOpenFirstButton, optionsFirstButton, optionsClosedButton;


    public GameObject MainMenuUI;
    public GameObject skipBTN;
    public CanvasGroup fadedbg;
    public OpeningCutscene openingCutscene;
    [SerializeField] private audioManager am;
    [SerializeField] private CanvasGroup fadeIn;
    [SerializeField] private PlayerController playerController;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(onOpenFirstButton);
        skipBTN.SetActive(false);
        Time.timeScale = 1f;
        am.playBGM("T1");
        fadeIn.alpha = 1;
        fadeIn.DOFade(0, 1f);
        if (versionText)
        {
            versionText.text = "Version:" + Application.version;
        }
    }

    public void Startgame()
    {
        if (PlayerPrefs.GetInt("BattleTutorialCleared") == 0)
        {
            MainMenuUI.GetComponent<CanvasGroup>().DOFade(0, 1.5f).OnComplete(() => { MainMenuUI.SetActive(false); });
            openingCutscene.dialogueSTART();
            skipBTN.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(skipBTN);
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
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsFirstButton);
    }

    public void OptionsClose()
    {
        fadedbg.DOFade(0, 1).OnComplete(() => { fadedbg.gameObject.SetActive(false); });
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsClosedButton);
    }
    public void Controls()
    {
        //add code for controls (maybe different scene?)
    }
}
