using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class fadeInScript : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadeIn;


    // Start is called before the first frame update
    void Start()
    {
        if (fadeIn)
        {
            fadeIn.alpha = 1;
            fadeIn.DOFade(0, 1f);
        }
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
}
