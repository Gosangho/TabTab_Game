using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace TabTabs.NamChanwoo
{
    public class RightButton : MonoBehaviour
    {
        BattleSystem BattleSystemInstance3;
        private void Start()
        {
            BattleSystemInstance3 = FindObjectOfType<BattleSystem>();
        }

        public void RightB()
        {
            BattleSystemInstance3.ClickNode = ENodeType.Right;
            Debug.Log(BattleSystemInstance3.ClickNode);
        }
    }
}
