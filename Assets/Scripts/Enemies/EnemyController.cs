using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{
    [Header("States")]
    [SerializeField] private EnemyIdleState idleState;
    [SerializeField] private EnemyFollowState followState;
    [SerializeField] private EnemyAttackState attackState;

    [Header("Triggers")]
    [SerializeField] private StateTrigger followTrigger;
    [SerializeField] private StateTrigger attackTrigger;

    [Header("Animator")]
    [SerializeField] private EnemyAnimator animator;

    [Header("Mask")]
    [SerializeField] private LayerMask wallMask;

    public EnemyStats Stats;

    public bool IsPlayerInFollowRange { get; set; }
    public bool IsPlayerInAttackRange { get; set; }

    private Rigidbody2D rb;
    public List<Node> Path = new();
    public Node CurrentNode;

    private bool isStopped = false;
    private bool canMove = true;

    public bool CanChangeStates = true;

    private EnemyStateMachine stateMachine;

    private EnemyState idleStateInstance;
    private EnemyState followStateInstance;
    private EnemyState attackStateInstance;

    private void Awake()
    {
        stateMachine = new();

        idleStateInstance = Instantiate(idleState);
        followStateInstance = Instantiate(followState);
        attackStateInstance = Instantiate(attackState);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        followTrigger.Initialize(this, followStateInstance, idleStateInstance);
        attackTrigger.Initialize(this, attackStateInstance, followStateInstance);

        idleStateInstance.Initialize(this, stateMachine);
        followStateInstance.Initialize(this, stateMachine);
        attackStateInstance.Initialize(this, stateMachine);

        stateMachine.Initialize(idleStateInstance);

        AStarManager.Instance.AddPathChangedListener(() => ReloadPath());
    }

    private void OnDestroy()
    {
        AStarManager.Instance.RemovePathChangedListener(() => ReloadPath());
    }

    private void Update()
    {
        if (isStopped) return;

        stateMachine.CurrentState.Update();
    }

    private void FixedUpdate()
    {
        if (isStopped) return;

        stateMachine.CurrentState.FixedUpdate();
    }

    public void Stop()
    {
        rb.linearVelocity = Vector2.zero;
        isStopped = true;
        CanChangeStates = false;
        animator.StopAnimator();
    }

    public void MoveEnemy(Vector2 velocity)
    {
        if (!canMove)
            return;
        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, velocity, 0.15f);
        animator.CheckFacingDirection(velocity);
        AnimationTriggerEvent(AnimationTriggerType.Move);
    }

    public void StateTriggered(EnemyState state)
    {
        if (!CanChangeStates) return;

        if (IsPlayerInAttackRange)
        {
            if (stateMachine.CurrentState != attackStateInstance)
                StateChange(attackStateInstance);
            return;
        }

        if (IsPlayerInFollowRange)
        {
            if (stateMachine.CurrentState != followStateInstance)
                StateChange(followStateInstance);
            return;
        }

        if (stateMachine.CurrentState != idleStateInstance)
        {
            StateChange(idleStateInstance);
            return;
        }
    }

    private void StateChange(EnemyState state)
    {
        stateMachine.ChangeState(state);
        rb.linearVelocity = Vector2.zero;
        Debug.Log($"State Changed to {state.GetType()}");
    }

    public void ReloadPath()
    {
        if (Path.Count != 0)
        {
            Node start = AStarManager.Instance.FindNearestNode(Path[0].Position);
            Node end = AStarManager.Instance.FindNearestNode(Path[Path.Count - 1].Position);
            Path = AStarManager.Instance.GeneratePath(start, end);
        }
        else
        {
            StopMovement();
        }
    }

    public void ChangePath(List<Node> newPath)
    {
        if (Path.Count != 0 && newPath.Count != 0 && Vector2.Distance(Path[0].Position, newPath[0].Position) <= 1)
            newPath[0] = Path[0];
        Path = newPath;
    }

    public void FollowPath(float speed)
    {
        if (Path.Count == 0 || Vector2.Distance(transform.position, Path[0].Position) > 2)
        {
            StopMovement();
            return;
        }

        CurrentNode = Path[0];
        Vector2 targetPos = CurrentNode.Position;
        Vector2 direction = (targetPos - (Vector2)transform.position).normalized;

        MoveEnemy(direction * speed);

        if (Vector2.Distance(transform.position, targetPos) < 0.1f)
            Path.RemoveAt(0);
    }

    private void StopMovement()
    {
        if (stateMachine.CurrentState != attackStateInstance) AnimationTriggerEvent(AnimationTriggerType.Idle);
        rb.linearVelocity = Vector2.zero;
    }

    public void AnimationTriggerEvent(AnimationTriggerType type)
    {
        switch (type)
        {
            case AnimationTriggerType.Attack: 
                animator.PlayAttackAnimation(); 
                break;
            case AnimationTriggerType.PrepareForAttack: 
                animator.PlayPrepareAttackAnimation(); 
                break;
            case AnimationTriggerType.Move: 
                animator.PlayWalkAnimation(); 
                break;
            default:
                animator.PlayIdleAnimation();
                break;
        }
    }

    public void Knockback(Vector3 source, float force)
    {
        rb.linearVelocity = Vector2.zero;
        Vector2 direction = (transform.position - source).normalized;
        rb.AddForce(direction * force, ForceMode2D.Impulse);
        CanChangeStates = false;
        canMove = false;
        StartCoroutine(WaitForKnockback(direction));
    }

    private IEnumerator WaitForKnockback(Vector2 direction)
    {
        int count = 0;
        while (count <= 25)
        {
            count++;
            if (InvalidateKnockback(direction))
                break;
            yield return new WaitForSeconds(0.01f);
        }
        rb.linearVelocity = Vector2.zero;
        CanChangeStates = true;
        canMove = true;
    }

    private bool InvalidateKnockback(Vector3 direction)
    {
        bool raycastHit = Physics2D.Raycast(transform.position, direction, 0.1f, wallMask);
        return raycastHit;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        foreach (Node node in Path)
        {
            if (node.CameFrom != null)
                Gizmos.DrawLine(node.CameFrom.Position, node.Position);
			Gizmos.DrawSphere(node.Position, 0.2F);
		}
    }
}
