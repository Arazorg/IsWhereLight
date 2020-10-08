using UnityEngine;

public class ThanksUI : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Слайдер эффектов")]
    [SerializeField] private SettingsButtons settingsButtons;
#pragma warning restore 0649

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && settingsButtons.IsThanksPanelState)
            settingsButtons.ThanksPanelOpenClose();
    }

    public void LinkTo(string link)
    {
        Application.OpenURL(link);
    }

}
