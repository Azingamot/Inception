using UnityEngine;

[System.Serializable]
public class ClockContext
{
    public int Minutes = 0;
    public int Hours = 8;
    public int Days = 1;
    public DayTime DayTime = DayTime.Day;

    public ClockContext()
    {
        Minutes = 0;
        Hours = 8;
        Days = 1;
        DayTime = DayTime.Day;
    }

    public ClockContext(int minutes, int hours, int days, DayTime dayTime)
    {
        Minutes = minutes;
        Hours = hours;
        Days = days;
        DayTime = dayTime;
    }
}
