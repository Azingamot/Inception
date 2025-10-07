using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer sr;
    private Vector2 baseScale;
    private bool isWalking = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        baseScale = transform.localScale;
    }
    public void Animate(Rigidbody2D rb, Vector2 mousePos)
    {
        CheckRotation(mousePos);
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
    /// »змен€ет анимацию игрока с Idle на Walking и наоборот
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
    /// ѕоворачивает игрока в зависимости от того в какой стороне от него находитс€ курсор
    /// </summary>
    private void CheckRotation(Vector2 mousePos)
    {
        //sr.flipX = mousePos.x < transform.position.x;
        Vector2 flip = new Vector2(mousePos.x < transform.position.x ? -baseScale.x : baseScale.x, baseScale.y);
        transform.localScale = flip; 
    }
}
