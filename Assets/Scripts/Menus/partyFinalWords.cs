using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;
using DG.Tweening;
using DIALOGUE;
using TMPro;
using UnityEngine.SceneManagement;

public class partyFinalWords : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Transform[] textLoc;
    private Character CreateCharacter(string name) => CharacterManager.instance.CreateCharacter(name);
    [SerializeField] private DialogueSystem ds;
    [SerializeField] private audioManager audioManager;
    [SerializeField] private youWinMenu youWinMenu;
    [SerializeField] private CanvasGroup cutToBlack;
    [SerializeField] private PlayerController playerController;

    [SerializeField] private BattleController battleController;

    [SerializeField] private Transform continuePromptOBJ;
    [SerializeField] private Transform[] continuePromptOBJLocs;

    private float oldDialogueSpeed = 1;
    // Start is called before the first frame update
    public void playFinalWords(string whichPartyMember)
    {
        audioManager.stopBGM(0.1f);
        //adjust font
        dialogueText.fontSize = 16;
        dialogueText.alignment = TextAlignmentOptions.Center;
        //move dialogue box to where it needs to be
        dialogueText.text = "";
        dialogueText.transform.position = textLoc[0].position;
        //cut to black
        cutToBlack.gameObject.SetActive(true);
        cutToBlack.alpha = 1;
        audioManager.stopHeartbeatSFX();
        //save what the dialogue speed was
        oldDialogueSpeed = ds.architect.speedMultiplier;
        //move continue prompt out of the way
        continuePromptOBJ.position = continuePromptOBJLocs[0].position;
        //determine which party member was killed and act accordingly
        switch (whichPartyMember.ToUpper())
        {
            case "KISA":
                StartCoroutine(completeDialogue(0));
                break;
            case "NICOL":
                StartCoroutine(completeDialogue(1));
                break;
            case "SOPHIE":
                // check if other allies have died
                if (!playerController.hasKisa && !playerController.hasNicol) // if sophie was the last one
                {
                    StartCoroutine(completeDialogue(4));
                } else if (!playerController.hasKisa || !playerController.hasNicol) // if someone else is dead
                {
                    StartCoroutine(completeDialogue(3));
                } else // if sophie was the first death
                {
                    StartCoroutine(completeDialogue(2));
                }
                break;
        }
    }

    //next thing to do: figure out how to make speech progress on click. its done somewhere in this code, but idk where honestly
    IEnumerator completeDialogue(int partyMember)
    {
        switch (partyMember)
        {
            case 0: // kisa death
                ds.architect.speedMultiplier = 0.5f;
                Character_Text kisa = CreateCharacter("kisa_killed") as Character_Text;
                yield return new WaitForSeconds(2.5f);
                kisa.Say("Alan? You were the one attacking me? How dare-");
                yield return new WaitForSeconds(4f);
                kisa.Say("W-wait! Stop! Alan!");
                yield return new WaitForSeconds(3f);
                ds.architect.speedMultiplier = 0.25f;
                kisa.Say("STOP!");
                yield return new WaitForSeconds(2f);
                dialogueText.transform.position = textLoc[1].position;
                audioManager.playSFX(2);
                yield return new WaitForSeconds(1f);
                audioManager.playSFX(11);
                yield return new WaitForSeconds(2f);
                audioManager.playSFX(16);
                yield return new WaitForSeconds(3f);
                break;
            case 1: // nicol death
                ds.architect.speedMultiplier = 0.5f;
                Character_Text nicol = CreateCharacter("nicol_killed") as Character_Text;
                yield return new WaitForSeconds(2.5f);
                nicol.Say("It appears my time is at an end, my life's flame extinguished by the hands of my rival.");
                yield return new WaitForSeconds(5f);
                nicol.Say("While I had hoped to fall in a grand showdown of good versus evil, it seems fate had different plans.");
                yield return new WaitForSeconds(5f);
                dialogueText.transform.position = textLoc[1].position;
                audioManager.playSFX(2);
                yield return new WaitForSeconds(1f);
                audioManager.playSFX(11);
                yield return new WaitForSeconds(2f);
                audioManager.playSFX(16);
                yield return new WaitForSeconds(3f);
                break;
            case 2: // sophie death if she is the only one killed
                ds.architect.speedMultiplier = 0.5f;
                Character_Text sophie = CreateCharacter("sophie_killed") as Character_Text;
                yield return new WaitForSeconds(2.5f);
                sophie.Say("You bitch. This is not sparring.");
                yield return new WaitForSeconds(4f);
                dialogueText.transform.position = textLoc[1].position;
                audioManager.playSFX(2);
                yield return new WaitForSeconds(1f);
                audioManager.playSFX(11);
                yield return new WaitForSeconds(2f);
                audioManager.playSFX(16);
                yield return new WaitForSeconds(3f);
                break;
            case 3: //sophie death if someone else is dead
                ds.architect.speedMultiplier = 0.5f;
                Character_Text sophie_x = CreateCharacter("sophie_killed") as Character_Text;
                yield return new WaitForSeconds(2.5f);
                sophie_x.Say("Damn it...");
                yield return new WaitForSeconds(3f);
                sophie_x.Say("Have you no shame? Taking the mantle of reaper, as if you have the right.");
                yield return new WaitForSeconds(5f);
                sophie_x.Say("Kill me, coward. I will be there waiting when you come to your pathetic end.");
                yield return new WaitForSeconds(5f);
                dialogueText.transform.position = textLoc[1].position;
                audioManager.playSFX(2);
                yield return new WaitForSeconds(1f);
                audioManager.playSFX(11);
                yield return new WaitForSeconds(2f);
                audioManager.playSFX(16);
                yield return new WaitForSeconds(3f);
                break;
            case 4: // sophie death if everyone else is dead
                //change the pitch of the overworld for the rest of the run
                audioManager.changePitch(1, 0.8f, 1);
                ds.architect.speedMultiplier = 0.5f;
                Character_Text sophie_genocide = CreateCharacter("sophie_killed") as Character_Text;
                yield return new WaitForSeconds(2.5f);
                sophie_genocide.Say("You...! You wield their magic?");
                yield return new WaitForSeconds(4f);
                sophie_genocide.Say("You have killed our companions and now I am next, is that it?");
                yield return new WaitForSeconds(4f);
                sophie_genocide.Say("From the start, was this your intention? Damn you...");
                yield return new WaitForSeconds(4f);
                dialogueText.transform.position = textLoc[1].position;
                audioManager.playSFX(2);
                yield return new WaitForSeconds(1f);
                audioManager.playSFX(11);
                yield return new WaitForSeconds(2f);
                audioManager.playSFX(16);
                yield return new WaitForSeconds(3f);
                break;
        }
        //set back to normal
        ds.architect.speedMultiplier = oldDialogueSpeed;
        battleController.AbsorbButton();
        openYouWin();
        cutToBlack.DOFade(0, 1).OnComplete(() => {
            cutToBlack.gameObject.SetActive(false);
            //battleController.AbsorbButton();
            dialogueText.fontSize = 8;
            dialogueText.alignment = TextAlignmentOptions.TopLeft;
            dialogueText.text = "";

            //bring back continue prompt
            continuePromptOBJ.gameObject.SetActive(false);
            continuePromptOBJ.position = continuePromptOBJLocs[1].position;
        });
    }
    public void openYouWin()
    {
        youWinMenu.gameObject.SetActive(true);
    }
}
