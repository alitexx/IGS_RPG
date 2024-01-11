using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    static public int[] valueList = { 69, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    static public string Name;
    public Character charData = new Character(Name, valueList, "Cube", "Is a cube");
    //public TextMeshPro text;
    public TMP_Text text;

    private void Start()
    {
        text.text = "Health: " + charData.stats["Health"];
    }
}
