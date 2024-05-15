using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


namespace TabTabs.NamChanwoo
{
    public class EnemyBase : CharacterBase
    {
        private NodeArea m_nodeArea;
        
        public NodeArea nodeArea => m_nodeArea;

        private Queue<Node> m_nodeQueue = new Queue<Node>();
        //public Test2BattleSystem BattleInstance2;
        public Test3Battle BattleInstance3;
        public Test4Battle BattleInstance4;
        [Header("Attack Properties")]
        public Slider m_attackGaugeSlider;
        [FormerlySerializedAs("m_chargAttackGauge")] [SerializeField] private float m_maxAttackGauge = 10.0f; 
        private float m_attackGauge = 10.0f; // 공격 쿨다운
        public GameObject RightOrc2Die;
        public GameObject LeftOrc2Die;
        
        
        public float AttackGauge
        {
            get => CurrentState == ECharacterState.Attacking ? 0.0f : m_attackGauge;
            set
            {
                m_attackGauge = Mathf.Clamp(value, 0.0f, m_maxAttackGauge);
                UpdateSliderAttackUI();
            }
        }

        
        [Header("Movement Settings")]
        [SerializeField] protected float m_moveSpeed = 4.0f;
        
        private void Awake()
        {
            m_nodeArea = GetComponentInChildren<NodeArea>();
        }
        
        private void Start()
        {
            if (m_rigidbody ==null)
            {
                m_rigidbody = GetComponent<Rigidbody2D>();
            }
            BattleInstance3 = FindObjectOfType<Test3Battle>();
            BattleInstance4 = FindObjectOfType<Test4Battle>();
            //BattleInstance2= FindObjectOfType<Test2BattleSystem>();
        }

        protected void Update()
        {
            /*Debug.Log("EnemyBase Update : " + CurrentState);*/
        }

        protected void FixedUpdate()
        {
            if (m_rigidbody== null)
                return;
            
            if (m_movementDirection == Vector2.zero)
            {
                m_rigidbody.velocity = Vector2.zero;
                
                if (CurrentState != ECharacterState.Die && CurrentState != ECharacterState.Attacking)
                {
                    SetState(ECharacterState.Idle);
                }
            }
            else
            {
                Vector2 newPosition = m_rigidbody.position + m_movementDirection * (m_moveSpeed * Time.fixedDeltaTime);
                m_rigidbody.MovePosition(newPosition);

            }
        }
        
        public void SetupAttackSliderUI(Slider attackSliderUI)
        {
            if (m_attackGaugeSlider ==null)
            {
                m_attackGaugeSlider = attackSliderUI;
                m_attackGauge = m_maxAttackGauge;
                m_attackGaugeSlider.maxValue = m_maxAttackGauge;
                m_attackGaugeSlider.value = m_attackGauge;
            }
        }

        private void UpdateSliderAttackUI()
        {
            if (m_attackGaugeSlider != null)
            {
                m_attackGaugeSlider.maxValue = m_maxAttackGauge;
                m_attackGaugeSlider.value = m_attackGauge;
            }
        }
        
        virtual public void Attack()
        {
            if (CurrentState != ECharacterState.Die)
            {
                SetState(ECharacterState.Attacking);
                AttackGauge = m_maxAttackGauge;
            }
        }

        public void AddNodes(Node spawnedNode)
        {
            m_nodeQueue.Enqueue(spawnedNode);
        }

        public Queue<Node> GetOwnNodes()
        {
            return m_nodeQueue;
        }
        
        public void IncreaseAttackGauge(float amount)
        {
            AttackGauge += amount;
        }
        
        public void HitIncreaseAttackGauge()
        {
            AttackGauge =AttackGauge + (m_maxAttackGauge * 0.1f);
        }
        
        public bool IsAttackGaugeEmpty()
        {
            return !(AttackGauge > 0.0f);
        }

        public void Hit()
        {
            SetState(ECharacterState.Hit);
            HitIncreaseAttackGauge();
        }
        
        public void Die()
        {
            SetState(ECharacterState.Die);

            BoxCollider2D orcCollider = GetComponent<BoxCollider2D>();

            //if (SceneManager.GetActiveScene().buildIndex == 1)
            //{
            //    if (BattleInstance2.selectEnemy == BattleInstance2.RightEnemy)
            //    {
            //        BattleInstance2.MonsterDie = true;
            //    }
            //}
            //if (SceneManager.GetActiveScene().buildIndex == 3)
            //{

            if(BattleInstance3 != null) {
                if (BattleInstance3.selectEnemy == BattleInstance3.RightEnemy)
                {
                    BattleInstance3.Right_MonsterDie = true;
                    BattleInstance3.Right_TrainAttack = true;
                    BattleInstance3.FirstDashAttack = false;
                    Destroy(gameObject);
                    Test3Spawn.Instance.RightAttackNum = 0; // 리셋
                    GameObject DieEffect = Instantiate(RightOrc2Die);
                    //Right_Orc2_Anim.RightAnim.SetTrigger("Die");
                    //StartCoroutine(Right_MonsterDie());
                }
                else
                {
                    BattleInstance3.Left_MonsterDie = true;
                    BattleInstance3.Left_TrainAttack = true;
                    BattleInstance3.FirstDashAttack = false;
                    Destroy(gameObject);
                    Test3Spawn.Instance.LeftAttackNum = 0; // 리셋
                    GameObject DieEffect = Instantiate(LeftOrc2Die);
                    //Left_Orc2_Anim.LeftAnim.SetTrigger("Die");
                    //StartCoroutine(Left_MonsterDie());
                }    
            }

            if(BattleInstance4 != null) {
                if (BattleInstance4.selectEnemy == BattleInstance4.RightEnemy)
                {
                    BattleInstance4.Right_MonsterDie = true;
                    BattleInstance4.Right_TrainAttack = true;
                    BattleInstance4.FirstDashAttack = false;
                    Destroy(gameObject);
                    Test3Spawn.Instance.RightAttackNum = 0; // 리셋
                    GameObject DieEffect = Instantiate(RightOrc2Die);
                    //Right_Orc2_Anim.RightAnim.SetTrigger("Die");
                    //StartCoroutine(Right_MonsterDie());
                }
                else
                {
                    BattleInstance4.Left_MonsterDie = true;
                    BattleInstance4.Left_TrainAttack = true;
                    BattleInstance4.FirstDashAttack = false;
                    Destroy(gameObject);
                    Test3Spawn.Instance.LeftAttackNum = 0; // 리셋
                    GameObject DieEffect = Instantiate(LeftOrc2Die);
                    //Left_Orc2_Anim.LeftAnim.SetTrigger("Die");
                    //StartCoroutine(Left_MonsterDie());
                }    
            }
            
            //}
            
            //StartCoroutine(Right_MonsterDie());
            //Destroy(gameObject);

            
            GameManager.NotificationSystem.SceneMonsterDeath?.Invoke(this);
        }
        //private IEnumerator Right_MonsterDie()
        //{
        //    float DieAnimDuration = 0.85f;
        //    yield return new WaitForSeconds(DieAnimDuration);
        //    Destroy(gameObject);
        //}
        //private IEnumerator Left_MonsterDie()
        //{
        //    float DieAnimDuration = 0.85f;
        //    yield return new WaitForSeconds(DieAnimDuration);
        //    Destroy(gameObject);
        //}
    }
}