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
      public bool TutorialPlay;// Ʃ�丮���� �����ߴ����� ����(���� 1ȸ����)
      public bool MakeNickName;// ID�� ����������� ����(���� 1ȸ����)
      public string PlayerName;// �÷��̾��� ����
      public int Gold;// �÷��̾ �������ִ� ���
      // �������1�� �⺻ ĳ����
      public bool SwordGirl2Get; // �÷��̾ �������2�� �����ϰ��ִ����� ����
      public bool SwordGirl3Get; // �÷��̾ �������3�� �����ϰ��ִ����� ����
      public bool LeonGet; // �÷��̾ ������ �����ϰ��ִ����� ����
      public bool[] PlayerAttandence = new bool[31]; // �÷��̾��� �⼮��Ȳ
  }
   


