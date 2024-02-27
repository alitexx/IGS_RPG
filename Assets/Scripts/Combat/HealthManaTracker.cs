using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManaTracker : MonoBehaviour
{
    static HealthManaTracker instance;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public int tankHealth;
    public int tankMana;
    public int mageHealth;
    public int mageMana;
    public int monkHealth;
    public int monkMana;
    public int bardHealth;
    public int bardMana;

    public void StoreStats(int tankH, int tankM, int mageH, int mageM, int monkH, int monkM, int bardH, int bardM)
    {
        tankHealth = tankH;
        tankMana = tankM;
        mageHealth = mageH;
        mageMana = mageM;
        monkHealth = monkM;
        monkMana = monkH;
        bardHealth = bardH;
        bardMana = bardM;
    }
}
