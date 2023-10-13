using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Button myButton; // 버튼 오브젝트를 연결할 public 변수
    public Button ReStartButton;
    public Sprite normalImage; // 원래 이미지
    public Sprite pressedImage; // 눌렸을 때 보여질 이미지

    private Image buttonImage;
    private bool isPressed = false;

    private void Start()
    {
        buttonImage = myButton.GetComponent<Image>();
        // 원래 이미지로 초기화
        buttonImage.sprite = normalImage;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // 눌렸을 때 보여질 이미지로 변경
        buttonImage.sprite = pressedImage;
        isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // 마우스를 땠을 때 원래 이미지로 변경
        buttonImage.sprite = normalImage;
        isPressed = false;
    }

    public void ReStartButton2()
    {
        StartCoroutine(ReStart());
    }
    IEnumerator ReStart()
    {
        Time.timeScale = 1.0f;
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(3);
    }
}
