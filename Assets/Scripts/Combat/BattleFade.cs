using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BattleFade : MonoBehaviour
{
    public GameObject battleFadeObj;
    [SerializeField] private RectTransform battleTXT;
    public Animator animator;
    public GameObject battleUI;
    public audioManager am;
    [SerializeField] private RectTransform[] battleTXTlocations;
    [SerializeField] private GameObject youWin;
    [SerializeField] private PlayerController playerController;


    void Start()
    {
        battleFadeObj.SetActive(true);
    }

    public void BattleStarted()
    {
        animator.SetBool("BattleStarting", false);
        PauseMenu.canOpenPause = false;
        battleUI.SetActive(true);
        playerController.gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void BattleEnded()
    {
        youWin.SetActive(false);
    }

    public void playBattleStartSFX()
    {
        PauseMenu.canOpenPause = false;
        am.playSFX(1);

        // start battle text start anim

        battleTXT.gameObject.SetActive(true);
        battleTXT.DOMove(battleTXTlocations[0].position, 0.35f).OnComplete(() => {
            battleTXT.DOMove(battleTXTlocations[1].position, 0.7f).OnComplete(() => {
                battleTXT.DOMove(battleTXTlocations[2].position, 0.25f).OnComplete(() => {
                    battleTXT.position = battleTXTlocations[3].position;
                    battleTXT.gameObject.SetActive(false);
                    PauseMenu.canOpenPause = true;
                });
            });
        });
    }
}
