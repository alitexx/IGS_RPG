using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private TextMeshProUGUI partyLevel;
    public static bool canOpenSave = true;
    //public GameObject SaveConfirm;

    // Start is called before the first frame update
    private void Update()
    {
        DistanceBetweenObjects = Vector3.Distance(transform.position, Target.transform.position);

        if (DistanceBetweenObjects <= maxDistance && Input.GetKeyDown(audioStatics.keycodeInterractButton) && !mainDialogueManager.dialogueRunning && SaveMenu.activeInHierarchy == false && !PauseMenu.GamePaused && battleMenu.activeInHierarchy == false && canOpenSave)
        {
            //call function to heal, and eventually save.
            levelManager.FullHeal();
            audioManager.playSFX(19);
            SaveMenu.SetActive(true);
            canOpenSave = false;
            playerController.isfrozen = true;
            PauseMenu.canOpenPause = false;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(saveFirstButton);
            partyLevel.text = ("Party Level: " + LevelManager.level);
        }

    }

    public void confirmedSave()
    {
        playerController.saveGame();
        //confirm save/heal as a menu or something (name.setactive(true)
        Debug.Log("YIPEEEE");
        PauseMenu.canOpenPause = true;
    }

    public void saveCooldown()
    {
        StartCoroutine(EnableSavepointAfterDelay());
    }

    private IEnumerator EnableSavepointAfterDelay()
    {
        yield return new WaitForSeconds(0.25f);
        canOpenSave = true;
    }
}
