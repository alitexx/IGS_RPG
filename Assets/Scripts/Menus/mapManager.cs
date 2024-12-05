using System;
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

    //Floor1 has 11 rooms, meaning it should be 11 numbers long. The default should be:
    //floor1RoomsDiscovered = 00000000000

    //Floor2 has 13 rooms, meaning it should be 13 numbers long. The default should be:
    //floor2RoomsDiscovered = 0000000000000

    //Floor3 has 15 rooms, meaning it should be 15 numbers long. The default should be:
    //floor3RoomsDiscovered = 000000000000000

    //Floor4 has 12 rooms, meaning it should be 12 numbers long. The default should be:
    //floor4RoomsDiscovered = 000000000000

    private int floor1RoomsDiscovered, floor2RoomsDiscovered, floor3RoomsDiscovered, floor4RoomsDiscovered = 000000000000000000000;
    //The maps per floor
    [SerializeField] private GameObject floor1Map, floor2Map, floor3Map, floor4Map, mapParent, pauseMenu;
    [SerializeField] private GameObject[] floor1Rooms, alanFloor1, floor2Rooms, alanFloor2, floor3Rooms, alanFloor3, floor4Rooms, alanFloor4;
    private bool openingClosingMenu = false;
    //How will I keep track of which rooms have exclamation points? A switch statement when walking into that room?
    private int floorNumber;
    //Keeps track of where Alan is. This needs to be passed in on a reloaded save.
    private int currentRoomNumber = 0;

    public int getRoomsDiscovered(int whichRoom)
    {
        switch (whichRoom)
        {
            case 1: return floor1RoomsDiscovered;
            case 2: return floor2RoomsDiscovered;
            case 3: return floor3RoomsDiscovered;
            case 4: return floor4RoomsDiscovered;
            default: return 0;
        }
    }

    public void setRoomsDiscovered(int whichRoom, int whatValue)
    {
        switch (whichRoom)
        {
            case 1: 
                floor1RoomsDiscovered = whatValue;
                break;
            case 2:
                floor2RoomsDiscovered = whatValue;
                break;
            case 3:
                floor3RoomsDiscovered = whatValue;
                break;
            case 4:
                floor4RoomsDiscovered = whatValue;
                break;
        }
    }

    //Updates the map when we get to a new level
    public void newLevelMapUpdate(int whatLevel)
    {
        floorNumber = whatLevel;
        Debug.Log(floorNumber);
        switch (floorNumber)
        {
            case 1:
                floor1Map.SetActive(true);
                break;
            case 2:
                floor1Map.SetActive(false);
                floor2Map.SetActive(true);
                break;
            case 3:
                floor2Map.SetActive(false);
                floor3Map.SetActive(true);
                break;
            case 4:
                floor3Map.SetActive(false);
                floor4Map.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void setMap()
    {
        //Check what floor we're on
        switch (floorNumber)
        {
            case 1:
                floor1Map.SetActive(true);
                setUpMapOnReload(floor1RoomsDiscovered, floor1Rooms);
                break;
            case 2:
                floor2Map.SetActive(true);
                setUpMapOnReload(floor2RoomsDiscovered, floor2Rooms);
                break;
            case 3:
                floor3Map.SetActive(true);
                setUpMapOnReload(floor3RoomsDiscovered, floor3Rooms);
                break;
            case 4:
                floor4Map.SetActive(true);
                setUpMapOnReload(floor4RoomsDiscovered, floor4Rooms);
                break;
            default:
                break;
        }
    }

    public void setUpMapOnReload(int discovered, GameObject[] arrayToCheck)
    {

        // Convert the integer into a binary string representation.
        string binaryString = Convert.ToString(discovered, 2);

        // Reverse the string if needed to match your game object's array (optional).
        char[] binaryArray = binaryString.PadLeft(arrayToCheck.Length, '0').ToCharArray();

        // Loop through each character in the binary array.
        for (int i = 0; i < arrayToCheck.Length; i++)
        {
            // Set the room active if the corresponding binary character is '1'.
            arrayToCheck[i].SetActive(binaryArray[i] == '1');
        }
    }


    //Each time the user walks into a room, check if that room has been traversed before. If it has, do nothing. If it hasn't, add it to the map.
    //Before doing all of this, check if there's an active exclamation point in the room. If there is, then disable it.
    //This event fires EACH TIME ALAN WALKS INTO A ROOM. MAKE SURE IT FIRES EACH TIME YOU WALK INTO A ROOM!
    public void discoverNewRoom(int floor, int whatRoom, GameObject associatedExclamation = null)
    {
        if(floor != floorNumber)
        {
            newLevelMapUpdate(floor);
        }
        // Disable the exclamation point if it's active.
        if(associatedExclamation != null)
        {
            associatedExclamation.SetActive(false);
        }
        //Set current room
        currentRoomNumber = whatRoom-1;

        // Get the current discovered rooms based on the floor.
        int currentRoomsDiscovered = GetCurrentDiscoveredRooms();

        string binaryString;
        // Convert the current discovered rooms into a binary string.
        switch (floorNumber)
        {
            case 1:
                binaryString = Convert.ToString(currentRoomsDiscovered, 2).PadLeft(floor1Rooms.Length, '0');
                break;
            case 2:
                binaryString = Convert.ToString(currentRoomsDiscovered, 2).PadLeft(floor2Rooms.Length, '0');
                break;
            case 3:
                binaryString = Convert.ToString(currentRoomsDiscovered, 2).PadLeft(floor3Rooms.Length, '0');
                break;
            case 4:
                binaryString = Convert.ToString(currentRoomsDiscovered, 2).PadLeft(floor4Rooms.Length, '0');
                break;
            default:
                throw new ArgumentOutOfRangeException("Invalid floor number");
        }

        // Check if the player has already discovered the room.
        if (binaryString[whatRoom-1] == '1')
        {
            // Room already discovered, do nothing.
            return;
        }

        // If the room hasn't been discovered, change the corresponding '0' to '1'.
        char[] binaryArray = binaryString.ToCharArray();
        binaryArray[whatRoom-1] = '1';

        //Enable the room on the map found there (another switch statement i know)
        switch (floorNumber)
        {
            case 1:
                floor1Rooms[currentRoomNumber].SetActive(true);
                break;
            case 2:
                floor2Rooms[currentRoomNumber].SetActive(true);
                break;
            case 3:
                floor3Rooms[currentRoomNumber].SetActive(true);
                break;
            case 4:
                floor4Rooms[currentRoomNumber].SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException("Invalid floor number");
        }

        // Convert the updated binary array back to a string.
        string updatedBinaryString = new string(binaryArray);

        // Convert the updated binary string back to an integer.
        int updatedRoomsDiscovered = Convert.ToInt32(updatedBinaryString, 2);

        // Save the updated integer.
        SaveDiscoveredRooms(updatedRoomsDiscovered);
        Debug.Log(updatedBinaryString);
    }

    // Function to get the current discovered rooms based on the floor.
    private int GetCurrentDiscoveredRooms()
    {
        switch (floorNumber)
        {
            case 1:
                return floor1RoomsDiscovered;
            case 2:
                return floor2RoomsDiscovered;
            case 3:
                return floor3RoomsDiscovered;
            case 4:
                return floor4RoomsDiscovered;
            default:
                throw new ArgumentOutOfRangeException("Invalid floor number");
        }
    }

    // Function to save the updated discovered rooms based on the floor.
    private void SaveDiscoveredRooms(int updatedRoomsDiscovered)
    {
        switch (floorNumber)
        {
            case 1:
                floor1RoomsDiscovered = updatedRoomsDiscovered;
                break;
            case 2:
                floor2RoomsDiscovered = updatedRoomsDiscovered;
                break;
            case 3:
                floor3RoomsDiscovered = updatedRoomsDiscovered;
                break;
            case 4:
                floor4RoomsDiscovered = updatedRoomsDiscovered;
                break;
            default:
                throw new ArgumentOutOfRangeException("Invalid floor number");
        }
    }


    //Check for when they press the M key, or whatever X is on the SNES controller
    //Make sure they aren't in dialogue or battle when they open the map. They can open it while the pause menu is open, but it just makes my life more complicated
    //Freeze Player

    //Have another function that forces the menu to open

    void Update()
    {
        //CHANGE JOYSTICK BUTTON TO X
        if ((Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.JoystickButton2)) && (mainDialogueManager.dialogueRunning == false && battleUI.activeInHierarchy == false && PauseMenu.canOpenPause == true) && openingClosingMenu == false)
        {
            openingClosingMenu = true;
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

    public void setOpeningClosingMenu(bool openingClosingMenu)
    {
        this.openingClosingMenu = openingClosingMenu;
    }

    public void OpenMap()
    {
        mapParent.SetActive(true);
        

        //Locate where Alan is
        switch(floorNumber)
        {
            //Display Alan
            case 1:
                alanFloor1[currentRoomNumber].SetActive(true);
                break;
            case 2:
                alanFloor2[currentRoomNumber].SetActive(true);
                break;
            case 3:
                alanFloor3[currentRoomNumber].SetActive(true);
                break;
            case 4:
                alanFloor4[currentRoomNumber].SetActive(true);
                break;
        }
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

        //Locate where Alan is
        switch (floorNumber)
        {
            //Stop displaying Alan
            case 1:
                alanFloor1[currentRoomNumber].SetActive(false);
                break;
            case 2:
                alanFloor2[currentRoomNumber].SetActive(false);
                break;
            case 3:
                alanFloor3[currentRoomNumber].SetActive(false);
                break;
            case 4:
                alanFloor4[currentRoomNumber].SetActive(false);
                break;
        }
    }

    //Used when something is unlocked. Open map, show an exclamation point where there has been a change, and closes it.
    //Maybe do some sparkly sfx to show that a new thing has happened and that its good!
    //Keep this exclamation point up until they walk to that room, then it disappears
    public void ForceOpenMap(GameObject whichExclamation)
    {
        playerController.isfrozen = true;
        fadeInScript.fadeToMap();
        // Find and display the right exclamation point
        whichExclamation.SetActive(true);

        // Start a coroutine to wait for 3 seconds before closing the menu
        StartCoroutine(WaitAndCloseMap());
    }

    private IEnumerator WaitAndCloseMap()
    {
        // Wait for 3 seconds
        yield return new WaitForSeconds(3f);

        // Fade out of map
        fadeInScript.fadeOutOfmap();

        // Unfreeze player after closing map
        playerController.isfrozen = false;
    }

}
