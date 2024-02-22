using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using LitJson;

public class RankingManager : MonoBehaviour
{
    public TextMeshProUGUI bestScore;
    public GameObject rankScoreList;
    // Start is called before the first frame update
    void Start()
    {
        LitJson.JsonData rankListJson = BackEndManager.Instance.GetBestScore();
        LitJson.JsonData rankMyJson = BackEndManager.Instance.GetMyBestScore();

        if(rankMyJson != null) {
            if(rankMyJson["rows"].Count > 0)
            {
                string rank = rankMyJson["rows"][0]["rank"].ToString();
                string nickname = "";
                 Debug.Log(rankMyJson["rows"][0].ToString());
                // 첫 번째 'rows' 객체에 접근
                LitJson.JsonData firstRow = rankListJson["rows"][0];

                // 'nickname' 키가 있는지 확인
                bool hasNickname = firstRow["nickname"] != null;

                if(hasNickname) {
                    nickname = rankListJson["rows"][0]["nickname"].ToString().Length > 6 ? rankListJson["rows"][0]["nickname"].ToString().Substring(0, 6) + "..." : rankListJson["rows"][0]["nickname"].ToString() ;
                }
                string score = rankMyJson["rows"][0]["score"].ToString();
                
                bestScore.text = rankMyJson["rows"][0]["rank"].ToString() + "  " + nickname + "  " +rankMyJson["rows"][0]["score"].ToString();
            }
        }


        if(rankListJson != null)
        {
            for(int i = 0; i < rankListJson["rows"].Count; i++)
            {
                if(i < rankScoreList.transform.childCount)
                {
                    // i번째 자식 Transform을 가져옵니다.
                    Transform child = rankScoreList.transform.GetChild(i);

                    Transform scoreRankTransform = child.Find("Score_Rank");
                    Transform scoreIDTransform = child.Find("Score_ID");
                    Transform scoreScoreTransform = child.Find("Score_Score");

                    // 해당 자식에서 TextMeshProUGUI 컴포넌트를 찾습니다.
                    TextMeshProUGUI scoreRankText = scoreRankTransform.GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI scoreIDText = scoreIDTransform.GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI scoreScoreText = scoreScoreTransform.GetComponent<TextMeshProUGUI>();


                    if (scoreRankText != null)
                    {
                        string nickname = "";
                        Debug.Log(rankListJson["rows"][i].ToString());

                        // 첫 번째 'rows' 객체에 접근
                        LitJson.JsonData firstRow = rankListJson["rows"][0];

                        // 'nickname' 키가 있는지 확인
                        bool hasNickname = firstRow["nickname"] != null;

                        if(hasNickname) {
                            nickname = rankListJson["rows"][0]["nickname"].ToString().Length > 6 ? rankListJson["rows"][0]["nickname"].ToString().Substring(0, 6) + "..." : rankListJson["rows"][0]["nickname"].ToString() ;
                        }
                        // TextMeshProUGUI의 텍스트를 rankListJson에서 가져온 값으로 설정합니다.
                        scoreRankText.text = rankListJson["rows"][i]["rank"].ToString(); 
                        scoreIDText.text = nickname; 
                        scoreScoreText.text =  rankListJson["rows"][i]["score"].ToString();
                    }
                }
            }
        }
    }

}
