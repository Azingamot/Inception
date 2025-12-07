using UnityEngine;

[System.Serializable]
public class EnemyPosition
{
    public Vector3 Position;
    public string EnemyUID;
    public GameObject EnemyInstance { get; set; }
}
