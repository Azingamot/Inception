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

    /// <summary>
    /// ������� ������ � ������������
    /// </summary>
    public void Move(Vector2 direction, Rigidbody2D rb)
    {
        speed = Mathf.Clamp01(direction.magnitude);
        rb.linearVelocity = speed * defaultSpeed * direction.normalized;
    }
}
