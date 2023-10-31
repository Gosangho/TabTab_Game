using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurorialControll : MonoBehaviour
{
    public List<TutorialBase> Tutorials;
    string NextSceneName = "";
    TutorialBase CurrentTutorial = null;
    int CurrentIndex = -1;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (CurrentTutorial != null)
        {
            CurrentTutorial.Execute(this);
        }
    }

    public void SetNextTutorial()
    {
        // 현재 튜토리얼의 Exit() 매소드 호출
        if (CurrentTutorial != null)
        {
            CurrentTutorial.Exit();
        }
        // 모든 튜토리얼이 끝났다면
        if (CurrentIndex >= Tutorials.Count-1)
        {
            CompletedTutorials();
            return;
        }
        // 다음 튜토리얼 진행
        CurrentIndex++;
        CurrentTutorial = Tutorials[CurrentIndex];
        
        CurrentTutorial.Enter();
    }

    public void CompletedTutorials()
    {
        CurrentTutorial = null;

        if (!NextSceneName.Equals(""))
        {
            SceneManager.LoadScene(NextSceneName);
        }
    }
}

