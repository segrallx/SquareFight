//using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FtLevelData
{
    struct FtLevelItem
    {
        public string Name;
        public FileInfo File;
    };

	// 关卡数据路径
    private static string LevelDataDir = "Assets/LevelData";
    private static List<FtLevelItem> mLevelItems = new List<FtLevelItem>();

    // 返回所有的关卡列表
    public static void GetLevelSelectInfo(out string[] names, out int[] index)
    {
        LoadLevelListItem();

        names = new string[mLevelItems.Count];
        index = new int[mLevelItems.Count];
        for (int i = 0; i < mLevelItems.Count; i++)
        {
            names[i] = mLevelItems[i].Name.Replace(".json", "");
            index[i] = i;
        }
    }


    // 加载所有的关开列表
    private static bool mInit;
    private static void LoadLevelListItem()
    {
        if (mInit)
        {
            return;
        }

        mInit = true;
        Debug.LogFormat("LoadLevelListItem");
        mLevelItems.Clear();
        DirectoryInfo theFolder = new DirectoryInfo(LevelDataDir);
        foreach (var fileInfo in theFolder.GetFiles())
        {
            if (!fileInfo.Name.EndsWith(".json"))
            {
                continue;
            }

            FtLevelItem item;
            item.Name = fileInfo.Name;
            item.File = fileInfo;
            mLevelItems.Add(item);
        }

        mLevelItems.Sort(delegate (FtLevelItem i1, FtLevelItem i2)
        {
            return i1.Name.CompareTo(i2.Name);
        });
    }

    // 获取某个关卡的数据
    public static bool GetLevelData(int idx, ref string data)
    {
        var ret = false;
        if (idx < 0 || idx > mLevelItems.Count)
        {
            return ret;
        }

        var levelItem = mLevelItems[idx];

        using (var reader = new StreamReader(levelItem.File.OpenRead()))
        {
            var fileContents = reader.ReadToEnd();
            data = fileContents;
        }
        return ret;
    }

	public static bool SetLevelData(int idx, string data)
    {
        var ret = false;
        if (idx < 0 || idx > mLevelItems.Count)
        {
            return ret;
        }

        var levelItem = mLevelItems[idx];
        using (var writer = new StreamWriter(levelItem.File.OpenWrite()))
        {
			writer.Write(data);
        }
        return ret;
    }


}
