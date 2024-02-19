using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using static BackEnd.SendQueue;
using LitJson;

public class BackEndManager : MonoBehaviour
{
    public static BackEndManager Instance { get; private set; }
    private string currentVersion = "";
    //비동기로 가입, 로그인을 할때에는 Update()에서 처리를 해야합니다. 이 값은 Update에서 구현하기 위한 플래그 값 입니다.
    BackendReturnObject bro = new BackendReturnObject();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        var bro = Backend.Initialize(true); // 뒤끝 초기화

        // 뒤끝 초기화에 대한 응답값
        if(bro.IsSuccess()) {
            Debug.Log("초기화 성공 : " + bro); // 성공일 경우 statusCode 204 Success
            // Application 버전 확인
            CheckApplicationVersion();
        } else {
            Debug.LogError("초기화 실패 : " + bro); // 실패일 경우 statusCode 400대 에러 발생
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    // Dispatcer에서 action 실행 (메인스레드)
    private void DispatcherAction(Action action)
    {
     
    }

    void backendCallback(BackendReturnObject BRO)
    {

        // 초기화 성공한 경우 실행
        if (BRO.IsSuccess())
        {
            // 구글 해시키 획득 
            if (!string.IsNullOrEmpty(Backend.Utils.GetGoogleHash()))
                Debug.Log(Backend.Utils.GetGoogleHash());

            // 서버시간 획득
            Debug.Log(Backend.Utils.GetServerTime());
            // Application 버전 확인
            CheckApplicationVersion();
        }
        // 초기화 실패한 경우 실행
        else
        {
            Debug.Log("초기화 실패 - " + BRO);
        }
    }


    private void CheckApplicationVersion()
    {
          Debug.Log("version info - ");
          LoginGuestBackend();
       /* Backend.Utils.GetLatestVersion(versionBRO =>
        {
            if (versionBRO.IsSuccess())
            {
                string latest = versionBRO.GetReturnValuetoJSON()["version"].ToString();
                Debug.Log("version info - current: " + currentVersion + " latest: " + latest);
                if (currentVersion != latest)
                {
                    int type = (int)versionBRO.GetReturnValuetoJSON()["type"];
                    // type = 1 : 선택, type = 2 : 강제
                }
                else
                {
                    // 뒷끝 게스트 로그인 시도
                    LoginGuestBackend();
                }
            }
            else
            {
                // 뒷끝 토큰 로그인 시도
                LoginGuestBackend();
            }
        });*/
    }

    public void LoginGuestBackend()
    {
        Debug.Log("로그인 callback - ");
        string id = ""; 
       // id = Backend.BMember.GetGuestID();
        Debug.Log("로컬 기기에 저장된 아이디 :" + id);

        BackendReturnObject bro = Backend.BMember.GuestLogin("게스트 로그인으로 로그인함");
        if(bro.IsSuccess())
        {
            DataManager.Instance.playerData.PlayerId = Backend.BMember.GetGuestID(); 
            Debug.Log("로컬 기기에 저장된 아이디 :" + DataManager.Instance.playerData.PlayerId);
            DBInitGetDate();
            DBInitCharacterDate();
        }   
    }

    public void DBInitGetDate() {
        Where where = new Where();
        where.Equal("PlayerId",  DataManager.Instance.playerData.PlayerId);

        var bro = Backend.GameData.GetMyData("playerData", where, 1);
        if(bro.IsSuccess() == false)
        {
            // 요청 실패 처리
            Debug.Log(bro);
            return;
        }
        if(bro.GetReturnValuetoJSON()["rows"].Count > 0)
        {
            DataManager.Instance.playerData.TutorialPlay = bool.Parse(bro.FlattenRows()[0]["TutorialPlay"].ToString());
            DataManager.Instance.playerData.MakeNickName = bool.Parse(bro.FlattenRows()[0]["MakeNickName"].ToString());
            DataManager.Instance.playerData.PlayerName = bro.FlattenRows()[0]["PlayerName"].ToString();
            DataManager.Instance.playerData.Gold = int.Parse(bro.FlattenRows()[0]["Gold"].ToString());
            DataManager.Instance.playerData.AdsYn = int.Parse(bro.FlattenRows()[0]["AdsYn"].ToString());
            DataManager.Instance.playerData.AdsDate = bro.FlattenRows()[0]["AdsDate"].ToString();
            DataManager.Instance.playerData.SwordGirl2Get = bool.Parse(bro.FlattenRows()[0]["SwordGirl2Get"].ToString());
            DataManager.Instance.playerData.SwordGirl3Get = bool.Parse(bro.FlattenRows()[0]["SwordGirl3Get"].ToString());
            DataManager.Instance.playerData.LeonGet = bool.Parse(bro.FlattenRows()[0]["LeonGet"].ToString());
            Debug.Log("PlayerAttandence : " + bro.FlattenRows()[0]["PlayerAttandence"]);

            // JsonData를 bool 배열로 변환
            bool[] boolArray = new bool[bro.FlattenRows()[0]["PlayerAttandence"].Count];
            for (int i = 0; i < bro.FlattenRows()[0]["PlayerAttandence"].Count; i++)
            {
                DataManager.Instance.playerData.PlayerAttandence[i] = (bool)bro.FlattenRows()[0]["PlayerAttandence"][i];
            }
        }
    }

    public void DBInitCharacterDate() {
        Where where = new Where();
        where.Equal("PlayerId",  DataManager.Instance.playerData.PlayerId);

        var bro = Backend.GameData.GetMyData("CharacterData", where, 10);
        if(bro.IsSuccess() == false)
        {
            // 요청 실패 처리
            Debug.Log(bro);
            return;
        }
        if(bro.GetReturnValuetoJSON()["rows"].Count > 0)
        {
            for (int i = 0; i < bro.GetReturnValuetoJSON()["rows"].Count; i++) {
                string characterName = bro.FlattenRows()[i]["CharacterName"].ToString();

                if("Sword1".Equals(characterName)) {
                    DataManager.Instance.swordGirl1.bestScore = int.Parse(bro.FlattenRows()[i]["BestScore"].ToString());
                    DataManager.Instance.swordGirl1.totalKillScore = int.Parse(bro.FlattenRows()[i]["TotalKillScore"].ToString());
                } else if("Sword2".Equals(characterName)) {
                    DataManager.Instance.swordGirl2.bestScore = int.Parse(bro.FlattenRows()[i]["BestScore"].ToString());
                    DataManager.Instance.swordGirl2.totalKillScore = int.Parse(bro.FlattenRows()[i]["TotalKillScore"].ToString());
                } else if("Sword3".Equals(characterName)) {
                    DataManager.Instance.swordGirl3.bestScore = int.Parse(bro.FlattenRows()[i]["BestScore"].ToString());
                    DataManager.Instance.swordGirl3.totalKillScore = int.Parse(bro.FlattenRows()[i]["TotalKillScore"].ToString());
                } else if("Leon".Equals(characterName)) {
                    DataManager.Instance.leon.bestScore = int.Parse(bro.FlattenRows()[i]["BestScore"].ToString());
                    DataManager.Instance.leon.totalKillScore = int.Parse(bro.FlattenRows()[i]["TotalKillScore"].ToString());
                }
            }
        }
    }

    public void DbSaveGameData()
    {
        // 해당 계정의 기존 데이터가 있는지 확인
        Where where = new Where();
        where.Equal("PlayerId",  DataManager.Instance.playerData.PlayerId);

        Debug.Log("DbSaveGameData::serverwhereStart");

        var bro = Backend.GameData.GetMyData("playerData", where, 1);
        if(bro.IsSuccess() == false)
        {
            // 요청 실패 처리
            Debug.Log(bro);
            return;
        }
        if(bro.GetReturnValuetoJSON()["rows"].Count <= 0)
        {
            Debug.Log("DbSaveGameData::serveInsertrStart");
            Param intParam = new Param();
            intParam.Add("PlayerId", DataManager.Instance.playerData.PlayerId);
            intParam.Add("TutorialPlay", DataManager.Instance.playerData.TutorialPlay);
            intParam.Add("MakeNickName", DataManager.Instance.playerData.MakeNickName);
            intParam.Add("PlayerName", DataManager.Instance.playerData.PlayerName);
            intParam.Add("Gold", DataManager.Instance.playerData.Gold);
            intParam.Add("AdsYn", DataManager.Instance.playerData.AdsYn);
            intParam.Add("AdsDate", DataManager.Instance.playerData.AdsDate);
            intParam.Add("SwordGirl2Get", DataManager.Instance.playerData.SwordGirl2Get);
            intParam.Add("SwordGirl3Get", DataManager.Instance.playerData.SwordGirl3Get);
            intParam.Add("LeonGet", DataManager.Instance.playerData.LeonGet);
            intParam.Add("PlayerAttandence", DataManager.Instance.playerData.PlayerAttandence);

            Backend.GameData.Insert("playerData", intParam, (callback) => {
                Debug.Log("내 playerInfo의 indate : " + callback);
            });
            return;
        } else {
            Debug.Log("DbSaveGameData::serverUpdateStart");
            Param upParam = new Param();
            upParam.Add("PlayerId", DataManager.Instance.playerData.PlayerId);
            upParam.Add("TutorialPlay", DataManager.Instance.playerData.TutorialPlay);
            upParam.Add("MakeNickName", DataManager.Instance.playerData.MakeNickName);
            upParam.Add("PlayerName", DataManager.Instance.playerData.PlayerName);
            upParam.Add("Gold", DataManager.Instance.playerData.Gold);
            upParam.Add("AdsYn", DataManager.Instance.playerData.AdsYn);
            upParam.Add("AdsDate", DataManager.Instance.playerData.AdsDate);
            upParam.Add("SwordGirl2Get", DataManager.Instance.playerData.SwordGirl2Get);
            upParam.Add("SwordGirl3Get", DataManager.Instance.playerData.SwordGirl3Get);
            upParam.Add("LeonGet", DataManager.Instance.playerData.LeonGet);
            upParam.Add("PlayerAttandence", DataManager.Instance.playerData.PlayerAttandence);

            Backend.GameData.Update("playerData", where, upParam, (callback) => {
                Debug.Log("내 playerInfo의 update : " + callback);
            });
        }
    }

    public void SaveBestScore(CharacterData characterData)
    {
        // 해당 계정의 기존 데이터가 있는지 확인
        Where where = new Where();
        where.Equal("PlayerId",  DataManager.Instance.playerData.PlayerId);
        where.Equal("CharacterName",  characterData.characterName);


        var bro = Backend.GameData.GetMyData("CharacterData", where, 1);
        if(bro.IsSuccess() == false)
        {
            // 요청 실패 처리
            Debug.Log(bro);
            return;
        }
        if(bro.GetReturnValuetoJSON()["rows"].Count <= 0)
        {
            Param intParam = new Param();
            intParam.Add("PlayerId", DataManager.Instance.playerData.PlayerId);
            intParam.Add("CharacterName", characterData.characterName);
            intParam.Add("BestScore", characterData.bestScore);
            intParam.Add("TotalKillScore", characterData.totalKillScore);

            Backend.GameData.Insert("CharacterData", intParam, (callback) => {
                Debug.Log("내 playerInfo의 indate : " + callback);
            });
            return;
        } else {
            Param upParam = new Param();
            upParam.Add("PlayerId", DataManager.Instance.playerData.PlayerId);
            upParam.Add("CharacterName", characterData.characterName);
            upParam.Add("BestScore", characterData.bestScore);
            upParam.Add("TotalKillScore", characterData.totalKillScore);

            Backend.GameData.Update("CharacterData", where, upParam, (callback) => {
                Debug.Log("내 playerInfo의 update : " + callback);
            });
        }
    }
}
