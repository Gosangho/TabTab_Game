using System;
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
            int inputScore = 0;
            bool isRecord = true;

            string characterName = "";
            test3BattleInstance = FindObjectOfType<Test3Battle>();
            scoreSystemInstance = FindObjectOfType<ScoreSystem>();
            dataManagerInstance = FindObjectOfType<DataManager>();

            resultScore.gameObject.SetActive(true);
            resultBestScore.gameObject.SetActive(true);

            resultScore.text = "Score : " + scoreSystemInstance.score.ToString();


            //최대 스코어를 갱신여부 확인
            if(scoreSystemInstance.score < scoreSystemInstance.swordGirl1PreviousBestScore)
            {
                isRecord = false;
            }
            if(scoreSystemInstance.score < scoreSystemInstance.swordGirl2PreviousBestScore)
            {
                isRecord = false;
            }
            if(scoreSystemInstance.score < scoreSystemInstance.swordGirl3PreviousBestScore)
            {
                isRecord = false;
            }
            if(scoreSystemInstance.score < scoreSystemInstance.leonPreviousBestScore)
            {
                isRecord = false;
            }
  

            if (SelectCharacter.swordGirl1)
            {
                if (test3BattleInstance.playerDie == true && scoreSystemInstance.swordGirl1PreviousBestScore < scoreSystemInstance.score)
                {
                    newRecordObj.gameObject.SetActive(true);
                    resultBestScore.text = "Best Score : " + scoreSystemInstance.score;
                    inputScore = scoreSystemInstance.score;
                }
                else
                {
                    newRecordObj.gameObject.SetActive(false);
                    resultBestScore.text = "Best Score : " + scoreSystemInstance.swordGirl1PreviousBestScore;
                    inputScore = scoreSystemInstance.swordGirl1PreviousBestScore;
                }

                DataManager.Instance.swordGirl1.characterName = "Lana";
                characterName  = DataManager.Instance.swordGirl1.characterName;
                BackEndManager.Instance.SaveBestScore(DataManager.Instance.swordGirl1, inputScore, isRecord);
              
            }
            else if (SelectCharacter.swordGirl2)
            {
                if (test3BattleInstance.playerDie == true && scoreSystemInstance.swordGirl2PreviousBestScore < scoreSystemInstance.score)
                {
                    newRecordObj.gameObject.SetActive(true);
                    resultBestScore.text = "Best Score : " + scoreSystemInstance.score;
                    inputScore = scoreSystemInstance.score;
                }
                else
                {
                    newRecordObj.gameObject.SetActive(false);
                    resultBestScore.text = "Best Score : " + scoreSystemInstance.swordGirl2PreviousBestScore;
                    inputScore = scoreSystemInstance.swordGirl2PreviousBestScore;
                }
                
                DataManager.Instance.swordGirl2.characterName = "Sia";
                characterName  = DataManager.Instance.swordGirl2.characterName;
                BackEndManager.Instance.SaveBestScore(DataManager.Instance.swordGirl2, inputScore, isRecord);

            }
            else if (SelectCharacter.swordGirl3)
            {
                if (test3BattleInstance.playerDie == true && scoreSystemInstance.swordGirl3PreviousBestScore < scoreSystemInstance.score)
                {
                    newRecordObj.gameObject.SetActive(true);
                    resultBestScore.text = "Best Score : " + scoreSystemInstance.score;
                    inputScore = scoreSystemInstance.score;
                }
                else
                {
                    newRecordObj.gameObject.SetActive(false);
                    resultBestScore.text = "Best Score : " + scoreSystemInstance.swordGirl3PreviousBestScore;
                    inputScore = scoreSystemInstance.swordGirl3PreviousBestScore;
                }

                DataManager.Instance.swordGirl3.characterName = "Zena";
                characterName  = DataManager.Instance.swordGirl3.characterName;
                BackEndManager.Instance.SaveBestScore(DataManager.Instance.swordGirl3, inputScore ,isRecord);
            }
            else
            {
                if (test3BattleInstance.playerDie == true && scoreSystemInstance.leonPreviousBestScore < scoreSystemInstance.score)
                {
                    newRecordObj.gameObject.SetActive(true);
                    resultBestScore.text = "Best Score : " + scoreSystemInstance.score;
                    inputScore = scoreSystemInstance.score;
                }
                else
                {
                    newRecordObj.gameObject.SetActive(false);
                    resultBestScore.text = "Best Score : " + scoreSystemInstance.leonPreviousBestScore;
                    inputScore = scoreSystemInstance.leonPreviousBestScore;
                }
                DataManager.Instance.leon.characterName = "leon";
                characterName  = DataManager.Instance.leon.characterName;
                BackEndManager.Instance.SaveBestScore(DataManager.Instance.leon, inputScore ,isRecord);
               
            }
            
        }
    }
}

