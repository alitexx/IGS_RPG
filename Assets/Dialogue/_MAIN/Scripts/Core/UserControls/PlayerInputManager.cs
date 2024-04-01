using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

public class PlayerInputManager : MonoBehaviour
{
    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) && PauseMenu.GamePaused == false)
        {
             PromptAdvance();
        }
    }
    public void PromptAdvance()
    {
        DialogueSystem.instance.OnUserPrompt_Next();
    }
}
