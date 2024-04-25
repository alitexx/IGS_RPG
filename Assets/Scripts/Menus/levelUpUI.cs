using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class levelUpUI : MonoBehaviour
{
    [SerializeField] private RectTransform[] locations;
    [SerializeField] private RectTransform lvlUpText;
    [SerializeField] private RectTransform lvlupboxes;
    [SerializeField] private RectTransform exitlvlUpBTN;
    [SerializeField] private GameObject[] boxesToDisplay;
    [SerializeField] private TextMeshProUGUI[] statBonusTXT;
    [SerializeField] private PlayerController pc;
    [SerializeField] private youWinMenu ywm;
    [SerializeField] private CanvasGroup darkbg;
    public BattleController battleController;

    public LevelManager levelManager;

    
    #region PreviousStats

    static public List<List<int>> previousStats = new List<List<int>>()
    {   //0: strength
        //1: magic attacl
        //2: defense
        //3: speed
        //4: health
        //5: max health
        //6: mana
        //7: max mana
        new List<int>() {10, 5, 5, 3, 18, 18, 4, 4},  // Alan
        new List<int>() {6, 6, 4, 4, 15, 15, 7, 7},    // Kisa
        new List<int>() {7, 8, 4, 5, 8, 8, 9, 9},    // Nicol
        new List<int>() {13, 7, 4, 6, 10, 10, 5, 5}  // Sophie
    };
    #endregion

    #region Newer Ststs

    static private List<int[]> currentStats = new List<int[]>()
    {   //0: strength
        //1: magic attacl
        //2: defense
        //3: speed
        //4: health
        //5: max health
        //6: mana
        //7: max mana
        new int[] {10, 5, 5, 3, 18, 18, 4, 4},  // Alan
        new int[] { 6, 6, 4, 2, 15, 15, 7, 7 },    // Kisa
        new int[] {7, 13, 4, 5, 8, 8, 9, 9},    // Nicol
        new int[] { 13, 7, 4, 6, 10, 10, 5, 5 }  // Sophie
    };

    #endregion


    private void OnEnable()
    {
        darkbg.DOFade(1, 0.75f);
        getCurrentStats();
        setStatsGained();
        boxesToDisplay[1].SetActive(pc.hasKisa);
        boxesToDisplay[2].SetActive(pc.hasNicol);
        boxesToDisplay[3].SetActive(pc.hasSophie);
        lvlUpText.DOMove(locations[0].position, 0.35f).OnComplete(() => {
            lvlupboxes.DOMove(locations[1].position, 0.75f).OnComplete(() => {
                exitlvlUpBTN.DOMove(locations[2].position, 1f);
            });
        });
    }

    void getCurrentStats()
    {
        currentStats[0] = levelManager.SetTankStats();
        currentStats[1] = levelManager.SetBardStats();
        currentStats[2] = levelManager.SetMageStats();
        currentStats[3] = levelManager.SetMonkStats();
    }

    public void setStatsGained()
    {
        for (int i = 0; i < 4; i++)
        {
            if ((!pc.hasKisa && i == 1) || (!pc.hasNicol && i == 2) || (!pc.hasSophie && i == 3))
            {
                continue;
            }
            /*
            statBonusTXT[i].text = 
                    "Strength: " + battleController.partyMembers[i].statSheet.stats["Strength"] + "(<color=#3B7D4F>+" + (battleController.partyMembers[i].statSheet.stats["Strength"]- previousStats[i][0]) + "</color>)\r\n" +
                    "Mag. ATK: " + battleController.partyMembers[i].statSheet.stats["Magic Attack"] + "(<color=#3B7D4F>+" + (battleController.partyMembers[i].statSheet.stats["Magic Attack"] - previousStats[i][1]) + "</color>\r\n" +
                    "Defense: " + battleController.partyMembers[i].statSheet.stats["Defense"] + "(<color=#3B7D4F>+" + (battleController.partyMembers[i].statSheet.stats["Defense"] - previousStats[i][2]) + "</color>)\r\n" +
                    "Speed: " + battleController.partyMembers[i].statSheet.stats["Speed"] + "(<color=#3B7D4F>+" + (battleController.partyMembers[i].statSheet.stats["Speed"] - previousStats[i][3]) + "</color>)\r\n" +
                    "Max Health: " + battleController.partyMembers[i].statSheet.stats["MaxHealth"] + "(<color=#3B7D4F>+" + (battleController.partyMembers[i].statSheet.stats["MaxHealth"] - previousStats[i][5]) + "</color>)\r\n" +
                    "Max Mana: " + battleController.partyMembers[i].statSheet.stats["MaxMana"] + "(<color=#3B7D4F>+" + (battleController.partyMembers[i].statSheet.stats["MaxMana"] - previousStats[i][7]) + "</color>)";

            previousStats[i][0] = battleController.partyMembers[i].statSheet.stats["Strength"];
            previousStats[i][1] = battleController.partyMembers[i].statSheet.stats["Magic Attack"];
            previousStats[i][2] = battleController.partyMembers[i].statSheet.stats["Defense"];
            previousStats[i][3] = battleController.partyMembers[i].statSheet.stats["Speed"];
            previousStats[i][4] = battleController.partyMembers[i].statSheet.stats["Health"];
            previousStats[i][5] = battleController.partyMembers[i].statSheet.stats["MaxHealth"];
            previousStats[i][6] = battleController.partyMembers[i].statSheet.stats["Mana"];
            previousStats[i][7] = battleController.partyMembers[i].statSheet.stats["MaxMana"];
            */

            statBonusTXT[i].text =
                    "Strength: " + currentStats[i][0] + " (<color=#3B7D4F>+" + (currentStats[i][0] - previousStats[i][0]) + "</color>)\r\n" +
                    "Mag. ATK: " + currentStats[i][1] + " (<color=#3B7D4F>+" + (currentStats[i][1] - previousStats[i][1]) + "</color>)\r\n" +
                    "Defense: " + currentStats[i][2] + " (<color=#3B7D4F>+" + (currentStats[i][2] - previousStats[i][2]) + "</color>)\r\n" +
                    "Speed: " + currentStats[i][3] + " (<color=#3B7D4F>+" + (currentStats[i][3] - previousStats[i][3]) + "</color>)\r\n" +
                    "Max Health: " + currentStats[i][5] + " (<color=#3B7D4F>+" + (currentStats[i][5] - previousStats[i][5]) + "</color>)\r\n" +
                    "Max Mana: " + currentStats[i][7] + " (<color=#3B7D4F>+" + (currentStats[i][7] - previousStats[i][7]) + "</color>)";

            previousStats[i][0] = currentStats[i][0];
            previousStats[i][1] = currentStats[i][1];
            previousStats[i][2] = currentStats[i][2];
            previousStats[i][3] = currentStats[i][3];
            previousStats[i][4] = currentStats[i][4];
            previousStats[i][5] = currentStats[i][5];
            previousStats[i][6] = currentStats[i][6];
            previousStats[i][7] = currentStats[i][7];
        }
    }

    public void exitLevelUpUI()
    {
        darkbg.DOFade(0, 0.75f);
        exitlvlUpBTN.DOMove(locations[4].position, 0.5f);
        lvlupboxes.DOMove(locations[4].position, 0.75f);
        lvlUpText.DOMove(locations[3].position, 1f).OnComplete(() => {
            ywm.ContinueLevelGaining();
            this.gameObject.SetActive(false);
        });
    }
}
