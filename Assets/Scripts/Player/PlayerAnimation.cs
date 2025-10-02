using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private InputActionReference look;
    [SerializeField] private Rigidbody2D rb;
    private Vector3 mousePos;
    private Animator animator;
    private Vector2 baseScale;
    private bool isWalking = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        baseScale = transform.localScale;
    }

    private void FixedUpdate()    
    {
        SetMousePosition();
        CheckRotation();
        if (rb.linearVelocity.magnitude > 0)
        {
            SwitchAnimation(true);
        }
        else
        {
            SwitchAnimation(false);
        }
    }

    /// <summary>
    /// �������� �������� ������ � Idle �� Walking � ��������
    /// </summary>
    /// <param name="walking"></param>
    private void SwitchAnimation(bool walking)
    {
        if (isWalking != walking)
        {
            isWalking = walking;
            animator.SetBool("Walking", isWalking);
        }
    }

    /// <summary>
    /// ������������ ������ � ����������� �� ���� � ����� ������� �� ���� ��������� ������
    /// </summary>
    private void CheckRotation()
    {
        //sr.flipX = mousePos.x < transform.position.x;
        Vector2 flip = new Vector2(mousePos.x < transform.position.x ? -baseScale.x : baseScale.x, baseScale.y);
        transform.localScale = flip;
    }

    /// <summary>
    /// �������� ������ � ������������ ������� � ������������
    /// </summary>
    private void SetMousePosition()
    {
        mousePos = Camera.main.ScreenToWorldPoint(look.action.ReadValue<Vector2>());
    }
}
