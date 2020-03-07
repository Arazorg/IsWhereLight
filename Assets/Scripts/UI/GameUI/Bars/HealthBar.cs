using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image imgHealthBar;
    public Text textHealth;
    public int Min, Max;
    public float CurrentPercent { get; private set; }
    public int CurrentValue { get; private set; }

    public void SetHealth(int health, int _Max, int _Min)
    {
        Max = _Max;
        Min = _Min;
        if (health != CurrentValue)
        {
            if (Max - Min == 0)
            {
                CurrentValue = 0;
                CurrentPercent = 0;
            }
            else
            {
                CurrentValue = health;
                CurrentPercent = (float)CurrentValue / (float)(Max - Min);
            }
        }
        textHealth.text = string.Format("{0} / {1}", CurrentValue, (Max - Min));
        imgHealthBar.fillAmount = CurrentPercent;
    }

    public void Damage(int damageAmount)
    {
        if (CurrentValue - damageAmount < 0)
            SetHealth(0, Max, Min);
        else
            SetHealth(CurrentValue - damageAmount, Max, Min);
    }

    public void Heal(int healAmount)
    {
        if (CurrentValue + healAmount > Max)
            SetHealth(Max, Max, Min);
        else
            SetHealth(CurrentValue + healAmount, Max, Min);
    }
}
