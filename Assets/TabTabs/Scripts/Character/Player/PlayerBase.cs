using System.Collections;
using System.Collections.Generic;
using TabTabs.NamChanwoo;
using UnityEngine;
using UnityEngine.Animations;
using Spine;
using Spine.Unity;
using UnityEngine.SceneManagement;

public class PlayerBase : CharacterBase
{
    public Animator PlayerAnim;
    public Transform PlayerTransform;
    void Start()
    {
        PlayerAnim = GetComponent<Animator>();
    }

    
    void Update()
    {
        
    }

    //public void PlayerAnimEvent()
    //{
    //    if (SceneManager.GetActiveScene().buildIndex==1||SceneManager.GetActiveScene().buildIndex==3)
    //    {// 첫번째 씬이라면
    //        Vector3 targetPosition = new Vector3(0.0f, 1.5f, 0f);
    //        transform.position = targetPosition;
    //    }
    //    else
    //    {
    //        Vector3 targetPosition = new Vector3(-1.2f, 1.5f, 0f);
    //        transform.position = targetPosition;
    //    }
    //}

}
