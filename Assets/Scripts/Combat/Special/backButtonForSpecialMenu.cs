using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class backButtonForSpecialMenu : MonoBehaviour
{
    [SerializeField] private Selectable buttonToMoveTo; // The button to move to when "down" is pressed.
    [SerializeField] private Selectable oldButton; // The button to move to when "down" is pressed.
    [SerializeField] private Button backButton;        // The back button that needs navigation updates.

    private void OnEnable()
    {
        // Ensure the initial selection is the correct button.
        UpdateBackButtonNavigation(buttonToMoveTo);
    }

    private void OnDisable()
    {
        UpdateBackButtonNavigation(oldButton);
    }

    private void UpdateBackButtonNavigation(Selectable button)
    {
        // Get the current navigation settings of the back button.
        Navigation navigation = backButton.navigation;

        // Modify the `selectOnDown` property to point to the desired button.
        navigation.selectOnDown = button;

        // Apply the updated navigation settings back to the back button.
        backButton.navigation = navigation;
    }
}
