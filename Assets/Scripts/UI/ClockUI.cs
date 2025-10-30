using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClockUI : MonoBehaviour
{
    [SerializeField] private Image sunImage;
    [SerializeField] private Image moonImage;
    [SerializeField] private TMP_Text clockText;
    [SerializeField] private TMP_Text dayCounter;

    private DayTime currentDayTime;
    private int currentDay;


    public void OnClockTick(ClockContext clockContext)
    {
        if (currentDayTime != clockContext.DayTime)
        {
            currentDayTime = clockContext.DayTime;
            switch (currentDayTime)
            {
                case DayTime.Day:
                    moonImage.fillClockwise = false;
                    sunImage.fillClockwise = true;
                    break;
                case DayTime.Night:
                    moonImage.fillClockwise = true;
                    sunImage.fillClockwise = false;
                    break;
            }
        }
        if (currentDay != clockContext.Days)
        {
            currentDay = clockContext.Days;
            SetDayText(currentDay);
        }

        SetClockText(clockContext.Minutes, clockContext.Hours);
        ControllFillAmount(clockContext.Hours);
    }

    private void ControllFillAmount(int hours)
    {
        switch (currentDayTime)
        {
            case DayTime.Day:
                SetSunFill(hours);
                break;
            case DayTime.Night:
                SetMoonFill(hours);
                break;
        }
    }

    private void SetClockText(int minutes, int hours)
    {
        string hourText = hours < 10 ? "0" + hours.ToString() : hours.ToString();
        string minutesText = minutes < 10 ? "0" + minutes.ToString() : minutes.ToString();
        clockText.text = hourText + ":" + minutesText;
    }

    private void SetSunFill(int hours)
    {
        if (hours > 6)
            hours -= 6;

        float fill = (float)hours / 14f;

        sunImage.fillAmount = Mathf.Clamp01(fill);
        moonImage.fillAmount = Mathf.Clamp01(1 - (fill));
    }

    private void SetMoonFill(int hours)
    {
        if (hours >= 20)
            hours = hours - 20;
        else if (hours <= 6)
        {
            hours += 4;
        }

        float fill = (float)hours / 10f;

        moonImage.fillAmount = Mathf.Clamp01(fill);
        sunImage.fillAmount = Mathf.Clamp01(1 - (fill));
    }

    private void SetDayText(int days)
    {
        dayCounter.text = "Day " + days.ToString();
    }
}
