using UnityEngine;

public class CharParametrs : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Скорость персонажа")]
    [SerializeField] private float charSpeed;

    [Tooltip("Скорость персонажа")]
    [SerializeField] private int charHp;

    [Tooltip("Скорость персонажа")]
    [SerializeField] private int charMane;
#pragma warning restore 0649

    public static CharParametrs instance;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public float CharSpeed { get { return charSpeed; } set { charSpeed = value; } }

    public int CharHp { get; set; }

    public int CharMane { get; set; }
}
