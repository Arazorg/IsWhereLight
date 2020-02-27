using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Background : MonoBehaviour, IPointerDownHandler
{
    public GameObject settingsButton;
    public GameObject settingsPanel;
    private SettingsInfo settingsInfo;
    
    void Start()
    {
        settingsInfo = GameObject.Find("SettingsHandler").GetComponent<SettingsInfo>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (settingsPanel.activeSelf == true)
        {
            settingsPanel.SetActive(false);
            settingsButton.SetActive(true);
        }
        settingsInfo.SaveSettings();
    }
}
