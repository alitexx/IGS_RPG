using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;

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
    public void dialogueSTART()
    {
        //validate input before continuing
        StartCoroutine(completeDialogue());
    }


    private Character CreateCharacter(string name) => CharacterManager.instance.CreateCharacter(name);

    IEnumerator completeDialogue()
    {
        //Character alan = CharacterManager.instance.CreateCharacter("alan");
        Character_Sprite alan = CreateCharacter("alan") as Character_Sprite;
        yield return new WaitForSeconds(2f);
        yield return alan.Hide();
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
        //look at video for transitioning sprites
    }
}
