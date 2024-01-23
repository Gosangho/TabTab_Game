using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Animations;

namespace TabTabs.NamChanwoo
{

    public class BattleSystem : GameSystem
    {
        // BattleSystem에서 구현될 것들
        // 1. 전투로직
        // 2. Timebar와 에너미의 체력 --
        // NodeSheet클래스의 m_NodeType;(방향키) => 변수로 가져다 씀
        // SpawnSystem클래스의 SpawnNode(GameObject enemy)함수 => 싱클톤
        // EnemyBase클래스의 m_nodeQueue변수(Queue 블록) => GetownNodes함수 가져다 씀
        //NodeSheet NodeSheetInstance;
        public EnemyBase selectEnemy;
        public CharacterBase CharacterBaseInstance;
        List<EnemyBase> SceneEnemyList = new List<EnemyBase>();
        public ENodeType ClickNode;
        public Button LeftButton;
        public Button UpButton;
        public Button RightButton;
        public PlayerBase PlayerBaseInstance;
        public GameObject Player_Effect;
        
        void Start()
        {
            ClickNode = ENodeType.Default;
            CharacterBaseInstance = FindObjectOfType<CharacterBase>();
            PlayerBaseInstance = FindObjectOfType<PlayerBase>();
            LeftButton.onClick.AddListener(LeftB);
            UpButton.onClick.AddListener(UpB);
            RightButton.onClick.AddListener(RightB);
        }

        public override void OnSystemInit()
        {
            GameManager.NotificationSystem.SceneMonsterSpawned.AddListener(HandleSceneMonsterSpawned);
        }

        private void HandleSceneMonsterSpawned(EnemyBase spawnedEnemy)
        {
            
            if (selectEnemy == null) 
                selectEnemy = spawnedEnemy;

            SceneEnemyList.Add(spawnedEnemy);
        }
        
        void Update()
        {
            if (ClickNode!=ENodeType.Default)
            {// ClickNode가 중립이 아니라면(버튼을 클릭했다면)
                
                if (selectEnemy==null) { return; }
                
                if (ClickNode==selectEnemy.GetOwnNodes().Peek().nodeSheet.m_NodeType)
                {//다음에 나갈 노드타입과 비교(같은 NodeType을 클릭했다면)

                    GameManager.NotificationSystem.NodeHitSuccess?.Invoke();
                    
                    // 1. 해당하는 enemy의 블럭 destroy
                    // 2. 캐릭터가 해당하는 enemy의 블럭위치로 이동 후 공격 애니메이션 재생 후 원래위치로 이동
                    // 3. 시간변수 +

                    CharacterBaseInstance.gameObject.transform.position = new Vector3(selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.x
                    , selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.y,0.0f);
                    //PlayerBaseInstance.PlayerAnim.SetTrigger("Atk1_Triger"); // 오크의 위치로 이동해 공격모션
                    Vector3 targetPosition = selectEnemy.GetOwnNodes().Peek().gameObject.transform.position;
                    Instantiate(Player_Effect, targetPosition, Quaternion.identity);
                    Destroy(selectEnemy.GetOwnNodes().Peek().gameObject);

                    selectEnemy.GetOwnNodes().Dequeue();
                    selectEnemy.Hit();
                    
                    if (selectEnemy.GetOwnNodes().Count<=0)
                    {
                        // 에너미 노드의 남아있는 갯수가 0보다 작거나 같다면
                        // 몬스터 제거 후 다시생성
                        selectEnemy.Die();
                        if (SceneEnemyList.Count > 0)
                        {
                            selectEnemy = SceneEnemyList[0];
                        }
                    }
                }
                else
                {
                    GameManager.NotificationSystem.NodeHitFail?.Invoke();
                    
                    if (selectEnemy!=null)
                    {
                        selectEnemy.Attack();
                    }
                    
                    // 아니라면
                    // 1. 캐릭터 Hp --
                    // 2. 캐릭터 원래위치로 이동
                    // 3. Enemy의 캐릭터 공격 애니메이션 재생
                    // 4. 노드 다시 생성하는 함수 호출
                    SpawnSystem.Instance.SpawnNode(selectEnemy);
                }

                ClickNode = ENodeType.Default; // reset
                Debug.Log(ClickNode);
            }
        }

        void LeftB()
        {
            ClickNode = ENodeType.Left;
        }
        void UpB()
        {
            ClickNode = ENodeType.Up;
        }
        void RightB()
        {
            ClickNode = ENodeType.Right;
        }
        

    }
}