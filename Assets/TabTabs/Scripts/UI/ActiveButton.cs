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
    void Start()
    {
        Button = GetComponent<Button>();
    }

    public void Active_B()
    {
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
        SceneManager.LoadScene(4);
    }

    public void GameEnd()
    {
        Application.Quit();
    }
}
