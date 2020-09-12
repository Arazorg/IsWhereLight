using UnityEngine;

public class LobbyEnvironment : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Река")]
    [SerializeField] private GameObject riverObject;

    [Tooltip("Портал")]
    [SerializeField] private GameObject portalObject;
#pragma warning restore 0649

    private AudioManager audioManager;

    public Transform Character
    {
        get { return character; }
        set { character = value; }
    }
    private Transform character;

    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    void Update()
    {
       
    }
}
