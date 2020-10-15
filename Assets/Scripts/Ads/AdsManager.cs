using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour
{
    //#if UNITY_ANDROID || UNITY_IOS

#pragma warning disable 0649
    [Tooltip("ID игры")]
    [SerializeField] private string gameID;

    [Tooltip("ID игры")]
    [SerializeField] private string placementID;
#pragma warning restore 0649

    public static AdsManager instance;
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

    void Start()
    {
        if (Advertisement.isSupported)
        {
            Advertisement.debugMode = true;
            Advertisement.Initialize(gameID); //ID here
        }
    }

    [System.Obsolete]
    public bool AdShow()
    {
        if (Advertisement.IsReady())
        {
            StartCoroutine(showAdsWithTimeOut(3));
            return true;
        }
        else
            return false;
    }

    public void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("<color=green>The ad was skipped before reaching the end.</color>");
                break;
            case ShowResult.Skipped:
                Debug.Log("<color=yellow>The ad was skipped before reaching the end.</color>");
                break;
            case ShowResult.Failed:
                Debug.LogError("<color=red>The ad failed to be shown.</color>");
                break;
        }
    }

    [System.Obsolete]
    IEnumerator showAdsWithTimeOut(float timeOut)
    {
        //Check if ad is supported on this platform 
        if (!Advertisement.isSupported)
        {
            Debug.LogError("<color=red>Ad is NOT supported</color>");
            yield break; //Exit coroutine function because ad is not supported
        }

        Debug.Log("<color=green>Ad is supported</color>");

        //Initialize ad if it has not been initialized
        if (!Advertisement.isInitialized)
        {
            //Initialize ad
            Advertisement.Initialize(gameID, true);
        }


        float counter = 0;
        bool adIsReady = false;

        // Wait for timeOut seconds until ad is ready
        while (counter < timeOut)
        {
            counter += Time.deltaTime;
            if (Advertisement.IsReady())
            {
                adIsReady = true;
                break; //Ad is //Ad is ready, Break while loop and continue program
            }
            yield return null;
        }

        //Check if ad is not ready after waiting
        if (!adIsReady)
        {
            Debug.LogError("<color=red>Ad failed to be ready in " + timeOut + " seconds. Exited function</color>");
            yield break; //Exit coroutine function because ad is not ready
        }

        Debug.Log("<color=green>Ad is ready</color>");

        //Check if zoneID is empty or null
        if (string.IsNullOrEmpty(placementID))
        {
            Debug.Log("<color=red>zoneId is null or empty. Exited function</color>");
            yield break; //Exit coroutine function because zoneId null
        }

        Debug.Log("<color=green>ZoneId is OK</color>");


        //Everything Looks fine. Finally show ad (Missing this part in your code)
        ShowOptions options = new ShowOptions();
        options.resultCallback = HandleShowResult;

        Advertisement.Show(placementID, options);
    }
    //#endif
}