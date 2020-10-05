using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChallengeUI : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Текст описания испытания")]
    [SerializeField] private TextMeshProUGUI challengeText;

    [Tooltip("UI панели усилений")]
    [SerializeField] private GameObject challengePanel;

    [Tooltip("UI панели усилений")]
    [SerializeField] private GameObject amplificationPanel;

    [Tooltip("Кнопка запуска игры")]
    [SerializeField] private GameObject playButton;
#pragma warning restore 0649

    public void ChooseChallenge(string challenge)
    {
        AudioManager.instance.Play("ClickUI");
        var challengeNumber = Convert.ToInt32(challenge[challenge.Length - 1]);
        CurrentGameInfo.instance.challengeNumber = challengeNumber;
        playButton.GetComponent<MovementUI>().MoveToEnd();
        challengeText.GetComponent<LocalizedText>().key = $@"{challenge}ChallengeDescription";
        challengeText.GetComponent<LocalizedText>().SetLocalization();
    }

    public void HideChallengeUI()
    {
        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            challengeText.GetComponentInParent<MovementUI>().MoveToStart();
            challengeText.GetComponent<LocalizedText>().key = "ChallengeText";
            challengeText.GetComponent<LocalizedText>().SetLocalization();
            GetComponent<MovementUI>().MoveToStart();
            challengePanel.GetComponent<MovementUI>().SetStart();
            amplificationPanel.GetComponent<MovementUI>().SetStart();
            playButton.GetComponent<MovementUI>().MoveToStart();
            CharAction.isDeath = false;
            GameObject.Find("Character(Clone)").GetComponent<CharController>().SetZeroSpeed(false);
        }
    }

    public void CancelChooseChallenge()
    {
        challengeText.GetComponent<LocalizedText>().key = "Discover";
        challengeText.GetComponent<LocalizedText>().SetLocalization();
        playButton.GetComponent<MovementUI>().SetStart();
    }

    public void OpenAmplificationPanel()
    {
        challengePanel.GetComponent<MovementUI>().MoveToEnd();
        amplificationPanel.GetComponent<MovementUI>().MoveToEnd();
    }

    public void CloseAmplificationPanel()
    {
        challengePanel.GetComponent<MovementUI>().MoveToStart();
        amplificationPanel.GetComponent<MovementUI>().MoveToStart();
    }

    public void GoToGame()
    {
        CurrentGameInfo.instance.challengeNumber = 2;
        AudioManager.instance.Play("ClickUI");
        SceneManager.LoadScene("Game");
    }
}
