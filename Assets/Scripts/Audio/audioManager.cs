using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class audioManager : MonoBehaviour
{
    protected float MasterVolume = audioStatics.MasterVolume;
    protected float BGMVolume = audioStatics.BGMVolume;
    protected float SFXVolume = audioStatics.SFXVolume;
    [SerializeField] private AudioSource[] BGMAvailable;
    [SerializeField] private AudioSource[] SFXAvailable;

    private static AudioSource currentlyPlaying;

    // HOW IT WORKS!!
    // you have to pass in the song name and the speed you'd like it to fade in/out
    // speed defaults to 1, but SPEED MUST ALWAYS BE GREATER THAN 0.1!!!
    // A script calls playBGM, playBGM checks to see if there is a song currently playing.
    // it then finds the audio using the string passed in. it reassigns currentlyPlaying to the new audio.

    public void playBGM(string songToPlay, float speed = 1)
    {
        stopBGM(speed, true);

        switch (songToPlay.ToUpper())
        {
            case "T1": case "OPENING":
                playSongUsingID(0, speed);
                break;
            case "T2": case "OVERWORLD":
                playSongUsingID(1, speed);
                break;
            case "T3": case "BATTLE":
                playSongUsingID(2, speed);
                break;
            case "T4": case "TENSE": case "CONVERSING(TENSE)":
                playSongUsingID(3, speed);
                break;
            case "T5": case "HAPPY": case "CONVERSING(HAPPY)":
                playSongUsingID(4, speed);
                break;
            case "T6": case "PARTYCIDE":
                playSongUsingID(5, speed);
                break;
            case "T7": case "LICH": case "THE LICH": case "THELICH":
                playSongUsingID(6, speed);
                break;
            case "T8": case "GAME OVER": case "GAMEOVER": case "DEFEAT":
                playSongUsingID(7, speed);
                break;
            case "T9": case "VICTORY":
                playSongUsingID(8, speed);
                break;
            case "T10": case "CREDITS":
                playSongUsingID(9, speed);
                break;
            default:
                Debug.LogWarning("The BGM [" + songToPlay + "] could not be found.");
                break;
        }
    }


    //stops any bgm that was playing
    public void stopBGM(float speed, bool playingMusicAfter = false)
    {
        if (speed < 0.1f)
        {
            Debug.LogWarning("Cannot fade in/out songs using speed that is < 0.1 seconds!");
            return;
        }
        if (currentlyPlaying)
        {
            if (playingMusicAfter)
            {
                currentlyPlaying.DOFade(0, (speed - 0.05f));
            } else
            {
                currentlyPlaying.DOFade(0, (speed - 0.05f)).OnComplete(() => { currentlyPlaying.Stop(); });
            }
        }
        
    }

    private void playSongUsingID(int ID, float speed)
    {
        BGMAvailable[ID].Play();
        BGMAvailable[ID].DOFade(0.5f, speed).OnComplete(() => {
            if (currentlyPlaying)
            {
                currentlyPlaying.Stop();
            }
            currentlyPlaying = BGMAvailable[ID];
        });
        // TURN BACK ON ONCE BGM/MASTER VOLUME WORK!!
        //BGMAvailable[ID].DOFade(BGMVolume * MasterVolume, speed).OnComplete(() => {
        //    if (currentlyPlaying)
        //    {
        //        currentlyPlaying.Stop();
        //    }
        //    currentlyPlaying = BGMAvailable[ID];
        //});
    }
}
