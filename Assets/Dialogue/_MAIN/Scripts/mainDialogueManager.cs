using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;
using DIALOGUE;
using DG.Tweening;
using UnityEngine.SceneManagement;

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
    [SerializeField] private CanvasGroup fadeOut;
    [SerializeField] private CanvasGroup cutsceneBG;
    [SerializeField] private GameObject[] tweenInPositions;
    [SerializeField] private SignInteraction firstSign;
    [SerializeField] private GameObject[] tweenOutPositions;
    [SerializeField] private Transform dialogueBox;
    [SerializeField] private audioManager am;
    [SerializeField] private GameObject[] cutsceneScenes;
    [SerializeField] private GameObject[] cutsceneVariations;
    [SerializeField] private GameObject continueTextPrompt;
    [SerializeField] private GameObject firstSlime;

    [SerializeField] private pauseMenuManager pauseMenuManager;

    public Animator battleFade;

    [SerializeField] private PlayerController playerController;

    // when loading something from resources, you dont specify the file extension
    //[SerializeField] private TextAsset fileName;
    public bool dialogueRunning = false; // Track if dialogue coroutine is running
    private string fileName = "prologue";

    private string currentlyRunningText = "";

    private void Start()
    {
        //dialogueSTART(fileName); // only here for testing
        if (playerController.BattleTutorialCleared == 1)
        {
            playerController.loadGame();
        }
        else // if they have not cleared the tutorial
        {
            dialogueSTART(fileName);
        }
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
            if (dialogueFile.StartsWith("end"))
            {
                cutsceneBG.DOFade(1, 1f);
            } else if (dialogueFile == "partySplit")
            {
                pauseMenuManager.partyMemberAbsent("KISA");
                pauseMenuManager.partyMemberAbsent("NICOL");
                pauseMenuManager.partyMemberAbsent("SOPHIE");
                firstSlime.SetActive(false);
            }
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
                case "level1_slime":
                case "level1_skeleton":
                case "tutorialEncounter":
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
            StopCoroutine(completeDialogue(currentlyRunningText));
            dialogueRunning = false; // Set the flag to false when stopping the coroutine
            if (currentlyRunningText == "prologue")
            {
                pauseMenuManager.partyMemberAdded("KISA");
                pauseMenuManager.partyMemberAdded("NICOL");
                pauseMenuManager.partyMemberAdded("SOPHIE");
                firstSign.enabled = true;
            }
            //for determining endings
            switch (currentlyRunningText) // if this is the end of a route
            {
                case "end_genocide":// 0 = everyone dead
                    creditsManager.endingID = 0;
                    fadeOut.DOFade(1,3).OnComplete(() => { SceneManager.LoadScene("Credits"); });
                    break;
                case "end_n":// 1 = only nicol alive
                    creditsManager.endingID = 1;
                    fadeOut.DOFade(1, 3).OnComplete(() => { SceneManager.LoadScene("Credits"); });
                    break;
                case "end_ns":// 2 = nicol and sophie alive
                    creditsManager.endingID = 2;
                    fadeOut.DOFade(1, 3).OnComplete(() => { SceneManager.LoadScene("Credits"); });
                    break;
                case "end_k_postFight":// 3 = kisa alive
                    creditsManager.endingID = 3;
                    fadeOut.DOFade(1, 3).OnComplete(() => { SceneManager.LoadScene("Credits"); });
                    break;
                case "end_ks":// 4 = kisa and sophie alive
                    creditsManager.endingID = 4;
                    fadeOut.DOFade(1, 3).OnComplete(() => { SceneManager.LoadScene("Credits"); });
                    break;
                case "end_kn":// 5 = kisa and nicol alive
                    creditsManager.endingID = 5;
                    fadeOut.DOFade(1, 3).OnComplete(() => { SceneManager.LoadScene("Credits"); });
                    break;
                case "end_kns":// 6 = everyone alive!!
                    creditsManager.endingID = 6;
                    fadeOut.DOFade(1, 3).OnComplete(() => { SceneManager.LoadScene("Credits"); });
                    break;
                case "end_k": // kisa fight GO!
                    Debug.Log("Kisa boss!! Kisa secret boss! KISA SECRET BOSS GO!!!");
                    break;
            }
            currentlyRunningText = "";
            continueTextPrompt.SetActive(false);
            top.DOMove(tweenOutPositions[0].transform.position, 2);
            bottom.DOMove(tweenOutPositions[1].transform.position, 2);
            dialogueBox.DOMove(tweenOutPositions[2].transform.position, 2);
            if (isBoss)
            {
                battleFade.SetBool("BattleStarting", true);
                for (int i = 0; i < cutsceneScenes.Length; i++)
                {
                    cutsceneScenes[i].SetActive(false);
                }
                return;
            }
            am.playBGM("T2");
            playerController.isfrozen = false;
        }
    }

    public void addCutscene(int whichImage)
    {
        //do a switch statement here for if the cutscene is one that requires changing based on current party members
        switch (whichImage)
        {
            case 0:
                if (!playerController.hasKisa)
                {
                    cutsceneVariations[0].SetActive(false);
                }
                if (!playerController.hasNicol)
                {
                    cutsceneVariations[1].SetActive(false);
                }
                if (!playerController.hasSophie)
                {
                    cutsceneVariations[2].SetActive(false);
                }
                break;
            case 2:
                if (!playerController.hasKisa)
                {
                    cutsceneVariations[3].SetActive(false);
                }
                if (!playerController.hasNicol)
                {
                    cutsceneVariations[4].SetActive(false);
                }
                if (!playerController.hasSophie)
                {
                    cutsceneVariations[5].SetActive(false);
                }
                if(playerController.hasKisa && !playerController.hasNicol && !playerController.hasSophie) // if this is a kisa only route
                {
                    cutsceneVariations[3].SetActive(false);
                    cutsceneVariations[6].SetActive(true);
                }
                break;
        }
        if (whichImage >= 0 && whichImage <= cutsceneScenes.Length)
        {
            cutsceneScenes[whichImage].SetActive(true);
        } else
        {
            Debug.LogWarning("You need to pass in a number from 0 to " + cutsceneScenes.Length.ToString() + "for cutscenes! See _MainDialogueManager game object for the list of cutscenes.");
        }
        
    }
}
