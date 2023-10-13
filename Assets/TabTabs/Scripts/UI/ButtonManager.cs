using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Button myButton; // ��ư ������Ʈ�� ������ public ����
    public Button ReStartButton;
    public Sprite normalImage; // ���� �̹���
    public Sprite pressedImage; // ������ �� ������ �̹���

    private Image buttonImage;
    private bool isPressed = false;

    private void Start()
    {
        buttonImage = myButton.GetComponent<Image>();
        // ���� �̹����� �ʱ�ȭ
        buttonImage.sprite = normalImage;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // ������ �� ������ �̹����� ����
        buttonImage.sprite = pressedImage;
        isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // ���콺�� ���� �� ���� �̹����� ����
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
