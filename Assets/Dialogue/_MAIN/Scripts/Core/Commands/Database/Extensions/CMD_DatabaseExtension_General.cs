using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace COMMANDS
{
    public class CMD_DatabaseExtension_General : CMD_DatabaseExtension
    {
        [SerializeField] private static mainDialogueManager MainDiaManager;
        [SerializeField] private static audioManager audioManager;

        new public static void Extend(CommandDatabase database)
        {
            database.AddCommand("wait", new Func<string, IEnumerator>(Wait));
            database.AddCommand("endDialogue", new Action<string> (endDialogue));
            database.AddCommand("endMenuDialogue", new Action<string>(endMenuDialogue));
            database.AddCommand("addCutscene", new Action<string>(addCutscene));
            database.AddCommand("playSFX", new Action<string>(playSFX));
            database.AddCommand("playBGM", new Action<string>(playBGM));
            database.AddCommand("stopBGM", new Action<string>(stopBGM));
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

            if (isBoss.ToLower() == "true")
            {
                MainDiaManager.dialogueEND(true);
            }
            else if (isBoss.ToLower() == "false")
            {
                MainDiaManager.dialogueEND(false);
            }
            else
            {
                Debug.Log(isBoss + " needs to be a true or a false.");
            }
        }

        private static void endMenuDialogue(string isSupport)
        {
            MainDiaManager = GameObject.FindGameObjectWithTag("MainDialogueManager").GetComponent<mainDialogueManager>();

            if (isSupport.ToLower() == "true")
            {
                MainDiaManager.menuDialogueEND(true);
            }
            else if (isSupport.ToLower() == "false")
            {
                MainDiaManager.menuDialogueEND(false);
            }
            else
            {
                Debug.Log(isSupport + " needs to be a true or a false.");
            }
        }
        private static void addCutscene(string whichCutscene)
        {
            MainDiaManager = GameObject.FindGameObjectWithTag("MainDialogueManager").GetComponent<mainDialogueManager>();
            MainDiaManager.addCutscene(int.Parse(whichCutscene));
        }
        private static void playSFX(string whichSFX)
        {
            audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<audioManager>();
            audioManager.playSFX(int.Parse(whichSFX));
        }
        private static void playBGM(string whichSong)
        {
            audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<audioManager>();
            audioManager.playBGM(whichSong);
        }

        private static void stopBGM(string speed = "1")
        {
            audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<audioManager>();
            audioManager.stopBGM(float.Parse(speed));
        }
    }
}