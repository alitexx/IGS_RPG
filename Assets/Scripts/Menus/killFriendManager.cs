using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class killFriendManager : MonoBehaviour
{
    [SerializeField] private GameObject tutorialMenu;
    [SerializeField] private GameObject areYouSureMenu;
    [SerializeField] private TextMeshProUGUI areYouSureText;
    [SerializeField] private CanvasGroup tutorialBG;
    [SerializeField] private GameObject truebgFade;
    [SerializeField] private RectTransform[] locations;
    [SerializeField] private RectTransform[] tweenInObjects;
    [SerializeField] private TextMeshProUGUI[] killText;
    [SerializeField] private TextMeshProUGUI[] befriendText;
    [SerializeField] private Image[] elementIcon;
    // might be needed later, commented out for now
    //[SerializeField] private LevelManager levelManager;
    [SerializeField] private Sprite[] elementsSprites;

    private void Awake()
    {
        if (true) // replace this with if they've seen the tutorial
        {
            tutorialBG.DOFade(1, 1);
            tutorialMenu.SetActive(true);
            tutorialMenu.GetComponent<RectTransform>().DOMove(locations[0].position, 1);
        }
        //else
        //{
        //    truebgFade.SetActive(true);
        //    tweenInObjects[2].GetComponent<RectTransform>().DOMove(locations[0].position, 1).OnComplete(() => {
        //        tweenInObjects[0].GetComponent<RectTransform>().DOMove(locations[0].position, 1);
        //        tweenInObjects[1].GetComponent<RectTransform>().DOMove(locations[0].position, 1);
        //    });
        //    tutorialBG.DOFade(0, 1).OnComplete(() => { tutorialBG.gameObject.SetActive(false); truebgFade.SetActive(true); });
        //}
    }

    public void closeKillFriendTutorial()
    {
        tutorialMenu.GetComponent<RectTransform>().DOMove(locations[1].position, 1).OnComplete(() => { tutorialMenu.SetActive(false); });
        tweenInObjects[2].GetComponent<RectTransform>().DOMove(locations[0].position, 1).OnComplete(() => {
            tweenInObjects[0].GetComponent<RectTransform>().DOMove(locations[0].position, 1);
            tweenInObjects[1].GetComponent<RectTransform>().DOMove(locations[0].position, 1);
        });
        tutorialBG.DOFade(0, 1).OnComplete(() => { tutorialBG.gameObject.SetActive(false); truebgFade.SetActive(true); });
    }


    public void EditTextInformation(string bossName)
    {
        switch (bossName.ToUpper())
        {
            //placeholders right now because my computer is stupid
            case "KISA":
                elementIcon[0].sprite = elementsSprites[0];
                elementIcon[1].sprite = elementsSprites[0];
                killText[0].text = "";
                killText[1].text = "";
                befriendText[0].text = "";
                befriendText[1].text = "";
                befriendText[2].text = "";
                break;
            case "NICOL":
                elementIcon[0].sprite = elementsSprites[1];
                elementIcon[1].sprite = elementsSprites[1];
                killText[0].text = "";
                killText[1].text = "";
                befriendText[0].text = "";
                befriendText[1].text = "";
                befriendText[2].text = "";
                break;
            case "SOPHIE":
                elementIcon[0].sprite = elementsSprites[2];
                elementIcon[1].sprite = elementsSprites[2];
                killText[0].text = "";
                killText[1].text = "";
                befriendText[0].text = "";
                befriendText[1].text = "";
                befriendText[2].text = "";
                break;
        }
    }

    public void exitKillFriendMenu()
    {
        truebgFade.SetActive(false);
        closeAreYouSure();
        tweenInObjects[0].GetComponent<RectTransform>().DOMove(locations[2].position, 1);
        tweenInObjects[1].GetComponent<RectTransform>().DOMove(locations[3].position, 1);
        tweenInObjects[2].GetComponent<RectTransform>().DOMove(locations[4].position, 1);
    }

    public void openAreYouSure(bool isKilling)
    {
        tutorialBG.gameObject.SetActive(true);
        tutorialBG.DOFade(1, 1);
        if (isKilling)
        {
            areYouSureText.text = "You have chosen to kill Kisa. Are you sure?";
        }
        else
        {
            areYouSureText.text = "You have chosen to befriend Kisa. Are you sure?";
        }
        areYouSureMenu.SetActive(true);
        areYouSureMenu.GetComponent<RectTransform>().DOMove(locations[0].position, 1);
    }

    public void closeAreYouSure()
    {
        tutorialBG.DOFade(0, 1).OnComplete(() => { tutorialBG.gameObject.SetActive(false);});
        areYouSureMenu.GetComponent<RectTransform>().DOMove(locations[1].position, 1).OnComplete(() => { areYouSureMenu.SetActive(false); });
    }

}
