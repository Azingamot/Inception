using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StringsGenerator))]
public class StringsGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        StringsGenerator generator = (StringsGenerator)target;
        if (GUILayout.Button("Generate"))
        {
            generator.Generate();
            Debug.Log("string loaded!");
        }
    }
}