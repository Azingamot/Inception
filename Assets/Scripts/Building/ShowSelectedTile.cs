using UnityEngine;

public class ShowSelectedTile : MonoBehaviour
{
    [SerializeField] private GameObject highLighter;
    public static ShowSelectedTile instance;
    private Color baseColor;
    private SpriteRenderer highlightRenderer;

    private void Awake()
    {
        if (instance == null)
        {
            highlightRenderer = highLighter.GetComponent<SpriteRenderer>();
            baseColor = highlightRenderer.color;
            instance = this;
            DeactivateHighlight();
        }
        else
        {
            Destroy(this);
        }
    }

    public void ActivateHighlight(Vector3 position, Color color)
    {
        if (!highLighter.activeInHierarchy)
        {
            highLighter.SetActive(true);
        }
        highLighter.transform.position = position;
        highlightRenderer.color = color;
    }

    public void DeactivateHighlight()
    {
        if (highLighter.activeInHierarchy)
            highLighter.SetActive(false);
    }
}
