using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "EventData", menuName = "Events/EventData")]
public class EventData : ScriptableObject
{
    public string UID;
    public int StartDay;
    public int EndDay;
    public int LowerThreshold;
    public int UpperThreshold;
    public float Chance;
    public bool HappenOnce = false;

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
