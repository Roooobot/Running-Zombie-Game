using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Playables;

public class MainMenu : MonoBehaviour
{
    Button NewGameBtn;
    Button LoadGameBtn;
    Button QuitBtn;

    PlayableDirector director;

    private void Awake()
    {
        NewGameBtn = transform.GetChild(1).GetComponent<Button>();
        LoadGameBtn = transform.GetChild(2).GetComponent<Button>();
        QuitBtn = transform.GetChild(3).GetComponent<Button>();

        director = FindObjectOfType<PlayableDirector>();
        director.stopped += NewGame;

        NewGameBtn.onClick.AddListener(PlayTimeline);
        LoadGameBtn.onClick.AddListener(ContinueGame);
        QuitBtn.onClick.AddListener(QuitGame);
    }

    void PlayTimeline()
    {
        director.Play();
    }

    void NewGame(PlayableDirector obj)
    {
        PlayerPrefs.DeleteAll();
        SceneController.Instance.TransitionToFirstLevel();
    }

    void ContinueGame()
    {
        SceneController.Instance.TransitionToLoadGame();
    }

     void QuitGame()
    {
        Application.Quit();
    }
}
