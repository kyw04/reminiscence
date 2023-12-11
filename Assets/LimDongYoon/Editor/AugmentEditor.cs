using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Augment))]
public class AugmentEditor : Editor
{
    SerializedProperty spriteProp;
    private void OnEnable()
    {
        spriteProp = serializedObject.FindProperty("sprite");
    }
    public override void OnInspectorGUI()
    {
        
        serializedObject.Update(); // SerializedObject를 업데이트합니다.

        Augment augment = (Augment)target;

        EditorGUI.BeginChangeCheck(); // 변경 감지를 시작합니다.

        // Enum 팝업을 통해 증강체 유형을 선택합니다.
        augment.augmentType = (Augment.AugmentType)EditorGUILayout.EnumPopup("Augment Type", augment.augmentType);

        // 선택된 enum 값에 따라 다른 필드 값을 설정합니다.
        switch (augment.augmentType)
        {
            case Augment.AugmentType.TwinBlades:
                UpdateAugmentFields(augment, 40000, "쌍둥이의 검", "턴이 종료될 때 완성된 문양의 개수가 2개일 경우 적의 체력을 10 감소 시킨다.", Augment.ActionType.TurnEnd, 0);
                break;
            case Augment.AugmentType.FateRejector:
                UpdateAugmentFields(augment, 40001, "운명 거부자", "문양을 완성하지 못하고 턴 종료를 했을 경우 퍼즐을 전부 셔플 한다.", Augment.ActionType.TurnEnd, 0);
                break;
            case Augment.AugmentType.Eclipse:
                UpdateAugmentFields(augment, 40002, "일식", "전투가 시작될 때 정중앙에 움직일 수 없는 블록을 생성한다. 내 턴이 종료될 때 최대 체력의 5%만큼 회복한다.", Augment.ActionType.Both, 0);
                break;
            case Augment.AugmentType.MastersAmulet:
                UpdateAugmentFields(augment, 40003, "스승의 모자", "스테이지를 클리어할 때마다 공격력이 1 증가한다.", Augment.ActionType.SceneEnd, 0);
                break;
            case Augment.AugmentType.NaturalTalent:
                UpdateAugmentFields(augment, 40004, "천부적 재능", "행동 횟수가 영구히 1회 증가한다.", Augment.ActionType.Continuous, 0);
                break;
            case Augment.AugmentType.BrokenHornOfTheBeast:
                UpdateAugmentFields(augment, 40005, "마수의 부러진 뿔", "적턴이 종료될 때 랜덤한 블록 한 개가 파괴된다. 적의 체력이 10%감소된 채로 시작한다.", Augment.ActionType.Both, 0);
                break;
            case Augment.AugmentType.KingsChoice:
                UpdateAugmentFields(augment, 40006, "왕의 선택", "첫 턴에는 적에게 받는 피해량이 0으로 변경된다.", Augment.ActionType.OnHit, 0);
                break;
            case Augment.AugmentType.RoyalSigil:
                UpdateAugmentFields(augment, 40007, "왕실의 문양", "스테이지를 클리어할 때마다 최대체력의 30%만큼 회복한다.", Augment.ActionType.SceneEnd, 0);
                break;
            case Augment.AugmentType.IdolOfJealousy:
                UpdateAugmentFields(augment, 40008, "질투의 우상", "적보다 체력이 낮으면 공격력이 10 증가한다.", Augment.ActionType.Continuous, 0);
                break;
            case Augment.AugmentType.Meteor:
                UpdateAugmentFields(augment, 40009, "유성", "턴이 종료될 때 적의 체력을 10 감소시킨다.", Augment.ActionType.TurnEnd, 0);
                break;
            case Augment.AugmentType.IdolOfRejection:
                UpdateAugmentFields(augment, 40010, "거부의 우상", "턴 종료 시 행동 횟수가 남아있다면 10% 확률로 적의 공격을 방어한다.", Augment.ActionType.TurnEnd, 0);
                break;
            case Augment.AugmentType.UnyieldingWill:
                UpdateAugmentFields(augment, 40011, "불굴의 의지", "전투당 한번 체력이 0이하일때 전투가 종료되지 않고 플레이어의 체력을 1로 변경한다.", Augment.ActionType.Continuous, 0);
                break;
            case Augment.AugmentType.WanderingMemories:
                UpdateAugmentFields(augment, 40012, "방황의 기억", "적턴이 종료될 때 정중앙 9개의 블록이 파괴된다.", Augment.ActionType.TurnEnd, 2);
                break;
            case Augment.AugmentType.SignOfTheAbyss:
                UpdateAugmentFields(augment, 40013, "나락의 징조", "적턴이 종료될 때 정중앙 9개의 블록을 제외한 블록들이 파괴된다.", Augment.ActionType.TurnEnd, 3);
                break;
            case Augment.AugmentType.WillManifestation:
                UpdateAugmentFields(augment, 40014, "의지의 발현", "행동 횟수가 1회 감소한다. 공격력이 10 증가한다.", Augment.ActionType.SceneLoad, 0);
                break;
            case Augment.AugmentType.KeyOfTheGate:
                UpdateAugmentFields(augment, 40015, "문의 열쇠", "전투 시작시 적의 체력이 50% 감소한다. 5턴 후 적은 모든 체력을 회복한다.", Augment.ActionType.SceneLoad, 0);
                break;
            case Augment.AugmentType.HeroicPath:
                UpdateAugmentFields(augment, 40016, "영웅의 길", "플레이어 공격력이 2배 증가하고 적의 공격력도 2배 증가한다.", Augment.ActionType.Continuous, 0);
                break;
            case Augment.AugmentType.ChosenOne:
                UpdateAugmentFields(augment, 40017, "선택받은자", "플레이어 공격력이 8 증가한다.", Augment.ActionType.Continuous, 0);
                break;


        }


        // 나머지 필드를 그리는 기본 인스펙터 UI를 사용합니다.
        DrawSpritePreview(spriteProp);
        DrawDefaultInspector();

        if (EditorGUI.EndChangeCheck()) // 변경이 감지된 경우
        {
            EditorUtility.SetDirty(augment); // 변경 사항을 저장하도록 설정합니다.
        }

        serializedObject.ApplyModifiedProperties(); // 변경 사항을 적용합니다.
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

    private void UpdateAugmentFields(Augment augment, int id, string name, string description, Augment.ActionType actionType, int priority)
    {
        augment.id = id;
        augment.name = name;
        augment.description = description;
        augment.actionType = actionType;
        augment.priority = priority;
    }
}