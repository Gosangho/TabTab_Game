using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEditor;
using BackEnd;
using static BackEnd.SendQueue;
using LitJson;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SceneManagement;


public class BackEndManager : MonoBehaviour
{
    public static BackEndManager Instance { get; private set; }
    private string currentVersion = "";
    static string RANK_UUID = "9a99a7a0-cfdc-11ee-b948-17c19bb75403";
    //비동기로 가입, 로그인을 할때에는 Update()에서 처리를 해야합니다. 이 값은 Update에서 구현하기 위한 플래그 값 입니다.
    BackendReturnObject bro = new BackendReturnObject();

    [SerializeField] private string uuid;

    public string UUID => uuid;

    public GameObject errorText;

    private void Awake()
    {
        if (string.IsNullOrEmpty(uuid))
        {
            uuid = System.Guid.NewGuid().ToString();
           // Debug.Log($"Generated UUID for {gameObject.name}: {uuid}");
        }

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

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    // Dispatcer에서 action 실행 (메인스레드)
    private void DispatcherAction(Action action)
    {
     
    }

    public void broInit()
    {
        //구글 로그인
       /* PlayGamesClientConfiguration config = new PlayGamesClientConfiguration
        .Builder()
       // .RequestServerAuthCode(false)
       // .RequestIdToken()
        .Build();

        //커스텀 된 정보로 GPGS 초기화
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
*/

        var bro = Backend.Initialize(true); // 뒤끝 초기화

        // 뒤끝 초기화에 대한 응답값
        if(bro.IsSuccess()) {
            Debug.Log("초기화 성공 : " + bro); // 성공일 경우 statusCode 204 Success
        } else {
            Debug.LogError("초기화 실패 : " + bro); // 실패일 경우 statusCode 400대 에러 발생
        }


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
        }
        // 초기화 실패한 경우 실행
        else
        {
            Debug.Log("초기화 실패 - " + BRO);
        }
    }

    public void gameInitInfo() {
        StartCoroutine(CheckApplicationVersion());
    }

    // Application 버전 확인
    IEnumerator CheckApplicationVersion()
    {
        // 앱 버전 체크 구현 위치
        Debug.Log("GetLatestVersion Start");
        var bro = Backend.Utils.GetLatestVersion();
        Debug.Log(bro);
        string version = Application.version;
        string serverVersion ="";
        #if UNITY_EDITOR
            serverVersion = Application.version;
        #else
            serverVersion = bro.GetReturnValuetoJSON()["version"].ToString();
        #endif
        //최신 버전일 경우
        int localVersionNumber = int.Parse(version.Replace(".", ""));
        int serverVersionNumber = int.Parse(serverVersion.Replace(".", ""));
        
        if(localVersionNumber  >= serverVersionNumber)
        {
            Debug.Log("localVersionNumber::"+localVersionNumber);
            //구글 계정 확인
            googleLogin();
          
        } else {
            string url = "https://play.google.com/store/apps/details?id=pixeldust.tabtab.com" ;
            Application.OpenURL(url);
        }
        


        yield return new WaitForSeconds(0.1f);
    }



    bool googleLogin() {
        bool isLogin = false;
        string userId ="115136368152474690130";
        string userName ="lsh235s";

        userId = SystemInfo.deviceUniqueIdentifier;
        Debug.Log("googleLogin::"+userId);

        if(!string.IsNullOrEmpty(uuid)){
            DataManager.Instance.playerData.PlayerId = userId;
            BackendReturnObject BRO = Backend.BMember.CustomSignUp(userId, userId);

            isLogin = true;
            // 사용자 정보 확인
            BackendReturnObject bros = Backend.BMember.CustomLogin(userId, userId);
            
            if(bros.IsSuccess()) {
                LoginBackend();
            } else {
                errorText.SetActive(true);
            }
        }

        /*
        #if UNITY_EDITOR
            DataManager.Instance.playerData.PlayerId = userId;
            BackendReturnObject BRO = Backend.BMember.CustomSignUp(userId, userId);
            isLogin = true;
            // 사용자 정보 확인
            BackendReturnObject bros = Backend.BMember.CustomLogin(userId, userId);
            
            Debug.Log(bros.IsSuccess());
            if(bros.IsSuccess()) {
                LoginBackend();
            } else {
                errorText.SetActive(true);
            }
        #else
            if(Social.localUser.authenticated == true){
                userId = Social.localUser.id;
                userName = Social.localUser.userName;
                DataManager.Instance.playerData.PlayerId = userId;

                BackendReturnObject BRO = Backend.BMember.CustomLogin(userId, userId);
        
                Debug.Log("이미 로그인 되어있음 "+userId+"/"+userName);
                Debug.Log(BRO);
                Debug.Log(BRO.IsSuccess());
                if(BRO.IsSuccess()) {
                    isLogin = true;
                    // 사용자 정보 확인
                    LoginBackend();
                } else {
                    errorText.SetActive(true);
                }
            } else {
                Social.localUser.Authenticate((bool success, string message) => {
                    if(success){
                        userId = Social.localUser.id;
                        userName = Social.localUser.userName;
                        Debug.Log("구글 로그인 성공 "+userId+"/"+userName);
                        Debug.Log("message : "+message);
                        // 로그인 성공 -> 뒤끝 서버에 획득한 구글 토큰으로 가입 요청
                        BackendReturnObject BRO = Backend.BMember.CustomSignUp(userId, userId);
                        Debug.Log(BRO);
                        isLogin = true;

                        DataManager.Instance.playerData.PlayerId = userId;

                        BackendReturnObject bros = Backend.BMember.CustomLogin(userId, userId);
                        Debug.Log(bros.IsSuccess());
                        if(bros.IsSuccess()) {
                            isLogin = true;
                            // 사용자 정보 확인
                            LoginBackend();
                        } else {
                            errorText.SetActive(true);
                        }
                    } else {
                        Debug.Log("구글 로그인 실패");
                        Debug.Log("message : "+message);
                        isLogin = false;
                        errorText.SetActive(true);
                    }
                 
                });
            }
        #endif*/

        return isLogin;
    }

    


    // 사용자 로그인 동기 방식으로 구현
    public void LoginBackend()
    {
        DBInitGetDate();
        DBInitCharacterDate();
        // 로그인 이후 닉네임 생성한 사용자 일경우

        if("default".Equals( DataManager.Instance.getCharacter(4)) ) {
            SceneManager.LoadScene("Opening");
        }  else {
            SceneManager.LoadScene("lobby");
        }

    }

    // 사용자 초기 정보 가져오기
    // 동기 방식으로 구현
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
            DataManager.Instance.playerData.AdsDate = bro.FlattenRows()[0]["AdsDate"].ToString();
            DataManager.Instance.playerData.AdsYn = int.Parse(bro.FlattenRows()[0]["AdsYn"].ToString());
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
            
            if(!DataManager.Instance.playerData.MakeNickName) {
                DataManager.Instance.selectCharacter(4,"default");
            }
        }
        DataManager.Instance.DbSaveGameData();
      //  AdsManager.Instance.InitAds();
    }

    // 캐릭터 별 초기 정보 가져오기
    // 동기 방식으로 구현
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
                    DataManager.Instance.SaveCharacterData("sword1.json", DataManager.Instance.swordGirl1);
                } else if("Sword2".Equals(characterName)) {
                    DataManager.Instance.swordGirl2.bestScore = int.Parse(bro.FlattenRows()[i]["BestScore"].ToString());
                    DataManager.Instance.swordGirl2.totalKillScore = int.Parse(bro.FlattenRows()[i]["TotalKillScore"].ToString());
                    DataManager.Instance.SaveCharacterData("sword2.json", DataManager.Instance.swordGirl2);
                } else if("Sword3".Equals(characterName)) {
                    DataManager.Instance.swordGirl3.bestScore = int.Parse(bro.FlattenRows()[i]["BestScore"].ToString());
                    DataManager.Instance.swordGirl3.totalKillScore = int.Parse(bro.FlattenRows()[i]["TotalKillScore"].ToString());
                    DataManager.Instance.SaveCharacterData("sword3.json", DataManager.Instance.swordGirl3);
                } else if("leon".Equals(characterName)) {
                    DataManager.Instance.leon.bestScore = int.Parse(bro.FlattenRows()[i]["BestScore"].ToString());
                    DataManager.Instance.leon.totalKillScore = int.Parse(bro.FlattenRows()[i]["TotalKillScore"].ToString());
                    DataManager.Instance.SaveCharacterData("leon.json", DataManager.Instance.leon);
                }
            }
        }
    }



    // 사용자 정보 저장하기
    // 동기 방식으로 구현
    public void DbSaveGameData()
    {
        StartCoroutine(DoDbSaveGameData());
    }

    IEnumerator DoDbSaveGameData() {
        // 해당 계정의 기존 데이터가 있는지 확인
        Where where = new Where();

        Debug.Log("DbSaveGameData::serverwhereStart:"+DataManager.Instance.playerData.PlayerId);
        where.Equal("PlayerId",  DataManager.Instance.playerData.PlayerId);

        Debug.Log("DbSaveGameData::serverwhereStart");

        var bro = Backend.GameData.GetMyData("playerData", where, 1);
        if(bro.IsSuccess() == false)
        {
            // 요청 실패 처리
            Debug.Log(bro);
        }
        if(bro.GetReturnValuetoJSON() != null) {
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
        yield return null;
    }


    public void DbSaveNickname(string newNickname)
    {
        Backend.BMember.CreateNickname(newNickname);
    }


    // 캐릭터 정보 저장하기
    // 동기 방식으로 구현
    public void SaveBestScore(CharacterData characterData, int inputScore, bool isRecord)
    {
        StartCoroutine(DoSaveBestScore(characterData, inputScore, isRecord));
    }

    IEnumerator DoSaveBestScore(CharacterData characterData, int inputScore, bool isRecord) {
        Where where = new Where();
        where.Equal("PlayerId",  DataManager.Instance.playerData.PlayerId);
        where.Equal("CharacterName",  characterData.characterName);
        var bro = Backend.GameData.GetMyData("CharacterData", where, 0);
        Debug.Log("SaveBestScore::serverwhereStart"+bro);
        if(bro.GetReturnValuetoJSON() != null ) {
            if(bro.GetReturnValuetoJSON()["rows"].Count <= 0)
            {
                Param intParam = new Param();
                intParam.Add("PlayerId", DataManager.Instance.playerData.PlayerId);
                intParam.Add("CharacterName", characterData.characterName);
                intParam.Add("PlayerName", DataManager.Instance.playerData.PlayerName);
                intParam.Add("BestScore", inputScore);
                intParam.Add("TotalKillScore", characterData.totalKillScore);

                Backend.GameData.Insert("CharacterData", intParam);

                if(isRecord) {
                    RankInputdate(inputScore, characterData.characterName);
                }

            } else {
                Param upParam = new Param();
                upParam.Add("PlayerId", DataManager.Instance.playerData.PlayerId);
                upParam.Add("CharacterName", characterData.characterName);
                upParam.Add("PlayerName", DataManager.Instance.playerData.PlayerName);
                upParam.Add("BestScore", inputScore);
                upParam.Add("TotalKillScore", characterData.totalKillScore);

                Backend.GameData.Update("CharacterData", where, upParam);
                if(isRecord) {
                    RankInputdate(inputScore, characterData.characterName);
                }
            }
        }
        yield return null;
    }
    public void RankInputdate(int inputScore, string characterName) {
        Where where = new Where();
        where.Equal("PlayerId",  DataManager.Instance.playerData.PlayerId);
        where.Equal("CharacterName",  characterName);
        var bro = Backend.GameData.GetMyData("CharacterData", where, 0);

        string rowInDate = bro.FlattenRows()[0]["inDate"].ToString();

        Debug.Log("SaveBestScore::update:"+ rowInDate);

        Param rankParam = new Param();
        rankParam.Add("BestScore", inputScore);
        rankParam.Add("PlayerId", DataManager.Instance.playerData.PlayerId);

        Backend.URank.User.UpdateUserScore(RANK_UUID, "CharacterData", rowInDate, rankParam);
        Debug.Log("랭킹 등록 결과 : " );
    
    }

   

    public JsonData GetBestScore() {
        // rankUuid 랭킹에서 1 ~ 100등 랭커 조회
        LitJson.JsonData rankListJson = null;

        BackendReturnObject bro = Backend.URank.User.GetRankList(RANK_UUID, 10);
        // 이후 처리
        if (bro.IsSuccess())
        {
            // 랭킹 조회 성공
            rankListJson = bro.GetFlattenJSON();
            Debug.Log("랭킹 조회 성공 : " + rankListJson["rows"].Count);
        }

        return rankListJson;
    }

    public JsonData GetMyBestScore() {
        LitJson.JsonData rankListJson = null;
        BackendReturnObject bro = Backend.URank.User.GetMyRank(RANK_UUID);
        // 이후 처리
        if (bro.IsSuccess())
        {
            // 랭킹 조회 성공
            rankListJson = bro.GetFlattenJSON();
        }

        return rankListJson;
    }

    public void PurchaseBackend(PurchaseEventArgs args) {
             /*
        뒤끝 영수증 검증 처리
        */
        var bro = Backend.Receipt.IsValidateGooglePurchase(args.purchasedProduct.receipt , "receiptDescription", false);
        Debug.Log("구매 영수증 검증 결과 : " + bro);

    }

    
    public string DblanguageCheckData(string language) {
        string returnString = "";
        bool isFilterString = Backend.Chat.IsFilteredString(language);
        Debug.Log("language:"+language);
        if(isFilterString) {
            returnString = "filterFalse";
        } else {
            BackendReturnObject bro = Backend.BMember.CheckNicknameDuplication ( language );
            if(!bro.IsSuccess())
            {
                returnString = "existName";
            }
        }
        Debug.Log("returnString:"+returnString);

        return returnString;
    }
}
