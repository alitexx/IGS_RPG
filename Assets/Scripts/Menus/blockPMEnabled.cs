using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class blockPMEnabled : MonoBehaviour
{
    public Button kisaBlock, nicolBlock, sophieBlock;
    public Button alanButton, kisaButton, nicolButton, sophieButton;
    public Button resumeButton;
    private int aliveCounter = 0;

    private void OnEnable()
    {
        //// Get the navigation of the target button
        //Navigation navigation = targetButton.navigation;

        //// Set it to explicit mode to allow customization
        //navigation.mode = Navigation.Mode.Explicit;

        //// Assign the buttons for each direction
        ////navigation.selectOnUp = upButton;
        ////navigation.selectOnDown = downButton;
        //navigation.selectOnLeft = leftButton;
        //navigation.selectOnRight = rightButton;

        //// Apply the modified navigation back to the button
        //targetButton.navigation = navigation;

        //Determine who is alive and who isn't
        if (kisaBlock.gameObject.activeInHierarchy)
        {
            aliveCounter++;
        }
        if (nicolBlock.gameObject.activeInHierarchy)
        {
            aliveCounter = aliveCounter + 2;
        }
        if (sophieBlock.gameObject.activeInHierarchy)
        {
            aliveCounter = aliveCounter + 4;
        }
        //1 = kisa only dead
        //2 = nicol only dead
        //3 = kisa and nicol dead
        //4 = sophie only dead
        //5 = sophie and kisa dead
        //6 = sophie and nicol dead
        //7 = everyone dead

        //Assume they are all living: set the navigation

        updateButton(alanButton, null, resumeButton, null, kisaButton);
        updateButton(kisaButton, null, resumeButton, alanButton, nicolButton);
        updateButton(nicolButton, null, resumeButton, kisaButton, sophieButton);
        updateButton(sophieButton, null, resumeButton, nicolButton, null);

        //Debug.Log(aliveCounter);
        switch (aliveCounter)
        {
            case 1://1 = kisa only dead

                updateButton(kisaBlock, null, resumeButton, alanButton, nicolButton);
                updateButton(alanButton, null, resumeButton, null, kisaBlock);
                updateButton(nicolButton, null, resumeButton, kisaBlock, sophieButton);
                break;
            case 2://2 = nicol only dead

                updateButton(nicolBlock, null, resumeButton, kisaButton, sophieButton);
                updateButton(kisaButton, null, resumeButton, alanButton, nicolBlock);
                updateButton(sophieButton, null, resumeButton, nicolBlock, null);
                break;
            case 3://3 = kisa and nicol dead
                updateButton(alanButton, null, resumeButton, null, kisaBlock);
                updateButton(nicolBlock, null, resumeButton, kisaBlock, sophieButton);
                updateButton(kisaBlock, null, resumeButton, alanButton, nicolBlock);
                updateButton(sophieButton, null, resumeButton, nicolBlock, null);
                break;
            case 4://4 = sophie only dead
                updateButton(nicolButton, null, resumeButton, kisaButton, sophieBlock);
                updateButton(sophieBlock, null, resumeButton, nicolButton, null);
                break;
            case 5://5 = sophie and kisa dead
                updateButton(alanButton, null, resumeButton, null, kisaBlock);
                updateButton(nicolButton, null, resumeButton, kisaBlock, sophieBlock);
                updateButton(kisaBlock, null, resumeButton, alanButton, nicolButton);
                updateButton(sophieBlock, null, resumeButton, nicolButton, null);
                break;
            case 6://6 = sophie and nicol dead
                updateButton(nicolBlock, null, resumeButton, kisaButton, sophieBlock);
                updateButton(kisaButton, null, resumeButton, alanButton, nicolBlock);
                updateButton(sophieBlock, null, resumeButton, nicolBlock, null);
                break;
            case 7://7 = everyone dead
                updateButton(alanButton, null, resumeButton, null, kisaBlock);
                updateButton(nicolBlock, null, resumeButton, kisaBlock, sophieBlock);
                updateButton(kisaBlock, null, resumeButton, alanButton, nicolBlock);
                updateButton(sophieBlock, null, resumeButton, nicolBlock, null);
                break;
        }
        aliveCounter = 0;
    }

    private void updateButton(Button buttonUpdated, Button up, Button down, Button left, Button right)
    {
        if (buttonUpdated != null)
        {
            // Get the navigation of the target button
            Navigation navigation = buttonUpdated.navigation;
            // Set it to explicit mode to allow customization
            navigation.mode = Navigation.Mode.Explicit;
            if (up != null)
            {
                navigation.selectOnUp = up;
            }
            if (right != null)
            {
                navigation.selectOnRight = right;
            }
            if (left != null)
            {
                navigation.selectOnLeft = left;
            }
            if (down != null)
            {
                navigation.selectOnDown = down;
            }
            buttonUpdated.navigation = navigation;
        }
    }
}
