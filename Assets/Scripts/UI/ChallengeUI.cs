using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChallengeUI : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Текст описания испытания")]
    [SerializeField] private TextMeshProUGUI challengeText;

    [Tooltip("UI панели выбора испытания")]
    [SerializeField] private GameObject challengePanel;

    [Tooltip("UI панели усилений")]
    [SerializeField] private GameObject amplificationPanel;

    [Tooltip("Кнопка запуска игры")]
    [SerializeField] private GameObject playButton;
#pragma warning restore 0649

    private bool isAmplificationState;
    private bool isChallengeState;
    private static GameObject lastSelectedButton = null;

    public void ChooseChallenge(string challengeName)
    {
        AudioManager.instance.Play("ClickUI");
        CurrentGameInfo.instance.challengeName = challengeName;
        if (challengeName != "ForestTraining")
            CurrentGameInfo.instance.challengeName += ProgressInfo.instance.forestLevelsStarsCount[challengeName];

        if (lastSelectedButton != null)
        {
            lastSelectedButton.GetComponent<Image>().color = Color.white;
            lastSelectedButton.GetComponent<RectTransform>().localScale = Vector3.one;
        }
           
        lastSelectedButton = EventSystem.current.currentSelectedGameObject;
        if(lastSelectedButton.GetComponent<Button>() != null)
        {
            lastSelectedButton.GetComponent<Image>().color = Color.red;
            lastSelectedButton.GetComponent<RectTransform>().localScale = new Vector3(1.25f, 1.25f, 1);
        }           

        playButton.GetComponent<MovementUI>().MoveToEnd();
        challengeText.GetComponent<LocalizedText>().key = $@"{challengeName}Description";
        challengeText.GetComponent<LocalizedText>().SetLocalization();
    }

    public void OpenChallengeUI()
    {
        isChallengeState = true;
        GetComponent<MovementUI>().MoveToEnd();
        GameButtons.isOpenPause = false;
    }

    void Update()
    {       
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isChallengeState)
                HideChallengeUI();
            else if (isAmplificationState)
                CloseAmplificationPanel();
        }
    }

    public void HideChallengeUI()
    {
        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            if (lastSelectedButton != null)
            {
                lastSelectedButton.GetComponent<Image>().color = Color.white;
                lastSelectedButton.GetComponent<RectTransform>().localScale = Vector3.one;
            }           
            challengeText.GetComponentInParent<MovementUI>().MoveToStart();
            challengeText.GetComponent<LocalizedText>().key = "ChallengeText";
            challengeText.GetComponent<LocalizedText>().SetLocalization();
            GetComponent<MovementUI>().MoveToStart();
            challengePanel.GetComponent<MovementUI>().SetStart();
            amplificationPanel.GetComponent<MovementUI>().SetStart();
            playButton.GetComponent<MovementUI>().MoveToStart();
            isChallengeState = false;
            CharAction.isDeath = false;
            GameObject.Find("Character(Clone)").GetComponent<CharController>().SetZeroSpeed(false);
            GameButtons.isOpenPause = true;
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
        isAmplificationState = true;
        isChallengeState = false;
        challengePanel.GetComponent<MovementUI>().MoveToEnd();
        amplificationPanel.GetComponent<MovementUI>().MoveToEnd();
    }

    public void CloseAmplificationPanel()
    {
        isAmplificationState = false;
        isChallengeState = true;
        challengePanel.GetComponent<MovementUI>().MoveToStart();
        amplificationPanel.GetComponent<MovementUI>().MoveToStart();
    }

    public void GoToGame()
    {
        GameButtons.isOpenPause = true;
        AudioManager.instance.Play("ClickUI");
        SceneManager.LoadScene("Game");
    }
}
