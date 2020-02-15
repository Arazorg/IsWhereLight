using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public CharInfo charInfo;
    public Image imgManaBar;
    public Text textMane;

    private int Min, Max;
    public float currentPercent;
    public int currentValue;

    public void SetMana(int mana, int _Max, int _Min)
    {
        Max = _Max;
        Min = _Min;
        if (mana != currentValue)
        {
            if (Max - Min == 0)
            {
                currentValue = 0;
                currentPercent = 0;
            }
            else
            {
                currentValue = mana;
                currentPercent = (float)currentValue / (float)(Max - Min);
            }
        }
        textMane.text = string.Format("{0} / {1}", currentValue, (Max - Min));
        imgManaBar.fillAmount = currentPercent;
    }

    public void Spend(int spendAmount)
    {
        if (currentValue - spendAmount < 0)
            SetMana(0, Max, Min);
        else
            SetMana(currentValue - spendAmount, Max, Min);
    }

    public void Fill(int fillAmount)
    {
        if (currentValue + fillAmount > Max)
            SetMana(Max, Max, Min);
        else
            SetMana(currentValue + fillAmount, Max, Min);
    }

    void Start()
    {
        charInfo = GameObject.Find("Character(Clone)").GetComponent<CharInfo>();
        //Debug.Log("ManaBAr " + charInfo.mana);
        //SetMana(charInfo.mana, charInfo.maxMana, 0);
    }
}
