using UnityEngine;
using UnityEngine.UI;

public class triggerBackButtonWithController : MonoBehaviour
{
    [SerializeField] private Button button;

    private void Update()
    {
        // Check if the "B" button is pressed and the GameObject is active
        // Checks if game object is active just in case; it kinda goes without saying but it's nice to have a failsafe
        if (Input.GetKeyDown(KeyCode.JoystickButton1) && gameObject.activeInHierarchy)
        {
            button.onClick.Invoke(); // Simulate a button click
        }
    }
}

