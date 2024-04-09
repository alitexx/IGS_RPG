using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameoverMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup fade;

    private void Awake()
    {
        fade.DOFade(0, 1).OnComplete(() => {
            fade.gameObject.SetActive(false);
        });
    }
    public void Continue() // This will for now just restart the level. we can worry about save states later
    {
        fade.gameObject.SetActive(true);
        fade.DOFade(1, 1).OnComplete(() => {
            SceneManager.LoadScene("RPG_World"); });

        }

    public void QuitGame()
    {
        fade.gameObject.SetActive(true);
        fade.DOFade(1, 1).OnComplete(() => { SceneManager.LoadScene("TitleScreen"); });
    }
}
