using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartupManager : MonoBehaviour
{
    // Start is called before the first frame update
    private IEnumerator Start()
    {
        while(!LocalizationManager.instance.GetisReady())
        {
            yield return null;
        }
    }

}
