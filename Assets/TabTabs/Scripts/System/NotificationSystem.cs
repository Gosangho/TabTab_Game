
using TabTabs.NamChanwoo;
using UnityEngine;
using UnityEngine.Events;


public class NotificationSystem : GameSystem
{
    [Header("Gameplay Events")]
    public UnityEvent<EnemyBase> SceneMonsterSpawned = new UnityEvent<EnemyBase>();
    
    [Header("Gameplay Events")]
    public UnityEvent<EnemyBase> SceneMonsterDeath = new UnityEvent<EnemyBase>();

    [Header("Gameplay Events")]
    public UnityEvent NodeHitSuccess = new UnityEvent();
    
    [Header("Gameplay Events")]
    public UnityEvent NodeHitFail = new UnityEvent();
}