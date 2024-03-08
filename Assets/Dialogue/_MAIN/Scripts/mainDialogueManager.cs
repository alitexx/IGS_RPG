using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;
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

    private void Start()
    {
        dialogueSTART(); // only here for testing
    }

    public void dialogueSTART()
    {
        //validate input before continuing
        StartCoroutine(completeDialogue());

        top.DOMoveY(4, 2);
        bottom.DOMoveY(-4, 2);

        dialogueBox.DOMoveY(0, 2);
        //bottom.DOMove(new Vector2(2, 2), 1);
    }


    private Character CreateCharacter(string name) => CharacterManager.instance.CreateCharacter(name);


    //next thing to do: figure out how to make speech progress on click. its done somewhere in this code, but idk where honestly
    IEnumerator completeDialogue()
    {
        Character_Sprite alan = CreateCharacter("alan") as Character_Sprite;
        Character_Sprite nicol = CreateCharacter("nicol") as Character_Sprite;
        //yield return new WaitForSeconds(2f);
        //yield return alan.Hide();
        yield return new WaitForSeconds(2f);
        yield return alan.Show();
        yield return new WaitForSeconds(2f);
        //alan.SetPosition(Vector2.zero);
        yield return alan.MoveToPosition(Vector2.one, smooth: true);
        alan.Say("hello i am bowser from the hit series Super Mario.");
        yield return new WaitForSeconds(2f);
        Sprite alanSprite = alan.GetSprite("pedro");
        alan.SetSprite(alanSprite, 0); // since theres no layering, the 0 isnt really necessary. just doing this so future katie doesnt forget
        yield return new WaitForSeconds(1f);
        alan.Say("AAAUUUUUGGGGGHHHHHHHHHH");
        yield return alan.TransitionColor(Color.red, speed: 0.3f);
        yield return nicol.Show();
        nicol.Say("YIPPEE I AM WORKING");
        yield return new WaitForSeconds(1f);
        alan.Say("YAY NICOL IS WORKING");
        yield return new WaitForSeconds(1f);
        top.DOMoveY(6, 2);
        bottom.DOMoveY(-6, 2);
        dialogueBox.DOMoveY(-2, 2);
    }
}
