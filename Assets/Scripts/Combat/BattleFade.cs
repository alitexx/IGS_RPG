using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFade : MonoBehaviour
{
    public GameObject battleFadeObj;
    public Animator animator;
    public GameObject battleUI;


    void Start()
    {
        battleFadeObj.SetActive(true);
    }

    public void BattleStarted()
    {
        animator.SetBool("BattleStarting", false);
        battleUI.SetActive(true);
    }

    public void BattleEnded()
    {
        animator.SetBool("BattleOver", false);
    }
}
