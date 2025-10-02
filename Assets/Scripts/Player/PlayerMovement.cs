using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// ����� ��� ���������� ��������� ������
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float defaultSpeed = 5f;
    private float speed;
    private Rigidbody2D rb;
    private Vector2 direction;
    [Header("Inputs")]
    [SerializeField] private InputActionReference move;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        MovementInput();
        Move();
    }


    /// <summary>
    /// ������� ������ � ������������
    /// </summary>
    private void Move()
    {
        rb.linearVelocity = speed * defaultSpeed * direction.normalized;
    }

    /// <summary>
    /// �������� ������ � ����������� �������� ������
    /// </summary>
    public void MovementInput()
    {
        direction = move.action.ReadValue<Vector2>();
        speed = Mathf.Clamp01(direction.magnitude);
    }
}
