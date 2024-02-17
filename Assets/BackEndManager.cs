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
       Enqueue(Backend.BMember.GuestLogin, "게스트 로그인으로 로그인함", callback => {
            if(callback.IsSuccess())
            {
                Debug.Log("게스트 로그인에 성공했습니다");
            }
       });
    }
}
