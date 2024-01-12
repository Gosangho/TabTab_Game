using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using static Define;

public static class Util
{
    public static List<Dictionary<string, object>> ReadTextAsset(string filename)
    {
        var list = new List<Dictionary<string, object>>();
        TextAsset data = Resources.Load(filename) as TextAsset;

        var lines = Regex.Split(data.text, LINE_SPLIT_PATTERN);

        if (lines.Length <= 1) return list;

        var headers = Regex.Split(lines[0], SPLIT_PATTERN);
        for (var i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], SPLIT_PATTERN);
            if (values.Length == 0 || values[0] == "") continue;

            var dic = new Dictionary<string, object>();
            for (var j = 0; j < headers.Length && j < values.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                object finalvalue = value;
                if (int.TryParse(value, out int intResult))
                {
                    finalvalue = intResult;
                }
                else if (float.TryParse(value, out float floatResult))
                {
                    finalvalue = floatResult;
                }
                dic[headers[j]] = finalvalue;
            }
            list.Add(dic);
        }
        return list;
    }

    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();
        return component;
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;

        return transform.gameObject;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }
}
