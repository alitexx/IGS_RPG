using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace DIALOGUE
{
    public class DialogueParser
    {
        // a word of any length as long as it is not proceeded by a white space
        private const string commandRegexPattern = @"[\w\[\]]*[^\s]\(";

        //takes a raw line and converts it to a dialogue line
        public static DIALOGUE_LINE Parse(string rawLine)
        {
            (string speaker, string dialogue, string commands) = RipContent(rawLine);
            return new DIALOGUE_LINE(speaker, dialogue, commands);
        }

        private static (string, string, string) RipContent(string rawLine)
        {
            string speaker = "", dialogue = "", commands = "";

            int dialogueStart = -1;
            int dialogueEnd = -1;
            bool isEscaped = false;

            for(int i = 0; i<rawLine.Length; i++)
            {
                char current = rawLine[i];
                if (current == '\\') // this could be a quote!
                {
                    isEscaped = !isEscaped;
                }
                else if (current == '"' && !isEscaped) // this is either our start or our ending dialogue
                {
                    if(dialogueStart == -1) // this is the beginning of the dialogue!
                    {
                        dialogueStart = i;
                    } else if (dialogueEnd == -1) // this is the end of the dialogue!
                    {
                        dialogueEnd = i;
                        break;
                    } else
                    {
                        isEscaped = false;
                    }
                }
            }

            //identify the command pattern
            Regex commandRegex = new Regex(commandRegexPattern);
            MatchCollection matches = commandRegex.Matches(rawLine);
            int commandStart = -1;
            foreach(Match match in matches)
            {
                if (match.Index < dialogueStart || match.Index > dialogueEnd)
                {
                    commandStart = match.Index;
                    break;
                }
            }

            if (commandStart != -1 && (dialogueStart == -1 && dialogueEnd == -1))
            {
                return ("", "", rawLine.Trim());
            }

            // if we get here in the function, we either have dialogue or a multi word argument in a command.
            // this figures out if it is dialogue
            if (dialogueStart != -1 && dialogueEnd != -1 && (commandStart == -1 || commandStart > dialogueEnd))
            {
                // we know that we have valid dialogue
                speaker = rawLine.Substring(0, dialogueStart).Trim();
                dialogue = rawLine.Substring(dialogueStart + 1, (dialogueEnd - dialogueStart) - 1).Replace("\\\"","\"");
                if (commandStart != -1)
                {
                    commands = rawLine.Substring(commandStart).Trim();
                }
            }
            else if (commandStart != -1 && dialogueStart > commandStart)
            {
                commands = rawLine;
            } else
            {
                dialogue = rawLine;
            }

            return (speaker, dialogue, commands);
        }
    }
}
