using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharInfo : MonoBehaviour
{
    //Gameobjects
    private ManaBar manaBar;
    //Values
    public int level;
    public int health;
    public int mana;
    public int maxHealth = 7;
    public int maxMana = 100;
    public string startGun;

    public CharInfo()
    {
        
    }

    void Start()
    {
        if (MenuButtons.firstPlay == true)
        {
            level = 1;
            mana = maxMana;
            health = maxHealth;
            startGun = "0";
            MenuButtons.firstPlay = false;
        }
        else
        {
            LoadChar();
        }

        manaBar = GameObject.Find("Canvas").GetComponentInChildren<ManaBar>();
        manaBar.SetMana(mana, maxMana, 0);
    }

    public void SaveChar()
    {  
        SaveSystem.SaveChar(this);
    }

    public void LoadChar()
    {
        CharData data = SaveSystem.LoadChar();

        level = data.level;
        health = data.health;
        mana = data.mana;
        startGun = data.startGun;
    }

    public void SpendMana(int spendAmount)
    {
        if (mana - spendAmount < 0)
            mana = 0;
        else
            mana -= spendAmount;
    }

    public void FillMana(int fillAmount)
    {
        if (mana + fillAmount > maxMana)
            mana = maxMana;
        else
            mana += fillAmount;
    }

}
