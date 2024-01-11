using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    //static is necessary to put variables in a constructor
    static public int[] valueList = { 69, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    //static public string Name;
    public Character charData = new Character("Bob", valueList, "Cube", "Is a cube");
    //public TextMeshPro text;
    public TMP_Text text;

    private void Start()
    {
        text.text = "Health: " + charData.stats["Health"];
    }
}
