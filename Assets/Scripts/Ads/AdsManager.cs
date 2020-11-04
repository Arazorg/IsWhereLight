using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour
{
    //#if UNITY_ANDROID || UNITY_IOS

#pragma warning disable 0649
    [Tooltip("ID игры")]
    [SerializeField] private string gameIDGoogle;

    [Tooltip("ID игры")]
    [SerializeField] private string gameIDApple;

    [Tooltip("ID игры")]
    [SerializeField] private string placementID;
#pragma warning restore 0649

    public static AdsManager instance;

    private bool isRevive;
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
        CheckAds();
    }

    private void CheckAds()
    {
        if (Advertisement.isSupported)
        {
            Advertisement.debugMode = true;

            if (Application.platform == RuntimePlatform.IPhonePlayer)
                Advertisement.Initialize(gameIDApple, true);
            else
            {
                Advertisement.Initialize(gameIDGoogle, true);
                Debug.Log("ok");
            }
        }
    }

    public void AdShow(bool isRevive = false)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
            AnotherUI.instance.ShowInternetNotReachablePanel();
        else
        {
            this.isRevive = isRevive;
            if (Advertisement.IsReady())
            {
                StartCoroutine(ShowAdsWithTimeOut(3));
            }
            else
            {
                CheckAds();
            }
        }
    }

    public void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                if (isRevive)
                    GameButtons.instance.Revive();
                else
                    ProgressInfo.instance.MoneyPlus(500);
                break;
            case ShowResult.Skipped:
                break;
            case ShowResult.Failed:
                break;
        }
    }

    IEnumerator ShowAdsWithTimeOut(float timeOut)
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
            if (Application.platform == RuntimePlatform.IPhonePlayer)
                Advertisement.Initialize(gameIDApple, true);
            else
                Advertisement.Initialize(gameIDGoogle, true);
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