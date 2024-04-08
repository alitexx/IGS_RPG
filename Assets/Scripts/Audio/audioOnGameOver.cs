using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioOnGameOver : MonoBehaviour
{
    [SerializeField] audioManager am;
    void Start()
    {
        am.playBGM("T8");
    }
}
