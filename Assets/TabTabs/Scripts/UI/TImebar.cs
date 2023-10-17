using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TImebar : MonoBehaviour
{
    public static Image timebarImage;
    public PlayerHeart PlayerHeart;
    public float StartTimeGauge = 0.5f; // ���۽� Timebar�� �������� 50%����
    [SerializeField]public float depletionRate = 0.1f; // �ʴ� Timebar�� ������ 10% �϶�
    [SerializeField]public float depletionRateIncrease = 0.01f; // �� 3���� óġ�Ҷ����� Ÿ�� ������ �϶��� 1%�� ���
    public int KillCount = 0;
    public GameObject ReStart_Button;
    void Start()    
    {
        timebarImage = GetComponent<Image>();
        PlayerHeart = FindObjectOfType<PlayerHeart>();
        timebarImage.fillAmount = StartTimeGauge;
    }

    void Update()
    {
        timebarImage.fillAmount -= Time.deltaTime * depletionRate; // TimebarGauge 1�ʴ� 10%�� �϶�

        if (KillCount % 3 == 0 && KillCount > 0)
        {// ���� 3���� ó���Ҷ�����
            depletionRate += depletionRateIncrease; // Ÿ�ӹ��� ������ �϶��ӵ� 1%�� ���
            KillCount = 0;
        }

        if (timebarImage.fillAmount <= 0)
        {
            PlayerHeart.PlayerHeartGauge--; // �÷��̾� ��Ʈ ������ -
            Left_Orc2_Anim.LeftAnim.SetTrigger("Left_Attack");
            Right_Orc2_Anim.RightAnim.SetTrigger("Right_Attack");
            ReStart_Button.SetActive(true);
            //Time.timeScale = 0.0f;
            // + ���ӿ���
        }
    }
}
