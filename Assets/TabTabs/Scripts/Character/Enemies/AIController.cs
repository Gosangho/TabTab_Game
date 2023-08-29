using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace TabTabs.NamChanwoo
{
    public class AIController : MonoBehaviour
    {
        [FormerlySerializedAs("m_character")]
        [Header("References")]
        [SerializeField] private EnemyBase m_enemyCharacter = null; // AI 캐릭터 참조
        
        [Header("Target Following Settings")]
        [SerializeField] private Vector2 m_detectionArea = new Vector2(5f, 5f); // 플레이어 감지 영역
        [SerializeField] private float m_targetDistance = 1.0f; // 표적 추적 거리
        [SerializeField] private float m_soughtDistanceFromTarget = 1.0f; // 이 거리에 따라 표적과의 거리를 유지합니다.
        private bool m_isCoroutineAttack = false;
        
        private CharacterBase m_target = null; // AI 캐릭터의 목표
        
        
        private void Start()
        {
            if (m_enemyCharacter == null)
                m_enemyCharacter = GetComponent<EnemyBase>();
        }
        
        private void FixedUpdate() // 고정 업데이트 함수 (물리 계산용)
        {
            if (!m_target)
                m_target = FindTarget();

            // 대상이 존재하는 경우
            if (m_target)
            {
                Vector2 direction = GetTargetMovementDirection();
            
                if (m_targetDistance <= m_soughtDistanceFromTarget)
                {
                    m_enemyCharacter.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    m_enemyCharacter.SetMovementDirection(Vector2.zero);
                    
                    if (!m_isCoroutineAttack)
                        AttackProcess();
                        
                }
                else
                {
                    m_enemyCharacter.SetMovementDirection(direction);    
                }
            }
        }
        
        public IEnumerator CountdownAttackGauge()
        {
            m_isCoroutineAttack = true;
            
            while( m_enemyCharacter.AttackGauge > 0)
            {
                yield return new WaitForSeconds(Time.fixedDeltaTime);
                m_enemyCharacter.IncreaseAttackGauge(-Time.fixedDeltaTime);
            }
            
            m_enemyCharacter.Attack();
            m_isCoroutineAttack = false;
        }
        
        private CharacterBase FindTarget()
        {
            Vector2 pointA = (Vector2)transform.position - m_detectionArea / 2;
            Vector2 pointB = (Vector2)transform.position + m_detectionArea / 2;
            
            Collider2D hit = Physics2D.OverlapArea(pointA, pointB, LayerManager.PlayerLayerMask);
            
            if (hit != null)
            {
                CharacterBase character = hit.transform.GetComponent<CharacterBase>();
                if(character != null)
                    return character;
            }
            return null; // 감지 영역 내에 물체가 없으면 null을 반환합니다.
        }
        
        private Vector2 GetTargetPosition() 
        {
            return m_target ? (Vector2)m_target.gameObject.transform.position : (Vector2)transform.position;
        }
        
        private Vector2 GetTargetMovementDirection()
        {
            Vector2 targetPosition = GetTargetPosition(); // 목표물의 위치 파악
            Vector2 currentPosition = transform. position; // 현재 위치 얻기
            Vector2 targetMovementDirection = targetPosition - currentPosition; // 대상의 이동 방향 계산
            
            m_targetDistance = targetMovementDirection.magnitude;
            
            targetMovementDirection.Normalize();
            targetMovementDirection.y = 0; // 왼쪽 또는 오른쪽으로만 이동하려면 y 값을 0으로 설정합니다.
    
            return targetMovementDirection;
        }
        
        private void AttackProcess()
        {
            if(m_enemyCharacter.CurrentState != ECharacterState.Attacking && m_enemyCharacter.CurrentState != ECharacterState.Die)
            {
                StartCoroutine(CountdownAttackGauge());
            }
        }
        
        private void OnDrawGizmosSelected()
        {
            // 감지 영역을 나타내는 사각형을 그립니다.
            Gizmos.color = Color.red;
            Vector2 pointA = (Vector2)transform.position - m_detectionArea / 2;
            Vector2 pointB = (Vector2)transform.position + m_detectionArea / 2;

            Gizmos.DrawLine(new Vector2(pointA.x, pointA.y), new Vector2(pointB.x, pointA.y));
            Gizmos.DrawLine(new Vector2(pointA.x, pointA.y), new Vector2(pointA.x, pointB.y));
            Gizmos.DrawLine(new Vector2(pointB.x, pointB.y), new Vector2(pointB.x, pointA.y));
            Gizmos.DrawLine(new Vector2(pointB.x, pointB.y), new Vector2(pointA.x, pointB.y));
        }
        
    }
}