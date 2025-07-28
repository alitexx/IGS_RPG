using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class fadeInScript : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadeIn;
    [SerializeField] private mapManager mapManager;
    private PlayerController playerController;


    // Start is called before the first frame update
    void Start()
    {
        fadeIn.alpha = 1;
        fadeIn.DOFade(0, 1f);
        playerController = FindObjectOfType<PlayerController>();
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
            PauseMenu.GamePaused = false;
            SceneManager.LoadScene("TitleScreen");
        });
    }

    public void fadeToMap()
    {
        //Fade to black. Wait.
        //Fade back in once menu was open
        PauseMenu.canOpenPause = false;
        fadeIn.blocksRaycasts = true;
        fadeIn.DOFade(1, 0.4f).OnComplete(() =>
        {
            fadeIn.DOFade(1, 0.5f).OnComplete(() =>
            {
                mapManager.OpenMap();
                fadeIn.DOFade(0, 0.5f).OnComplete(() =>
                {
                    mapManager.setOpeningClosingMenu(false);
                });
            });
        });
    }
    public void fadeOutOfmap()
    {
        //Fade to black. Wait.
        //Fade back in once menu was closed
        fadeIn.blocksRaycasts = true;
        fadeIn.DOFade(1, 0.4f).OnComplete(() =>
        {
            fadeIn.DOFade(1, 0.5f).OnComplete(() =>
            {
                mapManager.CloseMap();
                fadeIn.DOFade(0, 0.5f).OnComplete(() =>
                {
                    mapManager.setOpeningClosingMenu(false);
                    playerController.isfrozen = false;
                    PauseMenu.canOpenPause = true;
                });
            });
        });
    }
}
