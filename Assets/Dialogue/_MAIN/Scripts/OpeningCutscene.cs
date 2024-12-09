using CHARACTERS;
using DG.Tweening;
using DIALOGUE;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningCutscene : MonoBehaviour
{
    [SerializeField] private Transform dialogueBox;
    private Character CreateCharacter(string name) => CharacterManager.instance.CreateCharacter(name);
    [SerializeField] private GameObject[] cutsceneassets;
    [SerializeField] private DialogueSystem ds;
    [SerializeField] private MainMenu menu;
    //[SerializeField] private PlayerController playerController;
    private string tempInterractButton;
    //private void Start()
    //{
    //    dialogueSTART(); // only here for testing
    //}

    //MUST PASS IN THE 
    public void dialogueSTART()
    {
        if(PlayerPrefs.GetInt("BattleTutorialCleared") == 0)
        {
            ////validate input before continuing
            //tempInterractButton = audioStatics.interractButton;
            //audioStatics.interractButton = string.Empty;
            ds.architect.speedMultiplier = 0.5f;
            StartCoroutine(completeDialogue());
        }
        else
        {
            skipDialogue();
        }
    }

    public void skipDialogue()
    {
        menu.fadeToRPGWorld();
    }

    IEnumerator completeDialogue()
    {
        Character_Text alan = CreateCharacter("alan_firstCutscene") as Character_Text;
        //yield return new WaitForSeconds(2f);
        yield return new WaitForSeconds(1.5f);
        //image of isen fades in
        cutsceneassets[0].SetActive(true);
        alan.Say("Isen. A large island in the corner of the world, a place rarely traveled to due to treacherous waters and an abundance of thieves.");
        yield return new WaitForSeconds(7.5f);
        //image 2 fades in
        cutsceneassets[1].SetActive(true);
        alan.Say("I dedicated my life to protecting the innocent, working as a knight to bring order to my homeland.");
        yield return new WaitForSeconds(7f);
        //image 3 fades in
        cutsceneassets[2].SetActive(true);
        alan.Say("On the day of my promotion, I failed that duty when my own mentor, Leora, was brutally slain.");
        yield return new WaitForSeconds(6.5f);
        cutsceneassets[6].SetActive(false);
        //image 4 fades in
        cutsceneassets[3].SetActive(true);
        alan.Say("In my grief, I accepted a deal with an... otherworldly creature. Trade three souls for my mentor's return. It was the only way to bring her back, I had to.");
        yield return new WaitForSeconds(9f);
        alan.Say("I didn't want to kill three people, and I still don't want to. I have yet to collect a single soul.");
        yield return new WaitForSeconds(6.5f);
        //image 5 fades in
        cutsceneassets[4].SetActive(true);
        alan.Say("However, once again, I am being tempted. By happenstance, I have gathered a group of three on my way to defeat a lich, Morsophus, who was tormenting a town on the outskirts of Isen.");
        yield return new WaitForSeconds(10f);
        //image 6 fades in
        cutsceneassets[5].SetActive(true);
        alan.Say("I am hesitant. I do not want to decide the fate of three who have committed no crime. It is not right nor just, I know this. But still, I am tempted. My resolve is weakened without her guidance.");
        yield return new WaitForSeconds(10f);
        alan.Say("It did not help that from the outset we struggled to get along. Perhaps if we were a better team, I would not feel so conflicted.");
        yield return new WaitForSeconds(6f);
        //send player to game screen
        //audioStatics.interractButton = tempInterractButton;
        menu.fadeToRPGWorld(); //loads main level
    }
}

