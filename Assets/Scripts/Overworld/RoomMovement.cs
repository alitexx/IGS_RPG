/*using Cinemachine.Utility;
using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RoomMovement : MonoBehaviour
{
    public CinemachineVirtualCamera cameraList;
    public CinemachineBrain mainCameraBrain;

    private ICinemachineCamera currentCamera;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            currentCamera = mainCameraBrain.ActiveVirtualCamera;
            if (currentCamera != cameraList)
            {
                currentCamera.VirtualCameraGameObject.SetActive(false);
                cameraList.VirtualCameraGameObject.SetActive(true);
            }
        }
    }
}


//Code that didnt work, trying something else but wanna keep this just in case.

public GameObject virtualCam;


private void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Player") && !other.isTrigger)
    {
        virtualCam.SetActive(true);
    }
}

private void OnTriggerExit2D(Collider2D other)
{
    if (other.CompareTag("Player") && !other.isTrigger)
    {
        virtualCam.SetActive(false);
    }
}
*/