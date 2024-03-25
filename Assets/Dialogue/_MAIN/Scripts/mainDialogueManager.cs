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
    [SerializeField] private Transform dialogueBox;
    // when loading something from resources, you dont specify the file extension
    //[SerializeField] private TextAsset fileName;
    private string fileName = "TextFiles/openingScene";

    private void Start()
    {
        dialogueSTART(fileName); // only here for testing
    }

    //MUST PASS IN THE 
    public void dialogueSTART(string dialogueFile)
    {
        //validate input before continuing
        //StartCoroutine(completeDialogue(dialogueFile));

        //COME BACK HERE POOKIE!!! FIGURE OUT WHAT IS WRONG!!! PLEASE!!! please.


        //top.DOMoveY(80, 2);
        //bottom.DOMoveY(40, 2);

        //dialogueBox.DOMoveY(100, 2);
        //StartCoroutine(Run(dialogueFile));
        //bottom.DOMove(new Vector2(2, 2), 1);
    }


    //private Character CreateCharacter(string name) => CharacterManager.instance.CreateCharacter(name);
    IEnumerator Run(string dialogueFile)
    {
        List<string> lines = FileManager.ReadTextAsset(dialogueFile, false);
        DialogueSystem.instance.Say(lines);
        yield return null;
    }


    //next thing to do: figure out how to make speech progress on click. its done somewhere in this code, but idk where honestly
    IEnumerator completeDialogue(string dialogueFile)
    {
        yield return StartCoroutine(Run(dialogueFile));
    }

    public void dialogueEND()
    {
        Debug.Log("DIALOGUE HAS ENDED!! MOVIN THINGS TO WHERE THEY NEED TO BE");
        top.DOMoveY(4, 2);
        bottom.DOMoveY(-4, 2);

        dialogueBox.DOMoveY(0, 2);
        //StartCoroutine(Run(dialogueFile));
        //bottom.DOMove(new Vector2(2, 2), 1);
    }
}
