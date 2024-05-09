using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TabTabs.NamChanwoo
{
    public class MonsterTimebar : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;
        public PlayerHeart PlayerHeart;
        [SerializeField] public float depletionRate = 0.1f; // �ʴ� Timebar�� ������ 10% �϶�

        public float maxHealth = 100f;
        public float currentHealth = 100f;
        private Vector3 originalScale; // 최초 스케일 저장

        public GameObject userShield;
        public Button shieldButton;


        void Start()
        {
            PlayerHeart = FindObjectOfType<PlayerHeart>();
            originalScale = spriteRenderer.transform.localScale;
        }


        void Update()
        {
            currentHealth -= depletionRate * Time.deltaTime;
            if (currentHealth <= 99.80f)
            {
                userShield.SetActive(true);
            }

            if(currentHealth > 0)
            {
                float width = currentHealth / maxHealth * spriteRenderer.size.x;
                spriteRenderer.size = new Vector2(width, spriteRenderer.size.y);
                if(width <= 0.0f) {
                    currentHealth = 100f;
                    spriteRenderer.size = new Vector2( 0.94f, spriteRenderer.size.y);
                    // Timebar가 0되면 몬스터 공격 
                }
            }
           
        }

        public void buttonInit()
        {
            shieldButton = userShield.GetComponent<Button>();
            shieldButton.onClick.AddListener(shieldEvent);
        }

        public void shieldEvent() {
            currentHealth = 100f;
            spriteRenderer.size = new Vector2( 0.94f, spriteRenderer.size.y);
            userShield.SetActive(false);
        }

    }
}




