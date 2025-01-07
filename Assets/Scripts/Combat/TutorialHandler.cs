using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TutorialHandler : MonoBehaviour
{

    public BattleController battleController;

    public GameObject attackTutorial;
    public GameObject defendTutorial;
    public GameObject specialTutorial;
    public GameObject magicTutorial;
    public GameObject targetTutorial;

    //0= attack, 1=defend, 2 = special, 3= magic
    [SerializeField] private GameObject[] buttons;

    [SerializeField] private GameObject[] tutorialMenus;
    public GameObject[] tutorialHoles; // used to put holes in the dark bg so players can see things
    [SerializeField] private TextMeshProUGUI targetTutorialText;

    [SerializeField] private CanvasGroup darkBG;

    public int tutorialCounter;
    private GameObject previousHole; // for turning on/off the holes in the tutorial


    private void Start()
    {
        tutorialCounter = 0;

        attackTutorial.SetActive(false);
        defendTutorial.SetActive(false);
        specialTutorial.SetActive(false);
        magicTutorial.SetActive(false);
        targetTutorial.SetActive(false);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetActive(false);
        }
    }

    public void continueTutorial()
    {
        Debug.Log("CONTINUED TUTORIAL FROM "+tutorialCounter.ToString() + " TO " +(tutorialCounter+1).ToString());
        tutorialCounter++;
        //if(tutorialCounter == 4 && firstTime4 == false)
        //{
        //    tutorialCounter--;
        //    firstTime4 = true;
        //    return;
        //} else if (tutorialCounter == 5 && firstTime5 == false)
        //{
        //    Debug.Log("Womp womp next tutorial skipped");
        //    targetTutorial.SetActive(false);
        //    tutorialCounter--;
        //    firstTime5 = true;
        //    return;
        //} else 
        if (tutorialCounter >= 9)
        {
            if (tutorialCounter == 10)
            {
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].SetActive(false);
                }
            } else
            {
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].SetActive(true);
                }
            }
            return;
        }
        tutorialMenus[tutorialCounter - 1].SetActive(false);
        tutorialMenus[tutorialCounter].SetActive(true);
        if (previousHole)
        {
            previousHole.SetActive(false);
            previousHole = null;
        }
        switch (tutorialCounter)
        {
            case 1:
                tutorialHoles[0].SetActive(true);
                previousHole = tutorialHoles[0];
                break;
            case 3: // activate target tutorial
                buttons[0].SetActive(true);
                tutorialHoles[1].SetActive(true);
                previousHole = tutorialHoles[1];
                targetTutorialText.text = "Use the A and D keys to select a target, then press <color=#ad2f45>" + audioStatics.interractButton.ToString().ToUpper() + "</color> to confirm!";
                targetTutorial.SetActive(true);
                //tutorialMenus[tutorialCounter].SetActive(false);
                break;
            case 4:
                targetTutorial.SetActive(false);
                tutorialHoles[3].SetActive(true);
                previousHole = tutorialHoles[3];
                buttons[0].SetActive(false);
                buttons[1].SetActive(true);
                break;
            case 5:
                tutorialHoles[2].SetActive(true);
                previousHole = tutorialHoles[2];
                buttons[1].SetActive(false);
                buttons[2].SetActive(true);
                break;
            case 7:
                tutorialHoles[4].SetActive(true);
                previousHole = tutorialHoles[4];
                buttons[2].SetActive(false);
                buttons[3].SetActive(true);
                break;
            case 8:
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].SetActive(true);
                }
                break;
        }
    }

    public void fadeOutBG()
    {
        darkBG.DOFade(0, 0.75f);
    }

    public void fadeInBG()
    {
        darkBG.DOFade(1, 0.75f);
    }

    private void Update()
    {
        //if (Input.GetKeyDown(audioStatics.keycodeInterractButton) && battleController.state == BattleController.State.Busy && battleController.alanFireMagicButton.activeInHierarchy == false && battleController.backButton.activeInHierarchy) 
        //{
        //    continueTutorial();
        //} else if (Input.GetKeyDown(audioStatics.keycodeInterractButton) && (tutorialCounter == 6 || tutorialCounter == 7))
        //{
        //    continueTutorial();
        //}

        if (battleController.coroutineRunning == true)
        {
            targetTutorial.SetActive(true);
        }
        else
        {
            targetTutorial.SetActive(false);
        }

        if (battleController.state == BattleController.State.Busy)
        {
            fadeOutBG();
        }
        else
        {
            fadeInBG();
        }

        if (tutorialCounter == 3 || tutorialCounter == 4 || tutorialCounter == 7)
        {
            if (battleController.state == BattleController.State.WaitingForPlayer)
            {
                tutorialMenus[tutorialCounter].SetActive(true);
            }
            else
            {
                tutorialMenus[tutorialCounter].SetActive(false);
            }
        }
    }
}
