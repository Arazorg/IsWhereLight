using UnityEngine;

[CreateAssetMenu(menuName = "Bullets/Standart Bullets", fileName = "New Bullet")]
public class BulletData : ScriptableObject
{
    public enum BulletType
    {
        Gun,
        Sword,
        Bow,
        Laser
    }

#pragma warning disable 0649
    [Tooltip("Основной спрайт")]
    [SerializeField] private Sprite mainSprite;
    public Sprite MainSprite
    {
        get { return mainSprite; }
        protected set { }
    }

    [Tooltip("Аниматор")]
    [SerializeField] private RuntimeAnimatorController mainAnimator;
    public RuntimeAnimatorController MainAnimator
    {
        get { return mainAnimator; }
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
