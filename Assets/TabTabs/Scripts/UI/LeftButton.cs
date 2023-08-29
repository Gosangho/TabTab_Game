using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace TabTabs.NamChanwoo
{
    public class LeftButton : MonoBehaviour
    {
        BattleSystem BattleSystemInstance2;
        private void Start()
        {
            BattleSystemInstance2 = FindObjectOfType<BattleSystem>();
        }

        public void LeftB()
        {
            BattleSystemInstance2.ClickNode = ENodeType.Left;
            Debug.Log(BattleSystemInstance2.ClickNode);
        }
    }
}
