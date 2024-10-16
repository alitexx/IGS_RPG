using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SavepointScript : MonoBehaviour
{

    public GameObject Target;
    float maxDistance = 1.5f;
    float DistanceBetweenObjects;
    public LevelManager levelManager;
    public GameObject battleMenu;
    public GameObject saveFirstButton;
    public audioManager audioManager;
    public PlayerController playerController;
    public GameObject SaveMenu;
    [SerializeField] private mainDialogueManager mainDialogueManager;
    //public GameObject SaveConfirm;

    // Start is called before the first frame update
    private void Update()
    {

        DistanceBetweenObjects = Vector3.Distance(transform.position, Target.transform.position);

        if (DistanceBetweenObjects <= maxDistance && Input.GetKeyDown(audioStatics.keycodeInterractButton) && !mainDialogueManager.dialogueRunning && SaveMenu.activeInHierarchy == false && !PauseMenu.GamePaused && battleMenu.activeInHierarchy == false)
        {
            //call function to heal, and eventually save.
            levelManager.FullHeal();
            audioManager.playSFX(19);
            SaveMenu.SetActive(true);
            playerController.isfrozen = true;
            PauseMenu.canOpenPause = false;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(saveFirstButton);
        }

    }

    public void confirmedSave()
    {
        playerController.saveGame();
        //confirm save/heal as a menu or something (name.setactive(true)
        Debug.Log("YIPEEEE");
        PauseMenu.canOpenPause = true;
    }
}
