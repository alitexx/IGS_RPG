using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class killFriendBG : MonoBehaviour
{
    void OnEnable()
    {
        this.gameObject.GetComponent<CanvasGroup>().DOFade(1, 30);
    }

    public void fadeOut()
    {
        this.gameObject.GetComponent<CanvasGroup>().DOFade(0, 1).OnComplete(() => {
            this.gameObject.GetComponent<CanvasGroup>().alpha = 0;
            this.gameObject.SetActive(false);
            });
    }
}
