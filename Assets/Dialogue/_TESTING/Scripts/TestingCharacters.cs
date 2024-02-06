using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;

namespace TESTING
{
    public class TestingCharacters : MonoBehaviour
    {
        private Character CreateCharacter(string name) => CharacterManager.instance.CreateCharacter(name);
        void Start()
        {
            StartCoroutine(Test());
        }

        IEnumerator Test()
        {
            //Character alan = CharacterManager.instance.CreateCharacter("alan");
            Character_Sprite alan = CreateCharacter("alan") as Character_Sprite;
            yield return new WaitForSeconds(2f);
            yield return alan.Hide();
            yield return new WaitForSeconds(2f);
            yield return alan.Show();
            yield return new WaitForSeconds(2f);
            //alan.SetPosition(Vector2.zero);
            yield return alan.MoveToPosition(Vector2.one, smooth:true);
            alan.Say("hello i am bowser from the hit series Super Mario.");
            yield return new WaitForSeconds(2f);
            Sprite alanSprite = alan.GetSprite("pedro");
            alan.SetSprite(alanSprite, 0); // since theres no layering, the 0 isnt really necessary. just doing this so future katie doesnt forget
            yield return new WaitForSeconds(1f);
            alan.Say("AAAUUUUUGGGGGHHHHHHHHHH");
            yield return alan.TransitionColor(Color.red, speed: 0.3f);
            //look at video for transitioning sprites
        }

        //if loading from spritesheet, you'd want to do:
        //Sprite sprite_name = CHARACTER_NAME.GetSprite("Characters-your_sprite_name")
        //CHARACTER_NAME.SetSprite(sprite_name)


        // Update is called once per frame
        void Update()
        {

        }
    }
}