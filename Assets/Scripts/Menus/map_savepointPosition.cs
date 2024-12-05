using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class map_savepointPosition : MonoBehaviour
{
    [SerializeField] private GameObject alanCursorInQuestion;
    [SerializeField] private GameObject thisGameObject;
    private int noAlanx = -8;
    private int withAlanx = 1;



    private void Update()
    {
        // If Alan is not in this room, move to original position
        if (!alanCursorInQuestion.activeInHierarchy)
        {
            // Update x position when Alan is not in the room
            Debug.Log("Alan is NOT in this room");
            Vector3 newPosition = thisGameObject.transform.localPosition; // Use localPosition
            newPosition.x = noAlanx; // Set the x value
            thisGameObject.transform.localPosition = newPosition;
        }
        else
        {
            Debug.Log("Alan is in this room");
            // Update x position when Alan is in the room
            Vector3 newPosition = thisGameObject.transform.localPosition; // Use localPosition
            newPosition.x = withAlanx; // Set the x value
            thisGameObject.transform.localPosition = newPosition;
        }
    }
}
