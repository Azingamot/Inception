using UnityEngine;

public class EventsController : MonoBehaviour, IEventController
{
    [SerializeField] private DaytimeEvents dayController;
    [SerializeField] private DaytimeEvents nightController;

    public void EventsLoaded(object context)
    {
        if (context is ClockContext clockContext)
        {
            switch (clockContext.DayTime)
            {
                case DayTime.Day:
                    dayController.EventsLoaded(context);
                    break;
                case DayTime.Night:
                    nightController.EventsLoaded(context);
                    break;
                default:
                    goto case DayTime.Day;
            }
        }
    }

    public void EventsUpdated(object context)
    {
        if (context is ClockContext clockContext)
        {
            switch (clockContext.DayTime)
            {
                case DayTime.Day:
                    EventsStorage.UpdateEvents();
                    dayController.EventsUpdated(context);
                    break;
                case DayTime.Night:
                    nightController.EventsUpdated(context);
                    break;
                default:
                    goto case DayTime.Day;
            }
        }
    }
}
