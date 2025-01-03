using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] private GameObject continueButton;
    private void Update()
    {
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(audioStatics.keycodeInterractButton)) && PauseMenu.GamePaused == false)
            {
                PromptAdvance();
                continueButton.SetActive(false);
            }
    }
    public void PromptAdvance()
    {
        DialogueSystem.instance.OnUserPrompt_Next();
    }
}
