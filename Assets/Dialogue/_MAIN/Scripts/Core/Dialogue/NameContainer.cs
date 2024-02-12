using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace DIALOGUE
{
    [System.Serializable]
    public class NameContainer // ep 3 pt 3 23 min in to edit name container
    {
        [SerializeField] private GameObject root;
        [SerializeField] private TextMeshProUGUI nameText;

        public void Show(string nameToShow)
        {
            root.SetActive(true);

            if (nameToShow != string.Empty)
            {
                nameText.text = nameToShow;
            }
        }

        public void Hide()
        {
            root.SetActive(false);
        }
    }
}
