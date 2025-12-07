using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DaytimeEvents : MonoBehaviour, IEventController
{
    [SerializeField] private List<TimeEvent> startedEvents = new();
    [SerializeField] private List<TimeEvent> updatedEvents = new();

    public void EventsLoaded(object context)
    {
        foreach (var e in startedEvents)
        {
            if (!EventsStorage.EventHappen(e.EventData))
                e.EventToHappen.Invoke(e.EventData);
        }
    }

    public void EventsUpdated(object context)
    {
        if (context is ClockContext clockContext)
        {
            foreach (var e in updatedEvents)
            {
                CheckEventActivation(e, clockContext);
            }
        }
    }

    private void CheckEventActivation(TimeEvent e, ClockContext clockContext)
    {
        if (!EventsStorage.EventHappen(e.EventData) && ValidateEventTime(clockContext, e.EventData))
        {
            int randomInt = Random.Range(0, 100);
            if (randomInt < e.EventData.Chance)
            {
                e.EventToHappen.Invoke(e.EventData);
                EventsStorage.SetEventHappen(e.EventData, true);
            }
        }
    }

    private bool ValidateEventTime(ClockContext clockContext, EventData eventData)
    {
        return (clockContext.Days >= eventData.StartDay && clockContext.Days <= eventData.EndDay) &&
            (clockContext.Hours >= eventData.LowerThreshold && clockContext.Hours <= eventData.UpperThreshold);
    }
}
