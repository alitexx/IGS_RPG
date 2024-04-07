using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;
using DIALOGUE;
using DG.Tweening;

public class mainDialogueManager : MonoBehaviour
{
    //pass in values to determine where the story is/what text should be presented
    //values include:
    //int floor === defaults to 0. determines if this is a boss dialogue or not
    //      |
    //       --> keep at 0 if this is not the boss at the end of a floor
    //array? party === who is currently in the party. Defaults to an empty array
    //      |
    //       --> ALWAYS pass in current party members
    //bool isBoss === used to determine if this is a boss dialogue. defaults to false
    //string special === used for special cutscenes, namely the beginning lore drop

    [SerializeField] private Transform bottom;
    [SerializeField] private Transform top;
    [SerializeField] private GameObject[] tweenInPositions;
    [SerializeField] private SignInteraction firstSign;
    [SerializeField] private GameObject[] tweenOutPositions;
    [SerializeField] private Transform dialogueBox;
    [SerializeField] private audioManager am;

    public Animator battleFade;

    [SerializeField] private PlayerController playerController;

    // when loading something from resources, you dont specify the file extension
    //[SerializeField] private TextAsset fileName;
    public bool dialogueRunning = false; // Track if dialogue coroutine is running
    private string fileName = "sophiePostFight_xxx";

    private string currentlyRunningText = "";

    private void Start()
    {
        //am.playBGM("T4"); // temporary
        //StartCoroutine(completeDialogue("TextFiles/deleteAfterTesting"));
        dialogueSTART(fileName); // only here for testing
    }

    //MUST PASS IN THE 
    public void dialogueSTART(string dialogueFile)
    {
        //change bgm depending on what got passed in

        // Only start the coroutine if it's not already running
        if (!dialogueRunning)
        {
            StartCoroutine(completeDialogue("TextFiles/"+dialogueFile));
            dialogueRunning = true; // Set the flag to true when starting the coroutine

            currentlyRunningText = dialogueFile;
            playerController.isfrozen = true;

            top.DOMove(tweenInPositions[0].transform.position, 1.5f);
            bottom.DOMove(tweenInPositions[1].transform.position, 1.5f);
            dialogueBox.DOMove(tweenInPositions[2].transform.position, 1.5f);
            switch (dialogueFile)
            {
                case "kisaPostFight_k":
                case "nicolEncounter_k":
                case "nicolPostFight_kn":
                case "sophiePostFight_kns":
                    am.playBGM("T5");
                    break;
                case "kisaPostFight_x":
                case "nicolPostFight_kx":
                case "nicolPostFight_xx":
                case "sophiePostFight_knx":
                case "sophiePostFight_xnx":
                case "sophiePostFight_kxx":
                case "sophieEncounter_xxx":
                    am.playBGM("T6");
                    break;
                case "secondFloor_k":
                case "thirdFloor_kn":
                case "fourthFloor_kns":
                    break;
                default:
                    am.playBGM("T4");
                    break;
            }
        }
    }


    //private Character CreateCharacter(string name) => CharacterManager.instance.CreateCharacter(name);
    IEnumerator Run(string dialogueFile)
    {
        List<string> lines = FileManager.ReadTextAsset(dialogueFile, false);
        DialogueSystem.instance.Say(lines);
        yield return null;
    }
    IEnumerator completeDialogue(string dialogueFile)
    {
        yield return StartCoroutine(Run(dialogueFile));
    }

    public void dialogueEND(bool isBoss = false)
    {
        // Stop the dialogue coroutine if it's running
        if (dialogueRunning)
        {
            if (currentlyRunningText == "openingScene")
            {
                firstSign.enabled = true;
            }
            StopCoroutine(completeDialogue(currentlyRunningText));
            dialogueRunning = false; // Set the flag to false when stopping the coroutine
            currentlyRunningText = "";
            

            top.DOMove(tweenOutPositions[0].transform.position, 2);
            bottom.DOMove(tweenOutPositions[1].transform.position, 2);
            dialogueBox.DOMove(tweenOutPositions[2].transform.position, 2);
            if (isBoss)
            {
                battleFade.SetBool("BattleStarting", true);
                return;
            }
            am.playBGM("T2");
            playerController.isfrozen = false;
        }
    }
}
