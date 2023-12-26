using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCharacter : MonoBehaviour
{
    public static bool swordGirl1 = false;
    public static bool swordGirl2 = false;
    public static bool leon = true;
    public Image selectImage;
    public Sprite swordGirl1Image;
    public Sprite swordGirl2Image;
    public Sprite leonImage;

    void Start()
    {
        //selectImage = GetComponent<Image>();
        // ���� �κ��� ĳ���� ����â���� leon�� ĳ���͸� �����Ѵٸ� ������ false�� ����
        // leon = true
        // ��Ʋ3���� ������ SetActive(false)
        // leon = SetActive(true)
        // �������忡�� �� ������ ����
    }

    public void UpdateImage(Sprite newSprite)
    {
        selectImage.sprite = newSprite;
    }

    public void SelectSwordGirl1()
    {
        swordGirl1 = true;
        UpdateImage(swordGirl1Image);
        swordGirl2 = false;
        leon = false;
    }

    public void SelectSwordGirl2()
    {
        swordGirl1 = false;
        swordGirl2 = true;
        UpdateImage(swordGirl2Image);
        leon = false;
    }

    public void SelectLeon()
    {
        swordGirl1 = false;
        swordGirl2 = false;
        leon = true;
        UpdateImage(leonImage);
    }
    
}
