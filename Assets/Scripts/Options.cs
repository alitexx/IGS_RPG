using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;
using UnityEngine.UI;
using TMPro;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class Options : MonoBehaviour
{
    //for changing text speed
    [SerializeField] private DialogueSystem ds;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI buttonTXT;
    [SerializeField] private audioManager am;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private AudioSource voiceVol;
    [SerializeField] private Slider[] optionsSliders;
    [SerializeField] private GameObject DeleteSaveMenu;
    private bool waitingForKeyPress = false;
    private string keyPressed;

    void Start()
    {
        optionsSliders[0].value = audioStatics.MasterVolume;
        optionsSliders[1].value = audioStatics.BGMVolume;
        optionsSliders[2].value = audioStatics.SFXVolume;
        optionsSliders[3].value = audioStatics.VoiceVolume;
        optionsSliders[4].value = audioStatics.TextSpeedMultiplier;
        voiceVol.volume = audioStatics.VoiceVolume * audioStatics.MasterVolume;
        buttonTXT.text = audioStatics.interractButton.ToUpper();
    }
    void Update()
    {
        if (waitingForKeyPress)
        {
            buttonTXT.text = "Press A Key";
            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    //make sure they cannot assign w, a, s, d or up, down, left, right,
                    if (keyCode == KeyCode.W || keyCode == KeyCode.A || keyCode == KeyCode.S || keyCode == KeyCode.D || keyCode == KeyCode.UpArrow || keyCode == KeyCode.DownArrow || keyCode == KeyCode.LeftArrow || keyCode == KeyCode.RightArrow)
                    {
                        //wait for 1 second
                        StartCoroutine(WaitForKeyPress(keyCode));
                        continue;
                    }
                    keyPressed = keyCode.ToString();
                    buttonTXT.text = keyPressed.ToUpper();
                    waitingForKeyPress = false;
                    am.playSFX(25);
                    break;
                }
            }
        }
    }

    IEnumerator WaitForKeyPress(KeyCode keyCode)
    {
        waitingForKeyPress = false;
        buttonTXT.text = "Cannot Assign " + keyCode.ToString().ToUpper();
        yield return new WaitForSeconds(1f); // Wait for 1 second
        //return to old text
        buttonTXT.text = keyPressed;
    }


    public void StartWaitingForInput()
    {
        waitingForKeyPress = true;
    }
    public void onMasterSliderChanged(float value)
    {
        audioStatics.MasterVolume = value;
        if (audioManager.currentlyPlaying)
        {
            audioManager.currentlyPlaying.volume = audioStatics.MasterVolume * audioStatics.BGMVolume;
        }
    }
    public void onBGMSliderChanged(float value)
    {
        audioStatics.BGMVolume = value;
        if (audioManager.currentlyPlaying)
        {
            audioManager.currentlyPlaying.volume = audioStatics.MasterVolume * audioStatics.BGMVolume;
        }
    }
    public void onSFXSliderChanged(float value)
    {
        audioStatics.SFXVolume = value;
    }
    public void onVoiceSliderChanged(float value)
    {
        audioStatics.VoiceVolume = value;
        voiceVol.volume = audioStatics.VoiceVolume * audioStatics.MasterVolume;
    }
    public void onTextSpeedSliderChanged(float value)
    {
        audioStatics.TextSpeedMultiplier = value;
    }
    public void deleteSaveData()
    {
        //playerController.DeleteSave();
        DeleteSaveMenu.SetActive(true);
    }
}

