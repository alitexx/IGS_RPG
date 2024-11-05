using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class changeSelectedButtonOnActivate : MonoBehaviour
{
    // The button to select when this GameObject is activated
    public Button targetButton;

    private void OnEnable()
    {
        // Ensure a button is assigned
        if (targetButton != null)
        {
            // Set the target button as the selected one in the EventSystem
            EventSystem.current.SetSelectedGameObject(targetButton.gameObject);
        }
        else
        {
            Debug.LogWarning("No target button assigned to SelectButtonOnActivate script on " + gameObject.name);
        }
    }
}
