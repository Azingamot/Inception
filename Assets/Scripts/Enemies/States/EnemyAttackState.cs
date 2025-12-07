using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack State", menuName = "States/Attack State")]
public class EnemyAttackState : EnemyState
{
    private float attackTimer = 0;
    [SerializeField] private float timeBetweenAttacks = 2;
    [SerializeField] private float attackDelay = 0.5f;
    [SerializeField] private float firstAttackOffset = 0.5f;

    public override void AnimationTriggerEvent(AnimationTriggerType type)
    {
        base.AnimationTriggerEvent(type);
    }

    public override void EnterState()
    {
        attackTimer = firstAttackOffset;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FixedUpdate()
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            Attack();
            attackTimer = timeBetweenAttacks + attackDelay;
        }
    }
    
    private void Attack()
    {
        controller.CanChangeStates = false;
        controller.AnimationTriggerEvent(AnimationTriggerType.PrepareForAttack);
        controller.StartCoroutine(WaitForAttack());
    }

    private IEnumerator WaitForAttack()
    {
        yield return new WaitForSeconds(attackDelay);
        controller.AnimationTriggerEvent(AnimationTriggerType.Attack);
        controller.CanChangeStates = true;
    }

    public override void Update()
    {
        base.Update();
    }
}
