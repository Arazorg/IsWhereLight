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

    [Tooltip("Кнопка запуска игры")]
    [SerializeField] private GameObject playButton;
#pragma warning restore 0649

    public void ChooseChallenge(string challenge)
    {
        AudioManager.instance.Play("ClickUI");
        var challengeNumber = Convert.ToInt32(challenge[challenge.Length - 1]);
        CurrentGameInfo.instance.challengeNumber = challengeNumber;
        challengeText.GetComponentInParent<MovementUI>().MoveToEnd();
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
            playButton.GetComponent<MovementUI>().MoveToStart();
            GameObject.Find("Character(Clone)").GetComponent<CharController>().SetSpeed(false);
        }
    }

    public void CancelChooseChallenge()
    {
        challengeText.GetComponent<LocalizedText>().key = "ChallengeDiscover";
        challengeText.GetComponent<LocalizedText>().SetLocalization();
        playButton.GetComponent<MovementUI>().SetStart();
    }
}
