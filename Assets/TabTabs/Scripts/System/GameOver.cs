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
            int bestScore = 0;
            string characterName = "";
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
                characterName  = DataManager.Instance.swordGirl1.characterName;
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
                characterName  = DataManager.Instance.swordGirl2.characterName;
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
                characterName  = DataManager.Instance.swordGirl3.characterName;
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
                characterName  = DataManager.Instance.leon.characterName;
                BackEndManager.Instance.SaveBestScore(DataManager.Instance.leon);
            }

            // 모든 캐릭터의 최대 스코어를 갱신 했는지 확인 
            // user 테이블에 최고 점수를 저장하는 컬럼을 추가 예정
            if(bestScore < scoreSystemInstance.swordGirl1PreviousBestScore)
            {
                bestScore = scoreSystemInstance.score;
                characterName = "Sword1";
            }
            else if(bestScore < scoreSystemInstance.swordGirl2PreviousBestScore)
            {
                bestScore = scoreSystemInstance.score;
                characterName = "Sword2";
            }
            else if(bestScore < scoreSystemInstance.swordGirl3PreviousBestScore)
            {
                bestScore = scoreSystemInstance.score;
                characterName = "Sword3";
            }
            else if(bestScore < scoreSystemInstance.leonPreviousBestScore)
            {
                bestScore = scoreSystemInstance.score;
                characterName = "leon";
            }
            
            Debug.Log("bestScore : " + bestScore + "/ scoreSystemInstance.score : " + scoreSystemInstance.score);
            if(bestScore <= scoreSystemInstance.score ) {
                BackEndManager.Instance.RankInputdate(scoreSystemInstance.score, characterName);
            }
            
        }
    }
}

