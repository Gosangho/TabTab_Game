using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Opening : MonoBehaviour
{
    public void CameraBackGround()
    {
        Camera.main.backgroundColor = new Color(0f, 0f, 0f);
    }
    public void GoLobby()
    {
        SceneManager.LoadScene(4);
    }
}
