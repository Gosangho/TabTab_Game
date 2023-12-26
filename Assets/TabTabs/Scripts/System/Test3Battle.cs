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
        // BattleSystem���� ������ �͵�
        // 1. ��������
        // 2. Timebar�� ���ʹ��� ü�� --
        // NodeSheetŬ������ m_NodeType;(����Ű) => ������ ������ ��
        // SpawnSystemŬ������ SpawnNode(GameObject enemy)�Լ� => ��Ŭ��
        // EnemyBaseŬ������ m_nodeQueue����(Queue ���) => GetownNodes�Լ� ������ ��
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
        public bool Right_MonsterDie; // ������ ���Ͱ� �׾������ true�� ���� -> ���� �� false�� ����
        public bool Left_MonsterDie;
        public Node NodeInstance;
        public GameObject Character_Effect;
        bool FirstAttack; // ���ӽ��۽� ���ݹ�ư���� ���� �ѹ��� ����Ǵ� �ִϸ��̼� ����
        public bool Right_TrainAttack; // ������ ���͸� ���Ӱ����ߴ��� �Ǵ��ϴ� ����
        public bool Left_TrainAttack; // ���� ���͸� ���Ӱ����ߴ��� �Ǵ��ϴ� ����
        public GameObject ReStartButton;
        public ScoreSystem ScoreSystemInstance;
        public GameObject ScoreTextObj;
        public TImebar TimebarInstance;
        public bool FirstDashAttack;
        public GameObject swordGirl1_AfterImage; // �뽬 ��ư�� ������ ��� ĳ������ �ܻ��� ǥ�õǴ� �ִϸ��̼�
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
            {// ClickNode�� �߸��� �ƴ϶��(��ư�� Ŭ���ߴٸ�)

                if (selectEnemy == null) { return; }

                if (ClickNode == selectEnemy.GetOwnNodes().Peek().nodeSheet.m_NodeType)
                {//������ ���� ���Ÿ�԰� ��(���� NodeType�� Ŭ���ߴٸ�)

                    GameManager.NotificationSystem.NodeHitSuccess?.Invoke();
                    ScoreSystemInstance.score += 1; // ���ݼ����� Score +1
                    // 1. �ش��ϴ� enemy�� �� destroy
                    // 2. ĳ���Ͱ� �ش��ϴ� enemy�� ����ġ�� �̵� �� ���� �ִϸ��̼� ��� �� ������ġ�� �̵�
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

                    TImebar.timebarImage.fillAmount += 0.1f; // �ð����� +0.1f

                    CharacterBaseInstance.gameObject.transform.position = new Vector3(selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.x
                    , selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.y, 0.0f);

                    Vector3 scorePosition = selectEnemy.GetOwnNodes().Peek().transform.position; // ����� ��ġ�� ������
                    GameObject gameObject = Instantiate(ScoreTextObj, scorePosition, Quaternion.identity); // �����ġ�� ����

                    if (Right_TrainAttack == true || Left_TrainAttack == true)
                    {// �������̳� ���� ���͸� ���̰� �ٽ� �� �Ȼ��¿��� ���ݹ�ư�� �������
                        // ���ӿ��� -> ���� �ٽý���
                        ReStartButton.SetActive(true);
                        Time.timeScale = 0.0f; // ���Ӹ���
                        ReStart = true;
                    }

                    RandAnim();
                    if (FirstAttack)
                    {// ���� ���ݽø� �ߵ��Ǵ� �ִϸ��̼�
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
                        float RandAttackSound = Random.value; // 0~1������ ������ ��
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
                        // ���ʹ� ����� �����ִ� ������ 0���� �۰ų� ���ٸ�
                        // ���� ���� �� �ٽû���

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
                        {// ������ ���Ͱ� �������¶��
                            //Invoke("RightMonsterSpawn", 0.7f);
                            RightMonsterSpawn();
                            // ���� ��
                            Right_MonsterDie = false;
                        }
                        else
                        {// ���� ���Ͱ� �������¶��
                            //Invoke("LeftMonsterSpawn", 0.7f);
                            LeftMonsterSpawn();
                            // ���� ��
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

                    // �ƴ϶��
                    // 1. ĳ���� Hp --
                    // 2. ĳ���� ������ġ�� �̵�
                    // 3. Enemy�� ĳ���� ���� �ִϸ��̼� ���
                    // 4. ��� �ٽ� �����ϴ� �Լ� ȣ��
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
                Time.timeScale = 0.0f; // ���Ӹ���
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
            {// ���� ���õ� ���Ͱ� ������ �����̰�
                if (LeftEnemy.GetOwnNodes().Count == Test3Spawn.Instance.LeftAttackNum)
                {// ���ʸ��Ϳ� ������ ����� �Ѽ��� ���ٸ� == ������ ù��° �����

                    if (FirstDashAttack || FirstAttack)
                    {// FirstAttack : ���ӽ��۽� ù ������ �뽬��ư�� ��� GameOver
                        ReStartButton.SetActive(true);
                        Time.timeScale = 0.0f; // ���Ӹ���
                        ReStart = true;
                    }

                    ScoreSystemInstance.score += 1; // ���ݼ����� Score +1

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

                    TImebar.timebarImage.fillAmount += 0.1f; // �ð����� +0.1f

                    CharacterBaseInstance.gameObject.transform.position = new Vector3(selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.x
                    , selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.y, 0.0f); // ������ ù��° �����ġ�� �̵�
                    
                    Vector3 scorePosition = selectEnemy.GetOwnNodes().Peek().transform.position; // ����� ��ġ�� ������
                    GameObject ScoreTextobj = Instantiate(ScoreTextObj, scorePosition, Quaternion.identity); // �����ġ�� ����
                    
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

                    //PlayerBaseInstance.PlayerAnim.SetTrigger("Slide_Atk_1"); // ��ũ�� ��ġ�� �̵��� ���ݸ��
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
                        {// ������ ���Ͱ� �׾��ٸ�
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
                    {// FirstAttack : ���ӽ��۽� ù ������ �뽬��ư�� ��� GameOver
                        ReStartButton.SetActive(true);
                        Time.timeScale = 0.0f; // ���Ӹ���
                        ReStart = true;
                    }

                    ScoreSystemInstance.score += 1; // ���ݼ����� Score +1

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

                    TImebar.timebarImage.fillAmount += 0.1f; // �ð����� +0.1f
                    
                    CharacterBaseInstance.gameObject.transform.position = new Vector3(selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.x
                    , selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.y, 0.0f); // ������ ù��° �����ġ�� �̵�

                    Vector3 scorePosition = selectEnemy.GetOwnNodes().Peek().transform.position; // ����� ��ġ�� ������
                    GameObject ScoreTextobj = Instantiate(ScoreTextObj, scorePosition, Quaternion.identity); // �����ġ�� ����

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
                    //PlayerBaseInstance.PlayerAnim.SetTrigger("Slide_Atk_1"); // ��ũ�� ��ġ�� �̵��� ���ݸ��

                    Right_Orc2_Anim.RightAnim.SetTrigger("Right_Damage"); // ��ũ�� �ǰݸ�� ���

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
            float RandAttackSound = Random.value; // 0~1������ ������ ��
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
                    PlayerBaseInstance.PlayerAnim.SetTrigger("Atk_1"); // ��ũ�� ��ġ�� �̵��� ���ݸ��
                }
                else if (randAnim == 1)
                {
                    PlayerBaseInstance.PlayerAnim.SetTrigger("Atk_2"); // ��ũ�� ��ġ�� �̵��� ���ݸ��
                }
                else if (randAnim == 2)
                {
                    PlayerBaseInstance.PlayerAnim.SetTrigger("Atk_3"); // ��ũ�� ��ġ�� �̵��� ���ݸ��
                }
                else if (randAnim == 3)
                {
                    PlayerBaseInstance.PlayerAnim.SetTrigger("Atk_4"); // ��ũ�� ��ġ�� �̵��� ���ݸ��
                }
                else if (randAnim == 4)
                {
                    PlayerBaseInstance.PlayerAnim.SetTrigger("Atk_5"); // ��ũ�� ��ġ�� �̵��� ���ݸ��
                }
                else
                {// 5
                    PlayerBaseInstance.PlayerAnim.SetTrigger("Atk_7"); // ��ũ�� ��ġ�� �̵��� ���ݸ��
                }
                // * Atk_6�ִϸ��̼��� ���� ���ʽ��۽ø� �ߵ��Ǵ� �ִϸ��̼�
            }

        }

        void RandEffect()
        {
            //float randEffect = Random.Range(0f, 100f);
            //if (randEffect <= 20f)
            //{// randEffect : 20�ۼ�Ʈ Ȯ��
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
                //GameManager.NotificationSystem.SceneMonsterSpawned.Invoke(spawnEnemy); // ���Ͱ� �����Ǿ����� �ý��ۿ� �˸��ϴ�.
                Test3Spawn.Instance.Spawn_RightNode(spawnEnemy);
            }
            RightEnemy = spawnEnemy; // �� ����
            selectEnemy = spawnEnemy; // ������ ���Ͱ� ����Ʈ��

            GameObject LefttMonster = Left_Ork;
            GameObject LeftSpawnMonster = Instantiate(LefttMonster, new Vector3(-4.0f, 0.72f, 0), Quaternion.identity);
            EnemyBase spawnEnemy2 = LeftSpawnMonster.GetComponent<EnemyBase>();
            if (spawnEnemy2 != null)
            {
                //GameManager.NotificationSystem.SceneMonsterSpawned.Invoke(spawnEnemy); // ���Ͱ� �����Ǿ����� �ý��ۿ� �˸��ϴ�.
                Test3Spawn.Instance.SpawnLeft_Node(spawnEnemy2);
            }
            LeftEnemy = spawnEnemy2; // �� ����
        }
        public void RightMonsterSpawn()
        {
            GameObject RightMonster = Right_Ork;
            GameObject RightSpawnMonster = Instantiate(RightMonster, new Vector3(4.0f, 0.72f, 0), Quaternion.identity);
            EnemyBase spawnEnemy = RightSpawnMonster.GetComponent<EnemyBase>();
            if (spawnEnemy != null)
            {
                //GameManager.NotificationSystem.SceneMonsterSpawned.Invoke(spawnEnemy); // ���Ͱ� �����Ǿ����� �ý��ۿ� �˸��ϴ�.
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
                //GameManager.NotificationSystem.SceneMonsterSpawned.Invoke(spawnEnemy); // ���Ͱ� �����Ǿ����� �ý��ۿ� �˸��ϴ�.
                Test3Spawn.Instance.SpawnLeft_Node(spawnEnemy2);
            }
            selectEnemy = spawnEnemy2;
            LeftEnemy = spawnEnemy2;
        }
    }
}


