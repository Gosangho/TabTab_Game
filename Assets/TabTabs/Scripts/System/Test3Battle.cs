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
        public bool FirstAttack; // ���ӽ��۽� ���ݹ�ư���� ���� �ѹ��� ����Ǵ� �ִϸ��̼� ����
        public bool Right_TrainAttack; // ������ ���͸� ���Ӱ����ߴ��� �Ǵ��ϴ� ����
        public bool Left_TrainAttack; // ���� ���͸� ���Ӱ����ߴ��� �Ǵ��ϴ� ����
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
        public GameObject reStartObj;
        public SelectCharacter selectCharacterInstance;
        public bool playerDie = false;
        public GameObject resultObj;
        public Button continue_Button;
        public bool repetition;
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
            if (character2Object != null)
            {
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
            repetition = false;
            NodeInstance = FindObjectOfType<Node>();
            ScoreSystemInstance = FindObjectOfType<ScoreSystem>();
            TimebarInstance = FindObjectOfType<TImebar>();
            EffectInit();
            ContinueButton.continueButtonClick = false;
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

                    PlayerBaseInstance.gameObject.transform.position = new Vector3(selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.x
                    , selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.y, 0.0f);

                    Vector3 scorePosition = selectEnemy.GetOwnNodes().Peek().transform.position; // ����� ��ġ�� ������
                    GameObject gameObject = Instantiate(ScoreTextObj, scorePosition, Quaternion.identity); // �����ġ�� ����

                    if (Right_TrainAttack == true || Left_TrainAttack == true)
                    {// �������̳� ���� ���͸� ���̰� �ٽ� �� �Ȼ��¿��� ���ݹ�ư�� �������(GameOver)
                        // ���ӿ��� -> ���� �ٽý���
                        //Time.timeScale = 0.0f; // ���Ӹ���
                        playerDie = true;
                        PlayerBase.PlayerAnim.SetTrigger("Die");
                        resultObj.gameObject.SetActive(true);
                        continue_Button.gameObject.SetActive(true);
                        reStartObj.gameObject.SetActive(true);

                        if (ContinueButton.continueButtonClick == true)
                        {
                            continue_Button.gameObject.SetActive(false);
                        }
                        else
                        {
                            continue_Button.gameObject.SetActive(true);
                        }
                    }

                    RandAnim();
                    if (FirstAttack)
                    {// ���� ���ݽø� �ߵ��Ǵ� �ִϸ��̼�
                        if (SelectCharacter.swordGirl1)
                        {
                            GameObject swordGirl1FirstAttack =
                            Instantiate(swordGirl1_FirstAttack, PlayerBaseInstance.gameObject.transform.position, Quaternion.identity);
                        }
                        else if (SelectCharacter.swordGirl2)
                        {
                            GameObject swordGirl2FirstAttack =
                            Instantiate(swordGirl2_FirstAttack, PlayerBaseInstance.gameObject.transform.position, Quaternion.identity);
                        }
                        else
                        {
                            GameObject leonFirstAttack =
                            Instantiate(leon_FirstAttack, PlayerBaseInstance.gameObject.transform.position, Quaternion.identity);
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
                        float rand = Random.Range(0f, 1f);
                        if (rand <= 0.05f)
                        {// 5% Ȯ���� 1��� �߰�(���Ͱ� ����ҽ�)
                            DataManager.Instance.playerData.Gold += 1;
                        }
                        // �߰� 10��� Ȯ�� ���
                        else if (rand <= 0.0005f)
                        {// 0.05% Ȯ���� 10��带 �߰�(���Ͱ� ����ҽ�)
                            DataManager.Instance.playerData.Gold += 10;
                        }

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
                            DataManager.Instance.swordGirl1.totalKillScore++;
                        }
                        else if (SelectCharacter.swordGirl2)
                        {
                            DataManager.Instance.swordGirl2.totalKillScore++;
                        }
                        else
                        {
                            DataManager.Instance.leon.totalKillScore++;
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

            if (TImebar.timebarImage.fillAmount <=0 && repetition == false)
            {// Time�� �� �����ٸ�(GameOver) timeOver
                repetition = true;
                Left_Orc2_Anim.LeftAnim.SetTrigger("Left_Attack");
                Right_Orc2_Anim.RightAnim.SetTrigger("Right_Attack");
                playerDie = true;
                PlayerBase.PlayerAnim.SetTrigger("Die");
                resultObj.gameObject.SetActive(true);
                reStartObj.gameObject.SetActive(true);
                //continue_Button.gameObject.SetActive(true);

                if (ContinueButton.continueButtonClick == true)
                {
                    continue_Button.gameObject.SetActive(false);
                }
                else
                {
                    continue_Button.gameObject.SetActive(true);
                }
                //Time.timeScale = 0.0f; // ���Ӹ���
            }
        }

        void Attack()
        {
            ClickNode = ENodeType.Attack;
            if (playerDie == true)
            {
                ClickNode = ENodeType.Default;
            }
        }

        void SelectEnemy()
        {
            if (selectEnemy == RightEnemy && playerDie == false)
            {// ���� ���õ� ���Ͱ� ������ �����̰�
                if (LeftEnemy.GetOwnNodes().Count == Test3Spawn.Instance.LeftAttackNum)
                {// ���ʸ��Ϳ� ������ ����� �Ѽ��� ���ٸ� == ������ ù��° �����

                    if (FirstDashAttack || FirstAttack)
                    {// FirstAttack : ���ӽ��۽� ù ������ �뽬��ư�� ��� GameOver
                        //Time.timeScale = 0.0f; // ���Ӹ���
                        playerDie = true;
                        PlayerBase.PlayerAnim.SetTrigger("Die");
                        resultObj.gameObject.SetActive(true);
                        continue_Button.gameObject.SetActive(true);
                        reStartObj.gameObject.SetActive(true);

                        if (ContinueButton.continueButtonClick == true)
                        {
                            continue_Button.gameObject.SetActive(false);
                        }
                        else
                        {
                            continue_Button.gameObject.SetActive(true);
                        }
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

                    PlayerBaseInstance.gameObject.transform.position = new Vector3(selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.x
                    , selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.y, 0.0f); // ������ ù��° �����ġ�� �̵�
                    
                    Vector3 scorePosition = selectEnemy.GetOwnNodes().Peek().transform.position; // ����� ��ġ�� ������
                    GameObject ScoreTextobj = Instantiate(ScoreTextObj, scorePosition, Quaternion.identity); // �����ġ�� ����

                    Debug.Log(SelectCharacter.swordGirl1);
                    Debug.Log(SelectCharacter.swordGirl2);
                    Debug.Log(SelectCharacter.leon);

                    if (SelectCharacter.swordGirl1)
                    {
                        swordGirl1_AfterImage.transform.localScale = new Vector3(-1.0f, swordGirl1_AfterImage.transform.localScale.y, swordGirl1_AfterImage.transform.localScale.z);
                        Instantiate(swordGirl1_AfterImage, PlayerBaseInstance.gameObject.transform.position, Quaternion.identity);
                    }
                    else if (SelectCharacter.swordGirl2)
                    {
                        swordGirl2_AfterImage.transform.localScale = new Vector3(-1.0f, swordGirl2_AfterImage.transform.localScale.y, swordGirl2_AfterImage.transform.localScale.z);
                        Instantiate(swordGirl2_AfterImage, PlayerBaseInstance.gameObject.transform.position, Quaternion.identity);
                    }
                    else
                    {
                        leon_AfterImage.transform.localScale = new Vector3(-1.0f, leon_AfterImage.transform.localScale.y, leon_AfterImage.transform.localScale.z);
                        Instantiate(leon_AfterImage, PlayerBaseInstance.gameObject.transform.position, Quaternion.identity);
                    }

                    Left_Orc2_Anim.LeftAnim.SetTrigger("Left_Damage");

                    Character_Effect.transform.localScale = new Vector3(-1.0f, Character_Effect.transform.localScale.y, Character_Effect.transform.localScale.z);
                    
                    RandEffect();

                    Destroy(selectEnemy.GetOwnNodes().Peek().gameObject);

                    selectEnemy.GetOwnNodes().Dequeue();

                    selectEnemy.Hit();

                    PlayerBase.PlayerTransform.localScale =
                    new Vector3(-1f, PlayerBase.PlayerTransform.localScale.y, PlayerBase.PlayerTransform.localScale.z);

                    FirstDashAttack = true;
                    
                    if (selectEnemy.GetOwnNodes().Count <= 0)
                    {
                        
                        float rand = Random.Range(0f, 1f);
                        if (rand <= 0.05f)
                        {// 5% Ȯ���� 1��� �߰�(���Ͱ� ����ҽ�)
                            DataManager.Instance.playerData.Gold += 1;
                        }
                        // �߰� 10��� Ȯ�� ���
                        else if (rand <= 0.0005f)
                        {// 0.05% Ȯ���� 10��带 �߰�(���Ͱ� ����ҽ�)
                            DataManager.Instance.playerData.Gold += 10;
                        }

                        FirstDashAttack = false;
                        selectEnemy.Die();
                        audioManager.Instance.SfxAudioPlay_Enemy("Enemy_Dead");

                        if (SelectCharacter.swordGirl1)
                        {
                            DataManager.Instance.swordGirl1.totalKillScore++;
                        }
                        else if (SelectCharacter.swordGirl2)
                        {
                            DataManager.Instance.swordGirl2.totalKillScore++;
                        }
                        else
                        {
                            DataManager.Instance.leon.totalKillScore++;
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
            else if (selectEnemy == LeftEnemy && playerDie == false)
            {
                if (RightEnemy.GetOwnNodes().Count == Test3Spawn.Instance.RightAttackNum)
                {

                    if (FirstDashAttack || FirstAttack)
                    {// FirstAttack : ���ӽ��۽� ù ������ �뽬��ư�� ��� GameOver
                        //Time.timeScale = 0.0f; // ���Ӹ���
                        playerDie = true;
                        PlayerBase.PlayerAnim.SetTrigger("Die");
                        resultObj.gameObject.SetActive(true);
                        continue_Button.gameObject.SetActive(true);
                        reStartObj.gameObject.SetActive(true);

                        if (ContinueButton.continueButtonClick == true)
                        {
                            continue_Button.gameObject.SetActive(false);
                        }
                        else
                        {
                            continue_Button.gameObject.SetActive(true);
                        }
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

                    PlayerBaseInstance.gameObject.transform.position = new Vector3(selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.x
                    , selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.y, 0.0f); // ������ ù��° �����ġ�� �̵�

                    Vector3 scorePosition = selectEnemy.GetOwnNodes().Peek().transform.position; // ����� ��ġ�� ������
                    GameObject ScoreTextobj = Instantiate(ScoreTextObj, scorePosition, Quaternion.identity); // �����ġ�� ����

                    if (SelectCharacter.swordGirl1)
                    {
                        swordGirl1_AfterImage.transform.localScale = new Vector3(1.0f, swordGirl1_AfterImage.transform.localScale.y, swordGirl1_AfterImage.transform.localScale.z);
                        Instantiate(swordGirl1_AfterImage, PlayerBaseInstance.gameObject.transform.position, Quaternion.identity);
                    }
                    else if (SelectCharacter.swordGirl2)
                    {
                        swordGirl2_AfterImage.transform.localScale = new Vector3(1.0f, swordGirl2_AfterImage.transform.localScale.y, swordGirl2_AfterImage.transform.localScale.z);
                        Instantiate(swordGirl2_AfterImage, PlayerBaseInstance.gameObject.transform.position, Quaternion.identity);
                    }
                    else
                    {
                        leon_AfterImage.transform.localScale = new Vector3(1.0f, leon_AfterImage.transform.localScale.y, leon_AfterImage.transform.localScale.z);
                        Instantiate(leon_AfterImage, PlayerBaseInstance.gameObject.transform.position, Quaternion.identity);
                    }

                    Right_Orc2_Anim.RightAnim.SetTrigger("Right_Damage"); // ��ũ�� �ǰݸ�� ���

                    Character_Effect.transform.localScale = new Vector3(1.0f, Character_Effect.transform.localScale.y, Character_Effect.transform.localScale.z);
                    
                    RandEffect();
                    //Vector3 targetPosition = selectEnemy.GetOwnNodes().Peek().gameObject.transform.position;

                    Destroy(selectEnemy.GetOwnNodes().Peek().gameObject);

                    selectEnemy.GetOwnNodes().Dequeue();

                    selectEnemy.Hit();

                    PlayerBase.PlayerTransform.localScale =
                    new Vector3(1f, PlayerBase.PlayerTransform.localScale.y, PlayerBase.PlayerTransform.localScale.z);

                    FirstDashAttack = true;
                    
                    if (selectEnemy.GetOwnNodes().Count <= 0)
                    {
                        float rand = Random.Range(0f, 1f);
                        if (rand <= 0.05f)
                        {// 5% Ȯ���� 1��� �߰�(���Ͱ� ����ҽ�)
                            DataManager.Instance.playerData.Gold += 1;
                        }
                        // �߰� 10��� Ȯ�� ���
                        else if (rand <= 0.0005f)
                        {// 0.05% Ȯ���� 10��带 �߰�(���Ͱ� ����ҽ�)
                            DataManager.Instance.playerData.Gold += 10;
                        }

                        FirstDashAttack = false;
                        selectEnemy.Die();
                        audioManager.Instance.SfxAudioPlay_Enemy("Enemy_Dead");
                        TimebarInstance.KillCount += 1;

                        if (SelectCharacter.swordGirl1)
                        {
                            DataManager.Instance.swordGirl1.totalKillScore++;
                        }
                        else if (SelectCharacter.swordGirl2)
                        {
                            DataManager.Instance.swordGirl2.totalKillScore++;
                        }
                        else
                        {
                            DataManager.Instance.leon.totalKillScore++;
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
                    PlayerBase.PlayerAnim.SetTrigger("Atk_1"); // ��ũ�� ��ġ�� �̵��� ���ݸ��
                }
                else if (randAnim == 1)
                {
                    PlayerBase.PlayerAnim.SetTrigger("Atk_2"); // ��ũ�� ��ġ�� �̵��� ���ݸ��
                }
                else if (randAnim == 2)
                {
                    PlayerBase.PlayerAnim.SetTrigger("Atk_3"); // ��ũ�� ��ġ�� �̵��� ���ݸ��
                }
                else if (randAnim == 3)
                {
                    PlayerBase.PlayerAnim.SetTrigger("Atk_4"); // ��ũ�� ��ġ�� �̵��� ���ݸ��
                }
                else if (randAnim == 4)
                {
                    PlayerBase.PlayerAnim.SetTrigger("Atk_5"); // ��ũ�� ��ġ�� �̵��� ���ݸ��
                }
                else
                {// 5
                    PlayerBase.PlayerAnim.SetTrigger("Atk_7"); // ��ũ�� ��ġ�� �̵��� ���ݸ��
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

        void EffectInit()
        {
            Character_Effect.transform.localScale =
            new Vector3(1.0f, Character_Effect.transform.localScale.y, Character_Effect.transform.localScale.z);

            swordGirl1_AfterImage.transform.localScale =
            new Vector3(1.0f, swordGirl1_AfterImage.transform.localScale.y, swordGirl1_AfterImage.transform.localScale.z);

            swordGirl2_AfterImage.transform.localScale =
            new Vector3(1.0f, swordGirl2_AfterImage.transform.localScale.y, swordGirl2_AfterImage.transform.localScale.z);

            leon_AfterImage.transform.localScale =
            new Vector3(1.0f, leon_AfterImage.transform.localScale.y, leon_AfterImage.transform.localScale.z);
        }
    }
}


