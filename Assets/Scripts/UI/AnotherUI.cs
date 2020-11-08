using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnotherUI : MonoBehaviour
{
    public static AnotherUI instance;


    private GameObject internetNotReachablePanel = null;
    private float timeToInternetPanel = float.MaxValue;

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


    void Update()
    {
        if (Time.realtimeSinceStartup > timeToInternetPanel)
        {
            internetNotReachablePanel.GetComponent<MovementUI>().MoveToStart();
            timeToInternetPanel = float.MaxValue;
        }
    }

    public void ShowInternetNotReachablePanel()
    {
        var internetPanelTime = 1.75f;
        if(internetNotReachablePanel == null)
            internetNotReachablePanel = GameObject.Find("Canvas").transform.Find("InternetNotReachablePanel").gameObject;
        internetNotReachablePanel.GetComponent<MovementUI>().MoveToEnd();
        timeToInternetPanel = Time.realtimeSinceStartup + internetPanelTime;
    }
}
