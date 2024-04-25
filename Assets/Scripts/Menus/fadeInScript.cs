using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class fadeInScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<CanvasGroup>().alpha = 1;
        this.GetComponent<CanvasGroup>().DOFade(0, 1f);
    }

    public void fadeToRPGWorld()
    {
        this.GetComponent<CanvasGroup>().DOFade(1, 1f).OnComplete(() =>
            {
                SceneManager.LoadScene("RPG_World");
            });
    }
    public void fadeToMainMenu()
    {
        this.GetComponent<CanvasGroup>().DOFade(1, 1f).OnComplete(() =>
        {
            SceneManager.LoadScene("TitleScreen");
        });
    }
}
