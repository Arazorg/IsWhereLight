using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Puddles/Standart Puddle", fileName = "New Puddle")]
public class PuddleData : ScriptableObject
{
    public enum PuddleType
    {
        Poison,
        Frost,
        Fire
    }

#pragma warning disable 0649
    [Tooltip("Спрайт для партикла")]
    [SerializeField] private Sprite particleSprite;
    public Sprite ParticleSprite
    {
        get { return particleSprite; }
        protected set { }
    }

    [Tooltip("Цвет лужи")]
    [SerializeField] private Color puddleColor;
    public Color PuddleColor
    {
        get { return puddleColor; }
        protected set { }
    }

    [Tooltip("Тип лужи")]
    [SerializeField] private PuddleType typeOfPuddle;
    public PuddleType TypeOfPuddle
    {
        get { return typeOfPuddle; }
        protected set { }
    }
#pragma warning restore 0649
}
