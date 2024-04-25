using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavepointScript : MonoBehaviour
{

    public GameObject Target;
    float maxDistance = 1.5f;
    float DistanceBetweenObjects;
    public LevelManager levelManager;
    public audioManager audioManager;
    public PlayerController playerController;
    public GameObject SaveMenu;
    [SerializeField] private mainDialogueManager mainDialogueManager;
    //public GameObject SaveConfirm;

    // Start is called before the first frame update
    private void Update()
    {

        DistanceBetweenObjects = Vector3.Distance(transform.position, Target.transform.position);

        if (DistanceBetweenObjects <= maxDistance && Input.GetKeyDown(audioStatics.interractButton) && !mainDialogueManager.dialogueRunning)
        {
            //call function to heal, and eventually save.
            levelManager.FullHeal();
            audioManager.playSFX(19);
            SaveMenu.SetActive(true);
            playerController.isfrozen = true;
            PauseMenu.canOpenPause = false;
        }

    }

    public void confirmedSave()
    {
        playerController.saveGame();
        //confirm save/heal as a menu or something (name.setactive(true)
        Debug.Log("YIPEEEE");
    }
}
