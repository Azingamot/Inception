using System.Collections;
using System.IO;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(fileName = "Follow State", menuName = "States/Follow State")]
public class EnemyFollowState : EnemyState
{
    private Coroutine pathFindingCoroutine;
    float minPathUpdateTime = 0.2f;
    float maxPathUpdateTime = 0.05f;

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
        controller.FollowPath(controller.Stats.BaseSpeed);
    }

    private IEnumerator PathFindingLoop()
    {
        while (true)
        {
            Node enemyNode = AStarManager.Instance.FindNearestNode(controller.transform.position);
            Node targetNode = AStarManager.Instance.FindNearestNode(PlayerPosition.GetData());

            float distance = Vector2.Distance(enemyNode.Position, targetNode.Position);

            float updateDelay = Mathf.Lerp(minPathUpdateTime, maxPathUpdateTime, distance / 10f);

            controller.ChangePath(AStarManager.Instance.GeneratePath(enemyNode, targetNode));

            yield return new WaitForSeconds(updateDelay);
        }
    }
}
