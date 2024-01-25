using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameoverMenu : MonoBehaviour
{
    public GameObject MainMenuUI;

    public void Continue() // This will for now just restart the level. we can worry about save states later
    {
        SceneManager.LoadScene("RPG_World"); //loads main level
    }

    public void QuitGame()
    {
        Application.Quit(); //quits
    }
}
