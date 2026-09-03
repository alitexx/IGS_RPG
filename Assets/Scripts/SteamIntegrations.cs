using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
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

        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);

        EnforceInvariantCulture();

        Instance = this;
        DontDestroyOnLoad(gameObject); // keep this alive across scenes
    }

    public static void EnforceInvariantCulture()
    {
        // Force the main application thread to remain uniform globally
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;


        // Ensure the current active running thread matches
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;
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
