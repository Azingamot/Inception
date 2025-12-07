using UnityEngine;

public enum TriggerType
{
    Follow,
    Attack
}

public class PlayerLocateTrigger : StateTrigger
{
    [SerializeField] private float viewDistance;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private TriggerType triggerType;

    private void FixedUpdate()
    {
        CheckForPlayer();
    }

    private void CheckForPlayer()
    {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, viewDistance, playerLayer);
        bool playerIn = collider != null && collider.GetComponent<Player>() != null;

        if (playerIn)
            PlayerInView();
        else
            PlayerLeaveView();
    }

    private void PlayerInView()
    {
        if (triggerType == TriggerType.Follow)
            controller.IsPlayerInFollowRange = true;
        else if (triggerType == TriggerType.Attack)
            controller.IsPlayerInAttackRange = true;

        TriggerStart();
    }

    private void PlayerLeaveView()
    {
        if (triggerType == TriggerType.Follow)
            controller.IsPlayerInFollowRange = false;
        else if (triggerType == TriggerType.Attack)
            controller.IsPlayerInAttackRange = false;

        TriggerEnd();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, viewDistance);
    }
}