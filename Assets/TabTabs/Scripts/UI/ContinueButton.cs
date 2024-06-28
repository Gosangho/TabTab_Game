using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TabTabs.NamChanwoo
{
    public class ContinueButton : MonoBehaviour
    {
        Test3Battle test3BattleInstance;
        public static bool continueButtonClick = false; // �̾��ϱ�(1ȸ)�� �̹� ����ߴ����� ����
        public GameObject resultObj;
        public GameObject newRecordObj;
        public GameObject reStartObj;
        public PlayerBase playerBaseInstance;
        public TextMeshProUGUI resultObjText;
        public TextMeshProUGUI resultBestObjText;

        private void OnEnable()
        {
            test3BattleInstance = FindObjectOfType<Test3Battle>();
            playerBaseInstance = FindObjectOfType<PlayerBase>();
        }

        public void ContinueB()
        {
            if (continueButtonClick == false)
            {
                if (DataManager.Instance.playerData.AdsYn == 0)
                {
                    AdsManager.Instance.continueButtonInstance = this;
                    AdsManager.Instance.rewardedAdPlay();
                }
                else
                {
                    GetReward();
                }
            } 
        }

        public void GetReward() 
        {
            TImebar.timebarImage.fillAmount = 0.5f;
            reStartObj.gameObject.SetActive(false);
            resultObj.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
            test3BattleInstance.playerDie = false;
            resultObjText.gameObject.SetActive(false);
            resultBestObjText.gameObject.SetActive(false);
            newRecordObj.gameObject.SetActive(false);
            test3BattleInstance.Right_TrainAttack = false;
            test3BattleInstance.Left_TrainAttack = false;
            test3BattleInstance.FirstAttack = true;
            test3BattleInstance.FirstDashAttack = true;
            PlayerBase.PlayerAnim.SetTrigger("Continue");
            continueButtonClick = true;
            test3BattleInstance.repetition = false;
        }
    }
}

