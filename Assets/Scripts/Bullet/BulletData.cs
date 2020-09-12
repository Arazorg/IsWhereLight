using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bullets/Standart Bullets", fileName = "New Bullet")]
public class BulletData : ScriptableObject
{
    public enum BulletType
    {
        Gun,
        Sword,
        Bow,
        Laser,
        Grenade
    }

#pragma warning disable 0649
    [Tooltip("Основной спрайт")]
    [SerializeField] private Sprite mainSprite;
    public Sprite MainSprite
    {
        get { return mainSprite; }
        protected set { }
    }

    [Tooltip("Размер коллайдера")]
    [SerializeField] private Vector2 colliderSize;
    public Vector2 ColliderSize
    {
        get { return colliderSize; }
        protected set { }
    }

    [Tooltip("Смещение коллайдера")]
    [SerializeField] private Vector2 colliderOffset;
    public Vector2 ColliderOffset
    {
        get { return colliderOffset; }
        protected set { }
    }

    [Tooltip("Аниматор")]
    [SerializeField] private List<RuntimeAnimatorController> animators;
    public List<RuntimeAnimatorController> Animators
    {
        get { return animators; }
        protected set { }
    }

    [Tooltip("Тип пули")]
    [SerializeField] private BulletType typeOfBullet;
    public BulletType TypeOfBullet
    {
        get { return typeOfBullet; }
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

    [Tooltip("Время удаления пули")]
    [SerializeField] private float deleteTime;
    public float DeleteTime
    {
        get { return deleteTime; }
        protected set { }
    }
#pragma warning restore 0649
}
