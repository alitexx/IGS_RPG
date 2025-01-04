using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class changeSelectedButtonOnActivate : MonoBehaviour
{
    // The button to select when this GameObject is activated
    [SerializeField] private Button targetButton;
    [SerializeField] private Button toContinue;

    [SerializeField] private TutorialHandler tutorialHandler;

    private void OnEnable()
    {
        // Ensure a button is assigned
        if (targetButton != null)
        {
            // Set the target button as the selected one in the EventSystem
            EventSystem.current.SetSelectedGameObject(targetButton.gameObject);

            // Add a listener to the button click event
            targetButton.onClick.AddListener(OnTargetButtonPressed);
        }
        else
        {
            Debug.LogWarning("No target button assigned to SelectButtonOnActivate script on " + gameObject.name);
        }
    }

    private void OnDisable()
    {
        // Remove the listener when the GameObject is disabled to prevent duplicate calls
        if (targetButton != null)
        {
            targetButton.onClick.RemoveListener(OnTargetButtonPressed);
        }
    }

    // This function is called when the target button is pressed
    private void OnTargetButtonPressed()
    {
        // Ensure this only runs if the GameObject is active
        if (gameObject.activeInHierarchy)
        {
            tutorialHandler.continueTutorial();
        }
    }
}

