using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Standart Enemy", fileName = "New Enemy")]
public class EnemyData : ScriptableObject
{
    [Tooltip("Main animator")]
    [SerializeField] private RuntimeAnimatorController mainAnimator;
    public RuntimeAnimatorController MainAnimator
    {
        get { return MainAnimator; }
        protected set { } 
    }

    [Tooltip("Speed of enemy")]
    [SerializeField] private float speed;
    public float Speed
    {
        get { return speed; }
        protected set { }
    }

    [Tooltip("Attack of enemy")]
    [SerializeField] private float attack;
    public float Attack
    {
        get { return attack; }
        protected set { }
    }
}
