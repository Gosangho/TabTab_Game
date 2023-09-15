using System;
using System.Collections;
using System.Collections.Generic;
using TabTabs.NamChanwoo;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TabTabs.NamChanwoo
{
    
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance = null;

        public static GameManager Instance => _instance;

        private Dictionary<Type, GameSystem> m_systems = null;

        // System Access Shortcuts
        public static SpawnSystem SpawnSystem => GetSystem<SpawnSystem>();
        public static NotificationSystem NotificationSystem => GetSystem<NotificationSystem>();
        //public static UISystem UISystem => GetSystem<UISystem>();
        public static BattleSystem BattleSystem => GetSystem<BattleSystem>();


        private void Awake()
        {
            _instance = this;

            FindSystems();
            InitializeSystems();
        }


        private void OnEnable()
        {
            StartSystems();
        }

        private void OnDisable()
        {
            StopSystems();
        }

        private void FindSystems()
        {
            GameSystem[] systems = FindObjectsOfType<GameSystem>();

            m_systems = new Dictionary<Type, GameSystem>();

            foreach (GameSystem system in systems)
            {
                Type type = system.GetType();
                Debug.Assert(!m_systems.ContainsKey(type), $"게임 시스템 {type.Name}이(가) 이미 등록되었습니다");
                m_systems[type] = system;
            }
        }

        private void InitializeSystems()
        {
            foreach (GameSystem system in m_systems.Values)
            {
                system.OnSystemInit();
            }
        }

        private void StartSystems()
        {
            foreach (GameSystem system in m_systems.Values)
            {
                system.OnSystemStart();
            }
        }

        private void StopSystems()
        {
            foreach (GameSystem system in m_systems.Values)
            {
                system.OnSystemStop();
            }
        }

        public static T GetSystem<T>() where T : GameSystem
        {
            GameSystem system = null;
            bool systemFound = _instance.m_systems.TryGetValue(typeof(T), out system);
            Debug.Assert(systemFound, $"게임 시스템 {typeof(T).Name}을(를) 찾을 수 없습니다.");
            return (T)system;
        }


    }

}