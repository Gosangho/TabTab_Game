using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TabTabs.NamChanwoo
{
    public class TutorialBattleSystem : MonoBehaviour
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
        public bool MonsterDie = false; // 어느 쪽 몬스터가 죽었는지 확인하는 bool형 변수
        public Node NodeInstance;
        public GameObject Character_Effect;
        bool FirstAttack; // 게임시작시 공격버튼으로 최초 한번만 재생되는 애니메이션 변수
        //public bool Right_TrainAttack; // 오른쪽 몬스터를 연속공격했는지 판단하는 변수
        //public bool Left_TrainAttack; // 왼쪽 몬스터를 연속공격했는지 판단하는 변수
        //public GameObject ReStartButton;
        public ScoreSystem ScoreSystemInstance;
        public GameObject ScoreTextObj;
        public TImebar TimebarInstance;
        public GameObject Player_AfterImage;
        public bool RightMonsterDie = false;
        public bool LeftMonsterDie = false;
        public DialogTest DialogTestInstance;
        bool TutorialRightMonsterDie = false;
        void Start()
        {
            ClickNode = ENodeType.Default;
            DialogTestInstance = FindObjectOfType<DialogTest>();
            CharacterBaseInstance = FindObjectOfType<CharacterBase>();
            PlayerBaseInstance = FindObjectOfType<PlayerBase>();
            AttackButton.onClick.AddListener(Attack);
            SelectEnemyButton.onClick.AddListener(SelectEnemy);
            StartSpawn();
            MonsterDie = false;
            //Right_TrainAttack = false;
            //Left_TrainAttack = false;
            FirstAttack = true;
            NodeInstance = FindObjectOfType<Node>();
            ScoreSystemInstance = FindObjectOfType<ScoreSystem>();
            TimebarInstance = FindObjectOfType<TImebar>();
            Character_Effect.transform.localScale =
            new Vector3(1.0f, Character_Effect.transform.localScale.y, Character_Effect.transform.localScale.z);
        }

        void Update()
        {
            if (ClickNode != ENodeType.Default)
            {// ClickNode가 중립이 아니라면 == (버튼을 클릭했다면)

                if (selectEnemy == null) { return; }

                if (ClickNode == selectEnemy.GetOwnNodes().Peek().nodeSheet.m_NodeType)
                {//다음에 나갈 노드타입과 비교(같은 NodeType을 클릭했다면)

                    GameManager.NotificationSystem.NodeHitSuccess?.Invoke();
                    ScoreSystemInstance.score += 1; // 공격성공시 Score +1
                                                    // 1. 해당하는 enemy의 블럭 destroy
                                                    // 2. 캐릭터가 해당하는 enemy의 블럭위치로 이동 후 공격 애니메이션 재생 후 원래위치로 이동

                    TImebar.timebarImage.fillAmount += 0.1f; // 시간변수 +0.1f

                    CharacterBaseInstance.gameObject.transform.position = new Vector3(selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.x
                    , selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.y, 0.0f);

                    Vector3 scorePosition = selectEnemy.GetOwnNodes().Peek().transform.position; // 노드의 위치를 가져옴
                    GameObject gameObject = Instantiate(ScoreTextObj, scorePosition, Quaternion.identity); // 노드위치에 생성

                    //if (Right_TrainAttack == true || Left_TrainAttack == true)
                    //{// 오른쪽 몬스터나 왼쪽 몬스터를 연속으로 공격했다면
                    // // 게임오버 -> 게임 다시시작
                    //    Debug.Log("게임오버");
                    //    ReStartButton.SetActive(true);
                    //    Time.timeScale = 0.0f; // 게임멈춤
                    //}
                    RandAttackAudio();
                    RandEnemyHitAudio();
                    RandAnim();
                    DialogTestInstance.FirstAttack = true;
                    if (FirstAttack)
                    {// 최초 공격시만 발동되는 애니메이션
                        PlayerBaseInstance.PlayerAnim.SetTrigger("Atk_6");
                        FirstAttack = false;
                        float RandAttackSound = Random.value; // 0~1사이의 무작위 값
                        if (RandAttackSound < 0.4f)
                        {
                            audioManager.Instance.SfxAudioPlay("Char_Attack1");
                        }
                        else if (RandAttackSound < 0.8f)
                        {
                            audioManager.Instance.SfxAudioPlay("Char_Attack2");
                        }
                        else
                        {
                            audioManager.Instance.SfxAudioPlay("Char_Spirit");
                        }
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

                    Vector3 targetPosition = selectEnemy.GetOwnNodes().Peek().gameObject.transform.position;
                    Destroy(selectEnemy.GetOwnNodes().Peek().gameObject);

                    selectEnemy.GetOwnNodes().Dequeue();
                    selectEnemy.Hit();

                    if (selectEnemy.GetOwnNodes().Count <= 0)
                    {
                        TimebarInstance.KillCount += 1;
                        audioManager.Instance.SfxAudioPlay_Enemy("Enemy_Dead");
                        if (selectEnemy == RightEnemy)
                        {
                            RightMonsterDie = true;
                            TutorialRightMonsterDie = true;
                        }
                        else
                        {
                            LeftMonsterDie = true;
                        }
                        Destroy(selectEnemy.gameObject);
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

            if (RightMonsterDie == true && LeftMonsterDie == true)
            {
                MonsterDie = true; // 튜토리얼 테스트에서 Wait for Until안에 들어갈 변수
            }
        }

        void Attack()
        {
            ClickNode = ENodeType.Attack;
        }

        void SelectEnemy()
        {
            if (selectEnemy == RightEnemy && RightMonsterDie)
            {// 현재 선택된 몬스터가 오른쪽 몬스터이고
                if (LeftEnemy.GetOwnNodes().Count == Test3Spawn.Instance.LeftAttackNum)
                {// 왼쪽몬스터에 생성된 노드의 총수가 같다면 == 몬스터의 첫번째 노드라면
                    ScoreSystemInstance.score += 1; // 공격성공시 Score +1
                    RandDashAttackAudio();
                    RandEnemyHitAudio();
                    //Right_TrainAttack = false;
                    DialogTestInstance.FirstAttack = true;

                    selectEnemy = LeftEnemy;

                    TImebar.timebarImage.fillAmount += 0.1f; // 시간변수 +0.1f

                    CharacterBaseInstance.gameObject.transform.position = new Vector3(selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.x
                    , selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.y, 0.0f); // 몬스터의 첫번째 노드위치로 이동

                    Vector3 scorePosition = selectEnemy.GetOwnNodes().Peek().transform.position; // 노드의 위치를 가져옴
                    GameObject ScoreTextobj = Instantiate(ScoreTextObj, scorePosition, Quaternion.identity); // 노드위치에 생성

                    GameObject PlayerAfterImage = Instantiate(Player_AfterImage, CharacterBaseInstance.gameObject.transform.position, Quaternion.identity);
                    SpriteRenderer spriteRenderer = PlayerAfterImage.GetComponent<SpriteRenderer>();
                    spriteRenderer.flipX = true;
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
                    if (selectEnemy.GetOwnNodes().Count <= 0)
                    {
                        audioManager.Instance.SfxAudioPlay_Enemy("Enemy_Dead");
                        if (selectEnemy == RightEnemy)
                        {
                            RightMonsterDie = true;
                        }
                        else
                        {
                            LeftMonsterDie = true;
                        }
                        Destroy(selectEnemy.gameObject);
                        TimebarInstance.KillCount += 1;
                    }
                }
            }
            else if (selectEnemy == LeftEnemy)
            {
                if (RightEnemy.GetOwnNodes().Count == Test3Spawn.Instance.RightAttackNum)
                {
                    ScoreSystemInstance.score += 1; // 공격성공시 Score +1
                    RandDashAttackAudio();
                    RandEnemyHitAudio();
                    //Left_TrainAttack = false;
                    DialogTestInstance.FirstAttack = true;

                    selectEnemy = RightEnemy;

                    TImebar.timebarImage.fillAmount += 0.1f; // 시간변수 +0.1f

                    CharacterBaseInstance.gameObject.transform.position = new Vector3(selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.x
                    , selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.y, 0.0f); // 몬스터의 첫번째 노드위치로 이동

                    Vector3 scorePosition = selectEnemy.GetOwnNodes().Peek().transform.position; // 노드의 위치를 가져옴
                    GameObject ScoreTextobj = Instantiate(ScoreTextObj, scorePosition, Quaternion.identity); // 노드위치에 생성

                    GameObject PlayerAfterImage = Instantiate(Player_AfterImage, CharacterBaseInstance.gameObject.transform.position, Quaternion.identity);
                    SpriteRenderer spriteRenderer = PlayerAfterImage.GetComponent<SpriteRenderer>();
                    spriteRenderer.flipX = false;
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

                    if (selectEnemy.GetOwnNodes().Count <= 0)
                    {
                        audioManager.Instance.SfxAudioPlay_Enemy("Enemy_Dead");
                        if (selectEnemy == RightEnemy)
                        {
                            RightMonsterDie = true;
                        }
                        else
                        {
                            LeftMonsterDie = true;
                        }
                        Destroy(selectEnemy.gameObject);
                       TimebarInstance.KillCount += 1;  
                    }
                }
            }
        }

        public void RandAnim()
        {
            int randAnim = Random.Range(0, 6);
            if (FirstAttack == false)
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

            //Vector3 targetPosition = selectEnemy.GetOwnNodes().Peek().gameObject.transform.position;
            //Instantiate(Character_Effect, targetPosition, Quaternion.identity);
            float randEffect = Random.Range(0f, 100f);
            if (randEffect <= 20f)
            {// randEffect : 20퍼센트 확률
                Vector3 targetPosition = selectEnemy.GetOwnNodes().Peek().gameObject.transform.position;
                Instantiate(Character_Effect, targetPosition, Quaternion.identity);
            }
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
        public void RandAttackAudio()
        {
            float RandAttackSound = Random.value; // 0~1사이의 무작위 값
            if (RandAttackSound < 0.4f)
            {
                audioManager.Instance.SfxAudioPlay("Char_Attack1");
            }
            else if (RandAttackSound < 0.8f)
            {
                audioManager.Instance.SfxAudioPlay("Char_Attack2");
            }
            else
            {
                RandEffect();
                audioManager.Instance.SfxAudioPlay("Char_Spirit");
            }
        }
        public void RandDashAttackAudio()
        {
            int Ran = Random.Range(0, 2);
            if (Ran == 0)
            {
                audioManager.Instance.SfxAudioPlay("Char_Dash1");
            }
            else
            {
                audioManager.Instance.SfxAudioPlay("Char_Dash2");
            }
        }
        public void RandEnemyHitAudio()
        {
            int Ran = Random.Range(0, 2);
            if (Ran == 0)
            {
                audioManager.Instance.SfxAudioPlay_Enemy("Enemy_Hit");
            }
            else
            {
                audioManager.Instance.SfxAudioPlay_Enemy("Enemy_Dead");
            }
        }
    }
}


