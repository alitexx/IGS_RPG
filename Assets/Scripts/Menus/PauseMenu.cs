using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GamePaused = false;
    [SerializeField] public PlayerController playerController;
    public GameObject PauseMenuUI;
    public GameObject SettingsUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) //on escape press
        {
            if (GamePaused)
            {
                Resume(); //if its paused, resume
            }
            else
            {
                Pause(); //if game is going, pause
            }
        }
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false); // deactivate the pause menu
        playerController.isfrozen = false;
        GamePaused = false; // change bool
    }
    
    void Pause()
    {
        PauseMenuUI.SetActive(true); // activate the pause menu
        playerController.isfrozen = true;
        GamePaused = true; // change bool
    }

    public void Settings()
    {
        SettingsUI.SetActive(true);
        PauseMenuUI.SetActive(false);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
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
