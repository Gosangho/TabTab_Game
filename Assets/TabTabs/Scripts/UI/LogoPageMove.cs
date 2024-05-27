using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Spine.Unity; 



public class LogoPageMove : MonoBehaviour
{
    
    public SkeletonAnimation skeletonAnimation; // Inspector에서 할당

    FadeScene FadeSceneInstance;

    void Start()
    {
        // 애니메이션 상태(State)의 이벤트에 메소드를 바인딩
        skeletonAnimation.state.Complete += AnimationComplete;
        FadeSceneInstance = FindObjectOfType<FadeScene>();
    }

    private void AnimationComplete(Spine.TrackEntry trackEntry)
    {
        FadeSceneInstance.LoadLogoLoginScene();
    }


    void OnDestroy()
    {
        // 컴포넌트가 파괴될 때 이벤트 구독 해제
        if (skeletonAnimation != null)
        {
            skeletonAnimation.state.Complete -= AnimationComplete;
        }
    }

}
