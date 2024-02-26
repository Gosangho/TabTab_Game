using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TabTabs.NamChanwoo
{
    public class TutorialBattleSystem : MonoBehaviour
    {
        // BattleSystem���� ������ �͵�
        // 1. ��������
        // 2. Timebar�� ���ʹ��� ü�� --
        // NodeSheetŬ������ m_NodeType;(����Ű) => ������ ������ ��
        // SpawnSystemŬ������ SpawnNode(GameObject enemy)�Լ� => ��Ŭ��
        // EnemyBaseŬ������ m_nodeQueue����(Queue ����) => GetownNodes�Լ� ������ ��
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
        public bool MonsterDie = false; // ��� �� ���Ͱ� �׾����� Ȯ���ϴ� bool�� ����
        public Node NodeInstance;
        public GameObject Character_Effect;
        bool FirstAttack; // ���ӽ��۽� ���ݹ�ư���� ���� �ѹ��� ����Ǵ� �ִϸ��̼� ����
        //public bool Right_TrainAttack; // ������ ���͸� ���Ӱ����ߴ��� �Ǵ��ϴ� ����
        //public bool Left_TrainAttack; // ���� ���͸� ���Ӱ����ߴ��� �Ǵ��ϴ� ����
        //public GameObject ReStartButton;
        //public ScoreSystem ScoreSystemInstance;
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
           // CharacterBaseInstance = FindObjectOfType<CharacterBase>();
            PlayerBaseInstance = FindObjectOfType<PlayerBase>();
            AttackButton.onClick.AddListener(Attack);
            SelectEnemyButton.onClick.AddListener(SelectEnemy);
            StartSpawn();
            MonsterDie = false;
            //Right_TrainAttack = false;
            //Left_TrainAttack = false;
            FirstAttack = true;
            NodeInstance = FindObjectOfType<Node>();
            //ScoreSystemInstance = FindObjectOfType<ScoreSystem>();
            TimebarInstance = FindObjectOfType<TImebar>();
            Character_Effect.transform.localScale =
            new Vector3(1.0f, Character_Effect.transform.localScale.y, Character_Effect.transform.localScale.z);
        }

        void Update()
        {
            if (ClickNode != ENodeType.Default)
            {// ClickNode�� �߸��� �ƴ϶�� == (��ư�� Ŭ���ߴٸ�)

                if (selectEnemy == null) { return; }

                if (ClickNode == selectEnemy.GetOwnNodes().Peek().nodeSheet.m_NodeType)
                {//������ ���� ���Ÿ�԰� ��(���� NodeType�� Ŭ���ߴٸ�)

                    GameManager.NotificationSystem.NodeHitSuccess?.Invoke();
                    //ScoreSystemInstance.score += 1; // ���ݼ����� Score +1
                                                    // 1. �ش��ϴ� enemy�� ���� destroy
                                                    // 2. ĳ���Ͱ� �ش��ϴ� enemy�� ������ġ�� �̵� �� ���� �ִϸ��̼� ��� �� ������ġ�� �̵�

                    TImebar.timebarImage.fillAmount += 0.1f; // �ð����� +0.1f

                    PlayerBaseInstance.gameObject.transform.position = new Vector3(selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.x
                    , selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.y, 0.0f);

                    Vector3 scorePosition = selectEnemy.GetOwnNodes().Peek().transform.position; // ����� ��ġ�� ������
                    GameObject gameObject = Instantiate(ScoreTextObj, scorePosition, Quaternion.identity); // �����ġ�� ����

                    //if (Right_TrainAttack == true || Left_TrainAttack == true)
                    //{// ������ ���ͳ� ���� ���͸� �������� �����ߴٸ�
                    // // ���ӿ��� -> ���� �ٽý���
                    //    Debug.Log("���ӿ���");
                    //    ReStartButton.SetActive(true);
                    //    Time.timeScale = 0.0f; // ���Ӹ���
                    //}
                    RandAttackAudio();
                    RandEnemyHitAudio();
                    RandAnim();
                    DialogTestInstance.FirstAttack = true;
                    if (FirstAttack)
                    {// ���� ���ݽø� �ߵ��Ǵ� �ִϸ��̼�
                        PlayerBase.PlayerAnim.SetTrigger("Atk_6");
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

            if (RightMonsterDie == true && LeftMonsterDie == true)
            {
                PlayerBaseInstance.gameObject.transform.position = new Vector3(0.0f, 0.72f, 0.0f);
                MonsterDie = true; // Ʃ�丮�� �׽�Ʈ���� Wait for Until�ȿ� �� ����
            }
        }

        void Attack()
        {
            ClickNode = ENodeType.Attack;
        }

        void SelectEnemy()
        {
            if (selectEnemy == RightEnemy && RightMonsterDie)
            {// ���� ���õ� ���Ͱ� ������ �����̰�
                if (LeftEnemy.GetOwnNodes().Count == Test3Spawn.Instance.LeftAttackNum)
                {// ���ʸ��Ϳ� ������ ����� �Ѽ��� ���ٸ� == ������ ù��° �����
                    //ScoreSystemInstance.score += 1; // ���ݼ����� Score +1
                    RandDashAttackAudio();
                    RandEnemyHitAudio();
                    //Right_TrainAttack = false;
                    DialogTestInstance.FirstAttack = true;

                    selectEnemy = LeftEnemy;

                    TImebar.timebarImage.fillAmount += 0.1f; // �ð����� +0.1f

                    PlayerBaseInstance.gameObject.transform.position = new Vector3(selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.x
                    , selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.y, 0.0f); // ������ ù��° �����ġ�� �̵�

                    Vector3 scorePosition = selectEnemy.GetOwnNodes().Peek().transform.position; // ����� ��ġ�� ������
                    GameObject ScoreTextobj = Instantiate(ScoreTextObj, scorePosition, Quaternion.identity); // �����ġ�� ����

                    GameObject PlayerAfterImage = Instantiate(Player_AfterImage, PlayerBaseInstance.gameObject.transform.position, Quaternion.identity);
                    SpriteRenderer spriteRenderer = PlayerAfterImage.GetComponent<SpriteRenderer>();
                    spriteRenderer.flipX = true;
                    PlayerBase.PlayerAnim.SetTrigger("Slide_Atk_1"); // ��ũ�� ��ġ�� �̵��� ���ݸ��

                    Left_Orc2_Anim.LeftAnim.SetTrigger("Left_Damage");

                    Character_Effect.transform.localScale = new Vector3(-1.0f, Character_Effect.transform.localScale.y, Character_Effect.transform.localScale.z);

                    RandEffect();
                    //Vector3 targetPosition = selectEnemy.GetOwnNodes().Peek().gameObject.transform.position;
                    Destroy(selectEnemy.GetOwnNodes().Peek().gameObject);

                    selectEnemy.GetOwnNodes().Dequeue();

                    selectEnemy.Hit();

                    PlayerBase.PlayerTransform.localScale =
                    new Vector3(-1f, PlayerBase.PlayerTransform.localScale.y, PlayerBase.PlayerTransform.localScale.z);
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
                    //ScoreSystemInstance.score += 1; // ���ݼ����� Score +1
                    RandDashAttackAudio();
                    RandEnemyHitAudio();
                    //Left_TrainAttack = false;
                    DialogTestInstance.FirstAttack = true;

                    selectEnemy = RightEnemy;

                    TImebar.timebarImage.fillAmount += 0.1f; // �ð����� +0.1f

                    PlayerBaseInstance.gameObject.transform.position = new Vector3(selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.x
                    , selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.y, 0.0f); // ������ ù��° �����ġ�� �̵�

                    Vector3 scorePosition = selectEnemy.GetOwnNodes().Peek().transform.position; // ����� ��ġ�� ������
                    GameObject ScoreTextobj = Instantiate(ScoreTextObj, scorePosition, Quaternion.identity); // �����ġ�� ����

                    GameObject PlayerAfterImage = Instantiate(Player_AfterImage, PlayerBaseInstance.gameObject.transform.position, Quaternion.identity);
                    SpriteRenderer spriteRenderer = PlayerAfterImage.GetComponent<SpriteRenderer>();
                    spriteRenderer.flipX = false;
                    PlayerBase.PlayerAnim.SetTrigger("Slide_Atk_1"); // ��ũ�� ��ġ�� �̵��� ���ݸ��

                    Right_Orc2_Anim.RightAnim.SetTrigger("Right_Damage"); // ��ũ�� �ǰݸ�� ���

                    Character_Effect.transform.localScale = new Vector3(1.0f, Character_Effect.transform.localScale.y, Character_Effect.transform.localScale.z);

                    RandEffect();
                    //Vector3 targetPosition = selectEnemy.GetOwnNodes().Peek().gameObject.transform.position;

                    Destroy(selectEnemy.GetOwnNodes().Peek().gameObject);

                    selectEnemy.GetOwnNodes().Dequeue();

                    selectEnemy.Hit();

                    PlayerBase.PlayerTransform.localScale =
                    new Vector3(1f, PlayerBase.PlayerTransform.localScale.y, PlayerBase.PlayerTransform.localScale.z);

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

            //Vector3 targetPosition = selectEnemy.GetOwnNodes().Peek().gameObject.transform.position;
            //Instantiate(Character_Effect, targetPosition, Quaternion.identity);
            float randEffect = Random.Range(0f, 100f);
            if (randEffect <= 20f)
            {// randEffect : 20�ۼ�Ʈ Ȯ��
                Vector3 targetPosition = selectEnemy.GetOwnNodes().Peek().gameObject.transform.position;
                Instantiate(Character_Effect, targetPosition, Quaternion.identity);
            }
        }

        public void StartSpawn()
        {
            GameObject RightMonster = Right_Ork;
            GameObject RightSpawnMonster = Instantiate(RightMonster, new Vector3(0.3f, 0.72f, 0), Quaternion.identity);
            EnemyBase spawnEnemy = RightSpawnMonster.GetComponent<EnemyBase>();
            if (spawnEnemy != null)
            {
                //GameManager.NotificationSystem.SceneMonsterSpawned.Invoke(spawnEnemy); // ���Ͱ� �����Ǿ����� �ý��ۿ� �˸��ϴ�.
                Test3Spawn.Instance.Spawn_RightNode(spawnEnemy);
            }
            RightEnemy = spawnEnemy; // �� ����
            selectEnemy = spawnEnemy; // ������ ���Ͱ� ����Ʈ��

            GameObject LefttMonster = Left_Ork;
            GameObject LeftSpawnMonster = Instantiate(LefttMonster, new Vector3(-0.3f, 0.72f, 0), Quaternion.identity);
            EnemyBase spawnEnemy2 = LeftSpawnMonster.GetComponent<EnemyBase>();
            if (spawnEnemy2 != null)
            {
                //GameManager.NotificationSystem.SceneMonsterSpawned.Invoke(spawnEnemy); // ���Ͱ� �����Ǿ����� �ý��ۿ� �˸��ϴ�.
                Test3Spawn.Instance.SpawnLeft_Node(spawnEnemy2);
            }
            LeftEnemy = spawnEnemy2; // �� ����
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
    }
}


