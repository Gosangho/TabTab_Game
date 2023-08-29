using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullDeathEffect : MonoBehaviour
{
    // Start is called before the first frame update

    private Animator animator;
    
    void Start()
    {
        Animator animator = GetComponent<Animator>();  // Get reference to Animator component
        animator.Play("MonsterDeath");  // Play the animation
    }

    public void Delete()
    {
        Debug.Log("몬스터이펙트가 삭제되었습니다..");
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
