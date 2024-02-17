using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeScene : MonoBehaviour
{
    public Animator Transition;
    public float TransitionTime = 0.1f;
    public static bool isBattle = false;
    public static bool isLobby = false;
    public static bool isTutorial = false;

    public void Opennig()
    {
        if (DataManager.Instance.playerData.TutorialPlay == true)
        {
            isLobby = true;
            StartCoroutine(_LoadLobbyScene());
        }
        else
        {
            isTutorial = true;
            StartCoroutine(_LoadeTutorialScene());
        }
    }

    public void LoadLobbyScene()
    {
        isLobby = true;
        StartCoroutine(_LoadLobbyScene());
    }

    IEnumerator _LoadLobbyScene()
    {
        Time.timeScale = 1.0f;

        Transition.SetTrigger("FadeSceneStart");

        yield return new WaitForSeconds(TransitionTime);

        SceneManager.LoadScene(5);
    }

    IEnumerator _LoadeSceneBattle()
    {
        Time.timeScale = 1.0f;

        Transition.SetTrigger("FadeSceneStart");

        yield return new WaitForSeconds(TransitionTime);

        SceneManager.LoadScene(5);
    }

    IEnumerator _LoadeTutorialScene()
    {
        Time.timeScale = 1.0f;

        Transition.SetTrigger("FadeSceneStart");

        yield return new WaitForSeconds(TransitionTime);

        SceneManager.LoadScene(5);
    }

    public void LoadBattleScene()
    {
        isBattle = true;
        StartCoroutine(_LoadeSceneBattle());
    }

    public void LoadTutorialScene()
    {
        isTutorial = true;
        StartCoroutine(_LoadeTutorialScene());
    }
}
