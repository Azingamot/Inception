using System.Collections;
using TMPro;
using UnityEngine;

public class InteractionUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_Text promptText;
    private Coroutine waitCoroutine;

    private void Awake()
    {
        Hide();
    }

    public void Show(string prompt)
    {
        promptText.text = prompt;
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void Hide()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void TemporaryShow(string prompt, float time = 1)
    {
        Show(prompt);
        if (waitCoroutine != null) 
            StopCoroutine(waitCoroutine);
        waitCoroutine = StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= 0.005f;
            yield return null;
        }
        Hide();
    }
}
