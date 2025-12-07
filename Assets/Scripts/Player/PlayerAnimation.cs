using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private ParticleSystem walkParticles;
    [SerializeField] private float timeToSpawn = 0.5f;
    private Animator animator;
    private Vector2 baseScale;
    private bool isWalking = false;
    private float timer = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        baseScale = transform.localScale;
    }
    public void Animate(Rigidbody2D rb, Vector2 mousePos)
    {
        CheckRotation(mousePos);
        if (rb.linearVelocity.magnitude > 0)
        {
            SpawnParticles();
            SwitchAnimation(true);
        }
        else
        {
            SwitchAnimation(false);
        }
    }

    /// <summary>
    /// Изменяет анимацию игрока с Idle на Walking и наоборот
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
    /// Проигрывает частицы ходьбы
    /// </summary>
    private void SpawnParticles()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            walkParticles.Play(false);
            timer = timeToSpawn;
        }
    }

    /// <summary>
    /// Поворачивает игрока в зависимости от того в какой стороне от него находится курсор
    /// </summary>
    private void CheckRotation(Vector2 mousePos)
    {;
        Vector2 flip = new Vector2(mousePos.x < transform.position.x ? -baseScale.x : baseScale.x, baseScale.y);
        transform.localScale = flip; 
    }
}
