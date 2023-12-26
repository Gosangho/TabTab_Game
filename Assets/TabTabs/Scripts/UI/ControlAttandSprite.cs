using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlAttandSprite : MonoBehaviour
{
    public Image currentImage;
    [SerializeField] Sprite toDayAttand; // ������ �⼮����
    [SerializeField] Sprite lateAttand; // ���� ��¥�� �ȵ� �⼮����
    [SerializeField] Sprite notGetAttand; // ��¥�� ������ �������� �⼮����
    void Start()
    {
        currentImage = GetComponent<Image>();

        int day = int.Parse(gameObject.name);

        if (day < System.DateTime.Now.Day && AttandManager.AttandInstance.attandDay[day - 1] == false)
            currentImage.sprite = lateAttand;
        else if (day < System.DateTime.Now.Day && AttandManager.AttandInstance.attandDay[day - 1] == true)
            currentImage.sprite = toDayAttand;
        else
            currentImage.sprite = notGetAttand;
    }

    public void UpdateSprite()
    {
        currentImage.sprite = toDayAttand;
        Color currentColor = currentImage.color;
        currentColor.r = 0;
        currentColor.g = 0;
        currentColor.b = 0;
        currentImage.color = currentColor;
    }
}
