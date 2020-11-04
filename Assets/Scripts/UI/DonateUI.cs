using UnityEngine;
using UnityEngine.SceneManagement;

public class DonateUI : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Стартовое UI лобби")]
    [SerializeField] private LobbyUI lobbyUI;
#pragma warning restore 0649
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            HideDonatePanel();
    }

    public void HideDonatePanel()
    {
        AudioManager.instance.Play("ClickUI");
        GetComponent<MovementUI>().MoveToStart();
        if (SceneManager.GetActiveScene().name == "Lobby")
            lobbyUI.IsLobbyState = true;
    }

    public void ShowAd()
    {
        AdsManager.instance.AdShow();
    }
}
