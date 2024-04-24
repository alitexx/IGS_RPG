using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class pauseMenuManager : MonoBehaviour
{
    [SerializeField] private Transform[] partyMemberIcons;
    [SerializeField] private GameObject[] locations;
    [SerializeField] private GameObject[] buttonOn;
    [SerializeField] private GameObject[] buttonOff;
    [SerializeField] private pause_characterInspection p_ci;
    [SerializeField] private GameObject characterinspector;
    //main menu confirmation
    [SerializeField] private GameObject darkenBG;
    [SerializeField] private GameObject ConfirmMainMenu;
    //options
    [SerializeField] private GameObject OptionsTXT;
    [SerializeField] private GameObject PartyLevelTXT;
    [SerializeField] private GameObject OptionsMenu;
    [SerializeField] private GameObject Buttons;

    [SerializeField] private Image[] partymemberImages;
    [SerializeField] private Sprite[] partymemberSprites;
    [SerializeField] private GameObject[] partymemberIconAssets;

    //SaveMenu Stuff
    [SerializeField] private GameObject SaveMenu;
    [SerializeField] private SavepointScript savepointScript;
    [SerializeField] private PlayerController playerController;

    // changing font of names
    [SerializeField] private TextMeshProUGUI[] charNames;
    [SerializeField] private TMP_FontAsset[] fonts;

    private void OnEnable()
    {
        //place all sprites where they need to be
        //if the options menu is open, close it
        if (OptionsMenu.activeSelf)
        {
            Buttons.transform.position = locations[0].transform.position;
            OptionsTXT.transform.position = locations[7].transform.position;
            PartyLevelTXT.transform.position = locations[8].transform.position;
            OptionsMenu.transform.position = locations[10].transform.position;
            OptionsMenu.SetActive(false);

        } else if (ConfirmMainMenu.activeSelf) // if the confirm menu is open, close it
        {
            darkenBG.SetActive(false);
            ConfirmMainMenu.SetActive(false);
        }
        for (int i = 0; i < 4; i++) // for each party member, make sure they're in the correct position and the menu isnt open
        {
            partyMemberIcons[i].position = locations[0].transform.position;
            buttonOn[i].SetActive(true);
            buttonOff[i].SetActive(false);
        }
        characterinspector.SetActive(false);

    }

    //this is already done somewhere else, just leaving the code in case it breaks anything
    public void changePartyLevelTXT(int partylevel)
    {
        PartyLevelTXT.GetComponent<TextMeshProUGUI>().text = ("Party Level:" + LevelManager.level);
    }

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

    public void openOptionsMenu()
    {
        partyMemberIcons[0].DOMove(locations[1].transform.position, 1);
        partyMemberIcons[1].DOMove(locations[1].transform.position, 1);
        partyMemberIcons[2].DOMove(locations[2].transform.position, 1);
        partyMemberIcons[3].DOMove(locations[2].transform.position, 1);
        //make locations for each of these
        OptionsTXT.transform.DOMove(locations[8].transform.position, 1);
        PartyLevelTXT.transform.DOMove(locations[7].transform.position, 1);
        OptionsMenu.SetActive(true);
        OptionsMenu.transform.DOMove(locations[0].transform.position, 1);
        Buttons.transform.DOMove(locations[9].transform.position, 1);
        //move buttons
    }

    public void closeOptionsMenu()
    {
        partyMemberIcons[0].DOMove(locations[0].transform.position, 1);
        partyMemberIcons[1].DOMove(locations[0].transform.position, 1);
        partyMemberIcons[2].DOMove(locations[0].transform.position, 1);
        partyMemberIcons[3].DOMove(locations[0].transform.position, 1);
        Buttons.transform.DOMove(locations[0].transform.position, 1);

        OptionsTXT.transform.DOMove(locations[7].transform.position, 1);
        PartyLevelTXT.transform.DOMove(locations[8].transform.position, 1);

        OptionsMenu.transform.DOMove(locations[10].transform.position, 1).OnComplete(() => { OptionsMenu.SetActive(false); });
        
    }

    public void confirmMainMenu()
    {
        //DOFade for the bg
        darkenBG.SetActive(true);
        ConfirmMainMenu.SetActive(true);
    }
    public void exitConfirmMenu()
    {
        //DOFade for the bg
        darkenBG.SetActive(false);
        ConfirmMainMenu.SetActive(false);
    }

    public void partyMemberAdded(string partyMember)
    {
        switch (partyMember.ToUpper())
        {
            case "KISA":
                partymemberImages[0].sprite = partymemberSprites[0];
                partymemberIconAssets[0].SetActive(false);
                charNames[0].font = fonts[0];
                break;
            case "NICOL":
                partymemberImages[1].sprite = partymemberSprites[2];
                partymemberIconAssets[2].SetActive(false);
                charNames[1].font = fonts[0];
                break;
            case "SOPHIE":
                partymemberImages[2].sprite = partymemberSprites[4];
                partymemberIconAssets[4].SetActive(false);
                charNames[2].font = fonts[0];
                break;
        }
    }
    public void partyMemberKilled(string partyMember)
    {
        switch (partyMember.ToUpper())
        {
            case "KISA":
                partymemberImages[0].sprite = partymemberSprites[1];
                //partymemberIconAssets[1].SetActive(true);
                charNames[0].font = fonts[1];
                break;
            case "NICOL":
                partymemberImages[1].sprite = partymemberSprites[3];
                //partymemberIconAssets[3].SetActive(true);
                charNames[1].font = fonts[1];
                break;
            case "SOPHIE":
                partymemberImages[2].sprite = partymemberSprites[5];
                //partymemberIconAssets[5].SetActive(true);
                charNames[2].font = fonts[1];
                break;
        }
    }

    public void returnSaveMenu()
    {
        SaveMenu.SetActive(false);
        playerController.isfrozen = false;
        PauseMenu.canOpenPause = true;
    }

    public void confirmSave()
    {
        savepointScript.confirmedSave();
        SaveMenu.SetActive(false);
        playerController.isfrozen = false;
    }
}
