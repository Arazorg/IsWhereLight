using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Allies/Standart Ally", fileName = "New Ally")]
public class AllyData : ScriptableObject
{
    public enum AttackType
    {
        Melee,
        Distant
    }

#pragma warning disable 0649
    [Tooltip("Основной аниматор")]
    [SerializeField] private RuntimeAnimatorController mainAnimator;
    public RuntimeAnimatorController MainAnimator
    {
        get { return mainAnimator; }
        protected set { }
    }

    [Tooltip("Позиция в слоях спрайтов")]
    [SerializeField] private int layerOrder;
    public int LayerOrder
    {
        get { return layerOrder; }
        protected set { }
    }

    [Tooltip("Размер коллайдера действия")]
    [SerializeField] private Vector2 actionColliderSize;
    public Vector2 ActionColliderSize
    {
        get { return actionColliderSize; }
        protected set { }
    }

    [Tooltip("Смещение коллайдера действия")]
    [SerializeField] private Vector2 actionColliderOffset;
    public Vector2 ActionColliderOffset
    {
        get { return actionColliderOffset; }
        protected set { }
    }

    [Tooltip("Размер коллайдера")]
    [SerializeField] private Vector2 colliderSize;
    public Vector2 СolliderSize
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


    [Tooltip("Скорость врага")]
    [SerializeField] private int speed;
    public int Speed
    {
        get { return speed; }
        protected set { }
    }

    [Tooltip("Здоровье врага")]
    [SerializeField] private int health;
    public int Health
    {
        get { return health; }
    }

    [Tooltip("Цель атаки врага")]
    [SerializeField] private string target;
    public string Target
    {
        set
        {
            target = value;
        }
        get { return target; }
    }

    [Tooltip("Имя союзника")]
    [SerializeField] private string allyName;
    public string AllyName
    {
        get { return allyName; }
    }

    [Tooltip("Название оружия союзника")]
    [SerializeField] private string allyWeaponName;
    public string AllyWeaponName
    {
        get { return allyWeaponName; }
    }
#pragma warning restore 0649
}
