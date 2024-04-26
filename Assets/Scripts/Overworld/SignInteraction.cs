using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SignInteraction : MonoBehaviour
{
    public GameObject SignUI;
    public GameObject Target;

    //this isn't needed for all signs
    [SerializeField] private TextMeshProUGUI optionalText;

    float maxDistance = 5f;
    float DistanceBetweenObjects;

    void Update()
    {
        SignUI.SetActive(false);

        DistanceBetweenObjects = Vector3.Distance(transform.position, Target.transform.position);

        if (DistanceBetweenObjects <= maxDistance)
        {
            SignUI.SetActive(true);
            //check if it exists first, then act accordingly
            if (optionalText)
            {
                switch (optionalText.name)
                {
                    case "room2Sign":
                        optionalText.text = "Press " + audioStatics.interractButton.ToUpper() + " to interact with levers, they open locked doors.";
                        break;
                }
            }
        }
           
        if(DistanceBetweenObjects > maxDistance)
        {
            SignUI.SetActive(false);
        }
    }
}
