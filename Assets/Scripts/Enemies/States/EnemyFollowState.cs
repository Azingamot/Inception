using System.Collections;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Follow State", menuName = "States/Follow State")]
public class EnemyFollowState : EnemyState
{
    private Coroutine pathFindingCoroutine;

    public override void AnimationTriggerEvent(AnimationTriggerType type)
    {
        base.AnimationTriggerEvent(type);
    }

    public override void EnterState()
    {
        controller.CurrentNode = AStarManager.Instance.FindNearestNode(controller.transform.position);
        controller.Path.Clear();

        pathFindingCoroutine = controller.StartCoroutine(PathFindingLoop());
    }

    public override void ExitState()
    {
        controller.StopCoroutine(pathFindingCoroutine);
    }

    public override void FixedUpdate()
    {
        controller.FollowPath(controller.Speed);
    }

    private IEnumerator PathFindingLoop()
    {
        while (true)
        {
            Node enemyNode = AStarManager.Instance.FindNearestNode(controller.transform.position);
            Node targetNode = AStarManager.Instance.FindNearestNode(PlayerPosition.GetData());

            List<Node> path = AStarManager.Instance.GeneratePath(enemyNode, targetNode);

            float distance = Vector2.Distance(enemyNode.Position, path[0].Position);

            float updateDelay = distance / controller.Speed;

            controller.ChangePath(path);

            yield return new WaitForSeconds(updateDelay);
        }
    }
}
