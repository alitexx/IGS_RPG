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
    }

    public void continueTutorial()
    {
        tutorialCounter++;
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
                tutorialHoles[1].SetActive(true);
                previousHole = tutorialHoles[1];
                targetTutorialText.text = "Use the A and D keys to select a target, then press " + audioStatics.interractButton.ToString() + " to confirm!";
                targetTutorial.SetActive(true);
                //tutorialMenus[tutorialCounter].SetActive(false);
                break;
            case 5:
                tutorialHoles[2].SetActive(true);
                previousHole = tutorialHoles[2];
                break;
            case 7:
                tutorialHoles[3].SetActive(true);
                previousHole = tutorialHoles[3];
                break;
            case 8:
                tutorialHoles[4].SetActive(true);
                previousHole = tutorialHoles[4];
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
        if (Input.GetKeyDown(audioStatics.interractButton) && battleController.state == BattleController.State.Busy && battleController.alanFireMagicButton.activeInHierarchy == false && battleController.backButton.activeInHierarchy) 
        {
            continueTutorial();
        }

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

        if (tutorialCounter == 4)
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

        if (tutorialCounter == 6)
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
        //if (tutorialCounter == 0)
        //{
        //    if (battleController.backButton.activeInHierarchy)
        //    {
        //        targetTutorial.SetActive(true);
        //    }
        //    else
        //    {
        //        targetTutorial.SetActive(false);   
        //    }

        //    if (battleController.fightingButtons.activeInHierarchy)
        //    {
        //        attackTutorial.SetActive(true);
        //        defendTutorial.SetActive(false);
        //        specialTutorial.SetActive(false);
        //        magicTutorial.SetActive(false);
        //    }
        //    else
        //    {
        //        attackTutorial.SetActive(false);
        //        defendTutorial.SetActive(false);
        //        specialTutorial.SetActive(false);
        //        magicTutorial.SetActive(false);
        //    }
        //}
        //else if (tutorialCounter == 1)
        //{
        //    targetTutorial.SetActive(false);

        //    if (battleController.fightingButtons.activeInHierarchy)
        //    {
        //        attackTutorial.SetActive(false);
        //        defendTutorial.SetActive(true);
        //        specialTutorial.SetActive(false);
        //        magicTutorial.SetActive(false);
        //    }
        //    else
        //    {
        //        attackTutorial.SetActive(false);
        //        defendTutorial.SetActive(false);
        //        specialTutorial.SetActive(false);
        //        magicTutorial.SetActive(false);
        //    }
        //}
        //else if (tutorialCounter == 2)
        //{
        //    if (battleController.fightingButtons.activeInHierarchy)
        //    {
        //        attackTutorial.SetActive(false);
        //        defendTutorial.SetActive(false);
        //        specialTutorial.SetActive(true);
        //        magicTutorial.SetActive(false);
        //    }
        //    else
        //    {
        //        attackTutorial.SetActive(false);
        //        defendTutorial.SetActive(false);
        //        specialTutorial.SetActive(false);
        //        magicTutorial.SetActive(false);
        //    }
        //}
        //else if (tutorialCounter == 3)
        //{
        //    if (battleController.backButton.activeInHierarchy && battleController.alanFireMagicButton.activeInHierarchy == false)
        //    {
        //        targetTutorial.SetActive(true);
        //    }
        //    else
        //    {
        //        targetTutorial.SetActive(false);
        //    }

        //    if (battleController.fightingButtons.activeInHierarchy)
        //    {
        //        attackTutorial.SetActive(false);
        //        defendTutorial.SetActive(false);
        //        specialTutorial.SetActive(false);
        //        magicTutorial.SetActive(true);
        //    }
        //    else
        //    {
        //        attackTutorial.SetActive(false);
        //        defendTutorial.SetActive(false);
        //        specialTutorial.SetActive(false);
        //        magicTutorial.SetActive(false);
        //    }
        //}
        //else
        //{
        //    targetTutorial.SetActive(false);
        //    attackTutorial.SetActive(false);
        //    defendTutorial.SetActive(false);
        //    specialTutorial.SetActive(false);
        //    magicTutorial.SetActive(false);
        //}
    }
}
