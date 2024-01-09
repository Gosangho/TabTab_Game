using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


  [System.Serializable]
  public class CharacterData
  {
      public string characterName;
      public int bestScore;
      public int totalKillScore;
  }

  [System.Serializable]
  public class PlayerData
  {
      public bool TutorialPlay;// 튜토리얼을 진행했는지의 여부(최초 1회진행)
      public bool MakeNickName;// ID를 만들었는지의 여부(최초 1회진행)
      public string PlayerName;// 플레이어의 네임
      public int Gold;// 플레이어가 가지고있는 골드
  }
   


