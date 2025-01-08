using UnityEngine;
using UnityEngine.UI;

public class sophieMagicButton : MonoBehaviour
{
    public GameObject otherButton; // Assign the other button in the Inspector

    [SerializeField] private Button thisButton;
    [SerializeField] private Button otherUIButton;
    [SerializeField] private GameObject kisaUIButton;
    [SerializeField] private GameObject nicolUIButton;

    void Start()
    {
        // Get the Button components
        if (thisButton == null)
        {
            thisButton = GetComponent<Button>();
        }
        if (otherButton != null)
        {
            otherUIButton = otherButton.GetComponent<Button>();
        }
    }

    private void OnEnable()
    {
        // Check if both buttons are active
        if (gameObject.activeSelf && otherButton.activeSelf && kisaUIButton.activeInHierarchy == false && nicolUIButton.activeInHierarchy == false)
        {
            UpdateButtonNavigation();
        }
    }

    void UpdateButtonNavigation()
    {
        // Update navigation for this button
        Navigation thisNav = thisButton.navigation;
        thisNav.mode = Navigation.Mode.Explicit; // Set explicit navigation mode
        thisNav.selectOnLeft = otherUIButton; // Go to other button when pressing left
        thisButton.navigation = thisNav;

        // Update navigation for the other button
        Navigation otherNav = otherUIButton.navigation;
        otherNav.mode = Navigation.Mode.Explicit; // Set explicit navigation mode
        otherNav.selectOnRight = thisButton; // Go to this button when pressing right
        otherUIButton.navigation = otherNav;
    }
}
