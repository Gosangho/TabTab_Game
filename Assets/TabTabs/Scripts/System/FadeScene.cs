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

    public void LoadLobbyScene()
    {
        isLobby = true;
        StartCoroutine(_LoadLobbyScene());
    }

    IEnumerator _LoadLobbyScene()
    {
        Time.timeScale = 1.0f;

        //audioManager.Instance.SfxAudioPlay("Ui_Click");
        
        Transition.SetTrigger("FadeSceneStart");

        yield return new WaitForSeconds(TransitionTime);

        SceneManager.LoadScene(6);
    }

    IEnumerator _LoadeSceneBattle()
    {
        Time.timeScale = 1.0f;

        Transition.SetTrigger("FadeSceneStart");
        //audioManager.Instance.SfxAudioPlay("Ui_Click");

        yield return new WaitForSeconds(TransitionTime);

        SceneManager.LoadScene(6);
    }

    IEnumerator _LoadeTutorialScene()
    {
        Time.timeScale = 1.0f;

        //audioManager.Instance.SfxAudioPlay("Ui_Click");

        Transition.SetTrigger("FadeSceneStart");

        yield return new WaitForSeconds(TransitionTime);

        SceneManager.LoadScene(6);
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
