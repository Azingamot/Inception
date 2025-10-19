using UnityEngine;

public class ShowBuildingPlacement : MonoBehaviour
{
    [SerializeField] private GameObject highLighter;
    public static ShowBuildingPlacement instance;
    private SpriteRenderer highlightRenderer;

    private void Awake()
    {
        if (instance == null)
        {
            highlightRenderer = highLighter.GetComponent<SpriteRenderer>();
            instance = this;
            DeactivateHighlight();
        }
    }

    public void ActivateHighlight(Vector3 position, Color color, Sprite sprite)
    {
        if (!highLighter.activeInHierarchy)
        {
            highLighter.SetActive(true);
        }
        highLighter.transform.position = position;
        highlightRenderer.color = new Color(color.r, color.g, color.b, 0.5f);
        highlightRenderer.sprite = sprite;
    }

    public void DeactivateHighlight()
    {
        if (highLighter.activeInHierarchy)
            highLighter.SetActive(false);
    }
}
