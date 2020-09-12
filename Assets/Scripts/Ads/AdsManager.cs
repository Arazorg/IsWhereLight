using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour
{
//#if UNITY_ANDROID || UNITY_IOS
    public static int countPress = 0;

    void Start()
    {
        if (Advertisement.isSupported)
        {
            Advertisement.debugMode = true;
            Advertisement.Initialize("3572524"); //ID here
        }
    }

    public static bool AdShow()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
            return true;
        }
        else
            return false;
    }
//#endif
}
