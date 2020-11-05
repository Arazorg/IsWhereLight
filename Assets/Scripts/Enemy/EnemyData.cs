using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Standart Enemy", fileName = "New Enemy")]
public class EnemyData : ScriptableObject
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
        set { mainAnimator = value; }
    }

    [Tooltip("Позиция в слоях спрайтов")]
    [SerializeField] private int layerOrder;
    public int LayerOrder
    {
        get { return layerOrder; }
        set { layerOrder = value; }
    }

    [Tooltip("Размер коллайдера действия")]
    [SerializeField] private Vector2 actionColliderSize;
    public Vector2 ActionColliderSize
    {
        get { return actionColliderSize; }
        set { actionColliderSize = value; }
    }

    [Tooltip("Смещение коллайдера действия")]
    [SerializeField] private Vector2 actionColliderOffset;
    public Vector2 ActionColliderOffset
    {
        get { return actionColliderOffset; }
        set { actionColliderOffset = value; }
    }

    [Tooltip("Размер коллайдера")]
    [SerializeField] private Vector2 colliderSize;
    public Vector2 СolliderSize
    {
        get { return colliderSize; }
        set { colliderSize = value; }
    }

    [Tooltip("Смещение коллайдера")]
    [SerializeField] private Vector2 colliderOffset;
    public Vector2 ColliderOffset
    {
        get { return colliderOffset; }
        set { colliderOffset = value; }
    }


    [Tooltip("Скорость врага")]
    [SerializeField] private int speed;
    public int Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    [Tooltip("Здоровье врага")]
    [SerializeField] private int health;
    public int Health
    {
        get { return health; }
        set { health = value; }
    }

    [Tooltip("Тип атаки врага")]
    [SerializeField] private AttackType typeOfAttack;
    public AttackType TypeOfAttack
    {
        get { return typeOfAttack; }
        set { typeOfAttack = value; }
    }

    [Tooltip("Атака врага")]
    [SerializeField] private int damage;
    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    [Tooltip("Радиус атаки врага")]
    [SerializeField] private float attackRange;
    public float AttackRange
    {
        get { return attackRange; }
        set { attackRange = value; }
    }

    [Tooltip("Угол атаки врага")]
    [SerializeField] private float attackAngle;
    public float AttackAngle
    {
        get { return attackAngle; }
        set { attackAngle = value; }
    }

    [Tooltip("Пуля врага")]
    [SerializeField] private BulletData dataOfBullet;
    public BulletData DataOfBullet
    {
        get { return dataOfBullet; }
        set { dataOfBullet = value; }
    }

    [Tooltip("Цель атаки врага")]
    [SerializeField] private string target;
    public string Target
    {
        get { return target; }
        set { target = value; }
    }

    [Tooltip("Имя врага")]
    [SerializeField] private string enemyName;
    public string EnemyName
    {
        get { return enemyName; }
        set { enemyName = value; }
    }

    [Tooltip("Частота атаки врага")]
    [SerializeField] private float fireRate;
    public float FireRate
    {
        get { return fireRate; }
        set { fireRate = value; }
    }
#pragma warning restore 0649
}