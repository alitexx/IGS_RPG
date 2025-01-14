using UnityEngine;
using UnityEngine.UI;

public class specialTutorial : MonoBehaviour
{
    public GameObject targetGameObject; // The GameObject to monitor
    [SerializeField] private Button parentButton; // The parent button
    private Navigation originalNavigation; // To store the original navigation settings
    private bool navigationModified = false; // To track if navigation has been modified

    void Start()
    {
        // Get the parent button component
        if (parentButton == null)
        {
            Debug.LogError("This script must be attached to a Button object.");
            Destroy(this);
            return;
        }

        // Store the original navigation settings
        originalNavigation = parentButton.navigation;
    }

    void OnEnable()
    {
        // This is triggered when the parent object or this object is activated
        if (targetGameObject != null && targetGameObject.activeSelf)
        {
            // Disable navigation if the target game object is active
            Navigation noNavigation = new Navigation { mode = Navigation.Mode.None };
            parentButton.navigation = noNavigation;
            navigationModified = true;
        } else
        {
            parentButton.navigation = originalNavigation;
            navigationModified = false;
            Destroy(this);
        }
    }

    void OnDisable()
    {
        // Restore navigation when the parent or this object is deactivated
        if (navigationModified)
        {
            parentButton.navigation = originalNavigation;
            navigationModified = false;
            Destroy(this);
        }
    }
}
