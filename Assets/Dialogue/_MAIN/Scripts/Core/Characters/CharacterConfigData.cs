using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CHARACTERS
{
    [System.Serializable]
    public class CharacterConfigData
    {
        public string name;
        public string alias;
        public Character.CharacterType characterType;
        public Sprite charNameHolder;
        //may not be an audio clip, double check when implementing
        public AudioClip charVoice;

        //go to episode 5 part 1 for name color, dilogue color, fonts, etc.

        public CharacterConfigData Copy()
        {
            CharacterConfigData result = new CharacterConfigData();

            result.name = name;
            result.alias = alias;
            result.characterType = characterType;
            result.charNameHolder = charNameHolder;
            result.charVoice = charVoice;

            return result;
        }

        public static CharacterConfigData Default //the default values if a character isnt found
        {
            get
            {
                CharacterConfigData result = new CharacterConfigData();

                result.name = "";
                result.alias = "";
                result.characterType = Character.CharacterType.Text;

                return result;
            }
        }
        
    }
}