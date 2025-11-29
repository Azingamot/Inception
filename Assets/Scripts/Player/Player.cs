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
    [SerializeField] private PlayerHealth playerHealth;

    [Header("Inputs")]
    [SerializeField] private InputActionReference move;
    [SerializeField] private InputActionReference look;
    [SerializeField] private InputActionReference useInput;

    private Vector2 direction;
    private Rigidbody2D rb;
    private Vector3 mousePos;
    private bool isActive = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        DialogueSystem.Instance.OnDialogueStart.AddListener(Disable);
        DialogueSystem.Instance.OnDialogueEnd.AddListener(Enable);
        useInput.action.started += UseItem;
    }

    private void OnDisable()
    {
        DialogueSystem.Instance.OnDialogueStart.RemoveListener(Disable);
        DialogueSystem.Instance.OnDialogueEnd.RemoveListener(Enable);
        useInput.action.started -= UseItem;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovementInput();
        SetMousePosition();
        if (isActive) playerMovement.Move(direction, rb);
        playerAnimation.Animate(rb, mousePos);
    }

    private void Disable()
    {
        isActive = false;
    }

    private void Enable()
    {
        isActive = true;
    }

    private void UseItem(InputAction.CallbackContext context)
    {
        if (isActive)
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

    public PlayerHealth ReceiveHealth()
    {
        return playerHealth;
    }

    public void Load(PlayerData playerData)
    {
        transform.position = playerData.PlayerPosition;
        playerHealth.Health = playerData.Health;
        playerHealth.MaxHealth = playerData.MaxHealth;
    }
}
