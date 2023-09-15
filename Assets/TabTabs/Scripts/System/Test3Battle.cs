using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Animations;


namespace TabTabs.NamChanwoo
{
    public class Test3Battle : GameSystem
    {
        // BattleSystem에서 구현될 것들
        // 1. 전투로직
        // 2. Timebar와 에너미의 체력 --
        // NodeSheet클래스의 m_NodeType;(방향키) => 변수로 가져다 씀
        // SpawnSystem클래스의 SpawnNode(GameObject enemy)함수 => 싱클톤
        // EnemyBase클래스의 m_nodeQueue변수(Queue 블록) => GetownNodes함수 가져다 씀
        //NodeSheet NodeSheetInstance;
        public EnemyBase selectEnemy;
        public EnemyBase LeftEnemy;
        public EnemyBase RightEnemy;
        public CharacterBase CharacterBaseInstance;
        public List<EnemyBase> SceneEnemyList = new List<EnemyBase>();
        public ENodeType ClickNode;
        public Button AttackButton;
        public Button SelectEnemyButton;
        public PlayerBase PlayerBaseInstance;
        public GameObject Left_Ork;
        public GameObject Right_Ork;
        public bool MonsterDie; // 어느 쪽 몬스터가 죽었는지 확인하는 bool형 변수
        public Node NodeInstance;
        public GameObject Character_Effect;
        bool FirstAttack; // 게임시작시 공격버튼으로 최초 한번만 재생되는 애니메이션 변수

        void Start()
        {
            ClickNode = ENodeType.Default;
            CharacterBaseInstance = FindObjectOfType<CharacterBase>();
            PlayerBaseInstance = FindObjectOfType<PlayerBase>();
            AttackButton.onClick.AddListener(Attack);
            SelectEnemyButton.onClick.AddListener(SelectEnemy);
            StartSpawn();
            MonsterDie = false;
            FirstAttack = true;
            NodeInstance = FindObjectOfType<Node>();
            Character_Effect.transform.localScale = 
            new Vector3(1.0f, Character_Effect.transform.localScale.y, Character_Effect.transform.localScale.z);

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
            if (ClickNode != ENodeType.Default)
            {// ClickNode가 중립이 아니라면(버튼을 클릭했다면)

                if (selectEnemy == null) { return; }

                if (ClickNode == selectEnemy.GetOwnNodes().Peek().nodeSheet.m_NodeType)
                {//다음에 나갈 노드타입과 비교(같은 NodeType을 클릭했다면)

                    GameManager.NotificationSystem.NodeHitSuccess?.Invoke();

                    // 1. 해당하는 enemy의 블럭 destroy
                    // 2. 캐릭터가 해당하는 enemy의 블럭위치로 이동 후 공격 애니메이션 재생 후 원래위치로 이동

                    TImebar.timebarImage.fillAmount += 0.1f; // 시간변수 +0.1f

                    CharacterBaseInstance.gameObject.transform.position = new Vector3(selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.x
                    , selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.y, 0.0f);

                    

                    RandAnim();
                    if (FirstAttack)
                    {// 최초 공격시만 발동되는 애니메이션
                        PlayerBaseInstance.PlayerAnim.SetTrigger("Atk_6");
                        FirstAttack = false;
                    }
                    RandEffect();
                    
                    if (selectEnemy == RightEnemy)
                    {
                        Right_Orc2_Anim.RightAnim.SetTrigger("Right_Damage");
                    }
                    else
                    {
                        Left_Orc2_Anim.LeftAnim.SetTrigger("Left_Damage");
                    }

                    //else
                    //{

                    //}
                    Vector3 targetPosition = selectEnemy.GetOwnNodes().Peek().gameObject.transform.position;
                    Destroy(selectEnemy.GetOwnNodes().Peek().gameObject);

                    selectEnemy.GetOwnNodes().Dequeue();
                    selectEnemy.Hit();

                    if (selectEnemy.GetOwnNodes().Count <= 0)
                    {
                        // 에너미 노드의 남아있는 갯수가 0보다 작거나 같다면
                        // 몬스터 제거 후 다시생성

                        //if (selectEnemy == RightEnemy)
                        //{
                        //    Right_Orc2_Anim.RightAnim.SetTrigger("Right_Die");
                        //}
                        //else
                        //{
                        //    Left_Orc2_Anim.LeftAnim.SetTrigger("Left_Damage");
                        //}
                        selectEnemy.Die();

                        if (MonsterDie)
                        {// 오른쪽 몬스터가 죽은상태라면
                            // MonsterDie(Bool)가 true상태라면 
                            Invoke("RightMonsterSpawn", 0.7f);
                            //RightMonsterSpawn();
                            // 스폰 후
                            MonsterDie = false;
                        }
                        else
                        {// MonsterDie가 false상태라면
                            Invoke("LeftMonsterSpawn", 0.7f);
                            //LeftMonsterSpawn();
                        }
                        //HandleSceneMonsterSpawned(SceneEnemyList[0]);

                        //if (SceneEnemyList.Count > 0)
                        //{
                        //    selectEnemy = SceneEnemyList[0];
                        //    selectEnemy.SetupAttackSliderUI(GameManager.UISystem.AttackSliderUI);
                        //}
                    }
                }
                else
                {
                    GameManager.NotificationSystem.NodeHitFail?.Invoke();

                    if (selectEnemy != null)
                    {
                        selectEnemy.Attack();
                    }

                    // 아니라면
                    // 1. 캐릭터 Hp --
                    // 2. 캐릭터 원래위치로 이동
                    // 3. Enemy의 캐릭터 공격 애니메이션 재생
                    // 4. 노드 다시 생성하는 함수 호출
                    if (selectEnemy == RightEnemy)
                    {
                        Test3Spawn.Instance.Spawn_RightNode(selectEnemy);
                    }
                    else if (selectEnemy == LeftEnemy)
                    {
                        Test3Spawn.Instance.SpawnLeft_Node(selectEnemy);
                    }
                }

                ClickNode = ENodeType.Default; // reset
                Debug.Log(ClickNode);
            }
        }

        void Attack()
        {
            ClickNode = ENodeType.Attack;
        }

        void SelectEnemy()
        {
            if (selectEnemy == RightEnemy)
            {
                if (selectEnemy.GetOwnNodes().Count==7)
                {// 몬스터에게 남아있는 노드가 7 이상이라면 (몬스터의 첫번째 노드만 셀렉애너미 버튼으로 타격가능)
                    selectEnemy = LeftEnemy;
                    TImebar.timebarImage.fillAmount += 0.1f; // 시간변수 +0.1f
                    CharacterBaseInstance.gameObject.transform.position = new Vector3(selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.x
                    , selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.y, 0.0f); // 몬스터의 첫번째 노드위치로 이동

                    PlayerBaseInstance.PlayerAnim.SetTrigger("Slide_Atk_1"); // 오크의 위치로 이동해 공격모션
                    Left_Orc2_Anim.LeftAnim.SetTrigger("Left_Damage"); 
                    Character_Effect.transform.localScale = new Vector3(-1.0f, Character_Effect.transform.localScale.y, Character_Effect.transform.localScale.z);
                    RandEffect();
                    //Vector3 targetPosition = selectEnemy.GetOwnNodes().Peek().gameObject.transform.position;

                    Destroy(selectEnemy.GetOwnNodes().Peek().gameObject);

                    selectEnemy.GetOwnNodes().Dequeue();

                    selectEnemy.Hit();

                    PlayerBaseInstance.PlayerTransform.localScale =
                    new Vector3(-1f, PlayerBaseInstance.PlayerTransform.localScale.y, PlayerBaseInstance.PlayerTransform.localScale.z);
                }
            }
            else if (selectEnemy == LeftEnemy)
            {
                if (selectEnemy.GetOwnNodes().Count == 7)
                {// 몬스터에게 남아있는 노드가 7 이상이라면 (몬스터의 첫번째 노드만 셀렉애너미 버튼으로 타격가능)
                    selectEnemy = RightEnemy;
                    TImebar.timebarImage.fillAmount += 0.1f; // 시간변수 +0.1f
                    CharacterBaseInstance.gameObject.transform.position = new Vector3(selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.x
                    , selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.y, 0.0f); // 몬스터의 첫번째 노드위치로 이동

                    PlayerBaseInstance.PlayerAnim.SetTrigger("Slide_Atk_1"); // 오크의 위치로 이동해 공격모션
                    Right_Orc2_Anim.RightAnim.SetTrigger("Right_Damage"); // 오크의 피격모션 재생
                    Character_Effect.transform.localScale = new Vector3(1.0f, Character_Effect.transform.localScale.y, Character_Effect.transform.localScale.z);
                    RandEffect();
                    //Vector3 targetPosition = selectEnemy.GetOwnNodes().Peek().gameObject.transform.position;

                    Destroy(selectEnemy.GetOwnNodes().Peek().gameObject);

                    selectEnemy.GetOwnNodes().Dequeue();

                    selectEnemy.Hit();

                    PlayerBaseInstance.PlayerTransform.localScale =
                    new Vector3(1f, PlayerBaseInstance.PlayerTransform.localScale.y, PlayerBaseInstance.PlayerTransform.localScale.z);
                }
            }
        }

        public void RandAnim()
        {
            int randAnim = Random.Range(0, 6);
            if (FirstAttack==false)
            {
                if (randAnim == 0)
                {
                    PlayerBaseInstance.PlayerAnim.SetTrigger("Atk_1"); // 오크의 위치로 이동해 공격모션
                }
                else if (randAnim == 1)
                {
                    PlayerBaseInstance.PlayerAnim.SetTrigger("Atk_2"); // 오크의 위치로 이동해 공격모션
                }
                else if (randAnim == 2)
                {
                    PlayerBaseInstance.PlayerAnim.SetTrigger("Atk_3"); // 오크의 위치로 이동해 공격모션
                }
                else if (randAnim == 3)
                {
                    PlayerBaseInstance.PlayerAnim.SetTrigger("Atk_4"); // 오크의 위치로 이동해 공격모션
                }
                else if (randAnim == 4)
                {
                    PlayerBaseInstance.PlayerAnim.SetTrigger("Atk_5"); // 오크의 위치로 이동해 공격모션
                }
                else
                {// 5
                    PlayerBaseInstance.PlayerAnim.SetTrigger("Atk_7"); // 오크의 위치로 이동해 공격모션
                }
                // * Atk_6애니메이션은 게임 최초시작시만 발동되는 애니메이션
            }

        }

        void RandEffect()
        {

            Vector3 targetPosition = selectEnemy.GetOwnNodes().Peek().gameObject.transform.position;
            Instantiate(Character_Effect, targetPosition, Quaternion.identity);
            //float randEffect = Random.Range(0f, 100f);
            //if (randEffect <= 20f)
            //{// randEffect : 20퍼센트 확률
            //    Vector3 targetPosition = selectEnemy.GetOwnNodes().Peek().gameObject.transform.position;
            //    Instantiate(Character_Effect, targetPosition, Quaternion.identity);
            //}
        }

        public void StartSpawn()
        {
            GameObject RightMonster = Right_Ork;
            GameObject RightSpawnMonster = Instantiate(RightMonster, new Vector3(4.0f, 0.72f, 0), Quaternion.identity);
            EnemyBase spawnEnemy = RightSpawnMonster.GetComponent<EnemyBase>();
            if (spawnEnemy != null)
            {
                //GameManager.NotificationSystem.SceneMonsterSpawned.Invoke(spawnEnemy); // 몬스터가 스폰되었음을 시스템에 알립니다.
                Test3Spawn.Instance.Spawn_RightNode(spawnEnemy);
            }
            RightEnemy = spawnEnemy; // 값 저장
            selectEnemy = spawnEnemy; // 오른쪽 몬스터가 디폴트값

            GameObject LefttMonster = Left_Ork;
            GameObject LeftSpawnMonster = Instantiate(LefttMonster, new Vector3(-4.0f, 0.72f, 0), Quaternion.identity);
            EnemyBase spawnEnemy2 = LeftSpawnMonster.GetComponent<EnemyBase>();
            if (spawnEnemy2 != null)
            {
                //GameManager.NotificationSystem.SceneMonsterSpawned.Invoke(spawnEnemy); // 몬스터가 스폰되었음을 시스템에 알립니다.
                Test3Spawn.Instance.SpawnLeft_Node(spawnEnemy2);
            }
            LeftEnemy = spawnEnemy2; // 값 저장
        }
        public void RightMonsterSpawn()
        {
            GameObject RightMonster = Right_Ork;
            GameObject RightSpawnMonster = Instantiate(RightMonster, new Vector3(4.0f, 0.72f, 0), Quaternion.identity);
            EnemyBase spawnEnemy = RightSpawnMonster.GetComponent<EnemyBase>();
            if (spawnEnemy != null)
            {
                //GameManager.NotificationSystem.SceneMonsterSpawned.Invoke(spawnEnemy); // 몬스터가 스폰되었음을 시스템에 알립니다.
                Test3Spawn.Instance.Spawn_RightNode(spawnEnemy);
            }
            selectEnemy = spawnEnemy;
            RightEnemy = spawnEnemy;
        }
        public void LeftMonsterSpawn()
        {
            GameObject LefttMonster = Left_Ork;
            GameObject LeftSpawnMonster = Instantiate(LefttMonster, new Vector3(-4.0f, 0.72f, 0), Quaternion.identity);
            EnemyBase spawnEnemy2 = LeftSpawnMonster.GetComponent<EnemyBase>();
            if (spawnEnemy2 != null)
            {
                //GameManager.NotificationSystem.SceneMonsterSpawned.Invoke(spawnEnemy); // 몬스터가 스폰되었음을 시스템에 알립니다.
                Test3Spawn.Instance.SpawnLeft_Node(spawnEnemy2);
            }
            selectEnemy = spawnEnemy2;
            LeftEnemy = spawnEnemy2;
        }
    }
}


