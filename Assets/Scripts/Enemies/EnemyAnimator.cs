using UnityEngine;

[RequireComponent (typeof(Animator))]
public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] private Animator weaponAnimator;
    private Animator mainAnimator;
    private bool isFacingRight;
    private Vector2 baseScale;

    private void Awake()
    {
        baseScale = transform.localScale;
        mainAnimator = GetComponent<Animator>();
    }

    public void CheckFacingDirection(Vector2 direction)
    {
        if (isFacingRight && direction.x < 0)
        {
            Vector2 flip = new Vector2(-baseScale.x, baseScale.y);
            transform.localScale = flip;
            isFacingRight = !isFacingRight;
        }
        else if (!isFacingRight && direction.x > 0)
        {
            Vector2 flip = new Vector2(baseScale.x, baseScale.y);
            transform.localScale = flip;
            isFacingRight = !isFacingRight;
        }
    }

    public void PlayPrepareAttackAnimation()
    {
        mainAnimator.SetTrigger("Attack");
        mainAnimator.SetFloat("State", 0);
    }

    public void PlayAttackAnimation()
    {
        RotateWeaponToPlayer();
        weaponAnimator.SetTrigger("Attack");
    }

    public void PlayWalkAnimation()
    {
        mainAnimator.SetFloat("State", 1);
    }

    public void PlayIdleAnimation()
    {
        mainAnimator.SetFloat("State", 0);
    }

    public void StopAnimator()
    {
        weaponAnimator.StopPlayback();
        mainAnimator.StopPlayback();
        mainAnimator.enabled = false;
        weaponAnimator.enabled = false;
    }

    private void RotateWeaponToPlayer()
    {
        Vector3 playerPos = PlayerPosition.GetData();
        Vector2 direction = (playerPos - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;

        weaponAnimator.transform.rotation = Quaternion.Euler(0,0,angle);
    }
}
