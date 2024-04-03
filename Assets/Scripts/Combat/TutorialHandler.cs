using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHandler : MonoBehaviour
{
    public BattleController battleController;

    public GameObject attackTutorial;
    public GameObject defendTutorial;
    public GameObject specialTutorial;
    public GameObject magicTutorial;
    public GameObject targetTutorial;

    private int tutorialCounter;

    private void Start()
    {
        tutorialCounter = 0;

        attackTutorial.SetActive(false);
        defendTutorial.SetActive(false);
        specialTutorial.SetActive(false);
        magicTutorial.SetActive(false);
        targetTutorial.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(battleController.confirmKey) && battleController.state == BattleController.State.Busy && battleController.backButton.activeInHierarchy && battleController.alanFireMagicButton.activeInHierarchy == false)
        {
            tutorialCounter++;
        }

        if (tutorialCounter == 0)
        {
            if (battleController.backButton.activeInHierarchy)
            {
                targetTutorial.SetActive(true);
            }
            else
            {
                targetTutorial.SetActive(false);   
            }

            if (battleController.fightingButtons.activeInHierarchy)
            {
                attackTutorial.SetActive(true);
                defendTutorial.SetActive(false);
                specialTutorial.SetActive(false);
                magicTutorial.SetActive(false);
            }
            else
            {
                attackTutorial.SetActive(false);
                defendTutorial.SetActive(false);
                specialTutorial.SetActive(false);
                magicTutorial.SetActive(false);
            }
        }
        else if (tutorialCounter == 1)
        {
            targetTutorial.SetActive(false);

            if (battleController.fightingButtons.activeInHierarchy)
            {
                attackTutorial.SetActive(false);
                defendTutorial.SetActive(true);
                specialTutorial.SetActive(false);
                magicTutorial.SetActive(false);
            }
            else
            {
                attackTutorial.SetActive(false);
                defendTutorial.SetActive(false);
                specialTutorial.SetActive(false);
                magicTutorial.SetActive(false);
            }
        }
        else if (tutorialCounter == 2)
        {
            if (battleController.fightingButtons.activeInHierarchy)
            {
                attackTutorial.SetActive(false);
                defendTutorial.SetActive(false);
                specialTutorial.SetActive(true);
                magicTutorial.SetActive(false);
            }
            else
            {
                attackTutorial.SetActive(false);
                defendTutorial.SetActive(false);
                specialTutorial.SetActive(false);
                magicTutorial.SetActive(false);
            }
        }
        else if (tutorialCounter == 3)
        {
            if (battleController.backButton.activeInHierarchy && battleController.alanFireMagicButton.activeInHierarchy == false)
            {
                targetTutorial.SetActive(true);
            }
            else
            {
                targetTutorial.SetActive(false);
            }

            if (battleController.fightingButtons.activeInHierarchy)
            {
                attackTutorial.SetActive(false);
                defendTutorial.SetActive(false);
                specialTutorial.SetActive(false);
                magicTutorial.SetActive(true);
            }
            else
            {
                attackTutorial.SetActive(false);
                defendTutorial.SetActive(false);
                specialTutorial.SetActive(false);
                magicTutorial.SetActive(false);
            }
        }
        else
        {
            targetTutorial.SetActive(false);
            attackTutorial.SetActive(false);
            defendTutorial.SetActive(false);
            specialTutorial.SetActive(false);
            magicTutorial.SetActive(false);
        }
    }
}
