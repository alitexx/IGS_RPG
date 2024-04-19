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
    [SerializeField] private Animator animator;

    IEnumerator runText()
    {
        // Turn on the animation controller
        animator.enabled = true;

        // Play the animation
        animator.Play("credits");

        // Wait until the animation completes
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            yield return null;
        }

        // Disable the animation controller
        animator.enabled = false;
        textScroll.position = finalTextPos.position;

        fadeOutBG.DOFade(0, 5).OnComplete(() => {
            ReturnButton.DOFade(1, 1);
        });
    }


private void OnEnable()
    {
        switch (endingID)
        {
            case 0:
                charactersOnTitle[0].SetActive(false);
                charactersOnTitle[1].SetActive(false);
                charactersOnTitle[2].SetActive(false);
                endingText.text = "";
                break;
            case 1:
                charactersOnTitle[0].SetActive(false);
                charactersOnTitle[2].SetActive(false);
                break;
            case 2:
                charactersOnTitle[1].SetActive(false);
                charactersOnTitle[2].SetActive(false);
                break;
            case 3:
                charactersOnTitle[1].SetActive(false);
                charactersOnTitle[2].SetActive(false);
                charactersOnTitle[3].SetActive(false);
                break;
            case 4:
                charactersOnTitle[1].SetActive(false);
                break;
            case 5:
                charactersOnTitle[2].SetActive(false);
                break;
            case 6:
                endingText.text = "After the events at the lich’s tower, Alan takes a much-needed break from his duties to prioritize his mental well-being. This time is short-lived, as Alan tends to find himself in the middle of conflict, fighting for the side of justice. He would go on many adventures, but unlike this quest, the temptation to revive his mentor had vanished. He would sometimes get wistful reminiscing about Leora, but he accepted the past was the past and it was time to move on.\r\n\r\nOnce their celebration concluded, Kisa ran home to tell her family of her journey. They were dismissive of her accomplishments and critical of her desire to continue the dangerous life of an adventurer. Kisa was furious, especially since she went through this ordeal to impress them. She ran away days later, choosing a life of excitement over a life of normalcy.\r\n\r\nNicol disappeared soon after the party’s celebration, continuing his carefree exploration of this world. He still treasures the bond he has with his companions, somehow winding up back in Isen for holidays to reunite with them. Despite all of his journey and experiences, Nicol never lost his childlike wonder for uncovering the secrets of the world\r\n\r\nSophie held a small funeral for her fallen students on her arrival home, still feeling a twinge of guilt; if only she knew of the danger awaiting them, she could have prevented their deaths. Twas a fleeting thought, as she knew she could not change the past. Sophie reopened her dojo and expected this to become a distant memory, yet she found herself reminiscing on the fond times with her companions. It was then that she knew how to respond to Kisa, and finally told Kisa the words she wanted to hear.\r\n";
                break;
        }
        audioManager.playBGM("T10");
        fadeInBG.DOFade(0, 2).OnComplete(() => {
            fadeInBG.gameObject.SetActive(false);
            StartCoroutine(runText());
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
