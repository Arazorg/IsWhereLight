using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Floors/Standart Floor", fileName = "New Floor")]
public class FloorData : ScriptableObject
{
#pragma warning disable 0649
    [Tooltip("Основной спрайт")]
    [SerializeField] private Sprite[] mainSprite;
    public Sprite[] MainSprite
    {
        get { return mainSprite; }
        protected set { }
    }

    [Tooltip("Название поверхности")]
    [SerializeField] private string floorName;
    public string FloorName
    {
        get { return floorName; }
        protected set { }
    }

    [Tooltip("Материал поверхности")]
    [SerializeField] private PhysicsMaterial2D material;
    public PhysicsMaterial2D Material
    {
        get { return material; }
        protected set { }

    }
    [Tooltip("Модификация скорости игрока")]
    [SerializeField] private float speedModification;
    public float SpeedModification
    {
        get { return speedModification; }
        protected set { }
    }

    [Tooltip("Сила скольжения поверхности")]
    [SerializeField] private float glidePower;
    public float GlidePower
    {
        get { return glidePower; }
        protected set { }
    }
#pragma warning restore 0649
}
