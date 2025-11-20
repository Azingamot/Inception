using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SaveUI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TMP_Text saveName;
    [SerializeField] private Vector3 scaleValue;
    [SerializeField] private float scaleSpeed;

    private Image background;
    private int index = -1;
    private SavesUI savesUI;
    private Vector3 initialScale;
    private Coroutine scaleCoroutine;


    public void Initialize(int index, string name, SavesUI savesUI)
    {
        this.index = index;
        saveName.text = name;
        this.savesUI = savesUI;
        initialScale = transform.localScale;
        background = GetComponentInChildren<Image>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        savesUI.OpenSave(index);
        savesUI.ChangeSelection(this);
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
            transform.localScale = Vector3.Lerp(transform.localScale, desiredScale, elapsedTime);
            yield return null;
        }
    }

    public void Select()
    {
        background.color = Color.gray;
    }

    public void Deselect()
    {
        background.color = Color.white;
    }
}
