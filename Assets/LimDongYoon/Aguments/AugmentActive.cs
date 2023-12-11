using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AugmentActive : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public NodeBase nodeBase;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateEffect(Augment augment)
    {
        switch (augment.id)
        {
            case 40000:
                // "쌍둥이의 검" 효과 구현
                TwinBlades();

                break;
            // 다른 증강체 효과들을 여기에 구현합니다.

            case 40001:

            case 40002:
                Eclipse();
                break;

            case 40003:
                TeacherHat();
                break;

            case 40004:
                NaturalTalent();
                break;

            case 40005:
                BrokenHorn();
                break;
            // ...
            default:
                break;
        }
    }

    public void TwinBlades()
    {
        if (GameManager.instance.foundPatternCount == 2 && GameManager.instance.gameState == GameState.EndTurn)
        {
            GameManager.instance.enemy.GetDamage(nodeBase, 10);
        }
    }

    public void Fatereject()
    {

    }

    public void Eclipse()
    {
        if (playerMovement.canvasActivated)
        {
            //GameNodeType.None;
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
            //
        }
    }

    public void IndomitableWill()
    {

    }


}
