using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFiles : MonoBehaviour
{
    // when loading something from resources, you dont specify the file extension
    //private string fileName = "textFile.txt";
    [SerializeField] private TextAsset fileName;
    void Start()
    {
        StartCoroutine(Run());
    }

    IEnumerator Run()
    {
        //List<string> lines = FileManager.ReadTextFile(fileName, true);
        List<string> lines = FileManager.ReadTextAsset(fileName, true);
        // ^ for text assets
        foreach (string line in lines)
            Debug.Log(line);


        yield return null;
    }
}
