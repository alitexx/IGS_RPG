using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

public class Options : MonoBehaviour
{
    //for changing text speed
    [SerializeField] private DialogueSystem ds;

    public void onMasterSliderChanged(float value)
    {
        audioStatics.MasterVolume = value;
    }
    public void onBGMSliderChanged(float value)
    {
        audioStatics.BGMVolume = value;
    }
    public void onSFXSliderChanged(float value)
    {
        audioStatics.SFXVolume = value;
    }
    public void onVoiceSliderChanged(float value)
    {
        audioStatics.VoiceVolume = value;
    }
    public void onTextSpeedSliderChanged(float value)
    {
        ds.architect.speedMultiplier = value;
    }


}
