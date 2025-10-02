using UnityEngine;

public class ShowSelectedTile : MonoBehaviour
{
    [SerializeField] private GameObject highLighter;
    public static ShowSelectedTile instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DeactivateHighlight();
        }
        else
        {
            Destroy(this);
        }
    }

    public void ActivateHighlight(Vector3 position)
    {
        if (!highLighter.activeInHierarchy)
        {
            highLighter.SetActive(true);
        }
        highLighter.transform.position = position;
    }

    public void DeactivateHighlight()
    {
        if (highLighter.activeInHierarchy)
            highLighter.SetActive(false);
    }
}
