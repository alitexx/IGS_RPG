using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public static bool GamePaused = false;
    [SerializeField] public PlayerController playerController;
    public GameObject PauseMenuUI;
    public GameObject battleUI;
    public GameObject quickPauseUI;
    public GameObject SettingsUI;
    [SerializeField] private audioManager am;
    [SerializeField] private mainDialogueManager mainDialogueManager;
    [SerializeField] private TextMeshProUGUI PartyLevelTXT;
    public static bool canOpenPause = true;

    [SerializeField] private GameObject quickPauseBtn, previouslySelectedBtn;

    private void Start()
    {
        canOpenPause = true;
        GamePaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && canOpenPause) //on escape press
        {
            if (mainDialogueManager.dialogueRunning || battleUI.activeInHierarchy == true)
            {
                if (GamePaused)
                {
                    resume_quickpause();
                }
                else
                {
                    previouslySelectedBtn = EventSystem.current.currentSelectedGameObject;
                    pause_quickpause();
                }
            } else
            {
                if (GamePaused)
                {
                    Resume(); //if its paused, resume
                }
                else
                {
                    Pause(); //if game is going, pause
                    PartyLevelTXT.text = ("Party Level: " + LevelManager.level);
                }
            }
        }
    }

    private void resume_quickpause()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(previouslySelectedBtn);
        am.playSFX(28);
        quickPauseUI.SetActive(false);
        Time.timeScale = 1.0f;
        GamePaused = false; // change bool
        previouslySelectedBtn = null;
    }

    private void pause_quickpause()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(quickPauseBtn);
        am.playSFX(27);
        quickPauseUI.SetActive(true);
        Time.timeScale = 0f;
        GamePaused = true; // change bool
    }


    public void Resume()
    {
        am.playSFX(28);
        PauseMenuUI.SetActive(false); // deactivate the pause menu
        playerController.isfrozen = false;
        GamePaused = false; // change bool
    }
    
    void Pause()
    {
        am.playSFX(27);
        PauseMenuUI.SetActive(true); // activate the pause menu
        playerController.isfrozen = true;
        GamePaused = true; // change bool
    }

    //public void Settings()
    //{
    //    SettingsUI.SetActive(true);
    //    PauseMenuUI.SetActive(false);
    //}

    public void LoadMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        SceneManager.LoadScene("TitleScreen"); // loads menu
    }

    public void QuitGame()
    {
        Application.Quit(); //quits
    }

    public void Back()
    {
        SettingsUI.SetActive(false);
        PauseMenuUI.SetActive(true);
    }
}
