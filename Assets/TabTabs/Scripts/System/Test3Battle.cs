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
        public bool MonsterDie; // ��� �� ���Ͱ� �׾����� Ȯ���ϴ� bool�� ����
        public Node NodeInstance;
        public GameObject Character_Effect;
        bool FirstAttack; // ���ӽ��۽� ���ݹ�ư���� ���� �ѹ��� ����Ǵ� �ִϸ��̼� ����

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
            {// ClickNode�� �߸��� �ƴ϶��(��ư�� Ŭ���ߴٸ�)

                if (selectEnemy == null) { return; }

                if (ClickNode == selectEnemy.GetOwnNodes().Peek().nodeSheet.m_NodeType)
                {//������ ���� ���Ÿ�԰� ��(���� NodeType�� Ŭ���ߴٸ�)

                    GameManager.NotificationSystem.NodeHitSuccess?.Invoke();

                    // 1. �ش��ϴ� enemy�� �� destroy
                    // 2. ĳ���Ͱ� �ش��ϴ� enemy�� ����ġ�� �̵� �� ���� �ִϸ��̼� ��� �� ������ġ�� �̵�

                    TImebar.timebarImage.fillAmount += 0.1f; // �ð����� +0.1f

                    CharacterBaseInstance.gameObject.transform.position = new Vector3(selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.x
                    , selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.y, 0.0f);

                    

                    RandAnim();
                    if (FirstAttack)
                    {// ���� ���ݽø� �ߵ��Ǵ� �ִϸ��̼�
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

                        if (MonsterDie)
                        {// ������ ���Ͱ� �������¶��
                            // MonsterDie(Bool)�� true���¶�� 
                            Invoke("RightMonsterSpawn", 0.7f);
                            //RightMonsterSpawn();
                            // ���� ��
                            MonsterDie = false;
                        }
                        else
                        {// MonsterDie�� false���¶��
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
                {// ���Ϳ��� �����ִ� ��尡 7 �̻��̶�� (������ ù��° ��常 �����ֳʹ� ��ư���� Ÿ�ݰ���)
                    selectEnemy = LeftEnemy;
                    TImebar.timebarImage.fillAmount += 0.1f; // �ð����� +0.1f
                    CharacterBaseInstance.gameObject.transform.position = new Vector3(selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.x
                    , selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.y, 0.0f); // ������ ù��° �����ġ�� �̵�

                    PlayerBaseInstance.PlayerAnim.SetTrigger("Slide_Atk_1"); // ��ũ�� ��ġ�� �̵��� ���ݸ��
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
                {// ���Ϳ��� �����ִ� ��尡 7 �̻��̶�� (������ ù��° ��常 �����ֳʹ� ��ư���� Ÿ�ݰ���)
                    selectEnemy = RightEnemy;
                    TImebar.timebarImage.fillAmount += 0.1f; // �ð����� +0.1f
                    CharacterBaseInstance.gameObject.transform.position = new Vector3(selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.x
                    , selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.y, 0.0f); // ������ ù��° �����ġ�� �̵�

                    PlayerBaseInstance.PlayerAnim.SetTrigger("Slide_Atk_1"); // ��ũ�� ��ġ�� �̵��� ���ݸ��
                    Right_Orc2_Anim.RightAnim.SetTrigger("Right_Damage"); // ��ũ�� �ǰݸ�� ���
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

            Vector3 targetPosition = selectEnemy.GetOwnNodes().Peek().gameObject.transform.position;
            Instantiate(Character_Effect, targetPosition, Quaternion.identity);
            //float randEffect = Random.Range(0f, 100f);
            //if (randEffect <= 20f)
            //{// randEffect : 20�ۼ�Ʈ Ȯ��
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


