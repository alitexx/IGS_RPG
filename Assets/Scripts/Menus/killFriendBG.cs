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

    // This function is called when the behaviour becomes disabled or inactive
    void OnDisable()
    {
        this.gameObject.GetComponent<CanvasGroup>().alpha = 0;
    }
}
