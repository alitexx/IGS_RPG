using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelTeleports : MonoBehaviour
{

    //public Transform Teleport1;
    //public Transform Teleport2;
    public GameObject portalParent1;
    public GameObject portalParent2;
    public GameObject portalParent3;
    public GameObject portalParent4;
    public Collider2D Player;
    public Transform destination1;
    public Transform destination2;
    public Transform destination3;
    public Transform destination4;

    public GameObject continueToNextLevelBTN;

    public GameObject ContinueUI;
    public PlayerController PlayerController;

    public audioManager audioManager;

    public float distance = 0.2f;
    public int Level = 1;
    [SerializeField] private mapManager mapManager;
    [SerializeField] private BattleController battleController;

    private void Start()
    {
        //Only one of them has a map manager attached
        if(mapManager != null)
        {
            mapManager.newLevelMapUpdate(Level);
        }
    }

    //We can add this back later, but atm i think this might be unneccessary
    //public void Update()
    //{
    //    Level = PlayerController.Level;
    //}

    public void OnTriggerEnter2D(Collider2D col)
    {
        //Checks level when player goes to try a new level
        Level = PlayerController.Level;
        if (Vector2.Distance(transform.position, col.transform.position) > distance)
        {
            EventSystem.current.SetSelectedGameObject(null);
            PauseMenu.canOpenPause = false;
            ContinueUI.SetActive(true);
            EventSystem.current.SetSelectedGameObject(continueToNextLevelBTN);
            PlayerController.isfrozen = true;
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        PauseMenu.canOpenPause = true;
    }

    public void Next()
    {
        EventSystem.current.SetSelectedGameObject(null);
        audioManager.playSFX(23);
        PlayerController.isfrozen = false;
        PauseMenu.canOpenPause = true;
        if (Level == 1)
        {
            Player.transform.position = new Vector3(destination1.position.x, destination1.position.y);
            Destroy(portalParent1);
        }
        else if (Level == 2)
        {
            Player.transform.position = new Vector3(destination2.position.x, destination2.position.y);
            Destroy(portalParent2);
        }
        else if(Level == 3)
        {
            Player.transform.position = new Vector3(destination3.position.x, destination3.position.y);
            Destroy(portalParent3);
        }
        else
        {
            Player.transform.position = new Vector3(destination4.position.x, destination4.position.y);
            Destroy(portalParent4);
        }
        battleController.hasContemplatedKilling = false; //reset for supports
        mapManager.newLevelMapUpdate(PlayerController.Level);
    }

    public void Stay()
    {
        EventSystem.current.SetSelectedGameObject(null);
        PauseMenu.canOpenPause = true;
        PlayerController.isfrozen = false;
        ContinueUI.SetActive(false);
        return;
    }
}
