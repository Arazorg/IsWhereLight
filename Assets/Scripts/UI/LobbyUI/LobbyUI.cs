using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyUI : MonoBehaviour
{
    [Tooltip("Текст выбора персонажа")]
    [SerializeField] private LocalizedText characterText;

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
