using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class deleteSaveButtons : MonoBehaviour
{
    [SerializeField] private GameObject doNotSave, deleteSaveEntry;
    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(doNotSave);
    }
    private void OnDisable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(deleteSaveEntry);
    }
}
