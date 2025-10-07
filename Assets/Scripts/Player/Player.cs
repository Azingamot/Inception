using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerAnimation playerAnimation;
    [SerializeField] private PlayerItemInHand playerItemInHand;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerItemUse playerItemUse;

    [Header("Inputs")]
    [SerializeField] private InputActionReference move;
    [SerializeField] private InputActionReference look;
    [SerializeField] private InputActionReference useInput;

    private Vector2 direction;
    private Rigidbody2D rb;
    private Vector3 mousePos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        useInput.action.started += UseItem;
    }

    private void OnDisable()
    {
        useInput.action.started -= UseItem;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovementInput();
        SetMousePosition();
        playerMovement.Move(direction, rb);
        playerAnimation.Animate(rb, mousePos);
    }

    private void UseItem(InputAction.CallbackContext context)
    {
        playerItemUse.UseItem();
    }

    /// <summary>
    /// Получает данные о направлении движения игрока
    /// </summary>
    public void MovementInput()
    {
        direction = move.action.ReadValue<Vector2>();
    }

    /// <summary>
    /// Получает данные о расположении курсора в пространстве
    /// </summary>
    private void SetMousePosition()
    {
        mousePos = Camera.main.ScreenToWorldPoint(look.action.ReadValue<Vector2>());
        mousePos.z = 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position,mousePos);
    }
}
