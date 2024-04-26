using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CHARACTERS;
using UnityEngine.UI;

namespace DIALOGUE
{
    // recieves the lines from a file. gives the OK to conversation manager to print those lines

    public class DialogueSystem : MonoBehaviour
    {
        [SerializeField]private DialogueSystemConfigurationSO _config;
        public DialogueSystemConfigurationSO config => _config;


        public DialogueContainer dialogueContainer = new DialogueContainer();
        private ConversationManager conversationManager;
        public TextArchitect architect;

        public static DialogueSystem instance { get; private set; }

        public delegate void DialogueSystemEvent();
        public event DialogueSystemEvent onUserPrompt_Next;

        public AudioSource charVoice;
        public Image textbox;

        public GameObject continueButton; // for the bottom of the screen

        public bool isRunningConversation => conversationManager.isRunning;

        public void setTextSpeed(float value)
        {
            architect.speedMultiplier = value;
        }

        private void Awake() 
        {
            if (instance == null)
            {
                instance = this;
                Initialize();
            }
            else
            {
                DestroyImmediate(gameObject);
            }
        }

        bool _initialized = false;
        private void Initialize()
        {
            if (_initialized) { return; }
            architect = new TextArchitect(dialogueContainer.dialogueText);
            architect.continueButton = this.continueButton;
            architect.speedMultiplier = audioStatics.TextSpeedMultiplier;
            // to share architects
            conversationManager = new ConversationManager(architect, charVoice);
        }

        public void OnUserPrompt_Next()
        {
            onUserPrompt_Next?.Invoke();
        }

        public void ApplySpeakerDataToDialogueContainer(string speakerName)
        {
            Character character = CharacterManager.instance.GetCharacter(speakerName);
            CharacterConfigData config = character != null ? character.config : CharacterManager.instance.GetCharacterConfig(speakerName);

            //////////////////////////////////////CHANGE GUI STUFF HERE!!!!!!!!!!!! USE CONFIG!!!!!!!!!!!!!!!!!!

            textbox.sprite = config.charNameHolder;
            charVoice.clip = config.charVoice;


        }


        public void ShowSpeakerName(string speakerName = "")
        {
            dialogueContainer.nameText.Show(speakerName);
            //if (speakerName.ToLower() != "narrator")
            //{
            //    dialogueContainer.nameText.Show(speakerName);
            //}
            //else
            //{
            //    HideSpeakerName();
            //}
        }

        //come here if you want to change name color
        //public void HideSpeakerName() => dialogueContainer.nameText.Hide();

        public Coroutine Say(string speaker, string dialogue)
        {
            List<string> conversation = new List<string>() { $"{speaker} \"{dialogue}\"" };
            return Say(conversation);
        }

        public Coroutine Say(List<string> conversation)
        {
            return conversationManager.StartConversation(conversation);
        }

    }
}
