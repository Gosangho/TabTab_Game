using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeScene : MonoBehaviour
{
    public Animator Transition;
    public float TransitionTime = 1f;

    public void LoadLobbyScene()
    {
        StartCoroutine(_LoadLobbyScene());
    }

    IEnumerator _LoadLobbyScene()
    {
        Time.timeScale = 1.0f;

        audioManager.Instance.SfxAudioPlay("Ui_Click");
        
        Transition.SetTrigger("FadeSceneStart");

        yield return new WaitForSeconds(TransitionTime);

        SceneManager.LoadScene(4);
    }

    IEnumerator _LoadeSceneBattle()
    {
        Time.timeScale = 1.0f;

        audioManager.Instance.SfxAudioPlay("Ui_Click");

        Transition.SetTrigger("FadeSceneStart");

        yield return new WaitForSeconds(TransitionTime);

        SceneManager.LoadScene(3);
    }

    IEnumerator _LoadeTutorialScene()
    {
        Time.timeScale = 1.0f;

        audioManager.Instance.SfxAudioPlay("Ui_Click");

        Transition.SetTrigger("FadeSceneStart");

        yield return new WaitForSeconds(TransitionTime);

        SceneManager.LoadScene(5);
    }

    

    public void LoadBattleScene()
    {
        StartCoroutine(_LoadeSceneBattle());
    }

    public void LoadTutorialScene()
    {
        StartCoroutine(_LoadeTutorialScene());
    }
}
