using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    //static is necessary to put variables in a constructor
    static public int[] valueList = {
        /*Strength*/ 2,
        /*Magic Attack*/ 1,
        /*Defense*/ 2, 
        /*Speed*/ 3, 
        /*Health*/ 4, 
        /*MaxHealth*/ 5,
        /*Mana*/ 6,
        /*MaxMana*/ 7,
        /*EXP*/ 0,
        /*LvlUpThreshold*/ 10 };
    public Character charData = new Character("Bob", valueList, "Cube", "Is a cube");
    //public TextMeshPro text;
    public TMP_Text text;

    public EnemyControl targetEnemy;

    private void Start()
    {
        text.text = "Health: " + charData.stats["Health"];
    }

    //A physical attack that uses the strength stat
    public void PhysicalAttack()
    {
        targetEnemy.PhysicalHurt(charData.stats["Strength"]);
    }

    //A magical attack that uses the magic attack stat and is the element of fire
    public void MagicAttack()
    {
        targetEnemy.MagicHurt(charData.stats["Magic Attack"], charData.magicType);
    }

    public void Hit()
    {

    }
}
