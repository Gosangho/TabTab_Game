using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TabTabs.NamChanwoo
{
    public class GameOver : MonoBehaviour
    {
        public Button continue_Button;
        public GameObject newRecordObj;
        ScoreSystem scoreSystemInstance;
        Test3Battle test3BattleInstance;
        DataManager dataManagerInstance;
        public TextMeshProUGUI resultScore;
        public TextMeshProUGUI resultBestScore;

        private void OnEnable()
        {// ������Ʈ�� Ȱ��ȭ�Ǹ� ����Ǵ� �Լ�
            test3BattleInstance = FindObjectOfType<Test3Battle>();
            scoreSystemInstance = FindObjectOfType<ScoreSystem>();
            dataManagerInstance = FindObjectOfType<DataManager>();

            resultScore.gameObject.SetActive(true);
            resultBestScore.gameObject.SetActive(true);

            resultScore.text = "Score : " + scoreSystemInstance.score.ToString();

            if (SelectCharacter.swordGirl1)
            {
                resultBestScore.text = "Best Score : " + scoreSystemInstance.swordGirl1PreviousBestScore.ToString();
                if (test3BattleInstance.playerDie == true && scoreSystemInstance.swordGirl1PreviousBestScore < scoreSystemInstance.score)
                {
                    newRecordObj.gameObject.SetActive(true);
                }
                else
                {
                    newRecordObj.gameObject.SetActive(false);
                }
                DataManager.Instance.swordGirl1.characterName = "Sword1";
                BackEndManager.Instance.SaveBestScore(DataManager.Instance.swordGirl1);
            }
            else if (SelectCharacter.swordGirl2)
            {
                resultBestScore.text = "Best Score : " + scoreSystemInstance.swordGirl2PreviousBestScore.ToString();
                if (test3BattleInstance.playerDie == true && scoreSystemInstance.swordGirl2PreviousBestScore < scoreSystemInstance.score)
                {
                    newRecordObj.gameObject.SetActive(true);
                }
                else
                {
                    newRecordObj.gameObject.SetActive(false);
                }
                DataManager.Instance.swordGirl2.characterName = "Sword2";
                BackEndManager.Instance.SaveBestScore(DataManager.Instance.swordGirl2);
            }
            else if (SelectCharacter.swordGirl3)
            {
                resultBestScore.text = "Best Score : " + scoreSystemInstance.swordGirl3PreviousBestScore.ToString();
                if (test3BattleInstance.playerDie == true && scoreSystemInstance.swordGirl3PreviousBestScore < scoreSystemInstance.score)
                {
                    newRecordObj.gameObject.SetActive(true);
                }
                else
                {
                    newRecordObj.gameObject.SetActive(false);
                }
                DataManager.Instance.swordGirl3.characterName = "Sword3";
                BackEndManager.Instance.SaveBestScore(DataManager.Instance.swordGirl3);
            }
            else
            {
                resultBestScore.text = "Best Score : " + scoreSystemInstance.leonPreviousBestScore.ToString();
                if (test3BattleInstance.playerDie == true && scoreSystemInstance.leonPreviousBestScore < scoreSystemInstance.score)
                {
                    newRecordObj.gameObject.SetActive(true);
                }
                else
                {
                    newRecordObj.gameObject.SetActive(false);
                }
                DataManager.Instance.leon.characterName = "leon";
                BackEndManager.Instance.SaveBestScore(DataManager.Instance.leon);
            }
        }
    }
}

