using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TImebar : MonoBehaviour
{
    public static Image timebarImage;
    public PlayerHeart PlayerHeart;
    
    void Start()
    {
        timebarImage = GetComponent<Image>();
        PlayerHeart = FindObjectOfType<PlayerHeart>();
    }

    void Update()
    {
        timebarImage.fillAmount -= Time.deltaTime * 0.1f;

        if (timebarImage.fillAmount <= 0)
        {
            PlayerHeart.PlayerHeartGauge--; // �÷��̾� ��Ʈ ������ -
            timebarImage.fillAmount = 1.0f;
            Left_Orc2_Anim.LeftAnim.SetTrigger("Left_Attack");
            Right_Orc2_Anim.RightAnim.SetTrigger("Right_Attack");
            // + ���ӿ���
        }
    }
}
