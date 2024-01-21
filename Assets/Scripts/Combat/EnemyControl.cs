using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    //current not using this script


    static public int[] valueList = {
        /*Strength*/ 2,
        /*Magic Attack*/ 1,
        /*Defense*/ 4, 
        /*Speed*/ 3, 
        /*Health*/ 4, 
        /*MaxHealth*/ 5,
        /*Mana*/ 6,
        /*MaxMana*/ 7};
    static public string Name;
    public EnemyData enemyData = new EnemyData(Name, valueList, "Cube");
    //public TextMeshPro text;
    /*
    public TMP_Text text;

    public Character targetCharacter;

    private void Start()
    {
        text.text = "Health: " + enemyData.stats["Health"];
        
    }

    //Gets hit by a physical attack
    public void PhysicalHurt(int strength)
    {
        //subtract health and update display
        if (strength < enemyData.stats["Defense"] - 1)
        {

        }
        else
        {
            enemyData.stats["Health"] = enemyData.stats["Health"] - ((1 + strength) - enemyData.stats["Defense"]);
        }
        text.text = "Health: " + enemyData.stats["Health"];
    }

    //Gets hit by a magical attack
    public void MagicHurt(int magicAttack, string magicType)
    {
        //Vunerability to Fire
        if (magicType == "Fire")
        {
            enemyData.stats["Health"] = enemyData.stats["Health"] - ((1 + magicAttack) * 2);
        }
        else
        {
            enemyData.stats["Health"] = enemyData.stats["Health"] - (1 + magicAttack);
        }

        text.text = "Health: " + enemyData.stats["Health"];
    }
    */


}
