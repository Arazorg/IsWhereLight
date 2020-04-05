using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bullets/Standart Bullets", fileName = "New Bullet")]
public class BulletData : ScriptableObject
{
    [Tooltip("Основной спрайт")]
    [SerializeField] private Sprite mainSprite;
    public Sprite MainSprite
    {
        get { return mainSprite; }
        protected set { }
    }

    [Tooltip("Урон пули")]
    [SerializeField] private int damage;
    public int Damage
    {
        get { return damage; }
        protected set { }
    }

    [Tooltip("Скорость пули")]
    [SerializeField] private float speed;
    public float Speed
    {
        get { return speed; }
        protected set { }
    }

    [Tooltip("Разброс пули")]
    [SerializeField] private float scatter;
    public float Scatter
    {
        get { return scatter; }
        protected set { }
    }
}
