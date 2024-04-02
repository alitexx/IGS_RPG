using COMMANDS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;

namespace DIALOGUE
{
    //takes lines given by the dialogue system and prints them to the screen
    public class ConversationManager
    {
        private DialogueSystem dialogueSystem => DialogueSystem.instance;

        private Coroutine process = null;
        public bool isRunning => process != null;
        private bool userPrompt = false;

        private TextArchitect architect = null;
        public ConversationManager(TextArchitect architect, AudioSource audioPlayer)
        {
            this.architect = architect;
            architect.audioPlayer = audioPlayer;
            dialogueSystem.onUserPrompt_Next += OnUserPrompt_Next;
        }

        private void OnUserPrompt_Next()
        {
            userPrompt = true;
        }

        public Coroutine StartConversation(List<string> conversation)
        {
            StopConversation();
            process = dialogueSystem.StartCoroutine(RunningConversation(conversation));

            return process;
        }

        public void StopConversation()
        {
            if (!isRunning)
            {
                return;
            }

            dialogueSystem.StopCoroutine(process);
            process = null;
        }

        IEnumerator RunningConversation(List<string> conversation)
        {
            for(int i=0; i<conversation.Count; i++)
            {
                //dont show any blank spaces or try to run logic through them
                if (string.IsNullOrWhiteSpace(conversation[i]))
                {
                    continue;
                }
                DIALOGUE_LINE line = DialogueParser.Parse(conversation[i]);
                //show dialogue
                if (line.hasDialogue)
                {
                    yield return Line_RunDialogue(line);
                }
                //do the commands
                if (line.hasCommands)
                {
                    yield return Line_RunCommands(line);
                }

                if (line.hasDialogue)
                {
                    //wait for user input 
                    yield return WaitForUserInput();
                    CommandManager.instance.StopAllProcesses();
                }
            }
        }

        IEnumerator Line_RunDialogue(DIALOGUE_LINE line)
        {
            if (line.hasSpeaker)
            {
                HandleSpeakerLogic(line.speakerData);
            }
            // build dialogue
            //more specifically, take line segments and check for waiting, appending, etc.
            yield return BuildLineSegments(line.dialogueData);
        }

        private void HandleSpeakerLogic(DL_SPEAKER_DATA speakerData)
        {
            bool charMustBeCreated = (speakerData.makeCharacterEnter || speakerData.isCastingPosition || speakerData.isCastingExpressions);

            Character character = CharacterManager.instance.GetCharacter(speakerData.name, createIfDoesNotExist: charMustBeCreated);
            if (speakerData.makeCharacterEnter && (!character.isVisible && !character.isRevealing))
            {
                character.Show();
                //CharacterManager.instance.CreateCharacter(speakerData.name, revealAfterCreation: true);
            }
            //show character name to UI
            dialogueSystem.ShowSpeakerName(speakerData.displayname);
            DialogueSystem.instance.ApplySpeakerDataToDialogueContainer(speakerData.displayname);

            //cast positions
            if (speakerData.isCastingPosition)
            {
                character.MoveToPosition(speakerData.castPosition);
            }

            //cast expressions
            if (speakerData.isCastingExpressions)
            {
                foreach(var ce in speakerData.CastExpressions)
                {
                    character.OnReceiveCastingExpression(ce.layer, ce.expression);
                }
            }
        }


        IEnumerator Line_RunCommands(DIALOGUE_LINE line)
        {
            List<DL_COMMAND_DATA.Command> commands = line.commandData.commands;
            foreach (DL_COMMAND_DATA.Command command in commands)
            {
                if (command.waitForCompletion ||command.name == "wait")
                {
                    CoroutineWrapper cw = CommandManager.instance.Execute(command.name, command.arguments);
                    while (!cw.isDone)
                    {
                        if (userPrompt)
                        {
                            CommandManager.instance.StopCurrentProcess();
                            userPrompt = false;
                        }
                        yield return null;
                    }
                } else
                {
                    CommandManager.instance.Execute(command.name, command.arguments);
                }
            }
            yield return null;
        }

        IEnumerator BuildLineSegments(DL_DIALOGUE_DATA line)
        {
            for (int i = 0; i < line.segments.Count; i++)
            {
                DL_DIALOGUE_DATA.DIALOGUE_SEGMENT segment = line.segments[i];



                yield return WaitForDialogueSegmentTrigger(segment);

                yield return BuildDialogue(segment.dialogue,segment.appendText);
            }
        }

        IEnumerator WaitForDialogueSegmentTrigger(DL_DIALOGUE_DATA.DIALOGUE_SEGMENT segment)
        {
            switch (segment.startSignal)
            {
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.C:
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.A:
                    yield return WaitForUserInput();
                    break;
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.WC:
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.WA:
                    yield return new WaitForSeconds(segment.signalDelay);
                    break;
                default:
                    break;
            }
        }



        IEnumerator BuildDialogue(string dialogue, bool append = false)
        {
            if (!append)
            {
                architect.Build(dialogue);
            } else
            {
                architect.Append(dialogue);
            }
            

            while (architect.isBuilding)
            {
                if (userPrompt)
                {
                    if (!architect.hurryUp)
                    {
                        architect.hurryUp = true;
                        userPrompt = false;
                    }
                    else
                    {
                        architect.ForceComplete();
                    }
                    //userPrompt = true;
                }
                yield return null;
            }
        }

        IEnumerator WaitForUserInput()
        {
            while (!userPrompt)
            {
                yield return null;
            }
            userPrompt = false;
        }
    }
}
