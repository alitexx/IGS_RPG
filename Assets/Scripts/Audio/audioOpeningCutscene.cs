using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioOpeningCutscene : MonoBehaviour
{
    public audioManager am;
    private void Awake()
    {
        am.playBGM("T1", 0.5f);
    }
}
