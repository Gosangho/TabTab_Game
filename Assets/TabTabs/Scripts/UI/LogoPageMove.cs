using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Spine.Unity; 

public class LogoPageMove : MonoBehaviour
{
    
    public SkeletonAnimation skeletonAnimation; // Inspector에서 할당
    string sceneNameToLoad = "Opening"; // 여기에 이동하고자 하는 씬의 이름을 입력하세요
    string sceneNameToLoby = "lobby"; 

    void Start()
    {
        // 애니메이션 상태(State)의 이벤트에 메소드를 바인딩
        skeletonAnimation.state.Complete += AnimationComplete;
    }

    private void AnimationComplete(Spine.TrackEntry trackEntry)
    {
        if("default".Equals(DataManager.Instance.getCharacter(4))){
            SceneManager.LoadScene(sceneNameToLoad);
        } else {
            SceneManager.LoadScene(sceneNameToLoby);
        }
     
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
