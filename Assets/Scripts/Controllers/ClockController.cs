using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ClockController : MonoBehaviour
{
    [SerializeField] private UnityEvent<ClockContext> onClockTick;
    [SerializeField] private float clockSpeed = 0.5f;
    public static int Minutes = 0;
    public static int Hours = 6;
    public static int Days = 1;
    public static DayTime DayTime = DayTime.Day;
    private Coroutine clocksCoroutine;
    private bool isInitialized = false;

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
            yield return new WaitForSeconds(clockSpeed);
            CountNext();
        }
    }

    private void CountNext()
    {
        Minutes += 1;
        onClockTick.Invoke(new ClockContext(Minutes, Hours, Days, DayTime));
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

    public ClockContext Save()
    {
        return new ClockContext(Minutes, Hours, Days, DayTime);
    }

    public void Initialize(ClockContext clockContext)
    {
        Days = clockContext.Days;
        Hours = clockContext.Hours;
        Minutes = clockContext.Minutes;
        DayTime = clockContext.DayTime;

        isInitialized = true;

        if (clocksCoroutine != null)
            StopCoroutine(clocksCoroutine);

        clocksCoroutine = StartCoroutine(CountMinutes());
    }

    private void OnEnable()
    {
        if (!isInitialized)
            return;
        if (clocksCoroutine != null) 
            StopCoroutine(clocksCoroutine);
        clocksCoroutine = StartCoroutine(CountMinutes());
    }

    private void OnDisable()
    {
        if (clocksCoroutine != null)
            StopCoroutine(clocksCoroutine);
    }
}
