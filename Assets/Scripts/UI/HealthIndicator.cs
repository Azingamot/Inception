using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthIndicator : MonoBehaviour
{
    [SerializeField] private GameObject healthBarObject;
    [SerializeField] private Image healthBarImage;
    private Coroutine fillCoroutine;

    public void OnUpdate(float currentAmount, float maxAmount)
    {
        if (!healthBarObject.activeInHierarchy) healthBarObject.SetActive(true);
        else if (currentAmount == 0) healthBarObject.SetActive(false);
        StartChange(healthBarImage.fillAmount, Mathf.Clamp01(currentAmount / maxAmount));
    }

    protected void StartChange(float currentAmount, float newAmount)
    {
        if (fillCoroutine != null)
            StopCoroutine(fillCoroutine);
        fillCoroutine = StartCoroutine(FillChange(currentAmount, newAmount));
    }

    protected IEnumerator FillChange(float currentAmount, float newAmount)
    {
        float amount = currentAmount;
        float elapsedTime = 0;
        while (amount != newAmount)
        {
            elapsedTime += Time.deltaTime;  
            amount = Mathf.Lerp(amount, newAmount, elapsedTime);
            healthBarImage.fillAmount = amount;
            yield return null;
        }
    }
}
