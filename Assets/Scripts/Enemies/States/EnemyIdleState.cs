using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "Idle State", menuName = "States/Idle State")]
public class EnemyIdleState : EnemyState
{
    [SerializeField] private float patrolDistance = 3;
    public override void EnterState()
    {
        controller.CurrentNode = AStarManager.Instance.FindNearestNode(controller.transform.position);
        controller.Path.Clear();
    }

    public override void FixedUpdate()
    {
        Patrol();
        controller.FollowPath(controller.Speed);
    }

    private void Patrol()
    {
        if (controller.Path.Count == 0)
        {
            controller.Path = AStarManager.Instance.GeneratePath(controller.CurrentNode, AStarManager.Instance.RandomNode(controller.CurrentNode.Position, patrolDistance));
        }
    }
}
