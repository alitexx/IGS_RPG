using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBlock : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;
    private BoxCollider2D myBoxCollider;
    // Start is called before the first frame update
    void Start()
    {
        myBoxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.BattleTutorialCleared == 1)
        {
            myBoxCollider.enabled = true;
        }
    }
}
