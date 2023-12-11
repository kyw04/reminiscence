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
                // "�ֵ����� ��" ȿ�� ����
                TwinBlades();

                break;
            // �ٸ� ����ü ȿ������ ���⿡ �����մϴ�.

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
        //������ �ϼ����� ���ϰ� �� ���Ḧ ���� ��� ���� ��ü ����
    }

    public void Eclipse()
    {
        if (playerMovement.canvasActivated)
        {
            // ���߾ӿ� ������ �� ���� ������ ����
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
        // �� ���� ����� �� ������ ���� �Ѱ��� �ı��ȴ�.

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
            //�� ���� �� �ൿ Ƚ���� �����ִٸ� 10% Ȯ���� ���� ������ ����Ѵ�.
        }
    }

    public void IndomitableWill()
    {
        if(GameManager.instance.player.health <= 0)
        {
            //������ ��������ʰ�
            GameManager.instance.player.health = 1;
        }
    }
    public void MemoriesOfWandering()
    {
        // ������ ����� �� ���߾� 9���� ������ �ı��ȴ�.
    }

    public void OmenOfHell()
    {
        // ������ ����� �� ���߾� 9���� ������ ������ ���ϵ��� �ı��ȴ�.
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