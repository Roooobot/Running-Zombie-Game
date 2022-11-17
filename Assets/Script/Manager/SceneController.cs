using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>,IEndGameObserver
{
    public GameObject playerPrefab;
    public SceneFader sceneFaderPrefab;
    public SceneFader winFaderPrefab;
    public SceneFader failFaderPrefab;
    GameObject player;

    bool fadeFinished;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        fadeFinished=true;
        GameManager.Instance.AddObserver(this);
    }

    public void TransitionToFirstLevel()
    {
        StartCoroutine(LoadLevel("MainScenes"));
    }

    public void TransitionToLoadGame()
    {
        StartCoroutine(LoadLevel(SaveManager.Instance.SceneName));
     }

    public void TransitionToMain()
    {
        StartCoroutine(LoadMain());
    }

    IEnumerator LoadLevel(string scene)
    {
        SceneFader fader = Instantiate(sceneFaderPrefab);
        if (scene != "")
        {
            yield return StartCoroutine(fader.FadeOut(2f));
            yield return SceneManager.LoadSceneAsync(scene);
            yield return player = Instantiate(playerPrefab, GameManager.Instance.GetEntrance().position,GameManager.Instance.GetEntrance().rotation);

            SaveManager.Instance.SavePlayerData();
            yield return StartCoroutine(fader.FadeIn(2f));
            yield break;
        }
    }

    IEnumerator LoadMain()
    {
        SceneFader fader = Instantiate(sceneFaderPrefab);
        yield return StartCoroutine(fader.FadeOut(2f));
        yield return SceneManager.LoadSceneAsync("Main");
        yield return StartCoroutine(fader.FadeIn(2f));
        yield break;
    }

    IEnumerator Loadfail()
    {
        SceneFader fader = Instantiate(failFaderPrefab);
        yield return StartCoroutine(fader.FadeOut(2f));
        yield return StartCoroutine(fader.FadeIn(2f));
        yield break;
    }
    IEnumerator Loadwin()
    {
        SceneFader fader = Instantiate(winFaderPrefab);
        yield return StartCoroutine(fader.FadeOut(2f));
        yield return StartCoroutine(fader.FadeIn(2f));
        yield break;
    }

    public void EndNotify()
    {
        if (fadeFinished)
        {
            StartCoroutine(Loadfail());
            fadeFinished = false;
            Invoke(nameof(End), 4f);
        }
    }
    public void WinNotify()
    {
        if (fadeFinished)
        {
            StartCoroutine(Loadwin());
            fadeFinished = false;
            Invoke(nameof(End), 4f);
        }
    }


    void End()
    {
        StartCoroutine(LoadMain());
    }
}
