using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

public class TestParsing : MonoBehaviour
{
    [SerializeField] private TextAsset file;
    void Start()
    {
        SendFileToParse();
        //string line = "Spekaer \"YAHAHAHAHHAHA\" Command(Arguments)";
        //DialogueParser.Parse(line);
    }

    // Update is called once per frame
    void SendFileToParse()
    {
        // this isnt a real text asset as of rn
        List<string> lines = FileManager.ReadTextAsset("testFile", false);
        foreach (string line in lines)
        {
            DIALOGUE_LINE dl = DialogueParser.Parse(line);
        }
    }
}
