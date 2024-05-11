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

        void Start()
        {
            PlayerHeart = FindObjectOfType<PlayerHeart>();
            originalScale = spriteRenderer.transform.localScale;
            test4Battle = FindObjectOfType<Test4Battle>();
        }


        void Update()
        {
            //if(wayOrc == 0) {
                currentHealth -= depletionRate * Time.deltaTime;
                if (currentHealth <= 99.80f)
                {
                    userShield.SetActive(true);
                }
                if(currentHealth > 0)
                {    
                    float width = currentHealth / maxHealth * spriteRenderer.size.x;
                    spriteRenderer.size = new Vector2(width, spriteRenderer.size.y);
                }

                if(currentHealth <= 99.0f) {
                    if(wayOrc == 0) {
                        Left_Orc2_Anim.LeftAnim.SetTrigger("Left_Attack");
                    } else {
                        Right_Orc2_Anim.RightAnim.SetTrigger("Right_Attack");
                    }
                    PlayerHeart.PlayerHeartGauge -= 1;
                    currentHealth = 100f;
                    spriteRenderer.size = new Vector2( 0.94f, spriteRenderer.size.y);
                }
            // } else {
            //     Debug.Log("currentRightHealth::"+currentRightHealth);
    
            //     currentRightHealth -= depletionRate * Time.deltaTime;
              
            //     if (currentRightHealth <= 99.80f)
            //     {
            //         rightUserShield.SetActive(true);
            //     }
            //     if(currentRightHealth > 0)
            //     {
            //         float width = currentRightHealth / maxHealth * spriteRenderer.size.x;
            //         spriteRenderer.size = new Vector2(width, spriteRenderer.size.y);
            //     }
            //     if(currentRightHealth <= 99.0f) {
            //         Right_Orc2_Anim.RightAnim.SetTrigger("Right_Attack");

            //         PlayerHeart.PlayerHeartGauge -= 1;
            //         currentRightHealth = 100f;
            //         spriteRenderer.size = new Vector2( 0.94f, spriteRenderer.size.y);
            //     }
            // }

            if(PlayerHeart.PlayerHeartGauge == 0)
            {
                test4Battle.GameOverProcess();
            }
           
        }

        public void buttonInit()
        {
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

      //  public void rightShieldEvent() {
      //      currentRightHealth = 100f;
      //      spriteRenderer.size = new Vector2( 0.94f, spriteRenderer.size.y);
      //      userShield.SetActive(false);
     //   }

    }
}




