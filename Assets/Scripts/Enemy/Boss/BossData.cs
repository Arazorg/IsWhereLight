using UnityEngine;

[CreateAssetMenu(menuName = "Bosses/Standart Boss", fileName = "New Boss")]
public class BossData : ScriptableObject
{
    public enum BossType
    {
        Melee,
        Distant,
        Static
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

    [Tooltip("Здоровье босса")]
    [SerializeField] private int health;
    public int Health
    {
        get { return health; }
    }

    [Tooltip("Тип атаки босса")]
    [SerializeField] private BossType typeOfBoss;
    public BossType TypeOfBoss
    {
        get { return typeOfBoss; }
        protected set { }
    }

    [Tooltip("Урон босса")]
    [SerializeField] private int damage;
    public int Damage
    {
        get { return damage; }
        protected set { }
    }

    [Tooltip("Радиус атаки босса")]
    [SerializeField] private float attackRange;
    public float AttackRange
    {
        get { return attackRange; }
        protected set { }
    }

    [Tooltip("Угол атаки босса")]
    [SerializeField] private float attackAngle;
    public float AttackAngle
    {
        get { return attackAngle; }
        protected set { }
    }

    [Tooltip("Пуля босса")]
    [SerializeField] private BulletData dataOfBullet;
    public BulletData DataOfBullet
    {
        get { return dataOfBullet; }
        protected set { }
    }

    [Tooltip("Цель атаки босса")]
    [SerializeField] private string target;
    public string Target
    {
        set
        {
            target = value;
        }
        get { return target; }
    }

    [Tooltip("Имя босса")]
    [SerializeField] private string enemyName;
    public string EnemyName
    {
        get { return enemyName; }
    }

    [Tooltip("Частота атаки босса")]
    [SerializeField] private float fireRate;
    public float FireRate
    {
        get { return fireRate; }
        protected set { }
    }
#pragma warning restore 0649
}
