using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Data.Common;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    public Dictionary<int, Data.UserStage> UserStageDataDict { get; private set; } = new Dictionary<int, Data.UserStage>();


    public void Init()
    {
        //UserStageDataDict = LoadCsv<Data.UserStageData, int, Data.UserStage>("UserStageData", MakeAAA);
        LoadUserStageData();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
		TextAsset textAsset = Managers.Resource.Load<TextAsset>(path);
        return JsonUtility.FromJson<Loader>(textAsset.text);
	}

    void LoadUserStageData()
    {
        UserStageDataDict.Clear();
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"data/UserStageData");

        Data.UserStage[] ret1 = CSVSerializer.Deserialize<Data.UserStage>(textAsset.text);

        Debug.Log($"Length : {ret1.Length}");
        for (int i = 0; i < ret1.Length; i++)
        {
            Debug.Log($"{ret1[i].Index}, {ret1[i].StageName}");
            UserStageDataDict.Add(ret1[i].Index, ret1[i]); ;
        }

    }
}
