using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyUI : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Текст выбора персонажа")]
    [SerializeField] private LocalizedText characterText;
#pragma warning restore 0649

    private string characterKey;

    void Start()
    {
        Camera.main.backgroundColor = Color.black;
        characterKey = "chooseCharacter";
        characterText.key = characterKey;
        characterText.SetLocalization();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
