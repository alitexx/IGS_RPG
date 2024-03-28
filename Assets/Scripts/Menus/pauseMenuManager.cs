using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class pauseMenuManager : MonoBehaviour
{
    [SerializeField] private Transform[] partyMemberIcons;
    [SerializeField] private GameObject[] locations;
    [SerializeField] private GameObject[] buttonOn;
    [SerializeField] private GameObject[] buttonOff;
    [SerializeField] private pause_characterInspection p_ci;
    [SerializeField] private GameObject characterinspector;

    // when menu is opened, check the state of the party. party members you dont own are grayed out,
    // party members that are dead have a red X over them

    public void PartyMemberClicked(string whoWasIt)
    {
        switch (whoWasIt.ToUpper())
        {
            case "ALAN":
                partyMemberIcons[0].DOMove(locations[3].transform.position, 0.75f);
                partyMemberIcons[1].DOMove(locations[2].transform.position, 1);
                partyMemberIcons[2].DOMove(locations[2].transform.position, 1);
                partyMemberIcons[3].DOMove(locations[2].transform.position, 1);
                buttonOn[0].SetActive(false);
                buttonOff[0].SetActive(true);
                p_ci.characterInspection(0);
                break;
            case "KISA":
                partyMemberIcons[0].DOMove(locations[1].transform.position, 1);
                partyMemberIcons[1].DOMove(locations[4].transform.position, 0.75f);
                partyMemberIcons[2].DOMove(locations[2].transform.position, 1);
                partyMemberIcons[3].DOMove(locations[2].transform.position, 1);
                buttonOn[1].SetActive(false);
                buttonOff[1].SetActive(true);
                p_ci.characterInspection(1);
                break;
            case "NICOL":
                partyMemberIcons[0].DOMove(locations[1].transform.position, 1);
                partyMemberIcons[1].DOMove(locations[1].transform.position, 1);
                partyMemberIcons[2].DOMove(locations[5].transform.position, 0.75f);
                partyMemberIcons[3].DOMove(locations[2].transform.position, 1);
                buttonOn[2].SetActive(false);
                buttonOff[2].SetActive(true);
                p_ci.characterInspection(2);
                break;
            case "SOPHIE":
                partyMemberIcons[0].DOMove(locations[1].transform.position, 1);
                partyMemberIcons[1].DOMove(locations[1].transform.position, 1);
                partyMemberIcons[2].DOMove(locations[1].transform.position, 1);
                partyMemberIcons[3].DOMove(locations[6].transform.position, 0.75f);
                buttonOn[3].SetActive(false);
                buttonOff[3].SetActive(true);
                p_ci.characterInspection(3);
                break;
        }
        characterinspector.SetActive(true);
    }

    public void closePartyMemberMenu(string whoWasIt)
    {
        partyMemberIcons[0].DOMove(locations[0].transform.position, 1);
        partyMemberIcons[1].DOMove(locations[0].transform.position, 1);
        partyMemberIcons[2].DOMove(locations[0].transform.position, 1);
        partyMemberIcons[3].DOMove(locations[0].transform.position, 1);
        characterinspector.SetActive(false);
        //leaving just in case i need to return to this
        switch (whoWasIt.ToUpper())
        {
            case "ALAN":
                buttonOn[0].SetActive(true);
                buttonOff[0].SetActive(false);
                break;
            case "KISA":
                buttonOn[1].SetActive(true);
                buttonOff[1].SetActive(false);
                break;
            case "NICOL":
                buttonOn[2].SetActive(true);
                buttonOff[2].SetActive(false);
                break;
            case "SOPHIE":
                buttonOn[3].SetActive(true);
                buttonOff[3].SetActive(false);
                break;
        }

    }
}
