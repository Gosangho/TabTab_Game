using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using Spine;
using Spine.Unity;



    public class Right_Orc2_Anim : MonoBehaviour
    {
        public static Animator RightAnim;
        void Start()
        {
            RightAnim = GetComponent<Animator>();
            //InvokeRepeating("RightOrc2Attack", 4.0f, 4.0f);
        }

        void RightOrc2Attack()
        {
            //RightAnim.SetTrigger("Right_Attack");
        }
    }


