using UnityEngine;

public class CharInfo : MonoBehaviour
{
    //Gameobjects
    private ManaBar manaBar;
    //Values
    public int health;
    public int mana;
    public int maxHealth;
    public int maxMana;
    public string startGun;

    void Start()
    {
        if (MenuButtons.firstPlay == true)
        {
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
