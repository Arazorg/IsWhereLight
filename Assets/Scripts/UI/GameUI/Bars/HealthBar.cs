using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image imgHealthBar;
    public Text textHealth;

    private int Min, Max;
    public float currentPercent;
    public int currentValue;

    public void SetMaxMin(int health, int _Max, int _Min)
    {
        currentValue = health;
        Min = _Min;
        Max = _Max;
        SetHealth(health);
    }


    public void SetHealth(int currentHealth)
    {
        currentPercent = (float)currentHealth / (float)(Max - Min);
        textHealth.text = string.Format("{0} / {1}", currentHealth, (Max - Min));
        imgHealthBar.fillAmount = currentPercent;
    }
}
