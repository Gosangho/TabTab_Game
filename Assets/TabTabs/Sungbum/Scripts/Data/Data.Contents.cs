using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{ 
#region Stat
	[Serializable]
	public class Stat
	{
		public int level;
		public int maxHp;
		public int attack;
		public int totalExp;
	}

	[Serializable]
	public class StatData : ILoader<int, Stat>
	{
		public List<Stat> stats = new List<Stat>();

		public Dictionary<int, Stat> MakeDict()
		{
			Dictionary<int, Stat> dict = new Dictionary<int, Stat>();
			foreach (Stat stat in stats)
				dict.Add(stat.level, stat);
			return dict;
		}
	}
    #endregion

    #region UserStageData
    [Serializable]
    public class UserStage
    {
        public int Index;
        public string StageName;
        public int Chapter;
        public int SubChaper;
        public string Description;
        public int MissionClear_1;
        public int MissionClear_2;
        public int MissionClear_3;
        public int IsAllClear;
        public int MissionBonusCount;
    }

    [Serializable]
    public class UserStageData : ILoader<string, UserStage>
    {
        public List<UserStage> userClears = new();

        public Dictionary<string, UserStage> MakeDict()
        {
            Dictionary<string, UserStage> dict = new();
            foreach (UserStage userClear in userClears)
                dict.Add(userClear.StageName, userClear);
            return dict;
        }
    }
    #endregion
}