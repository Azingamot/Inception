using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactionRadius = 1.5f;
    [SerializeField] private LayerMask interactableMask;
    [SerializeField] private InteractionUI interactionUI;
    [SerializeField] private InputActionReference interactAction;

    private IInteractable currentTarget;

    private void OnEnable()
    {
        interactAction.action.started += HandleInteractPressed;
    }

    private void OnDisable()
    {
        interactAction.action.started -= HandleInteractPressed;
    }

    private void Update()
    {
        FindNearestInteractable();
        UpdateUI();
    }

    private void FindNearestInteractable()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, interactionRadius, interactableMask);

        if (colliders.Length == 0)
        {
            currentTarget = null;
            return;
        }

        currentTarget = colliders
            .Select(c => c.GetComponent<IInteractable>())
            .Where(i => i != null)
            .OrderBy(i => Vector2.Distance(transform.position, i.Transform.position))
            .FirstOrDefault();
    }

    private void UpdateUI()
    {
        if (currentTarget != null)
        {
            interactionUI.Show(GetKeyFromAction(interactAction.action));
        }
        else
        {
            interactionUI.Hide();
        }
    }

    private void HandleInteractPressed(InputAction.CallbackContext callbackContext)
    {
        currentTarget?.OnInteract(gameObject);
    }

    private string GetKeyFromAction(InputAction action)
    {
        return action.bindings[0].effectivePath.Split('/')[1].ToUpper();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
