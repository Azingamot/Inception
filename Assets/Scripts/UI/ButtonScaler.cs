using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScaler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Vector3 scaleValue;
    [SerializeField] private float scaleSpeed;
    private Vector3 initialScale;
    private Coroutine scaleCoroutine;

    private void Start()
    {
        initialScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (scaleCoroutine != null)
            StopCoroutine(scaleCoroutine);

        scaleCoroutine = StartCoroutine(ScaleLerp(scaleValue));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (scaleCoroutine != null)
            StopCoroutine(scaleCoroutine);

        scaleCoroutine = StartCoroutine(ScaleLerp(initialScale));
    }

    private IEnumerator ScaleLerp(Vector3 desiredScale)
    {
        float elapsedTime = 0;
        while (transform.localScale != desiredScale)
        {
            elapsedTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(transform.localScale, desiredScale, elapsedTime * scaleSpeed);
            yield return null;
        }
    }
}
