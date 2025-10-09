using UnityEngine;

public class PlayerPosition : MonoBehaviour
{
    private static Transform player;

    private void Awake()
    {
        player = GameObject.FindAnyObjectByType<Player>().transform;
    }

    public static bool CheckIfInRadius(Vector2 pos, int radius)
    {
        return Vector2.Distance(GetData(), pos) < radius;
    }

    public static Vector2 GetData()
    {
        if (player != null)
            return player.localPosition;
        return Vector2.zero;
    }
}
