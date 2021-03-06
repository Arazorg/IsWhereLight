﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManaBar : MonoBehaviour
{
    public Image imgManeBar;
    public TextMeshProUGUI textMane;

    private int Min, Max;
    public float currentPercent;
    public int currentValue;

    public void SetMaxMin(int mane, int _Max, int _Min)
    {
        currentValue = mane;
        Min = _Min;
        Max = _Max;
        SetMane(mane);
    }


    public void SetMane(int currentMane)
    {
        currentPercent = (float)currentMane / (float)(Max - Min);
        textMane.text = string.Format("{0} / {1}", currentMane, (Max - Min));
        imgManeBar.fillAmount = currentPercent;
    }
}
