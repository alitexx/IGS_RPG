using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace COMMANDS
{
    public class CMD_DatabaseExtension_General : CMD_DatabaseExtension
    {
        [SerializeField] private static mainDialogueManager MainDiaManager;


        new public static void Extend(CommandDatabase database)
        {
            database.AddCommand("wait", new Func<string, IEnumerator>(Wait));
            database.AddCommand("endDialogue", new Action<string> (endDialogue));
        }

        private static IEnumerator Wait(string data)
        {
            if (float.TryParse(data, out float time))
            {
                yield return new WaitForSeconds(time);
            }
        }

        private static void endDialogue(string isBoss)
        {
            
            MainDiaManager = GameObject.FindGameObjectWithTag("MainDialogueManager").GetComponent<mainDialogueManager>();

            if (isBoss == "true")
            {
                MainDiaManager.dialogueEND(true);
            }
            else if (isBoss == "false")
            {
                MainDiaManager.dialogueEND(false);
            }
            else
            {
                Debug.Log(isBoss + " needs to be a true or a false.");
            }

            //switch (bossName.ToUpper())
            //{
            //    case "KISA":
            //        break;
            //    case "NICOL":
            //        break;
            //    case "SOPHIE":
            //        break;
            //    case "LICH":
            //        break;
            //    case "SECRET":
            //        break;
            //}
        }
    }
}