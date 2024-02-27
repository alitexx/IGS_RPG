using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int currentEXP = 0;
    public int level = 1;
    public int lvlUpThreshold = 10;

    static LevelManager Instance;

    public BattleController battleController;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);

        level = 1;
        lvlUpThreshold = 10;
        currentEXP = 0;
    }

    public void LevelUp()
    {
        level++;
        currentEXP -= lvlUpThreshold;
        for (int i = 0; i < battleController.partyMembers.Count; i++)
        {
            if (battleController.partyMembers[i].statSheet.name == "Tank Guy")
            {
                battleController.partyMembers[i].statSheet.stats["Defense"] += 2;
                Debug.Log("Tank has leveled up.");
            }
            else if (battleController.partyMembers[i].statSheet.name == "Mage Guy")
            {
                battleController.partyMembers[i].statSheet.stats["Magic Attack"] += 2;
                Debug.Log("Mage has leveled up.");
            }
            else if (battleController.partyMembers[i].statSheet.name == "Bard Guy")
            {
                battleController.partyMembers[i].statSheet.stats["MaxHealth"] += 2;
                battleController.partyMembers[i].statSheet.stats["Health"] += 2;
                Debug.Log("Bard has leveled up.");
            }
            else if (battleController.partyMembers[i].statSheet.name == "Monk Guy")
            {
                battleController.partyMembers[i].statSheet.stats["Strength"] += 2;
                Debug.Log("Monk has leveled up.");
            }
        }
    }
}
