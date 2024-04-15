using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class creditsManager : MonoBehaviour
{
    //used to determine what ending text should pop up
    public static int endingID = 0;
    // 0 = everyone dead
    // 1 = only nicol alive
    // 2 = nicol and sophie alive
    // 3 = kisa alive
    // 4 = kisa and sophie alive
    // 5 = kisa and nicol alive
    // 6 = everyone alive!!
    [SerializeField] private TextMeshProUGUI endingText;
    [SerializeField] private RectTransform textScroll;
    [SerializeField] private RectTransform finalTextPos;
    [SerializeField] private GameObject[] charactersOnTitle;
    [SerializeField] private CanvasGroup fadeOutBG;
    [SerializeField] private CanvasGroup fadeInBG;
    [SerializeField] private CanvasGroup ReturnButton;
    [SerializeField] private audioManager audioManager;
    // 0 = kisa
    // 1 = nicol
    // 2 = sophie
    // 3 = alan


    private void OnEnable()
    {
        //switch (endingID)
        //{
        //    case 0:
        //        charactersOnTitle[0].SetActive(false);
        //        charactersOnTitle[1].SetActive(false);
        //        charactersOnTitle[2].SetActive(false);
        //        break;
        //    case 1:
        //        charactersOnTitle[0].SetActive(false);
        //        charactersOnTitle[2].SetActive(false);
        //        break;
        //    case 2:
        //        charactersOnTitle[1].SetActive(false);
        //        charactersOnTitle[2].SetActive(false);
        //        break;
        //    case 3:
        //        charactersOnTitle[1].SetActive(false);
        //        charactersOnTitle[2].SetActive(false);
        //        charactersOnTitle[3].SetActive(false);
        //        break;
        //    case 4:
        //        charactersOnTitle[1].SetActive(false);
        //        break;
        //    case 5:
        //        charactersOnTitle[2].SetActive(false);
        //        break;
        //}
        audioManager.playBGM("T10");
        fadeInBG.DOFade(0, 2).OnComplete(() => {
            fadeInBG.gameObject.SetActive(false);
            textScroll.DOMove(finalTextPos.position, 50).OnComplete(() => {
                fadeOutBG.DOFade(0, 5).OnComplete(() => {
                    ReturnButton.DOFade(1, 1);
                });
            });
        });
        
    }
    public void ReturnToTitle()
    {
        audioManager.playSFX(25);
        audioManager.stopBGM(2);
        fadeInBG.gameObject.SetActive(true);
        fadeInBG.DOFade(1, 2).OnComplete(() => {
            SceneManager.LoadScene("TitleScreen");
        });
    }

}
