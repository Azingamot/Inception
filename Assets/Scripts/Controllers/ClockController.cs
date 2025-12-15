using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ClockController : MonoBehaviour
{
    public const int NIGHT_START_HOURS = 21;
    public const int NIGHT_END_HOURS = 6;
    public static int NIGHT_LENGTH => (24 - NIGHT_START_HOURS) + NIGHT_END_HOURS;

    [SerializeField] private UnityEvent<ClockContext> onClockTick;
    [SerializeField] private UnityEvent<ClockContext> onClockHour;
    [SerializeField] private UnityEvent<ClockContext> onTimeChanged;
    [SerializeField] private float defaultClockSpeed = 0.5f;
    public static int Minutes = 0;
    public static int Hours = 6;
    public static int Days = 1;
    public static DayTime DayTime = DayTime.Day;
    private Coroutine clocksCoroutine;
    private bool isInitialized = false;

    private float clockSpeed;

    private void Start()
    {
        clockSpeed = defaultClockSpeed;
    }

    public void ChangeClockSpeed(float divider)
    {
        clockSpeed = defaultClockSpeed / divider;
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
            yield return new WaitForSeconds(clockSpeed);
            CountNext();
        }
    }

    private void CountNext()
    {
        Minutes += 1;
        onClockTick.Invoke(GetClockContext());
    }

    private void ControlDayTime()
    {
        if (DayTime == DayTime.Day && (Hours >= NIGHT_START_HOURS || Hours <= NIGHT_END_HOURS))
        {
            DayTime = DayTime.Night;
            onTimeChanged.Invoke(GetClockContext());
        }
        else if (DayTime == DayTime.Night && (Hours > NIGHT_END_HOURS && Hours < NIGHT_START_HOURS))
        {
            DayTime = DayTime.Day;
            onTimeChanged.Invoke(GetClockContext());
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
            onClockHour.Invoke(GetClockContext());
            Minutes = 0;
        }
    }

    public ClockContext Save()
    {
        return GetClockContext();
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

    public static ClockContext GetClockContext()
    {
        return new ClockContext(Minutes, Hours, Days, DayTime);
    }
}
