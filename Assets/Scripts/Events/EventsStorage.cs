using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class EventsStorage
{
    private static List<EventHappenData> eventHappenData = new();

    public static void InitialLoad()
    {
        EventData[] eventResources = Resources.LoadAll<EventData>("Events");
        eventHappenData = eventResources.Select(u => new EventHappenData()
        {
            EventData = u,
            Happen = false
        }).ToList();
    }

    public static void SetEventHappen(EventData eventData, bool happen = true)
    {
        EventHappenData data = eventHappenData.FirstOrDefault(u => u.EventData == eventData);
        if (data != null) 
            data.Happen = happen;
        else
            eventHappenData.Add(new EventHappenData() { EventData = eventData, Happen = happen });
    }

    public static bool EventHappen(EventData eventData)
    {
        EventHappenData data = eventHappenData.FirstOrDefault(u => u.EventData == eventData);
        if (data != null)
            return data.Happen;
        else
        {
            eventHappenData.Add(new EventHappenData() { EventData = eventData, Happen = false });
            return false;
        }
    }

    public static void UpdateEvents()
    {
        foreach (EventHappenData eventData in eventHappenData)
        {
            if (!eventData.EventData.HappenOnce)
                eventData.Happen = false;
        }
    }

    public static void Load(SaveData save)
    {
        eventHappenData = save.EventHappens;
        foreach (EventHappenData eventHappen in eventHappenData)
        {
            eventHappen.EventData = ResourcesHelper.FindEventResource(eventHappen.UID);
        }
    }

    public static List<EventHappenData> Save()
    {
        foreach (var data in eventHappenData)
        {
            data.UID = data.EventData.UID;
        }
        return eventHappenData;
    }
}
