using System.Collections;
using UnityEngine;

public class CoroutineFunctions : MonoBehaviour
{
    public static CoroutineFunctions instance;

    void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
