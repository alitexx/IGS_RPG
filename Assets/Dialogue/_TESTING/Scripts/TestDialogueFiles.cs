using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;
using COMMANDS;

public class TestDialogueFiles : MonoBehaviour
{
    void Start()
    {

        StartConversation();
        //StartCoroutine(Running());
        
    }

    //IEnumerator Running()
    //{
    //    //yield return CommandManager.instance.Execute("print");
    //    //yield return CommandManager.instance.Execute("print_1p", "yo amma");
    //    //yield return CommandManager.instance.Execute("print_mp", "line 1", "line3", "yomama");
    //    //do the same for lambdas/coroutines, see ep 4 pt 4 27:00
    //}

    // Update is called once per frame
    void StartConversation()
    {
        List<string> lines = FileManager.ReadTextAsset("MAIN_STORY", false);
        DialogueSystem.instance.Say(lines);
        //foreach (string line in lines)
        //{
        //    if (string.IsNullOrWhiteSpace(line))
        //    {
        //        continue;
        //    }
        //    DIALOGUE_LINE dlLine = DialogueParser.Parse(line);

        //    foreach(DL_DIALOGUE_DATA.DIALOGUE_SEGMENT segment in dlLine.dialogueData.segments)
        //    {
        //        DialogueSystem.instance.Say(lines);
        //    }

            //for each used to be DL_DIALOGUE_DATA.DIALOGUE_SEGMENT segment in dlLine.dialogue.segments
            //commands
            //for(int i = 0; i < dlLine.commandData.commands.Count; i++)
            //{
            //    //print the data to make sure its working
            //    DL_COMMAND_DATA.Command command = dlLine.commandData.commands[i];
            //    Debug.Log($"Command [{i}] '{command.name}' has arguments [{string.Join(",", command.arguments)}]");
            //}

        //DialogueSystem.instance.Say(lines);
    }
}
