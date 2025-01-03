using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class ButtonHoverEffect : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public GameObject otherUIWindow;  // Assign the other UI window here
    private bool isSelected = false;
    private Coroutine hoverCoroutine;
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private Image sprite;
    public string skillNameText;
    [SerializeField] private float speedToAppear = 3.0f;
    [SerializeField] private updateSPOnScreen updateSP;
    [SerializeField] private PlayerController playerController;

    // Trigger when the button is selected (via WASD/keyboard navigation)
    public void OnSelect(BaseEventData eventData)
    {
        //if (skillName)
        //{
        //    //original name, no space
        //    skillNameText = skillName.text;
        //    skillName.text = " " + skillNameText;
        //} else
        if (sprite)
        {
            sprite.color = new Color(255, 255, 255, 255);
        }
        if (!isSelected)
        {
            isSelected = true;
            hoverCoroutine = StartCoroutine(ShowWindowAfterDelay(speedToAppear));
        }

        Debug.Log(skillNameText);
        //If this skill is Tenacity, updateSP based on how many people are dead
        if(skillNameText.ToLower() == "tenacity")
        {
            //Find how many people are dead and act accordingly
            switch (SumDigits(playerController.getDeadCharacters()))
            {
                case 0: // None dead
                    //Costs 3 
                    updateSP.setSliderGlow(3);
                    break;
                case 1:
                case 2: // 1 or 2 people dead
                    //Costs 2
                    updateSP.setSliderGlow(2);
                    break;
                case 3: // everyone (minus Alan) dead
                    //Costs 1
                    updateSP.setSliderGlow(1);
                    break;
            }
            //Don't run other code to set SP
            return;
        }

        switch (otherUIWindow.name)
        {
            case "Description1":
                updateSP.setSliderGlow(1);
                break;
            case "Description2":
                updateSP.setSliderGlow(1);
                break;
            case "Description3":
                updateSP.setSliderGlow(2);
                break;
            case "Description4":
                updateSP.setSliderGlow(3);
                break;
        }
    }

    // Trigger when the button is deselected (user moves away from the button)
    public void OnDeselect(BaseEventData eventData)
    {
        if (skillName)
        {
            //remove the space from the name
            skillName.text = skillNameText;
        }
        else if (sprite)
        {
            sprite.color = new Color(255, 255, 255, 0);
        }
        isSelected = false;
        if (hoverCoroutine != null)
        {
            StopCoroutine(hoverCoroutine);  // Stop the coroutine if the button is deselected
        }
        otherUIWindow.SetActive(false);  // Optionally hide the window again when deselected

        //Change the glowing hover


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

    // Function to calculate the sum of digits of a three-digit number
    public int SumDigits(int number)
    {
        // Ensure the number is positive
        number = Mathf.Abs(number);

        // Extract each digit and calculate the sum
        int hundreds = number / 100;        // Get the hundreds place
        int tens = (number / 10) % 10;     // Get the tens place
        int ones = number % 10;            // Get the ones place

        return hundreds + tens + ones;
    }
}
