using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Data", menuName = "Enemies/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string UID;
    public GameObject EnemyPrefab;

#if UNITY_EDITOR
    private void OnValidate()
    {
       if (string.IsNullOrEmpty(UID))
        {
            UID = GUID.Generate().ToString();
            EditorUtility.SetDirty(this);
        }
    }
#endif
}
