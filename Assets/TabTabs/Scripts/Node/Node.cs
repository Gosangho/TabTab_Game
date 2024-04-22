using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace TabTabs.NamChanwoo
{
    public class Node : NodeBase<NodeSheet>
    {
        
        [SerializeField] private GameObject m_button;
        public GameObject nodeButton => m_button;
        public Sprite[] m_sprite;

        private SpriteRenderer nodeSprite;
        

        private void Awake()
        {
            nodeSprite = m_button.GetComponent<SpriteRenderer>();
            nodeSprite.sprite = m_sheet.m_NodeImage;
        }

        public override void Init_Right()
        {
            // Node의 스프라이트를 설정합니다.
            
            Debug.Log("SetSpriteNode3");
            
            // 부모 오브젝트인 NodeArea를 얻습니다.
            NodeArea nodeArea = GetComponentInParent<NodeArea>();
            
            // NodeArea로부터 사각형의 너비와 높이를 얻습니다.
            float width = nodeArea.GetRectangleWidth();
            float height = nodeArea.GetRectangleHeight();

            // 스프라이트의 실제 너비와 높이를 얻습니다.
            float spriteWidth = nodeSprite.sprite.bounds.size.x;
            float spriteHeight = nodeSprite.sprite.bounds.size.y;

            // 사각형의 너비와 높이를 스프라이트의 너비와 높이로 나누어서, 스케일 팩터를 계산합니다.
            float scaleX = width / spriteWidth;
            float scaleY = height / spriteHeight;

            // 계산한 스케일 팩터를 이용해 게임 오브젝트의 스케일을 설정합니다.
            gameObject.transform.localScale = new Vector3(scaleX, scaleY, 1f);
        }

        public override void Init_Left()
        {
            // Node의 스프라이트를 설정합니다.

            // 부모 오브젝트인 NodeArea를 얻습니다.
            NodeArea nodeArea = GetComponentInParent<NodeArea>();

            // NodeArea로부터 사각형의 너비와 높이를 얻습니다.
            float width = nodeArea.GetRectangleWidth();
            float height = nodeArea.GetRectangleHeight();

            // 스프라이트의 실제 너비와 높이를 얻습니다.
            float spriteWidth = nodeSprite.sprite.bounds.size.x;
            float spriteHeight = nodeSprite.sprite.bounds.size.y;

            // 사각형의 너비와 높이를 스프라이트의 너비와 높이로 나누어서, 스케일 팩터를 계산합니다.
            float scaleX = width / spriteWidth;
            float scaleY = height / spriteHeight;

            // 계산한 스케일 팩터를 이용해 게임 오브젝트의 스케일을 설정합니다.
            gameObject.transform.localScale = new Vector3(-scaleX, scaleY, 1f);
        }

        public void SetLocalScale(Vector3 scale)
        {
            transform.localScale = scale;
        }

        public void SetSpriteNode(int index)
        {
            if(index == 1) {
                nodeSprite.color = Color.red;
            } else if(index == 2) {
                nodeSprite.color = Color.blue;
            } else if(index == 3) {
                nodeSprite.color = Color.yellow;
            } else if(index == 4) {
                nodeSprite.sprite = m_sprite[1];
            }
        }
    }

}