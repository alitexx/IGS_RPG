using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public mapManager MapManager;
    public GameObject associatedExclamation;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            // Get the name of the object and split it by the underscore
            string roomName = this.gameObject.name;
            string[] roomParts = roomName.Split('_');

            // Parse the first part as the floor number and the second part as the room number
            if (roomParts.Length == 2 && int.TryParse(roomParts[0].Substring(4), out int floorNumber) && int.TryParse(roomParts[1], out int roomNumber))
            {
                Debug.Log($"Floor: {floorNumber}, Room: {roomNumber}");

                // Call the function with the two integers
                MapManager.discoverNewRoom(floorNumber, roomNumber);
                if (associatedExclamation != null)
                {
                    // Call the function with the two integers
                    MapManager.discoverNewRoom(floorNumber, roomNumber, associatedExclamation);
                }
            }
            else
            {
                Debug.LogError("Invalid room name format. Name that triggered error:" + roomName);
            }
        }
    }
}
