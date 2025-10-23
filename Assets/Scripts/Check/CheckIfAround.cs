using System.Linq;
using UnityEngine;

public class CheckIfAround : MonoBehaviour
{
    [SerializeField] private GameObject pointer;
    private bool seek = false;
    public static CheckIfAround instance;

    public void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void Enable()
    {
        seek = true;
    }

    public void Disable()
    {
        seek = false;
        ShowSelectedTile.instance.DeactivateHighlight();
        RemovePointer();
    }

    public void FixedUpdate()
    {
        if (!seek) return;

        SetPositionToHealthSystem(FindHealthSystem());

        transform.position = MouseInWorld();
    }

    private void SetPositionToHealthSystem(HealthSystem system)
    {
        if (system != null && seek)
        {
            SetPointer(system.transform.position);
        }
        else
        {
            RemovePointer();
        }
    }

    private void SetPointer(Vector2 position)
    {
        if (!pointer.activeInHierarchy)
            pointer.SetActive(true);
        pointer.transform.localPosition = position;
    }

    private void RemovePointer()
    {
        if (pointer.activeInHierarchy)
            pointer.SetActive(false);
    }

    private HealthSystem FindHealthSystem()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(MouseInWorld(), 1);
        Collider2D foundCollider = colliders.Where(u => u.TryGetComponent<HealthSystem>(out HealthSystem health)).FirstOrDefault();
        if (foundCollider != null)
        {
            return foundCollider.GetComponent<HealthSystem>();
        }
        return null;
    }

    private Vector2 MouseInWorld()
    {
        return Camera.main.ScreenToWorldPoint(MousePosition.GetData());
    }
}
