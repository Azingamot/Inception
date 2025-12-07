using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private TMP_Text clockText;

    public void EndGame()
    {
        clockText.text = ClockText(ClockController.GetClockContext());
        Time.timeScale = 0;
        animator.SetTrigger("End");
    }

	private string ClockText(ClockContext clockContext)
	{
		return string.Concat("Day ", clockContext.Days, " ", ClockFormatText(clockContext.Hours), ":", ClockFormatText(clockContext.Minutes));
	}

	private string ClockFormatText(int value)
	{
		return value < 10 ? "0" + value.ToString() : value.ToString();
	}
}
