using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestData", menuName = "Scriptable Objects/QuestData")]
public class QuestData : ScriptableObject
{
    public string UID;
    public string Title => NamesHelper.ReceiveName(name);
    public string Description => NamesHelper.ReceiveName(name + "_description");
    public List<RecipeElement> Requirements;
    public List<RecipeElement> Reward;
    public int HoursToFinish = 24;

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
