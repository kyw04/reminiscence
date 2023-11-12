using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Pattern))]
public class PatternInspector : Editor
{
    Pattern pattern;
    NodeBase nodeBase;

    void OnEnable()
    {
        pattern = target as Pattern;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayoutOption[] options = new GUILayoutOption[] {
            GUILayout.Width(25.0f),
            GUILayout.MinWidth(25.0f),
            GUILayout.ExpandWidth(false)
        };

        EditorGUILayout.LabelField("Pattern");
        for (int i = 0; i < 3; i++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int j = 0; j < 3; j++)
            {
                NodeBase nodeBase = pattern.nodePatternTemp[i].index[j];
                Color color = nodeBase ? nodeBase.baseColor : Color.gray;
                GUI.color = color;
                GUILayout.Button("", options);
                GUI.color = Color.white;
                pattern.nodePatternTemp[i].index[j] = EditorGUILayout.ObjectField(nodeBase, typeof(NodeBase), false) as NodeBase;
            }
            EditorGUILayout.EndHorizontal();
        }
        GUI.color = Color.white;
        serializedObject.Update();

        if (GUI.changed) EditorUtility.SetDirty(target);
    }
}
