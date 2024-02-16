using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TabTabs.NamChanwoo;
using GoogleMobileAds.Api;


namespace TabTabs.NamChanwoo
{
    public class AdsManager : MonoBehaviour
    {
        RewardedAd rewardedAd;
        BannerView bannerView;
        InterstitialAd interstitial;

        public ContinueButton continueButtonInstance;

        public static AdsManager  Instance { get; private set; }

  
        void Awake()
        {
            // 이미 인스턴스가 있는지 확인합니다.
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                // 중복되는 인스턴스가 있는 경우, 이 게임 객체를 파괴합니다.
                Destroy(this.gameObject);
            }

        }

        // Start is called before the first frame update
        void Start()
        {
            InitAds();
        }


        // 구글 광고 초기화
        public void InitAds()
        {
            string adUnitId = "ca-app-pub-3940256099942544/5224354917";

            string adinterstiId = "ca-app-pub-3940256099942544/1033173712";

            string adbanerId = "ca-app-pub-3940256099942544/6300978111";

            AdRequest request = new AdRequest.Builder().Build();

            if(bannerView != null) {
                bannerView.Destroy();
                bannerView = null;
            }

            bannerView = new BannerView(adbanerId, AdSize.Banner, AdPosition.Bottom);
            bannerView.LoadAd(request);

            if(DataManager.Instance.playerData.AdsYn == 0) {
                bannerView.Show();
            }

            RewardedAd.Load(adUnitId, request, LoadCallback);
            
            if (interstitial != null)
            {
                interstitial.Destroy();
                interstitial = null;
            }

            InterstitialAd.Load(adinterstiId, request,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                    "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                            + ad.GetResponseInfo());

                interstitial = ad;
            });


        }

        // RewardedAd 객체 생성
        public void LoadCallback(RewardedAd rewardedAd, LoadAdError loadAdError)
        {
            
            if(rewardedAd != null)
            {
                this.rewardedAd = rewardedAd;
            }
            else
            {
                Debug.Log(loadAdError.GetMessage());
            }
        }


        public void rewardedAdPlay()
        {
            if(rewardedAd != null) {
                if(rewardedAd.CanShowAd())
                {
                    rewardedAd.Show(GetReward);
                }
                else
                {
                    if (this.interstitial.CanShowAd())
                    {
                        this.interstitial.Show();
                        continueButtonInstance.GetReward();
                    }
                }
            } else {
                if (this.interstitial.CanShowAd())
                {
                    this.interstitial.Show();
                    continueButtonInstance.GetReward();
                }
            }

            InitAds();
        }


        public void GetReward(Reward reward)
        {
            continueButtonInstance.GetReward();
        }

        

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}