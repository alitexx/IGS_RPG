using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamIntegrations : MonoBehaviour
{

    public static SteamIntegrations Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        { 
            Destroy(gameObject); // prevent duplicates
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // keep this alive across scenes


    }
    void Start()
    {
        try
        {
            Steamworks.SteamClient.Init(3802720);
            Debug.Log(Steamworks.SteamClient.Name);
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }

    void Update()
    {
        Steamworks.SteamClient.RunCallbacks();
    }

    void OnApplicationQuit()
    {
        Steamworks.SteamClient.Shutdown();
    }

    public void UnlockAchievement(string id)
    {
        var ach = new Steamworks.Data.Achievement(id);
        ach.Trigger();

        Debug.Log($"Achievement {id} unlocked");
    }
}
