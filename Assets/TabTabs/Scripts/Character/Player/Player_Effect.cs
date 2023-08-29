using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Player_Effect : MonoBehaviour
{
    public Animator Player_Effect_Anim;
    private void Start()
    {
        Player_Effect_Anim = GetComponent<Animator>();
        Player_Effect_Anim.Play("Player_Effcet");
    }
    void PlayerEffcet()
    {
        Destroy(gameObject);
    }
}
