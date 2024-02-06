using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace DIALOGUE
{
    [System.Serializable]
    public class DialogueContainer
    {
        public GameObject root; // diabling this hides dialogue
        public NameContainer nameText; // name of the character talking
        public TextMeshProUGUI dialogueText; // the dialogue being spoken

    }
}
