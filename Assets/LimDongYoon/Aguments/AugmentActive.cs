using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Augment;

public class AugmentActive : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public NodeBase nodeBase;
    public List<Augment> equipedAugments = new List<Augment>();

    

    //Gamemanager 같은 전투시스템 클래스에서 호출
    public void TurnEnd()
    {
        var augments = GetTurnEndAugments(ActionType.TurnEnd);
        foreach (var augment in augments)
        {
            ActivateEffect(augment);
        }
    }
    public void SceneLoadAndBlockSpawn() { }

    public List<Augment> GetTurnEndAugments(ActionType actionType)
    {
        List<Augment> turnEndAugments = new List<Augment>();

        foreach (var augment in GameStateManager.Instance.equipedAguments)
        {
            if (augment.actionType == actionType)
            {
                turnEndAugments.Add(augment);
            }
        }

        return turnEndAugments;
    }



public void ActivateEffect(Augment augment)
    {
        switch (augment.augmentType)
        {
            case AugmentType.TwinBlades:
                TwinBlades();
                break;
            case AugmentType.FateRejector:
                break;
             
        }
        
    }

    public void TwinBlades()
    {
        //기능구현

        if (GameManager.instance.foundPatternCount == 2 && GameManager.instance.gameState == GameState.EndTurn)
        {
            GameManager.instance.enemy.GetDamage(nodeBase, 10);
        }
    }

    public void Fatereject()
    {
        //문양을 완성하지 못하고 턴 종료를 했을 경우 퍼즐 전체 셔플
    }

    public void Eclipse()
    {
        if (playerMovement.canvasActivated)
        {
            // 정중앙에 움직일 수 없는 블록을 생성
            //GameNodeType.None;
        }

        if(GameManager.instance.gameState == GameState.EndTurn)
        {
            GameManager.instance.player.health += 5 / GameManager.instance.player.health;
        }
    }

    public void TeacherHat()
    {
        if (GameManager.instance.gameState == GameState.End)
        {
            GameManager.instance.player.power += 1;
        }
    }

    public void NaturalTalent()
    {
        GameManager.instance.maxMovementCount += 1;
    }

    public void BrokenHorn()
    {
        // 적 턴이 종료될 때 랜덤한 블록 한개가 파괴된다.

        GameManager.instance.enemy.health -= 10 / GameManager.instance.enemy.health;
    }
    public void KingChoice()
    {
        if (GameManager.instance.foundPatternCount == 1)
        {
            GameManager.instance.player.GetDamage(nodeBase, 0);
        }
    }

    public void RoyalEmblem()
    {
        if (GameManager.instance.gameState == GameState.End)
        {
            GameManager.instance.player.health += 30 / GameManager.instance.player.maxHealth;
        }
    }
    public void IdolOfJealous()
    {
        if (GameManager.instance.enemy.health > GameManager.instance.player.health)
        {
            GameManager.instance.player.power += 10;
        }
    }
    public void Meteor()
    {
        if (GameManager.instance.gameState == GameState.EndTurn)
        {
            GameManager.instance.enemy.health -= 10;
        }
    }
    public void IdolOfRejection()
    {
        if (GameManager.instance.gameState == GameState.EndTurn && GameManager.instance.foundPatternCount != 3)
        {
            //턴 종료 시 행동 횟수가 남아있다면 10% 확률로 적의 공격을 방어한다.
        }
    }

    public void IndomitableWill()
    {
        if(GameManager.instance.player.health <= 0)
        {
            //전투가 종료되지않고
            GameManager.instance.player.health = 1;
        }
    }
    public void MemoriesOfWandering()
    {
        // 적턴이 종료될 때 정중앙 9개의 블록이 파괴된다.
    }

    public void OmenOfHell()
    {
        // 적턴이 종료될 때 정중앙 9개의 블록을 제외한 블록들이 파괴된다.
    }

    public void ManifestationOfWill()
    {
        GameManager.instance.maxMovementCount--;
        GameManager.instance.player.power += 10;
    }
    public void KeyOfDoor()
    {
        if(playerMovement.canvasActivated)
        {
            GameManager.instance.enemy.health -= 50 / GameManager.instance.enemy.maxHealth;
        }

        if(GameManager.instance.foundPatternCount == 5)
        {
            GameManager.instance.enemy.health = GameManager.instance.enemy.maxHealth;
        }
    }
    public void HeroRoad()
    {
        GameManager.instance.player.power *= 2;
        GameManager.instance.enemy.power*= 2;
    }

    public void ChosenOne()
    {
        GameManager.instance.player.power += 8;
    }
}
