using UnityEngine;

public abstract class StateTrigger : MonoBehaviour
{
    public bool IsTriggered { get; set; }

    protected EnemyController controller;
    protected EnemyState initialState;
    protected EnemyState triggerState;

    public void Initialize(EnemyController controller, EnemyState triggerState, EnemyState initialState)
    {
        this.controller = controller;
        this.triggerState = triggerState;
        this.initialState = initialState;
    }

    public void TriggerStart()
    {
        controller.StateTriggered(triggerState);
    }

    public void TriggerEnd()
    {
        controller.StateTriggered(initialState);
    }
}
