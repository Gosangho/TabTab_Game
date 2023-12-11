using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ActiveButton : MonoBehaviour
{
    public Button Button;
    public GameObject TargetUI;
    public Sprite FirstImage;
    public Sprite SecondImage;
    
    void Start()
    {
        Button = GetComponent<Button>();
    }

    public void Active_B()
    {
        //audioManager.Instance.SfxAudioPlay("Ui_Click");
        TargetUI.SetActive(!TargetUI.activeSelf);
        if (TargetUI.activeSelf)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

    public void LobbyScene()
    {
        //audioManager.Instance.SfxAudioPlay("Ui_Click");
    }

    public void GameEnd()
    {
        //audioManager.Instance.SfxAudioPlay("Ui_Click");
        Application.Quit();
    }
}
