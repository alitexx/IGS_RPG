using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keycodeManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        audioStatics.interractButton = audioStatics.keycodeInterractButton.ToString();
        if(audioStatics.keycodeInterractButton == KeyCode.Mouse0)
        {
            audioStatics.interractButton = "L Click";
        }
    }

}
