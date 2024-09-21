using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class optionsScrollManager : MonoBehaviour, ISelectHandler
{
    public Scrollbar targetScrollbar; // The slider to be moved
    public float buttonCount; // To store the count of how many buttons there are
    public float thisButtonNum; // The number out of button count this button is

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("I HAVE BEEN SELECTED");
        // When this slider/button is selected, adjust the target slider
        MoveSliderByScriptCount();
    }

    private void MoveSliderByScriptCount()
    {
        Debug.Log(targetScrollbar.value);
        // Example: Move the target slider by a value relative to the number of scripts
        targetScrollbar.value = 1.15f - (1/buttonCount)*thisButtonNum;
        Debug.Log(1.15f - (1 / buttonCount) * thisButtonNum);
    }
}
