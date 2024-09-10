using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonHoverEffect : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public GameObject otherUIWindow;  // Assign the other UI window here
    private bool isSelected = false;
    private Coroutine hoverCoroutine;
    [SerializeField] private TextMeshProUGUI skillName;
    public string skillNameText;

    // Trigger when the button is selected (via WASD/keyboard navigation)
    public void OnSelect(BaseEventData eventData)
    {
        //original name, no space
        skillNameText = skillName.text;
        skillName.text = " " + skillNameText;
        if (!isSelected)
        {
            isSelected = true;
            hoverCoroutine = StartCoroutine(ShowWindowAfterDelay(5.0f));  // 5 seconds delay
        }
    }

    // Trigger when the button is deselected (user moves away from the button)
    public void OnDeselect(BaseEventData eventData)
    {
        //remove the space from the name
        skillName.text = skillNameText;
        isSelected = false;
        if (hoverCoroutine != null)
        {
            StopCoroutine(hoverCoroutine);  // Stop the coroutine if the button is deselected
        }
        otherUIWindow.SetActive(false);  // Optionally hide the window again when deselected
    }

    // Coroutine to show the other UI window after a delay
    IEnumerator ShowWindowAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (isSelected)  // Only show the window if the button is still selected
        {
            otherUIWindow.SetActive(true);
        }
    }
}
