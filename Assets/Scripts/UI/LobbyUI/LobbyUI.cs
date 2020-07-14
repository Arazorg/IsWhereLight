using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Текст выбора персонажа")]
    [SerializeField] private LocalizedText characterText;

    [Tooltip("UI текста")]
    [SerializeField] private GameObject characterTextUI;

    [Tooltip("Кнопка назад в меню")]
    [SerializeField] private Button backToLobbyButton;
#pragma warning restore 0649

    private string characterKey;

    void Start()
    {
        Camera.main.backgroundColor = Color.black;
        characterKey = "chooseCharacter";
        characterText.key = characterKey;
        characterText.SetLocalization();
        characterTextUI.GetComponent<MovementUI>().MoveToEnd();
        backToLobbyButton.GetComponent<MovementUI>().MoveToEnd();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
