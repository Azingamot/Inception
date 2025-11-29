using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class CurrentQuests
{
    private static List<ActiveQuest> questData = new();

    public static void AddData(ActiveQuest data)
    {
        questData.Add(data);
    }

    public static void Load(SaveData data)
    {
        questData.Clear();
        foreach (ActiveQuest activeQuest in data.ActiveQuests)
        {
            questData.Add(activeQuest);
        }
    }

    public static List<ActiveQuest> Get()
    {
        return questData;
    }
}
