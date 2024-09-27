using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapManager : MonoBehaviour
{
    //Have each room associated with a number. Checks the array of gameobjects using the number of that room, and if that game object is not active, activate it.
    //Can't lose progress in rooms, so we never have to worry about disabling them
    //Need to save the rooms they have entered = new variable to save. Sorry alex
    //do one int per floor, as you can't go back so previous floors do not matter.
    public GameObject battleUI;
    public fadeInScript fadeInScript;
    [SerializeField] public PlayerController playerController;
    [SerializeField] private mainDialogueManager mainDialogueManager;
    private int floor1RoomsDiscovered, floor2RoomsDiscovered, floor3RoomsDiscovered, floor4RoomsDiscovered;
    //The maps per floor
    [SerializeField] private GameObject floor1Map, floor2Map, floor3Map, floor4Map, mapParent, pauseMenu;
    [SerializeField] private GameObject[] floor1Rooms, alanFloor1, floor2Rooms, alanFloor2, floor3Rooms, alanFloor3, floor4Rooms, alanFloor4;
    //How will I keep track of which rooms have exclamation points? A switch statement when walking into that room?
    [SerializeField] private GameObject[] exclamationPoints;
    //TEMP VARIABLES (until I figure out what is where)
    private int floorNumber;


    public void setMap()
    {
        //Check what floor we're on
        switch (floorNumber)
        {
            case 1:
                floor1Map.SetActive(true);
                setUpMapOnReload(floor1RoomsDiscovered);
                break;
            case 2:
                floor2Map.SetActive(true);
                setUpMapOnReload(floor2RoomsDiscovered);
                break;
            case 3:
                floor3Map.SetActive(true);
                setUpMapOnReload(floor3RoomsDiscovered);
                break;
            case 4:
                floor4Map.SetActive(true);
                setUpMapOnReload(floor4RoomsDiscovered);
                break;
            default:
                break;
        }
    }

    public void setUpMapOnReload(int discovered)
    {
        //discovered is a long binary number. Split it into characters and then into bools, if true then they have discovered this room. set active to true.

    }

    //Each time the user walks into a room, check if that room has been traversed before. If it has, do nothing. If it hasn't, add it to the map.
    //Before doing all of this, check if there's an active exclamation point in the room. If there is, then disable it.
    private void discoverNewRoom(int whatRoom)
    {
        //whatRoom is the position in the integer to change the value
        switch (floorNumber)
        {
            case 1:
                //Another case statement in here for the room. If its a room that has an exclamation point, deactivate it. Do this for all floors.
                break;
            case 2:
                //
                break;
            case 3:
                //
                break;
            case 4:
                //
                break;
            default:
                break;
        }
    }

    //Check for when they press the M key, or whatever X is on the SNES controller
    //Make sure they aren't in dialogue or battle when they open the map. They can open it while the pause menu is open, but it just makes my life more complicated
    //Freeze Player

    //Have another function that forces the menu to open

    void Update()
    {
        //CHANGE JOYSTICK BUTTON TO X
        if ((Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.JoystickButton2)) && (!mainDialogueManager.dialogueRunning || battleUI.activeInHierarchy == false))
        {
            if (!mapParent.activeInHierarchy)
            {
                playerController.isfrozen = true;
                fadeInScript.fadeToMap();
            }
            else
            {
                fadeInScript.fadeOutOfmap();
            }
            }
        }


    public void OpenMap()
    {
        mapParent.SetActive(true);
    }
    public void CloseMap()
    {
        //add something here to check if any other menus are open (continue to next level, save game)
        //make sure to go back to save game script and make sure it doesn't open when players are looking at map
        if (!pauseMenu.activeInHierarchy)
        {
            playerController.isfrozen = false;
        }
        mapParent.SetActive(false);
    }

    //Used when something is unlocked. Open map, show an exclamation point where there has been a change, and closes it.
    //Maybe do some sparkly sfx to show that a new thing has happened and that its good!
    //Keep this exclamation point up until they walk to that room, then it disappears
    private void ForceOpenMap(int whichExclamation)
    {
        playerController.isfrozen = true;
        fadeInScript.fadeToMap();
        //find and display the right exclamation point
        exclamationPoints[whichExclamation].SetActive(true);
        //wait 3 seconds somehow
        fadeInScript.fadeOutOfmap();
    }
}
