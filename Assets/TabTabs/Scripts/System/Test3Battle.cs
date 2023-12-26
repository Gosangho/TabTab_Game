using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using TMPro;


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
        public bool Right_MonsterDie; // 오른쪽 몬스터가 죽었을경우 true로 변경 -> 리젠 후 false로 변경
        public bool Left_MonsterDie;
        public Node NodeInstance;
        public GameObject Character_Effect;
        bool FirstAttack; // 게임시작시 공격버튼으로 최초 한번만 재생되는 애니메이션 변수
        public bool Right_TrainAttack; // 오른쪽 몬스터를 연속공격했는지 판단하는 변수
        public bool Left_TrainAttack; // 왼쪽 몬스터를 연속공격했는지 판단하는 변수
        public GameObject ReStartButton;
        public ScoreSystem ScoreSystemInstance;
        public GameObject ScoreTextObj;
        public TImebar TimebarInstance;
        public bool FirstDashAttack;
        public GameObject swordGirl1_AfterImage; // 대쉬 버튼을 눌렀을 경우 캐릭터의 잔상이 표시되는 애니메이션
        public GameObject swordGirl2_AfterImage;
        public GameObject leon_AfterImage;
        public GameObject swordGirl1_FirstAttack;
        public GameObject swordGirl2_FirstAttack;
        public GameObject leon_FirstAttack;
        public GameObject swordGirl1;
        public GameObject swordGirl2;
        public GameObject leon;
        public bool ReStart;
        private bool restartButtonActivated = false;
        public SelectCharacter selectCharacterInstance;

        void Start()
        {
            selectCharacterInstance = FindObjectOfType<SelectCharacter>();

            if (SelectCharacter.swordGirl1)
            {
                swordGirl1.SetActive(true);
            }
            else if (SelectCharacter.swordGirl2)
            {
                swordGirl2.SetActive(true);
            }
            else
            {
                leon.SetActive(true);
            }

            ClickNode = ENodeType.Default;
            GameObject character2Object = GameObject.FindGameObjectWithTag("Player");
            if (character2Object !=null)
            {
                CharacterBaseInstance = character2Object.GetComponent<CharacterBase>();
                PlayerBaseInstance = character2Object.GetComponent<PlayerBase>();
            }
            AttackButton.onClick.AddListener(Attack);
            SelectEnemyButton.onClick.AddListener(SelectEnemy);
            StartSpawn();
            Right_MonsterDie = false;
            Left_MonsterDie = false;
            Right_TrainAttack = false;
            Left_TrainAttack = false;
            FirstAttack = true;
            FirstDashAttack = true;
            ReStart = false;
            NodeInstance = FindObjectOfType<Node>();
            ScoreSystemInstance = FindObjectOfType<ScoreSystem>();
            TimebarInstance = FindObjectOfType<TImebar>();
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
                    ScoreSystemInstance.score += 1; // 공격성공시 Score +1
                    // 1. 해당하는 enemy의 블럭 destroy
                    // 2. 캐릭터가 해당하는 enemy의 블럭위치로 이동 후 공격 애니메이션 재생 후 원래위치로 이동
                    if (SelectCharacter.swordGirl1)
                    {
                        if (ScoreSystem.swordGirl1BestScore <= ScoreSystemInstance.score)
                        {
                            ScoreSystem.swordGirl1BestScore = ScoreSystemInstance.score;
                        }
                    }
                    else if (SelectCharacter.swordGirl2)
                    {
                        if (ScoreSystem.swordGirl2BestScore <= ScoreSystemInstance.score)
                        {
                            ScoreSystem.swordGirl2BestScore = ScoreSystemInstance.score;
                        }
                    }
                    else
                    {
                        if (ScoreSystem.leonBestScore <= ScoreSystemInstance.score)
                        {
                            ScoreSystem.leonBestScore = ScoreSystemInstance.score;
                        }
                    }

                    TImebar.timebarImage.fillAmount += 0.1f; // 시간변수 +0.1f

                    CharacterBaseInstance.gameObject.transform.position = new Vector3(selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.x
                    , selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.y, 0.0f);

                    Vector3 scorePosition = selectEnemy.GetOwnNodes().Peek().transform.position; // 노드의 위치를 가져옴
                    GameObject gameObject = Instantiate(ScoreTextObj, scorePosition, Quaternion.identity); // 노드위치에 생성

                    if (Right_TrainAttack == true || Left_TrainAttack == true)
                    {// 오른쪽이나 왼쪽 몬스터를 죽이고 다시 젠 된상태에서 공격버튼을 누를경우
                        // 게임오버 -> 게임 다시시작
                        ReStartButton.SetActive(true);
                        Time.timeScale = 0.0f; // 게임멈춤
                        ReStart = true;
                    }

                    RandAnim();
                    if (FirstAttack)
                    {// 최초 공격시만 발동되는 애니메이션
                        if (SelectCharacter.swordGirl1)
                        {
                            GameObject swordGirl1FirstAttack =
                            Instantiate(swordGirl1_FirstAttack, CharacterBaseInstance.gameObject.transform.position, Quaternion.identity);
                        }
                        else if (SelectCharacter.swordGirl2)
                        {
                            GameObject swordGirl2FirstAttack =
                            Instantiate(swordGirl2_FirstAttack, CharacterBaseInstance.gameObject.transform.position, Quaternion.identity);
                        }
                        else
                        {
                            GameObject leonFirstAttack =
                            Instantiate(leon_FirstAttack, CharacterBaseInstance.gameObject.transform.position, Quaternion.identity);
                        }

                        //PlayerBaseInstance.PlayerAnim.SetTrigger("Atk_6");

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
                    //RandEffect();
                    RandAttackAudio();
                    RandEnemyHitAudio();
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
                        audioManager.Instance.SfxAudioPlay_Enemy("Enemy_Dead");
                        TimebarInstance.KillCount += 1;

                        if (SelectCharacter.swordGirl1)
                        {
                            ScoreSystem.swordGirl1KillCount++;
                        }
                        else if (SelectCharacter.swordGirl2)
                        {
                            ScoreSystem.swordGirl2KillCount++;
                        }
                        else
                        {
                            ScoreSystem.leonKillCount++;
                        }

                        if (Right_MonsterDie)
                        {// 오른쪽 몬스터가 죽은상태라면
                            //Invoke("RightMonsterSpawn", 0.7f);
                            RightMonsterSpawn();
                            // 스폰 후
                            Right_MonsterDie = false;
                        }
                        else
                        {// 왼쪽 몬스터가 죽은상태라면
                            //Invoke("LeftMonsterSpawn", 0.7f);
                            LeftMonsterSpawn();
                            // 스폰 후
                            Left_MonsterDie = false;
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

            if (TimebarInstance.TimebarImagefillAmount <= 0 && !restartButtonActivated)
            {
                Left_Orc2_Anim.LeftAnim.SetTrigger("Left_Attack");
                Right_Orc2_Anim.RightAnim.SetTrigger("Right_Attack");
                ReStartButton.SetActive(true);
                ReStart = true;
                restartButtonActivated = true;
                Time.timeScale = 0.0f; // 게임멈춤
            }
        }

        void Attack()
        {
            ClickNode = ENodeType.Attack;
            if (Time.timeScale == 0.0f)
            {
                ReStart = true;
                ClickNode = ENodeType.Default;
            }
        }

        void SelectEnemy()
        {
            if (selectEnemy == RightEnemy && ReStart == false)
            {// 현재 선택된 몬스터가 오른쪽 몬스터이고
                if (LeftEnemy.GetOwnNodes().Count == Test3Spawn.Instance.LeftAttackNum)
                {// 왼쪽몬스터에 생성된 노드의 총수가 같다면 == 몬스터의 첫번째 노드라면

                    if (FirstDashAttack || FirstAttack)
                    {// FirstAttack : 게임시작시 첫 공격이 대쉬버튼일 경우 GameOver
                        ReStartButton.SetActive(true);
                        Time.timeScale = 0.0f; // 게임멈춤
                        ReStart = true;
                    }

                    ScoreSystemInstance.score += 1; // 공격성공시 Score +1

                    if (SelectCharacter.swordGirl1)
                    {
                        if (ScoreSystem.swordGirl1BestScore <= ScoreSystemInstance.score)
                        {
                            ScoreSystem.swordGirl1BestScore = ScoreSystemInstance.score;
                        }
                    }
                    else if (SelectCharacter.swordGirl2)
                    {
                        if (ScoreSystem.swordGirl2BestScore <= ScoreSystemInstance.score)
                        {
                            ScoreSystem.swordGirl2BestScore = ScoreSystemInstance.score;
                        }
                    }
                    else
                    {
                        if (ScoreSystem.leonBestScore <= ScoreSystemInstance.score)
                        {
                            ScoreSystem.leonBestScore = ScoreSystemInstance.score;
                        }
                    }

                    RandDashAttackAudio();
                    RandEnemyHitAudio();
                    Right_TrainAttack = false;

                    selectEnemy = LeftEnemy;

                    TImebar.timebarImage.fillAmount += 0.1f; // 시간변수 +0.1f

                    CharacterBaseInstance.gameObject.transform.position = new Vector3(selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.x
                    , selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.y, 0.0f); // 몬스터의 첫번째 노드위치로 이동
                    
                    Vector3 scorePosition = selectEnemy.GetOwnNodes().Peek().transform.position; // 노드의 위치를 가져옴
                    GameObject ScoreTextobj = Instantiate(ScoreTextObj, scorePosition, Quaternion.identity); // 노드위치에 생성
                    
                    if (SelectCharacter.swordGirl1)
                    {
                        GameObject swordGirl1AfterImage =
                        Instantiate(swordGirl1_AfterImage, CharacterBaseInstance.gameObject.transform.position, Quaternion.identity);
                        SpriteRenderer spriteRenderer = swordGirl1_AfterImage.GetComponent<SpriteRenderer>();
                        spriteRenderer.flipX = false;
                    }
                    else if (SelectCharacter.swordGirl2)
                    {
                        GameObject swordGirl2AfterImage =
                        Instantiate(swordGirl2_AfterImage, CharacterBaseInstance.gameObject.transform.position, Quaternion.identity);
                        SpriteRenderer spriteRenderer = swordGirl2_AfterImage.GetComponent<SpriteRenderer>();
                        spriteRenderer.flipX = false;
                    }
                    else
                    {
                        GameObject leonAfterImage =
                        Instantiate(leon_AfterImage, CharacterBaseInstance.gameObject.transform.position, Quaternion.identity);
                        SpriteRenderer spriteRenderer = leon_AfterImage.GetComponent<SpriteRenderer>();
                        spriteRenderer.flipX = false;
                    }
                    //GameObject PlayerAfterImage = Instantiate(swordGirl1_AfterImage, CharacterBaseInstance.gameObject.transform.position, Quaternion.identity);
                    //SpriteRenderer spriteRenderer = PlayerAfterImage.GetComponent<SpriteRenderer>();
                    //spriteRenderer.flipX = true;

                    //PlayerBaseInstance.PlayerAnim.SetTrigger("Slide_Atk_1"); // 오크의 위치로 이동해 공격모션
                    Left_Orc2_Anim.LeftAnim.SetTrigger("Left_Damage");

                    Character_Effect.transform.localScale = new Vector3(-1.0f, Character_Effect.transform.localScale.y, Character_Effect.transform.localScale.z);
                    
                    RandEffect();
                    //Vector3 targetPosition = selectEnemy.GetOwnNodes().Peek().gameObject.transform.position;
                    Destroy(selectEnemy.GetOwnNodes().Peek().gameObject);

                    selectEnemy.GetOwnNodes().Dequeue();

                    selectEnemy.Hit();

                    PlayerBaseInstance.PlayerTransform.localScale =
                    new Vector3(-1f, PlayerBaseInstance.PlayerTransform.localScale.y, PlayerBaseInstance.PlayerTransform.localScale.z);

                    FirstDashAttack = true;
                    
                    if (selectEnemy.GetOwnNodes().Count <= 0)
                    {

                        FirstDashAttack = false;
                        selectEnemy.Die();
                        audioManager.Instance.SfxAudioPlay_Enemy("Enemy_Dead");

                        if (SelectCharacter.swordGirl1)
                        {
                            ScoreSystem.swordGirl1KillCount++;
                        }
                        else if (SelectCharacter.swordGirl2)
                        {
                            ScoreSystem.swordGirl2KillCount++;
                        }
                        else
                        {
                            ScoreSystem.leonKillCount++;
                        }

                        TimebarInstance.KillCount += 1;

                        if (Right_MonsterDie)
                        {// 오른쪽 몬스터가 죽었다면
                            RightMonsterSpawn();

                            Right_MonsterDie = false;
                        }
                        else
                        {
                            LeftMonsterSpawn();
                        }
                    }
                }
            }
            else if (selectEnemy == LeftEnemy && ReStart == false)
            {
                if (RightEnemy.GetOwnNodes().Count == Test3Spawn.Instance.RightAttackNum)
                {

                    if (FirstDashAttack || FirstAttack)
                    {// FirstAttack : 게임시작시 첫 공격이 대쉬버튼일 경우 GameOver
                        ReStartButton.SetActive(true);
                        Time.timeScale = 0.0f; // 게임멈춤
                        ReStart = true;
                    }

                    ScoreSystemInstance.score += 1; // 공격성공시 Score +1

                    if (SelectCharacter.swordGirl1)
                    {
                        if (ScoreSystem.swordGirl1BestScore <= ScoreSystemInstance.score)
                        {
                            ScoreSystem.swordGirl1BestScore = ScoreSystemInstance.score;
                        }
                    }
                    else if (SelectCharacter.swordGirl2)
                    {
                        if (ScoreSystem.swordGirl2BestScore <= ScoreSystemInstance.score)
                        {
                            ScoreSystem.swordGirl2BestScore = ScoreSystemInstance.score;
                        }
                    }
                    else
                    {
                        if (ScoreSystem.leonBestScore <= ScoreSystemInstance.score)
                        {
                            ScoreSystem.leonBestScore = ScoreSystemInstance.score;
                        }
                    }

                    RandDashAttackAudio();
                    RandEnemyHitAudio();
                    Left_TrainAttack = false;

                    selectEnemy = RightEnemy;

                    TImebar.timebarImage.fillAmount += 0.1f; // 시간변수 +0.1f
                    
                    CharacterBaseInstance.gameObject.transform.position = new Vector3(selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.x
                    , selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.y, 0.0f); // 몬스터의 첫번째 노드위치로 이동

                    Vector3 scorePosition = selectEnemy.GetOwnNodes().Peek().transform.position; // 노드의 위치를 가져옴
                    GameObject ScoreTextobj = Instantiate(ScoreTextObj, scorePosition, Quaternion.identity); // 노드위치에 생성

                    if (SelectCharacter.swordGirl1)
                    {
                        GameObject swordGirl1AfterImage =
                        Instantiate(swordGirl1_AfterImage, CharacterBaseInstance.gameObject.transform.position, Quaternion.identity);
                        SpriteRenderer spriteRenderer = swordGirl1_AfterImage.GetComponent<SpriteRenderer>();
                        spriteRenderer.flipX = true;
                    }
                    else if (SelectCharacter.swordGirl2)
                    {
                        GameObject swordGirl2AfterImage =
                        Instantiate(swordGirl2_AfterImage, CharacterBaseInstance.gameObject.transform.position, Quaternion.identity);
                        SpriteRenderer spriteRenderer = swordGirl2_AfterImage.GetComponent<SpriteRenderer>();
                        spriteRenderer.flipX = true;
                    }
                    else
                    {
                        GameObject leonAfterImage =
                        Instantiate(leon_AfterImage, CharacterBaseInstance.gameObject.transform.position, Quaternion.identity);
                        SpriteRenderer spriteRenderer = leon_AfterImage.GetComponent<SpriteRenderer>();
                        spriteRenderer.flipX = true;
                    }

                    //GameObject PlayerAfterImage = Instantiate(swordGirl1_AfterImage, CharacterBaseInstance.gameObject.transform.position, Quaternion.identity);
                    //SpriteRenderer spriteRenderer = PlayerAfterImage.GetComponent<SpriteRenderer>();
                    //spriteRenderer.flipX = false;
                    //PlayerBaseInstance.PlayerAnim.SetTrigger("Slide_Atk_1"); // 오크의 위치로 이동해 공격모션

                    Right_Orc2_Anim.RightAnim.SetTrigger("Right_Damage"); // 오크의 피격모션 재생

                    Character_Effect.transform.localScale = new Vector3(1.0f, Character_Effect.transform.localScale.y, Character_Effect.transform.localScale.z);
                    
                    RandEffect();
                    //Vector3 targetPosition = selectEnemy.GetOwnNodes().Peek().gameObject.transform.position;

                    Destroy(selectEnemy.GetOwnNodes().Peek().gameObject);

                    selectEnemy.GetOwnNodes().Dequeue();

                    selectEnemy.Hit();

                    PlayerBaseInstance.PlayerTransform.localScale =
                    new Vector3(1f, PlayerBaseInstance.PlayerTransform.localScale.y, PlayerBaseInstance.PlayerTransform.localScale.z);

                    FirstDashAttack = true;
                    
                    if (selectEnemy.GetOwnNodes().Count <= 0)
                    {

                        FirstDashAttack = false;
                        selectEnemy.Die();
                        audioManager.Instance.SfxAudioPlay_Enemy("Enemy_Dead");
                        TimebarInstance.KillCount += 1;

                        if (SelectCharacter.swordGirl1)
                        {
                            ScoreSystem.swordGirl1KillCount++;
                        }
                        else if (SelectCharacter.swordGirl2)
                        {
                            ScoreSystem.swordGirl2KillCount++;
                        }
                        else
                        {
                            ScoreSystem.leonKillCount++;
                        }

                        if (Right_MonsterDie)
                        {
                            RightMonsterSpawn();
                            Right_MonsterDie = false;
                        }
                        else
                        {
                            LeftMonsterSpawn();
                        }
                    }
                }
            }
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
            //float randEffect = Random.Range(0f, 100f);
            //if (randEffect <= 20f)
            //{// randEffect : 20퍼센트 확률
            Vector3 targetPosition = selectEnemy.GetOwnNodes().Peek().gameObject.transform.position;
            Instantiate(Character_Effect, targetPosition, Quaternion.identity);
            
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


