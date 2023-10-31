using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TutorialBase : MonoBehaviour
{
    // abstract : 추상클래스 -> 자식클래스에서 반드시 재정의가 필요함
    // 튜토리얼을 시작할때 1회 호출
    public abstract void Enter();
    // 튜토리얼을 진행하는 동안 매 프레임당 호출
    public abstract void Execute(TurorialControll controll);
    // 튜토리얼을 종료할때 1회 호출
    public abstract void Exit();

}
