using UnityEngine;
public class ClockContext
{
    public int Minutes;
    public int Hours;
    public int Days;
    public DayTime DayTime;

    public ClockContext(int minutes, int hours, int days, DayTime dayTime)
    {
        Minutes = minutes;
        Hours = hours;
        Days = days;
        DayTime = dayTime;
    }
}
