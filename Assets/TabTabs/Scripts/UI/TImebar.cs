using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TabTabs.NamChanwoo
{
    public class TImebar : MonoBehaviour
    {
        public static Image timebarImage;
        public float TimebarImagefillAmount;
        public PlayerHeart PlayerHeart;
        public float StartTimeGauge = 0.5f; // ���۽� Timebar�� �������� 50%����
        [SerializeField] public float depletionRate = 0.1f; // �ʴ� Timebar�� ������ 10% �϶�
        [SerializeField] public float depletionRateIncrease = 0.01f; // �� 3���� óġ�Ҷ����� Ÿ�� ������ �϶��� 1%�� ���
        public int KillCount = 0;

        public SpriteRenderer backgroundSprite;
        private bool isFlashing = false; // 현재 깜박이고 있는지 여부

        void Start()
        {
            TimebarImagefillAmount = 0.5f;
            timebarImage = GetComponent<Image>();
            PlayerHeart = FindObjectOfType<PlayerHeart>();
            timebarImage.fillAmount = StartTimeGauge;
        }

        void Update()
        {
            TimebarImagefillAmount = timebarImage.fillAmount;
            
            timebarImage.fillAmount -= Time.deltaTime * depletionRate; // TimebarGauge 1�ʴ� 10%�� �϶�

            if (KillCount % 3 == 0 && KillCount > 0)
            {// ���� 3���� ó���Ҷ�����
                depletionRate += depletionRateIncrease; // Ÿ�ӹ��� ������ �϶��ӵ� 1%�� ���
                KillCount = 0;
            }
            if (timebarImage.fillAmount <= 0.3f && !isFlashing)
            {
                StartCoroutine(FlashBackground());
            }
            else if (timebarImage.fillAmount > 0.3f && isFlashing)
            {
                StopCoroutine(FlashBackground());
                isFlashing = false;
                backgroundSprite.color = Color.white; // 깜박임 중지 시 원래 색상으로 복원
            }
        }

        IEnumerator FlashBackground()
        {
            isFlashing = true;
            while (timebarImage.fillAmount <= 0.3f)
            {
                backgroundSprite.color = Color.red; // 빨간색으로 변경
                yield return new WaitForSeconds(0.5f); // 0.5초 대기
                backgroundSprite.color = Color.white; // 원래 색상으로 변경
                yield return new WaitForSeconds(0.5f); // 0.5초 대기
            }
            isFlashing = false;
        }
    }
}




