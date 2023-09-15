using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Left_Orc2_Anim : MonoBehaviour
{
    public static Animator LeftAnim;
    void Start()
    {
        LeftAnim = GetComponent<Animator>();
        //InvokeRepeating("LeftOrc2Attack", 4.0f, 4.0f);
    }

    void LeftOrc2Attack()
    {
        //LeftAnim.SetTrigger("Left_Attack");
    }
}
