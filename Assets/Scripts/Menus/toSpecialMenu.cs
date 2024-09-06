using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class toSpecialMenu : MonoBehaviour
{
    [SerializeField] private RectTransform[] locations;
    [SerializeField] private RectTransform specialMenu, characterInspectionMenu;

    public void specialMenuButtonClick()
    {
        if (specialMenu.gameObject.activeInHierarchy)
        {
            //Open Character Inspection
            ChangeMenu(characterInspectionMenu, specialMenu);
        } else
        {
            //Open Special Menu
            ChangeMenu(specialMenu, characterInspectionMenu);
        }
    }

    public void ChangeMenu(RectTransform enteringMenu, RectTransform exitingMenu)
    {
        //Tween exiting menu out
        exitingMenu.DOMove(locations[1].position, 0.2f).OnComplete(() => {
            exitingMenu.gameObject.SetActive(false);
            //When done, enable entering menu and disable exiting menu
            //Move menus to where they need to be (entering should be more to the right, exiting should be back in the middle)
            exitingMenu.SetPositionAndRotation(locations[0].position, locations[0].rotation);
            enteringMenu.SetPositionAndRotation(locations[1].position, locations[1].rotation);
            enteringMenu.gameObject.SetActive(true);
            //tween back to middle
            enteringMenu.DOMove(locations[0].position, 0.35f);
        });
    }
}
