using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace TabTabs.NamChanwoo
{

    public class Test3Spawn : MonoBehaviour
    {
        // �ν��Ͻ��� ���� ���� ����
        public static Test3Spawn Instance { get; private set; }

        [Header("Spawn Setting")]
        [SerializeField] bool IsSpawnAlignmentRandom = false;
        [SerializeField] public GameObject m_SpawnLocation_Right;
        [SerializeField] public GameObject m_SpawnLocation_Left;
        [SerializeField] public List<GameObject> m_NodeList = new List<GameObject>(); // ���Ϳ� ������ ��帮��Ʈ
        [SerializeField] private List<GameObject> m_monsterPrefabList; // ������ ���� ����Ʈ
        public Test3Battle BattleInstance;
        public GameObject AttackNode;
        public Vector2 SpawnNodePosition;
        public int RightAttackNum;
        public int LeftAttackNum;
        int RightRow;
        int LeftRow;

        /*[SerializeField] private GameObject m_Enemy;*/


        private void Awake()
        {
            // NodeSpawnManager �ν��Ͻ��� �ϳ��� �ִ��� Ȯ���Ͻʽÿ�.
            if (Instance == null)
            {
                Instance = this;
                //DontDestroyOnLoad(gameObject); // ���� ����: �� �����ڰ� ��� �ε� ���� ���ӵǵ��� �Ϸ���
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            GameManager.NotificationSystem.SceneMonsterDeath.AddListener(HandleSceneMonsterDeath);
            BattleInstance = FindObjectOfType<Test3Battle>();
            // SpawnLocation �ڽ� ��ü ã��
            m_SpawnLocation_Right = transform.Find("SpawnLocation_Right").gameObject;
            m_SpawnLocation_Left = transform.Find("SpawnLocation_Left").gameObject;
            //if (m_SpawnLocation == null)
            //{
            //    Debug.LogError("���� ��ü�� SpawnLocation�� ã�� �� �����ϴ�.");
            //    return;
            //}

            //SpawnMonster();
        }

        private void HandleSceneMonsterDeath(EnemyBase arg0)
        {
            float delayTime = 1.0f;
            StartCoroutine(DelaySpawn(delayTime));
        }

        private IEnumerator DelaySpawn(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            //if (selectMonster.selectEnemy==null)
            //{
            //    EnemySpawn();
            //}
        }

        public void Spawn_RightNode(EnemyBase enemyBase)
        {
            //��尡 0���� �ƴ϶�� ���� �� ��带 �����ϰ� Queue�� ���ϴ�.
            if (enemyBase.GetOwnNodes().Count != 0)
            {
                foreach (Node ownNode in enemyBase.GetOwnNodes())
                {
                    Destroy(ownNode.gameObject);
                }
                enemyBase.GetOwnNodes().Clear();
            }

            NodeArea nodeArea = enemyBase.nodeArea;

            // NodeArea�� �߰ߵǾ����� Ȯ���Ͻʽÿ�.
            if (nodeArea == null)
            {
                Debug.LogError("�� �Ǵ� ���� �ڽĿ��� NodeArea�� ã�� �� �����ϴ�.");
                return;
            }

            // ������ ��� ���� �� ���� �����ϴ�.
            int spawnNodeNum = nodeArea.Rows;
            // 7�� 3���� ���� -> �� �࿡�� 3���� ������ ��������Ʈ�� �������� ������ Ȯ���� ����
            for (RightRow = 0; RightRow < spawnNodeNum; RightRow++)
            {
                int ranAttackNode = Random.Range(0, 2); // 50�ۼ�Ʈ Ȯ��
                if (ranAttackNode == 0)
                {
                    // �� �࿡�� ���Ƿ� ���� �����մϴ�.
                    int randColumn = UnityEngine.Random.Range(0, nodeArea.Columns);

                    // ��Ͽ��� ���� �ش��ϴ� ��� �������� �����ɴϴ�.
                    GameObject nodePrefab = GetNodePrefab(randColumn);

                    // NodeArea���� �� ����� ��ġ�� �����ɴϴ�.
                    Vector2 spawnPosition = nodeArea.GetGridPosition(RightRow, randColumn);

                    // ���� ��ġ�� ��带 �ν��Ͻ�ȭ�ϰ� nodeArea�� �ڽ����� �����մϴ�.
                    GameObject spawnedNode = Instantiate(nodePrefab, spawnPosition, Quaternion.identity, nodeArea.transform);

                    // ����� �̸��� �����մϴ�.
                    spawnedNode.name = $"Node_{RightRow}_{randColumn}";

                    // NodeSheet�� ��带 �ʱ�ȭ�մϴ�.
                    Node nodeComponent = spawnedNode.GetComponent<Node>();

                    nodeComponent.Init_Right();

                    //���ʹ̰� �����ϴ� ��忡 �߰��մϴ�.
                    enemyBase.AddNodes(nodeComponent);
                    RightAttackNum++; // ������ ���Ϳ� ��� ��������Ʈ�� �����Ǿ����� �˷��ִ� ����
                }
                else
                {
                    Debug.Log("������ ������ ��������Ʈ�� �������� �ʽ��ϴ�.");
                }

                if (RightRow == 6 && RightAttackNum == 0)
                {// for���� 7�� ������������ ������ ��������Ʈ�� 0�� �ϰ��(���ܼ���)
                    int randColumn2 = UnityEngine.Random.Range(0, nodeArea.Columns);
                    int randRow = UnityEngine.Random.Range(0, 7);
                    GameObject nodePrefab2 = GetNodePrefab(randColumn2);
                    Vector2 spawnPosition2 = nodeArea.GetGridPosition(randRow, randColumn2);
                    GameObject spawnedNode2 = Instantiate(nodePrefab2, spawnPosition2, Quaternion.identity, nodeArea.transform);
                    Node nodeComponent2 = spawnedNode2.GetComponent<Node>();
                    nodeComponent2.Init_Right();
                    //���ʹ̰� �����ϴ� ��忡 �߰��մϴ�.
                    enemyBase.AddNodes(nodeComponent2);
                    RightAttackNum++;
                }
            }
            RightRow = 0;
        }
        public void SpawnLeft_Node(EnemyBase enemyBase)
        {
            //��尡 0���� �ƴ϶�� ���� �� ��带 �����ϰ� Queue�� ���ϴ�.
            if (enemyBase.GetOwnNodes().Count != 0)
            {
                foreach (Node ownNode in enemyBase.GetOwnNodes())
                {
                    Destroy(ownNode.gameObject);
                }
                enemyBase.GetOwnNodes().Clear();
            }

            NodeArea nodeArea = enemyBase.nodeArea;

            // NodeArea�� �߰ߵǾ����� Ȯ���Ͻʽÿ�.
            if (nodeArea == null)
            {
                Debug.LogError("�� �Ǵ� ���� �ڽĿ��� NodeArea�� ã�� �� �����ϴ�.");
                return;
            }

            // ������ ��� ���� �� ���� �����ϴ�.
            int spawnNodeNum = nodeArea.Rows;
            // spawnNodeNum = 7; 0,1,2,3,4,5,6
            for (LeftRow = 0; LeftRow < spawnNodeNum; LeftRow++)
            {
                int ranAttackNodeNum = Random.Range(0, 2); // �켱�� 50�ۼ�Ʈ Ȯ��

                if (ranAttackNodeNum == 0)
                {// �����Ǵ� Ȯ���� �����ٸ�
                 // �� �࿡�� ���Ƿ� ���� �����մϴ�.
                    int randColumn = UnityEngine.Random.Range(0, nodeArea.Columns);

                    // ��Ͽ��� ���� �ش��ϴ� ��� �������� �����ɴϴ�.
                    GameObject nodePrefab = GetNodePrefab(randColumn);

                    // NodeArea���� �� ����� ��ġ�� �����ɴϴ�.
                    Vector2 spawnPosition = nodeArea.GetGridPosition(LeftRow, randColumn);

                    // ���� ��ġ�� ��带 �ν��Ͻ�ȭ�ϰ� nodeArea�� �ڽ����� �����մϴ�.
                    GameObject spawnedNode = Instantiate(nodePrefab, spawnPosition, Quaternion.identity, nodeArea.transform);

                    // ����� �̸��� �����մϴ�.
                    spawnedNode.name = $"Node_{LeftRow}_{randColumn}";

                    // NodeSheet�� ��带 �ʱ�ȭ�մϴ�.
                    Node nodeComponent = spawnedNode.GetComponent<Node>();

                    nodeComponent.Init_Left();

                    //���ʹ̰� �����ϴ� ��忡 �߰��մϴ�.
                    enemyBase.AddNodes(nodeComponent);
                    LeftAttackNum++; // ���ʸ��Ϳ� ��� ��������Ʈ�� �����Ǿ����� �˷��ִ� ����
                }
                else
                {// ������ �ʾҴٸ� ��������Ʈ�� �������� ����
                    Debug.Log("���� ������ ��������Ʈ�� �������� �ʽ��ϴ�.");
                }

                if (LeftRow == 6 && LeftAttackNum == 0)
                {// for���� 7�� ������������ ������ ��������Ʈ�� 0�� �ϰ��(���ܼ���)
                    int randColumn2 = UnityEngine.Random.Range(0, nodeArea.Columns);
                    int randRow = UnityEngine.Random.Range(0, 7);
                    GameObject nodePrefab2 = GetNodePrefab(randColumn2);
                    Vector2 spawnPosition2 = nodeArea.GetGridPosition(randRow, randColumn2);
                    GameObject spawnedNode2 = Instantiate(nodePrefab2, spawnPosition2, Quaternion.identity, nodeArea.transform);
                    Node nodeComponent2 = spawnedNode2.GetComponent<Node>();
                    nodeComponent2.Init_Right();
                    //���ʹ̰� �����ϴ� ��忡 �߰��մϴ�.
                    enemyBase.AddNodes(nodeComponent2);
                    LeftAttackNum++;
                }
            }
            LeftRow = 0;
        }


        public void SpawnMode_B_NodeLeft(EnemyBase enemyBase, int monsterType)
        {
            if (enemyBase.GetOwnNodes().Count != 0)
            {
                foreach (Node ownNode in enemyBase.GetOwnNodes())
                {
                    Destroy(ownNode.gameObject);
                }
                enemyBase.GetOwnNodes().Clear();
            }

            NodeArea nodeArea = enemyBase.nodeArea;

            if (nodeArea == null)
            {
                Debug.LogError("NodeArea LogError.");
                return;
            }

            // ������ ��� ���� �� ���� �����ϴ�.
            int spawnNodeNum = nodeArea.Rows;
            Debug.Log("spawnNodeNum : " + spawnNodeNum);

            int[,] monsterTarget = {
                {0, 0, 1},
                {0, 0, 1},
                {0, 2, 0},
                {3, 0, 0},
                {3, 0, 0},
                {0, 4, 0}
            };
            if(monsterType == 0) {
                 monsterTarget = new int[,] {
                    {0, 0, 1},
                    {0, 0, 1},
                    {0, 2, 0},
                    {3, 0, 0},
                    {3, 0, 0},
                    {0, 4, 0}
                };
            } else {
                monsterTarget = new int[,] {
                    {0, 2, 0},
                    {0, 0, 3},
                    {0, 2, 0},
                    {1, 0, 0},
                    {0, 4, 0}
                };
            }

            // spawnNodeNum = 7; 0,1,2,3,4,5,6
            for (LeftRow = 0; LeftRow < monsterTarget.GetLength(0); LeftRow++)
            {
                for(int k=0 ; k < monsterTarget.GetLength(1); k++ ){
                    if (monsterTarget[LeftRow, k] != 0)
                    {
                       int randColumn = k;

                        GameObject nodePrefab = GetNodePrefab(randColumn);

                        Vector2 spawnPosition = nodeArea.GetGridPosition(LeftRow, randColumn);

                        GameObject spawnedNode = Instantiate(nodePrefab, spawnPosition, Quaternion.identity, nodeArea.transform);

                        Node node =  spawnedNode.GetComponent<Node>();

                        node.SetSpriteNode(monsterTarget[LeftRow, k]);

                        string color = "";
                        if( monsterTarget[LeftRow, k] == 1) {
                            color = "Red";
                        } else if(monsterTarget[LeftRow, k] == 2) {
                            color = "Blue";
                        } else if( monsterTarget[LeftRow, k] == 3) {
                            color = "Yellow";
                        } else if( monsterTarget[LeftRow, k]== 4) {
                            color = "Dash";
                        } else {
                            color = "White";
                        }
                     

                        spawnedNode.name = $"Node_{color}_{LeftRow}_{randColumn}";

                        Node nodeComponent = spawnedNode.GetComponent<Node>();

                        nodeComponent.Init_Left();

                        enemyBase.AddNodes(nodeComponent);
                        LeftAttackNum++; 
                    }
                    else
                    {// ������ �ʾҴٸ� ��������Ʈ�� �������� ����
                        Debug.Log("���� ������ ��������Ʈ�� �������� �ʽ��ϴ�.");
                    }
                }

                

                if (LeftRow == 6 && LeftAttackNum == 0)
                {// for���� 7�� ������������ ������ ��������Ʈ�� 0�� �ϰ��(���ܼ���)
                    int randColumn2 = UnityEngine.Random.Range(0, nodeArea.Columns);
                    int randRow = UnityEngine.Random.Range(0, 7);
                    GameObject nodePrefab2 = GetNodePrefab(randColumn2);
                    Vector2 spawnPosition2 = nodeArea.GetGridPosition(randRow, randColumn2);
                    GameObject spawnedNode2 = Instantiate(nodePrefab2, spawnPosition2, Quaternion.identity, nodeArea.transform);
                    Node nodeComponent2 = spawnedNode2.GetComponent<Node>();
                    nodeComponent2.Init_Right();
                    //���ʹ̰� �����ϴ� ��忡 �߰��մϴ�.
                    enemyBase.AddNodes(nodeComponent2);
                    LeftAttackNum++;
                }
            }
            LeftRow = 0;
        }
        public void SpawnMode_B_NodeRight(EnemyBase enemyBase, int monsterType)
        {
            if (enemyBase.GetOwnNodes().Count != 0)
            {
                foreach (Node ownNode in enemyBase.GetOwnNodes())
                {
                    Destroy(ownNode.gameObject);
                }
                enemyBase.GetOwnNodes().Clear();
            }

            NodeArea nodeArea = enemyBase.nodeArea;

            if (nodeArea == null)
            {
                Debug.LogError("NodeArea LogError.");
                return;
            }

            // ������ ��� ���� �� ���� �����ϴ�.
            int spawnNodeNum = nodeArea.Rows;

            int[,] monsterTarget = {
                {1, 0, 0},
                {1, 0, 0},
                {0, 2, 0},
                {0, 0, 3},
                {0, 0, 3},
                {0, 4, 0}
            };
            if(monsterType == 0) {
                 monsterTarget = new int[,] {
                    {1, 0, 0},
                    {1, 0, 0},
                    {0, 2, 0},
                    {0, 0, 3},
                    {0, 0, 3},
                    {0, 4, 0}
                };
            } else {
                monsterTarget = new int[,] {
                    {0, 2, 0},
                    {3, 0, 0},
                    {0, 2, 0},
                    {0, 0, 1},
                    {0, 4, 0}
                };
            }



            // spawnNodeNum = 7; 0,1,2,3,4,5,6
            for (LeftRow = 0; LeftRow < monsterTarget.GetLength(0); LeftRow++)
            {
                for(int k=0 ; k < monsterTarget.GetLength(1); k++ ){
                    if (monsterTarget[LeftRow, k] != 0)
                    {
                       int randColumn = k;

                        GameObject nodePrefab = GetNodePrefab(randColumn);

                        Vector2 spawnPosition = nodeArea.GetGridPosition(LeftRow, randColumn);

                        GameObject spawnedNode = Instantiate(nodePrefab, spawnPosition, Quaternion.identity, nodeArea.transform);

                        spawnedNode.GetComponent<Node>().SetSpriteNode(monsterTarget[LeftRow, k]);

                        string color = "";
                        if( monsterTarget[LeftRow, k] == 1) {
                            color = "Red";
                        } else if(monsterTarget[LeftRow, k] == 2) {
                            color = "Blue";
                        } else if( monsterTarget[LeftRow, k] == 3) {
                            color = "Yellow";
                        } else if( monsterTarget[LeftRow, k]== 4) {
                            color = "Dash";
                        } else {
                            color = "White";
                        }
                     

                        spawnedNode.name = $"Node_{color}_{LeftRow}_{randColumn}";


                        Node nodeComponent = spawnedNode.GetComponent<Node>();

                        nodeComponent.Init_Left();

                        enemyBase.AddNodes(nodeComponent);
                        LeftAttackNum++; 
                    }
                    else
                    {// ������ �ʾҴٸ� ��������Ʈ�� �������� ����
                        Debug.Log("���� ������ ��������Ʈ�� �������� �ʽ��ϴ�.");
                    }
                }

                

                if (LeftRow == 6 && LeftAttackNum == 0)
                {// for���� 7�� ������������ ������ ��������Ʈ�� 0�� �ϰ��(���ܼ���)
                    int randColumn2 = UnityEngine.Random.Range(0, nodeArea.Columns);
                    int randRow = UnityEngine.Random.Range(0, 7);
                    GameObject nodePrefab2 = GetNodePrefab(randColumn2);
                    Vector2 spawnPosition2 = nodeArea.GetGridPosition(randRow, randColumn2);
                    GameObject spawnedNode2 = Instantiate(nodePrefab2, spawnPosition2, Quaternion.identity, nodeArea.transform);
                    Node nodeComponent2 = spawnedNode2.GetComponent<Node>();
                    nodeComponent2.Init_Right();
                    //���ʹ̰� �����ϴ� ��忡 �߰��մϴ�.
                    enemyBase.AddNodes(nodeComponent2);
                    LeftAttackNum++;
                }
            }
            LeftRow = 0;
        }

        private GameObject GetNodePrefab(int Column)
        {
            //���� ������ �ƴ϶��
            if (!IsSpawnAlignmentRandom)
            {
                if (Column == 0)
                    return m_NodeList.Find(x => x.GetComponent<Node>().nodeSheet.m_NodeType == ENodeType.Attack);
                if (Column == 1)
                    return m_NodeList.Find(x => x.GetComponent<Node>().nodeSheet.m_NodeType == ENodeType.Attack);
                if (Column == 2)
                    return m_NodeList.Find(x => x.GetComponent<Node>().nodeSheet.m_NodeType == ENodeType.Attack);

                return null;
            }

            //else ���� �����̶��
            return m_NodeList[UnityEngine.Random.Range(0, m_NodeList.Count)];
        }


        //public void SpawnMonster()
        //{
        //    int randomIndex = UnityEngine.Random.Range(0, m_monsterPrefabList.Count);
        //    GameObject monsterPrefab_Left = m_monsterPrefabList[0];
        //    GameObject monsterPrefab_Right = m_monsterPrefabList[1];

        //    GameObject spawnMonster_Right = Instantiate(monsterPrefab_Right, m_SpawnLocation_Right.transform.position, Quaternion.identity);

        //    GameObject spawnMonster_Left = Instantiate(monsterPrefab_Left, m_SpawnLocation_Left.transform.position, Quaternion.identity);
        //    Transform buttonTransform = spawnMonster_Left.transform.Find("Button");

        //    if (buttonTransform != null)
        //    {
        //        GameObject buttonObject = buttonTransform.gameObject;
        //        buttonObject.transform.localScale = new Vector3(-0.5f,0.5f,0);
        //    }


        //    EnemyBase spawnEnemy = spawnMonster_Right.GetComponent<EnemyBase>();
        //    EnemyBase spawnEnemy2 = spawnMonster_Left.GetComponent<EnemyBase>();
        //    if (spawnEnemy != null)
        //    {
        //        GameManager.NotificationSystem.SceneMonsterSpawned.Invoke(spawnEnemy); // ���Ͱ� �����Ǿ����� �ý��ۿ� �˸��ϴ�.
        //        SpawnNode(spawnEnemy);
        //    }

        //    if (spawnEnemy2 != null)
        //    {
        //        GameManager.NotificationSystem.SceneMonsterSpawned.Invoke(spawnEnemy2); // ���Ͱ� �����Ǿ����� �ý��ۿ� �˸��ϴ�.
        //        SpawnNode(spawnEnemy2);
        //    }
        //}
        //public void RightEnemySpawn()
        //{
        //    GameObject RightMonster = m_monsterPrefabList[1];
        //    GameObject spawnMonster = Instantiate(RightMonster, m_SpawnLocation_Right.transform.position, Quaternion.identity);
        //    EnemyBase spawnEnemy = spawnMonster.GetComponent<EnemyBase>();
        //    if (spawnEnemy != null)
        //    {
        //        //GameManager.NotificationSystem.SceneMonsterSpawned.Invoke(spawnEnemy); // ���Ͱ� �����Ǿ����� �ý��ۿ� �˸��ϴ�.
        //        SpawnNode(spawnEnemy);
        //    }
        //}
    }
}



