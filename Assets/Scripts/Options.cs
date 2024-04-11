using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;
using UnityEngine.UI;
using TMPro;

public class Options : MonoBehaviour
{
    //for changing text speed
    [SerializeField] private DialogueSystem ds;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI buttonTXT;
    [SerializeField] private audioManager am;
    [SerializeField] private AudioSource voiceVol;
    private bool waitingForKeyPress = false;
    private string keyPressed;

    void Start()
    {
        //button.onClick.AddListener(StartWaitingForInput);
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
                    keyPressed = keyCode.ToString();
                    buttonTXT.text = keyPressed;
                    waitingForKeyPress = false;
                    am.playSFX(25);
                    break;
                }
            }
        }
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
        ds.architect.speedMultiplier = value;
    }


}

