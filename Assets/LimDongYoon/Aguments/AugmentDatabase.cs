using System;
using System.Collections.Generic;
using UnityEngine;
using static Augment;

public class AugmentDatabase : ScriptableObject
{
    public List<Augment> augments = new List<Augment>();

    private void Awake()
    {
        
    }

    public void CreateArguments()
    {
        
    /*// 쌍둥이의 검
    augments.Add(CreateAugment(40000, "쌍둥이의 검", "턴이 종료될 때 완성된 문양의 개수가 2개일 경우 적의 체력을 10 감소 시킨다.", Augment.ActionType.TurnEnd, 0));

    // 운명 거부자
    augments.Add(CreateAugment(40001, "운명 거부자", "문양을 완성하지 못하고 턴 종료를 했을 경우 퍼즐을 전부 셔플 한다.", Augment.ActionType.TurnEnd, 0));

    // 일식
    augments.Add(CreateAugment(40002, "일식", "전투가 시작될 때 정중앙에 움직일 수 없는 블록을 생성한다.", Augment.ActionType.SceneLoadAndBlockSpawn, 0));

    // 스승의 모자
    augments.Add(CreateAugment(40003, "스승의 모자", "스테이지를 클리어할 때마다 공격력이 1 증가한다.", Augment.ActionType.SceneEnd, 0));

    // 천부적 재능
    augments.Add(CreateAugment(40004, "천부적 재능", "행동 횟수가 영구히 1회 증가한다", Augment.ActionType.Always, 0));

    // 마수의 부러진 뿔
    augments.Add(CreateAugment(40005, "마수의 부러진 뿔", "적턴이 종료될 때 랜덤한 블록 한 개가 파괴된다.", Augment.ActionType.SceneLoadAndBlockSpawn, 0));

    // 왕의 선택
    augments.Add(CreateAugment(40006, "왕의 선택", "첫 턴에는 적에게 받는 피해량이 0으로 변경된다.", Augment.ActionType.EnemyAttack, 0));

    // 왕실의 문양
    augments.Add(CreateAugment(40007, "왕실의 문양", "스테이지를 클리어할 때마다 최대체력의 30%만큼 회복한다.", Augment.ActionType.SceneEnd, 0));

    // 질투의 우상
    augments.Add(CreateAugment(40008, "질투의 우상", "적보다 체력이 낮으면 공격력이 10 증가한다.", Augment.ActionType.Always, 0));

    // 유성
    augments.Add(CreateAugment(40009, "유성", "턴이 종료될 때 적의 체력을 10 감소시킨다.", Augment.ActionType.TurnEnd, 0));

    // 거부의 우상
    augments.Add(CreateAugment(40010, "거부의 우상", "턴 종료 시 행동 횟수가 남아있다면 10% 확률로 적의 공격을 방어한다.", Augment.ActionType.TurnEnd, 0));

    // 불굴의 의지
    augments.Add(CreateAugment(40011, "불굴의 의지", "전투당 한번 체력이 0이하일때 전투가 종료되지 않고 플레이어의 체력을 1로 변경한다.", Augment.ActionType.Always, 0));

    // 방황의 기억
    augments.Add(CreateAugment(40012, "방황의 기억", "적턴이 종료될 때 정중앙 9개의 블록이 파괴된다.", Augment.ActionType.TurnEnd, 2));

    // 나락의 징조
    augments.Add(CreateAugment(40013, "나락의 징조", "적턴이 종료될 때 정중앙 9개의 블록을 제외한 블록들이 파괴된다.", Augment.ActionType.TurnEnd, 3));

    // 의지의 발현
    augments.Add(CreateAugment(40014, "의지의 발현", "행동 횟수가 1회 감소한다. 공격력이 10 증가한다.", Augment.ActionType.SceneLoadAndBlockSpawn, 0));

    // 문의 열쇠
    augments.Add(CreateAugment(40015, "문의 열쇠", "전투 시작시 적의 체력이 50% 감소한다. 5턴 후 적은 모든 체력을 회복한다.", Augment.ActionType.SceneLoadAndEnemySpawn, 0));

    // 영웅의 길
    augments.Add(CreateAugment(40016, "영웅의 길", "플레이어 공격력이 2배 증가하고 적의 공격력도 2배 증가한다.", Augment.ActionType.Always, 0));

    // 선택받은자
    augments.Add(CreateAugment(40017, "선택받은자", "플레이어 공격력이 8 증가한다.", Augment.ActionType.Always, 0));*/


        // 다른 증강체들도 이와 유사하게 추가합니다...
    }

    Augment CreateAugment(int id, string name, string description, Augment.ActionType actionType, int priority)
    {
        Augment augment = ScriptableObject.CreateInstance<Augment>();
        augment.id = id;
        augment.name = name;
        augment.description = description;
        augment.actionType = actionType;
        augment.priority = priority;
        return augment;
    }
}