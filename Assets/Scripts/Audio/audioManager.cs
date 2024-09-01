using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class audioManager : MonoBehaviour
{
    [SerializeField] private AudioSource[] BGMAvailable;
    [SerializeField] private AudioSource[] SFXAvailable;
    [SerializeField] private AudioSource voiceVol;
    public static AudioSource currentlyPlaying;
    private int songID = 20; // for playbgm

    // HOW IT WORKS!!
    // you have to pass in the song name and the speed you'd like it to fade in/out
    // speed defaults to 1, but SPEED MUST ALWAYS BE GREATER THAN 0.1!!!
    // A script calls playBGM, playBGM checks to see if there is a song currently playing.
    // it then finds the audio using the string passed in. it reassigns currentlyPlaying to the new audio.
    private void Start()
    {
        voiceVol.volume = audioStatics.VoiceVolume * audioStatics.MasterVolume;
        BGMAvailable[0].DOFade(audioStatics.BGMVolume * audioStatics.MasterVolume, 0.5f);
    }
    public void playBGM(string songToPlay, float speed = 1)
    {
        switch (songToPlay.ToUpper()) // change songID based on the input
        {
            case "T1": case "OPENING":
                songID = 0;
                break;
            case "T2": case "OVERWORLD":
                songID = 1;
                break;
            case "T3": case "BATTLE":
                songID = 2;
                break;
            case "T4": case "TENSE": case "CONVERSING(TENSE)":
                songID = 3;
                break;
            case "T5": case "HAPPY": case "CONVERSING(HAPPY)":
                songID = 4;
                break;
            case "T6": case "PARTYCIDE":
                songID = 5;
                break;
            case "T7": case "LICH": case "THE LICH": case "THELICH":
                songID = 6;
                break;
            case "T8": case "GAME OVER": case "GAMEOVER": case "DEFEAT":
                songID = 7;
                break;
            case "T9": case "VICTORY":
                songID = 8;
                break;
            case "T10": case "CREDITS":
                songID = 9;
                break;
            default:
                Debug.LogWarning("The BGM [" + songToPlay + "] could not be found.");
                break;
        }
        if (BGMAvailable[songID] == currentlyPlaying) // if the song is already playing, do nothing
        {
            return;
        }
        else
        {
            stopBGM(speed, true);
            playSongUsingID(songID, speed);
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
                currentlyPlaying.DOFade(0, (speed - 0.05f)).OnComplete(() => { 
                    //this is the only music that stays at the same pitch
                    if(currentlyPlaying != BGMAvailable[1])
                    {
                        currentlyPlaying.pitch = 1f;
                    }
                });
            } else
            {
                currentlyPlaying.DOFade(0, (speed - 0.05f)).OnComplete(() => { currentlyPlaying.Stop(); currentlyPlaying.pitch = 1f; });
            }
        }
    }
    public void stopHeartbeatSFX() // only used for heartbeat
    {
        SFXAvailable[13].volume = 0f;
        SFXAvailable[13].Stop();
    }

    public void playSFX(string sfx)
    {
        switch (sfx.ToUpper())
        {
            case "START":
                playSFXUsingID(0);
                break;
            case "SLICE":
                playSFXUsingID(1);
                break;
            case "PUNCH":
                playSFXUsingID(2);
                break;
            case "FIRE":
                playSFXUsingID(3);
                break;
            case "ICE":
                playSFXUsingID(4);
                break;
            case "WIND":
                playSFXUsingID(5);
                break;
            case "THUNDER":
                playSFXUsingID(6);
                break;
            case "TAUNT":
                playSFXUsingID(7);
                break;
            case "EXAMINE":
                playSFXUsingID(8);
                break;
            case "SING":
                playSFXUsingID(9);
                break;
            case "TAKEDAMAGE":
                playSFXUsingID(10);
                break;
            case "HEAL":
                playSFXUsingID(11);
                break;
            case "DEFEND":
                playSFXUsingID(12);
                break;
            case "HEARTBEAT":
                playSFXUsingID(13);
                break;
            case "LEVELUP":
                playSFXUsingID(14);
                break;
            case "ENEMYDEFEATED":
                playSFXUsingID(15);
                break;
            case "GAINEXP":
                playSFXUsingID(16);
                break;
            case "DIE":
                playSFXUsingID(17);
                break;
            case "SAVE":
                playSFXUsingID(18);
                break;
            case "LEVERPULLED":
                playSFXUsingID(19);
                break;
            case "GARGOYLEINPLACE":
                playSFXUsingID(20);
                break;
            case "PUSHINGGARGOYLE":
                playSFXUsingID(21);
                break;
            case "DOORUNLOCKED":
                playSFXUsingID(22);
                break;
            case "CLIMBSTAIRS":
                playSFXUsingID(23);
                break;
            case "CONFIRM":
                playSFXUsingID(24);
                break;
            case "HOVER":
                playSFXUsingID(25);
                break;
            case "PAUSE":
                playSFXUsingID(26);
                break;
            case "UNPAUSE":
                playSFXUsingID(27);
                break;
            case "DENY":
                playSFXUsingID(28);
                break;
            case "DECLINE":
                playSFXUsingID(29);
                break;
            default:
                Debug.LogWarning("The SFX [" + sfx + "] could not be found.");
                break;
        }
    }
    public void playSFX(int id)
    {
        playSFXUsingID(id-1);
    }

    private void playSFXUsingID(int ID)
    {
        if(ID == 13)
        {
            SFXAvailable[ID].DOFade((audioStatics.SFXVolume * audioStatics.MasterVolume)/3, (60f));
            SFXAvailable[ID].Play();
            return;
        }
        SFXAvailable[ID].volume = (audioStatics.SFXVolume * audioStatics.MasterVolume);
        SFXAvailable[ID].Play();
    }

    //private void PlaySongUsingID(int ID, float speed)
    //{
    //    if (ID == 9)
    //    {
    //        BGMAvailable[10].volume = (audioStatics.SFXVolume * audioStatics.MasterVolume);
    //        BGMAvailable[10].Play();
    //        while (!BGMAvailable[10].isPlaying)
    //        {
    //            //play other music
    //        }
    //    } else
    //    {
    //        BGMAvailable[ID].Play();
    //        BGMAvailable[ID].DOFade(audioStatics.BGMVolume * audioStatics.MasterVolume, speed).OnComplete(() =>
    //        {
    //            if (currentlyPlaying)
    //            {
    //                currentlyPlaying.Stop();
    //            }
    //            currentlyPlaying = BGMAvailable[ID];
    //        });
    //    }
    //}

    private void playSongUsingID(int ID, float speed)
    {
        if (ID == 8) // if this is the you win screen
        {
            // Play the first audio clip
            AudioSource firstAudioSource = BGMAvailable[10];
            firstAudioSource.volume = audioStatics.BGMVolume * audioStatics.MasterVolume;
            firstAudioSource.pitch = 1;
            firstAudioSource.Play();
            if (youWinMenu.killedPartyMember) // if a party member has been killed, change pitch
            {
                changePitch(10, 0.95f, 2f);
            }

            // Schedule the second audio clip to play after the first one finishes
            StartCoroutine(PlayVictory(firstAudioSource, ID, speed));
        }
        else
        {
            // Play the second audio clip directly
            PlayOtherAudio(ID, speed);
        }
    }

    private IEnumerator PlayVictory(AudioSource firstAudioSource, int ID, float speed)
    {
        // Wait until the first audio clip finishes playing
        while (firstAudioSource.isPlaying)
        {
            yield return null;
        }

        // Play the second audio clip
        PlayOtherAudio(ID, speed);
    }

    private void PlayOtherAudio(int ID, float speed)
    {
        if (youWinMenu.killedPartyMember)
        {
            BGMAvailable[ID].pitch = 0.8f;
        }
        BGMAvailable[ID].Play();
        BGMAvailable[ID].DOFade(audioStatics.BGMVolume * audioStatics.MasterVolume, speed).OnComplete(() =>
        {
            if (youWinMenu.killedPartyMember)
            {
                BGMAvailable[10].pitch = 1f;
            }
            if (currentlyPlaying)
            {
                currentlyPlaying.Stop();
            }
            currentlyPlaying = BGMAvailable[ID];
        });
    }


    public void changePitch(int id, float toValue = 0.8f, float speed = 5f)
    {
        BGMAvailable[id].DOPitch(toValue, speed);
    }
}
