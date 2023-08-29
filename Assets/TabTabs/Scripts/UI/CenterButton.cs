using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace TabTabs.NamChanwoo
{
    public class CenterButton : MonoBehaviour
    {
        BattleSystem BattleSystemInstance1;
        private void Start()
        {
            BattleSystemInstance1 = FindObjectOfType<BattleSystem>();
        }

        public void CenterB()
        {
            BattleSystemInstance1.ClickNode = ENodeType.Up;
            Debug.Log(BattleSystemInstance1.ClickNode);
        }
    }
}

