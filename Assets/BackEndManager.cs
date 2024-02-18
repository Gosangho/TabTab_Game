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
        Backend.Utils.GetLatestVersion(versionBRO =>
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
        });
    }

    public void LoginGuestBackend()
    {
        Debug.Log("로그인 callback - ");
        string id = ""; 
        id = Backend.BMember.GetGuestID();
        Debug.Log("로컬 기기에 저장된 아이디 :" + id);
        
        if("".Equals(id)) {            
            Backend.BMember.GuestLogin("게스트 로그인으로 로그인함", callback => {
                Debug.Log("로그인 callback111 - ");
                if(callback.IsSuccess())
                {
                    DataManager.Instance.playerData.PlayerId = id;    
                    DataManager.Instance.SaveGameData();
                }
            });
        } else {
            DataManager.Instance.playerData.PlayerId = id;    
            DataManager.Instance.SaveGameData();
        }
    }

    public void DbSaveGameData()
    {
        Debug.Log("DbSaveGameData::serverStart");
        Param param = new Param();
        param.Add("PlayerId", DataManager.Instance.playerData.PlayerId);
        param.Add("TutorialPlay", DataManager.Instance.playerData.TutorialPlay);
        param.Add("MakeNickName", DataManager.Instance.playerData.MakeNickName);
        param.Add("PlayerName", DataManager.Instance.playerData.PlayerName);
        param.Add("Gold", DataManager.Instance.playerData.Gold);
        param.Add("AdsYn", DataManager.Instance.playerData.AdsYn);
        param.Add("AdsDate", DataManager.Instance.playerData.AdsDate);
        param.Add("SwordGirl2Get", DataManager.Instance.playerData.SwordGirl2Get);
        param.Add("SwordGirl3Get", DataManager.Instance.playerData.SwordGirl3Get);
        param.Add("LeonGet", DataManager.Instance.playerData.LeonGet);
        param.Add("PlayerAttandence", DataManager.Instance.playerData.PlayerAttandence);

        Debug.Log("DbSaveGameData::DataInputEnd");

        var bro = Backend.GameData.Insert("playerData", param);

        if(bro.IsSuccess())
        {
            string playerInfoIndate = bro.GetInDate();
            Debug.Log("내 playerInfo의 indate : " + playerInfoIndate);
        }
        else
        {
            Debug.LogError("게임 정보 삽입 실패 : " + bro.ToString());
        }
    }
}
