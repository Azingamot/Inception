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
        ControlFillAmount(clockContext.Hours);
    }

    private void ControlFillAmount(int hours)
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
        hours -= ClockController.NIGHT_END_HOURS;

        float fill = (float)hours / (ClockController.NIGHT_START_HOURS - ClockController.NIGHT_END_HOURS);

        sunImage.fillAmount = Mathf.Clamp01(fill);
        moonImage.fillAmount = Mathf.Clamp01(1 - (fill));
    }

    private void SetMoonFill(int hours)
    {
        if (hours >= ClockController.NIGHT_START_HOURS)
            hours -= ClockController.NIGHT_START_HOURS;
        else
            hours += 24 - ClockController.NIGHT_START_HOURS;

        float fill = (float)hours / ClockController.NIGHT_LENGTH;

        moonImage.fillAmount = Mathf.Clamp01(fill);
        sunImage.fillAmount = Mathf.Clamp01(1 - (fill));
    }

    private void SetDayText(int days)
    {
        dayCounter.text = "Day " + days.ToString();
    }
}
