using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FilledBarUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private TMP_Text countText;
    private int lastAmount = -1;
    private Coroutine fillCoroutine;

    public void ChangeBarFill(float currentAmount, float maxAmount)
    {
        int roundedAmount = Mathf.FloorToInt(currentAmount);
        if (DifferenceMoreThanValue(roundedAmount, lastAmount))
        {
            countText.text = roundedAmount.ToString();
            lastAmount = roundedAmount;
            StartChange(fillImage.fillAmount, Mathf.Clamp01(roundedAmount / maxAmount));
        }
    }

    private void StartChange(float currentAmount, float newAmount)
    {
        if (fillCoroutine != null)
            StopCoroutine(fillCoroutine);
        fillCoroutine = StartCoroutine(FillChange(currentAmount, newAmount));
    }

    private IEnumerator FillChange(float currentAmount, float newAmount)
    {
        float amount = currentAmount;
        float elapsedTime = DifferenceMoreThanValue(currentAmount, newAmount, 0.1f) ? 0 : 0.5f;
        while (amount != newAmount)
        {
            elapsedTime += Time.deltaTime;
            amount = Mathf.Lerp(amount, newAmount, elapsedTime);
            fillImage.fillAmount = amount;
            yield return null;
        }
    }

    private bool DifferenceMoreThanValue(float currentAmount, float newAmount, float value = 1)
    {
        return Mathf.Abs(currentAmount - newAmount) >= value;
    }
}
