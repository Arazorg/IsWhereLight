using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Standart Enemy", fileName = "New Enemy")]
public class EnemyData : ScriptableObject
{
    [Tooltip("Основной аниматор")]
    [SerializeField] private RuntimeAnimatorController mainAnimator;
    public RuntimeAnimatorController MainAnimator
    {
        get { return MainAnimator; }
        protected set { } 
    }

    [Tooltip("Скорость врага")]
    [SerializeField] private float speed;
    public float Speed
    {
        get { return speed; }
        protected set { }
    }

    [Tooltip("Здоровье врага")]
    [SerializeField] private float health;
    public float Health
    {
        get { return health; }
        protected set { }
    }

    [Tooltip("Атака врага")]
    [SerializeField] private float attack;
    public float Attack
    {
        get { return attack; }
        protected set { }
    }
}
