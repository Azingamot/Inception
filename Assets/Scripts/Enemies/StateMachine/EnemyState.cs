using UnityEngine;

public class EnemyState: ScriptableObject
{
    protected EnemyController controller;
    protected EnemyStateMachine stateMachine;

    public virtual void Initialize(EnemyController controller, EnemyStateMachine stateMachine)
    {
        this.controller = controller;
        this.stateMachine = stateMachine;
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void AnimationTriggerEvent(AnimationTriggerType type) { }
}
