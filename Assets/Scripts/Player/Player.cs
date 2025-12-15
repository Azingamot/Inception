using System.Collections;
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
    private bool canUseItems = true;
    private bool canMove = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerHealth.Load(this);
    }

    private void OnEnable()
    {
        DialogueSystem.Instance.OnDialogueStart.AddListener(DisableItemUsage);
        DialogueSystem.Instance.OnDialogueStart.AddListener(DisableMovement);
        DialogueSystem.Instance.OnDialogueEnd.AddListener(EnableItemUsage);
        DialogueSystem.Instance.OnDialogueEnd.AddListener(EnableMovement);
        useInput.action.started += UseItem;
    }

    private void OnDisable()
    {
        DialogueSystem.Instance.OnDialogueStart.RemoveListener(DisableItemUsage);
        DialogueSystem.Instance.OnDialogueStart.RemoveListener(DisableMovement);
        DialogueSystem.Instance.OnDialogueEnd.RemoveListener(EnableItemUsage);
        DialogueSystem.Instance.OnDialogueEnd.RemoveListener(EnableMovement);
        useInput.action.started -= UseItem;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovementInput();
        SetMousePosition();
        if (canMove) playerMovement.Move(direction, rb);
        playerAnimation.Animate(rb, mousePos);
    }

    public void DisableItemUsage()
    {
        canUseItems = false;
    }

    public void EnableItemUsage()
    {
        canUseItems = true;
    }

    public void DisableMovement()
    {
        canMove = false;
        playerAnimation.DisableRotation();
        rb.linearVelocity = Vector2.zero;
        StopAllCoroutines();
    }

    public void EnableMovement()
    {
        canMove = true;
        playerAnimation.EnableRotation();
    }

    private void UseItem(InputAction.CallbackContext context)
    {
        if (canUseItems)
            playerItemUse.UseItem();
    }

    public void MovementInput()
    {
        direction = move.action.ReadValue<Vector2>();
    }

    public void Knockback(Transform source, float power)
    {
        canUseItems = false;
        playerMovement.Knockback(source, power, rb);
        StartCoroutine(WaitForKnockback());
    }

    private IEnumerator WaitForKnockback()
    {
        yield return new WaitForSeconds(0.2f);
        canUseItems = true;
    }

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
