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

        Test4Battle test4Battle;
        public int wayOrc;
        public GameObject timebarObject;
        public bool isTimebar = false;

        public EnemyBase selectEnemy;

        void Start()
        {
            PlayerHeart = FindObjectOfType<PlayerHeart>();
            originalScale = spriteRenderer.transform.localScale;
            test4Battle = FindObjectOfType<Test4Battle>();
        }


        void Update()
        {
            if(isTimebar) {
                currentHealth -= depletionRate * Time.deltaTime;
               // Debug.Log(currentHealth);
                if (currentHealth <= 99.65f)
                {
                    userShield.SetActive(true);
                }
                if(currentHealth > 0)
                {    
                    float width = currentHealth / maxHealth * spriteRenderer.size.x;
                    spriteRenderer.size = new Vector2(width, spriteRenderer.size.y);
                }

                if(currentHealth <= 99.4f) {
                    selectEnemy.monster_anim.SetTrigger("Attack");
                    PlayerHeart.PlayerHeartGauge -= 1;
                    currentHealth = 100f;
                    spriteRenderer.size = new Vector2( 0.94f, spriteRenderer.size.y);
                }
                if(PlayerHeart.PlayerHeartGauge == 0)
                {
                    test4Battle.GameOverProcess();
                }
            }
        }


        public void buttonInit()
        {
            currentHealth = 100f;
            userShield.SetActive(false);
            if(wayOrc == 0) {
                shieldButton = userShield.GetComponent<Button>();
                shieldButton.onClick.AddListener(shieldEvent);
            } else {
                shieldButton = userShield.GetComponent<Button>();
                shieldButton.onClick.AddListener(shieldEvent);
            }
        }

        public void shieldEvent() {
            currentHealth = 100f;
            spriteRenderer.size = new Vector2( 0.94f, spriteRenderer.size.y);
            userShield.SetActive(false);
        }

        
        void OnDestroy()
        {
            currentHealth = 100f;
            spriteRenderer.size = new Vector2( 0.94f, spriteRenderer.size.y);
            if(userShield != null) {
                userShield.SetActive(false);
            }
        }

    }
}




