using UnityEditor;
using UnityEngine;
using System.Collections;
using Unity.EditorCoroutines.Editor;

namespace Map
{
    [CustomEditor(typeof(NodeBlueprint))]
    public class NodeBlueprintEditor : Editor
    {
        SerializedProperty nodeTypeProp;
        SerializedProperty spriteProp;
        SerializedProperty spritesProp;
        SerializedProperty nodeElementalTypeProp;
        bool showSprite = false;

        void OnEnable()
        {
            nodeTypeProp = serializedObject.FindProperty("nodeType");
            spriteProp = serializedObject.FindProperty("sprite");
            spritesProp = serializedObject.FindProperty("sprites");
            nodeElementalTypeProp = serializedObject.FindProperty("nodeElementalType");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(nodeTypeProp);
            
            // MinorEnemy일 때만 sprites 배열을 표시
            if ((NodeType)nodeTypeProp.enumValueIndex == NodeType.MinorEnemy || (NodeType)nodeTypeProp.enumValueIndex == NodeType.Boss )
            {
                EditorGUILayout.PropertyField(spritesProp, true);
                EditorGUILayout.PropertyField(nodeElementalTypeProp);
                EditorGUILayout.PropertyField(spriteProp);
            }
            else
            {
                EditorGUILayout.PropertyField(spriteProp); // 항상 표시될 sprite 필드
            }

            if (GUILayout.Button("Show Sprite After Delay"))
            {
                EditorCoroutineUtility.StartCoroutineOwnerless(ShowSpriteAfterDelay(5)); // 5초 지연
            }

            if (showSprite)
            {
                DrawSpritePreview(spriteProp);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private IEnumerator ShowSpriteAfterDelay(float delay)
        {
            showSprite = false;
            yield return new WaitForSeconds(delay);
            showSprite = true;
        }

        private void DrawSpritePreview(SerializedProperty spriteProp)
        {
            Sprite sprite = (Sprite)spriteProp.objectReferenceValue;
            if (sprite != null)
            {
                Rect spriteRect = new Rect(sprite.textureRect.x / sprite.texture.width,
                    sprite.textureRect.y / sprite.texture.height,
                    sprite.textureRect.width / sprite.texture.width,
                    sprite.textureRect.height / sprite.texture.height);

                GUILayout.Label("", GUILayout.Height(400)); // 또는 원하는 높이
                Rect lastRect = GUILayoutUtility.GetLastRect();
                GUI.DrawTextureWithTexCoords(lastRect, sprite.texture, spriteRect);
                // DrawSpritePreview 함수 구현은 이전과 동일
            }
        }
    }
}