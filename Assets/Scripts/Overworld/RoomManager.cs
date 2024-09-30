using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    string RoomNumberStr;
    int RoomNumberInt;

    public mapManager MapManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        RoomNumberStr = collision.gameObject.name;
        int.TryParse(RoomNumberStr, out RoomNumberInt);
        Debug.Log(RoomNumberInt);

        //call function
        MapManager.discoverNewRoom(RoomNumberInt);
    }
}
