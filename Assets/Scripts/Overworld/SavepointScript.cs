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
    //public GameObject SaveConfirm;

    // Start is called before the first frame update
    private void Update()
    {

        DistanceBetweenObjects = Vector3.Distance(transform.position, Target.transform.position);

        if (DistanceBetweenObjects <= maxDistance && Input.GetKeyDown(audioStatics.interractButton))
        {
            //call function to heal, and eventually save.
            SaveMenu.SetActive(true);
            playerController.isfrozen = true;
            
        }

    }

    public void confirmedSave()
    {
        levelManager.FullHeal();
        audioManager.playSFX(19);
        playerController.saveGame();
        //confirm save/heal as a menu or something (name.setactive(true)
        Debug.Log("YIPEEEE");
    }
}
