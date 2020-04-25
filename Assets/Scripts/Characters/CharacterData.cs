using UnityEngine;

[CreateAssetMenu(menuName = "Characters/Standart Character", fileName = "New Character")]
public class CharacterData : ScriptableObject
{

    [Tooltip("Аниматоры скинов")]
    [SerializeField] private RuntimeAnimatorController[] animations;
    public RuntimeAnimatorController[] Animations
    {
        get { return animations; }
        protected set { }
    }

    [Tooltip("Класс персонажа")]
    [SerializeField] private string characterClass;
    public string CharacterClass
    {
        get { return characterClass; }
        protected set { }
    }

    [Tooltip("Максимальное здоровье персонажа")]
    [SerializeField] private int maxHealth;
    public int MaxHealth
    {
        get { return maxHealth; }
        protected set { }
    }

    [Tooltip("Максимальная мана персонажа")]
    [SerializeField] private int maxMane;
    public int MaxMane
    {
        get { return maxMane; }
        protected set { }
    }

    [Tooltip("Стартовое оружие")]
    [SerializeField] private string startWeapon;
    public string StartWeapon
    {
        get { return startWeapon; }
        protected set { }
    }

    [Tooltip("Стартовое оружие")]
    [SerializeField] private int price;
    public int Price
    {
        get { return price; }
        protected set { }
    }
}
