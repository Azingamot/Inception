using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogueGenerator))]
public class DialogueGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DialogueGenerator generator = (DialogueGenerator)target;
        if (GUILayout.Button("Generate"))
        {
            generator.Generate();
        }
        if (GUILayout.Button("Generate with the specified language"))
        {
            generator.GenerateSpecified();
        }
        if (GUILayout.Button("Generate empty"))
        {
            generator.GenerateEmpty();
        }
    }
}