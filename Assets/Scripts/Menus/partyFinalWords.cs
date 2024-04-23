using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;
using DG.Tweening;
using DIALOGUE;
using UnityEngine.SceneManagement;

public class partyFinalWords : MonoBehaviour
{

    [SerializeField] private Transform dialogueBox;
    private Character CreateCharacter(string name) => CharacterManager.instance.CreateCharacter(name);
    [SerializeField] private DialogueSystem ds;
    [SerializeField] private audioManager audioManager;
    private float oldDialogueSpeed = 1;
    // Start is called before the first frame update
    public void playFinalWords(string whichPartyMember)
    {
        //move dialogue box to where it needs to be


        oldDialogueSpeed = ds.architect.speedMultiplier;
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
                StartCoroutine(completeDialogue(2));
                break;
        }
    }

    //next thing to do: figure out how to make speech progress on click. its done somewhere in this code, but idk where honestly
    IEnumerator completeDialogue(int partyMember)
    {
        switch (partyMember)
        {
            case 0:
                Character_Sprite kisa = CreateCharacter("kisa") as Character_Sprite;
                yield return kisa.Hide();
                yield return new WaitForSeconds(2.5f);
                kisa.Say("I am a certified yapper");
                audioManager.playSFX(2);
                yield return new WaitForSeconds(2f);
                audioManager.playSFX(18);
                yield return new WaitForSeconds(7.5f);
                ds.architect.speedMultiplier = 0.5f;
                break;
            case 1:
                Character_Sprite nicol = CreateCharacter("nicol") as Character_Sprite;
                yield return nicol.Hide();
                yield return new WaitForSeconds(2.5f);
                nicol.Say("I am a certified yapper");
                audioManager.playSFX(2);
                yield return new WaitForSeconds(2f);
                audioManager.playSFX(18);
                yield return new WaitForSeconds(7.5f);
                break;
            case 2:
                Character_Sprite sophie = CreateCharacter("sophie") as Character_Sprite;
                yield return sophie.Hide();
                yield return new WaitForSeconds(2.5f);
                sophie.Say("I am a certified yapper");
                audioManager.playSFX(2);
                yield return new WaitForSeconds(2f);
                audioManager.playSFX(18);
                yield return new WaitForSeconds(7.5f);
                break;
            case 3:
                Character_Sprite sophie_genocide = CreateCharacter("sophie") as Character_Sprite;
                yield return sophie_genocide.Hide();
                yield return new WaitForSeconds(2.5f);
                sophie_genocide.Say("I am a certified yapper");
                audioManager.playSFX(2);
                yield return new WaitForSeconds(2f);
                audioManager.playSFX(18);
                yield return new WaitForSeconds(7.5f);
                break;
        }

        ds.architect.speedMultiplier = oldDialogueSpeed;
    }
}
