using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

[CustomEditor(typeof(Pattern))]
public class PatternInspector : Editor
{
    Pattern pattern;

    void OnEnable()
    {
        pattern = target as Pattern;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.Space(10);

        Texture tempImage = (Texture)AssetDatabase.LoadAssetAtPath("Assets/6. Texture/pattern_inspector_image.png", typeof(Texture));
        GUILayoutOption[] smallOptions = new GUILayoutOption[] {
            GUILayout.Height(17.5f),
            GUILayout.MinHeight(17.5f),
            GUILayout.Width(17.5f),
            GUILayout.MinWidth(17.5f),
            GUILayout.ExpandHeight(false),
            GUILayout.ExpandWidth(false)
        };
        GUILayoutOption[] BigOptions = new GUILayoutOption[] {
            GUILayout.Height(100.0f),
            GUILayout.MinHeight(100.0f),
            GUILayout.Width(100.0f),
            GUILayout.MinWidth(100.0f),
            GUILayout.ExpandHeight(false),
            GUILayout.ExpandWidth(false)
        };

        EditorGUILayout.LabelField("Pattern");
        for (int i = 0; i < 3; i++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int j = 0; j < 3; j++)
            {
                NodeBase nodeBase = pattern.nodePatternTemp[j].index[i];
                Color color = nodeBase ? nodeBase.baseColor : Color.gray;
                GUI.color = color;
                GUILayout.Button("", smallOptions);
                GUI.color = Color.white;
                pattern.nodePatternTemp[j].index[i] = EditorGUILayout.ObjectField(nodeBase, typeof(NodeBase), false) as NodeBase;
                //pattern.nodePatternType[j, i] = nodeBase != null ? nodeBase.nodeType : NodeType.None;
            }
            EditorGUILayout.EndHorizontal();

            //EditorGUILayout.BeginHorizontal();
            //for (int j = 0; j < 3; j++)
            //{
            //    EditorGUILayout.EnumPopup(pattern.nodePatternType[j, i]);
            //}
            //EditorGUILayout.EndHorizontal();
        }
        GUI.color = Color.white;

        GUILayout.Space(25);
        EditorGUILayout.LabelField("Pattern Color");
        for (int i = 0; i < 3; i++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int j = 0; j < 3; j++)
            {
                NodeBase nodeBase = pattern.nodePatternTemp[j].index[i];
                Color color = nodeBase ? nodeBase.baseColor : Color.gray;
                GUI.color = color;
                GUILayout.Box(tempImage, BigOptions);
                //GUILayout.Button("", BigOptions);
            }
            EditorGUILayout.EndHorizontal();
        }
        GUI.color = Color.white;
        serializedObject.Update();

        if (GUI.changed) EditorUtility.SetDirty(target);
    }
}
