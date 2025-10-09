using System.Collections;
using UnityEngine;

public class ClockController : MonoBehaviour
{
    [SerializeField] private ClockUI clockUI;
    public static int Minutes = 0;
    public static int Hours = 6;
    public static int Days = 1;
    public static DayTime DayTime = DayTime.Day;

    private void Awake()
    {
        clockUI.OnClockTick(Minutes, Hours, Days, DayTime);
        StartCoroutine(CountMinutes());
    }

    private void FixedUpdate()
    { 
        ControlMinutes();
        ControlHours();
        ControlDayTime();
    }

    private IEnumerator CountMinutes()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.4f);
            CountNext();
        }
    }

    private void CountNext()
    {
        Minutes += 1;
        clockUI.OnClockTick(Minutes, Hours, Days, DayTime);
    }

    private void ControlDayTime()
    {
        if (DayTime == DayTime.Day && (Hours <= 6 || Hours >= 20))
        {
            DayTime = DayTime.Night;
        }
        else if (DayTime == DayTime.Night && (Hours > 6 && Hours < 20))
        {
            DayTime = DayTime.Day;
        }
    }

    private void ControlHours()
    {
        if (Hours >= 24)
        {
            Days += 1;
            Hours = 0;
        }
    }

    private void ControlMinutes()
    {
        if (Minutes >= 59)
        {
            Hours += 1;
            Minutes = 0;
        }
    }
}
