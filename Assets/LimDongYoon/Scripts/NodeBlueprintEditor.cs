using UnityEditor;
using UnityEngine;

namespace Map
{
    [CustomEditor(typeof(NodeBlueprint))]
    public class NodeBlueprintEditor : Editor
    {
        SerializedProperty nodeTypeProp;
        SerializedProperty spriteProp;
        SerializedProperty spritesProp;
        SerializedProperty nodeElementalTypeProp;

        void OnEnable()
        {
            nodeTypeProp = serializedObject.FindProperty("nodeType");
            spriteProp = serializedObject.FindProperty("sprite");
            spritesProp = serializedObject.FindProperty("sprites");
            nodeElementalTypeProp = serializedObject.FindProperty("nodeElementalTypeProp");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(nodeTypeProp);
            

            // MinorEnemy일 때만 sprites 배열을 표시
            if ((NodeType)nodeTypeProp.enumValueIndex == NodeType.MinorEnemy)
            {
                EditorGUILayout.PropertyField(spritesProp, true);
                EditorGUILayout.PropertyField(nodeElementalTypeProp);
                
            }
            else
            {
                EditorGUILayout.PropertyField(spriteProp); // 항상 표시될 sprite 필드
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}