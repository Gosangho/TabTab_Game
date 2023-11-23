using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SkipButton : MonoBehaviour
{
    public Image Image;
    public Sprite GoImage;
    private void Start()
    {
        Image = GetComponent<Image>();
    }
    public void LobbyButton()
    {
        SceneManager.LoadScene(4);
    }
}
