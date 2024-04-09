using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class continueDropDown : MonoBehaviour
{
    [SerializeField] private RectTransform menu;
    [SerializeField] private RectTransform[] locations;
    private void OnEnable()
    {
        menu.DOMove(locations[0].position, 0.75f);
    }

    public void closeContinueMenu()
    {
        menu.DOMove(locations[1].position, 0.75f).OnComplete(() => { menu.gameObject.SetActive(false); });
    }
}
