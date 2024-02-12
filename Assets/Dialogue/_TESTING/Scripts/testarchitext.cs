using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

namespace TESTING
{
    public class testarchitext : MonoBehaviour
    {

        DialogueSystem ds;
        TextArchitect architect;

        string [] lines = new string [5]
        {
            "The mysterious cat slipped through the shadows.",
            "In a distant galaxy, a brave explorer discovered a hidden treasure.",
            "As the sun set, the ancient castle revealed its haunting beauty.",
            "A sudden gust of wind carried whispers of long-forgotten stories.",
            "In the bustling city, a street musician played a melody that touched hearts."
        };
        // Start is called before the first frame update
        void Start()
        {
            ds = DialogueSystem.instance;
            architect = new TextArchitect(ds.dialogueContainer.dialogueText);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (architect.isBuilding)
                {
                    if (!architect.hurryUp)
                    {
                        architect.hurryUp = true;
                    } else
                    {
                        architect.ForceComplete();
                    }
                }
                else
                {
                    architect.Build(lines[Random.Range(0, lines.Length)]);
                }
            }
        }
    }
}
