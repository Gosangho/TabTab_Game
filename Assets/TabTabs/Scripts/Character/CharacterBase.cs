using System;
using UnityEngine;


namespace TabTabs.NamChanwoo
{
    public enum ECharacterState
    {
        Idle,
        Running,
        Hit,
        Attacking,
        Die
    }
    
    public enum EDirection
    {
        Left,
        Right,
    }

    public class CharacterBase : MonoBehaviour
    {
        public Rigidbody2D m_rigidbody;
        
        protected Vector2 m_movementDirection;

        private ECharacterState m_currentState;
        
        public ECharacterState CurrentState
        {
            get => m_currentState;
            private set
            {
                if (m_currentState != value)
                {
                    m_currentState = value;
                    ChangeState?.Invoke(m_currentState);
                }
            }
        }
        
        public event Action<ECharacterState> ChangeState;

        public bool IsMovingUp() => m_movementDirection.y > 0.0f;
        public bool IsMovingDown() => m_movementDirection.y < 0.0f;
        public bool IsMovingLeft() => m_movementDirection.x < 0.0f;
        public bool IsMovingRight() => m_movementDirection.x > 0.0f;
        public bool IsMoving() => m_movementDirection.magnitude > 0.0f;


        private void Awake()
        {
            if (m_rigidbody ==null)
            {
                m_rigidbody = GetComponent<Rigidbody2D>();
            }
        }

        private void Start()
        {
          
        }
        
        protected void FixedUpdate()
        {
        }
        
        public void SetState(ECharacterState newState)
        {
            if (m_currentState != newState)
            {
                m_currentState = newState;
                ChangeState?.Invoke(m_currentState);
            }
        }
        
        virtual public void Attack()
        {
            
        }
        
        public void SetMovementDirection(Vector2 direction)
        {
            m_movementDirection = direction;
        }

        virtual public void Die()
        {
            
        }
    }

}