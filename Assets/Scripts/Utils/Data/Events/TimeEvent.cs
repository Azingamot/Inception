using UnityEngine.Events;

[System.Serializable]
public class TimeEvent
{
    public EventData EventData;
    public UnityEvent<EventData> EventToHappen;
}