using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class pauseMenuManager : MonoBehaviour
{
    [SerializeField] private Transform[] partyMemberIcons;
    [SerializeField] private GameObject[] locations;


    public void PartyMemberClicked(string whoWasIt)
    {
        switch (whoWasIt.ToUpper())
        {
            case "ALAN":
                partyMemberIcons[1].DOMove(locations[2].transform.position, 1);
                partyMemberIcons[2].DOMove(locations[2].transform.position, 1);
                partyMemberIcons[3].DOMove(locations[2].transform.position, 1);
                break;
            case "KISA":
                partyMemberIcons[0].DOMove(locations[1].transform.position, 1);
                partyMemberIcons[2].DOMove(locations[2].transform.position, 1);
                partyMemberIcons[3].DOMove(locations[2].transform.position, 1);
                break;
            case "NICOL":
                partyMemberIcons[0].DOMove(locations[1].transform.position, 1);
                partyMemberIcons[1].DOMove(locations[1].transform.position, 1);
                partyMemberIcons[3].DOMove(locations[2].transform.position, 1);
                break;
            case "SOPHIE":
                partyMemberIcons[0].DOMove(locations[1].transform.position, 1);
                partyMemberIcons[1].DOMove(locations[1].transform.position, 1);
                partyMemberIcons[2].DOMove(locations[1].transform.position, 1);
                break;
        }
    }

    public void closePartyMemberMenu(string whoWasIt)
    {
        partyMemberIcons[0].DOMove(locations[0].transform.position, 1);
        partyMemberIcons[1].DOMove(locations[0].transform.position, 1);
        partyMemberIcons[2].DOMove(locations[0].transform.position, 1);
        partyMemberIcons[3].DOMove(locations[0].transform.position, 1);
        //leaving just in case i need to return to this
        //switch (whoWasIt.ToUpper())
        //{
        //    case "ALAN":
        //        break;
        //    case "KISA":
        //        break;
        //    case "NICOL":
        //        break;
        //    case "SOPHIE":
        //        break;
        //}
    }
}
