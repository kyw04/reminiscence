using System.Collections.Generic;
using UnityEngine;
using static Augment;

public class AugmentActive : MonoBehaviour
{
    public static AugmentActive instance;

    public GameObject fixedNode;
    public PlayerMovement playerMovement;
    public NodeBase nodeBase;
    public List<Augment> equipedAugments = new List<Augment>();

    private void Awake()
    {
        if (instance == null) { instance = this; }
    }

    //Gamemanager ���� �����ý��� Ŭ�������� ȣ��
    public void AugmentExecute(ActionType actionType)
    {
        var augments = GetAugments(actionType);
        foreach (var augment in augments)
        {
            ActivateEffect(augment);
        }
    }

    /*    public void SceneEnd()
        {
            var augments = GetSceneEndAugments(ActionType.SceneEnd);
            foreach(var augment in augments)
            {
                ActivateEffect(augment);
            }
        }
    */

    public List<Augment> GetAugments(ActionType actionType)
    {
        List<Augment> augments = new List<Augment>();

        foreach (var eAugment in GameStateManager.Instance.equipedAguments)
        {
            if (eAugment.actionType == actionType)
            {
                augments.Add(eAugment);
            }
        }

        return augments;
    }

    /*    public List<Augment> GetSceneEndAugments(ActionType actionType)
        {
            List<Augment> sceneEndAugments = new List<Augment>();

            foreach(var augment in GameStateManager.Instance.equipedAguments)
            {
                if(augment.actionType == actionType)
                {
                    sceneEndAugments.Add(augment);
                }
            }

            return sceneEndAugments;
        }
    */

    public void ActivateEffect(Augment augment)
    {
        switch (augment.augmentType)
        {
            case AugmentType.TwinBlades:
                TwinBlades();
                break;
            case AugmentType.FateRejector:
                Fatereject();
                break;
            case AugmentType.Eclipse:
                Eclipse();
                break;
            case AugmentType.MastersAmulet:
                MastersAmulet();
                break;
            case AugmentType.NaturalTalent:
                NaturalTalent();
                break;
            case AugmentType.BrokenHornOfTheBeast:
                BrokenHorn();
                break;
            case AugmentType.KingsChoice:
                KingChoice();
                break;
            case AugmentType.RoyalSigil:
                RoyalEmblem();
                break;
            case AugmentType.IdolOfJealousy:
                IdolOfJealous();
                break;
            case AugmentType.Meteor:
                Meteor();
                break;
            case AugmentType.IdolOfRejection:
                IdolOfRejection();
                break;
            case AugmentType.UnyieldingWill:
                IndomitableWill();
                break;
            case AugmentType.WanderingMemories:
                MemoriesOfWandering();
                break;
            case AugmentType.SignOfTheAbyss:
                OmenOfHell();
                break;
            case AugmentType.WillManifestation:
                ManifestationOfWill();
                break;
            case AugmentType.KeyOfTheGate:
                KeyOfDoor();
                break;
            case AugmentType.HeroicPath:
                HeroRoad();
                break;
            case AugmentType.ChosenOne:
                ChosenOne();
                break;
            // Add additional cases here for each AugmentType
            default:
                // Optional: Handle undefined augment type
                break;
        }
    }

    public void TwinBlades()
    {
        //��ɱ���
        if (GameManager.instance.foundPatternCount == 2 && GameManager.instance.gameState == GameState.EndTurn)
        {
            GameManager.instance.enemy.GetDamage(nodeBase, 10);
        }
    }

    public void Fatereject()
    {
        //������ �ϼ����� ���ϰ� �� ���Ḧ ���� ��� ���� ��ü ����
        if (GameManager.instance.foundPatternCount == 0)
        {
            HashSet<Node> nodes = new HashSet<Node>();
            for (int i = 0; i < GameManager.puzzleSize; i++)
            {
                for (int j = 0; j < GameManager.puzzleSize; j++)
                {
                    nodes.Add(GameManager.instance.puzzle[i, j]);
                }
            }

            GameManager.instance.NodeDelete(nodes);
        }
    }

    public void Eclipse()
    {
        Debug.Log("Eclipse");
        int center = GameManager.puzzleSize / 2 + GameManager.puzzleSize % 2 == 0 ? 0 : 1;
        Destroy(GameManager.instance.puzzle[center, center].transform.GetChild(0));
        Instantiate(fixedNode, GameManager.instance.puzzle[center, center].transform);

        if (playerMovement.canvasActivated)
        {
            // ���߾ӿ� ������ �� ���� ����� ����
            

            //GameNodeType.None;
        }

        if (GameManager.instance.gameState == GameState.EndTurn)
        {
            GameManager.instance.player.health += 5 / GameManager.instance.player.health;
        }
    }

    public void MastersAmulet()
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
        // �� ���� ����� �� ������ ��� �Ѱ��� �ı��ȴ�.
        int x = UnityEngine.Random.Range(0, GameManager.puzzleSize);
        int y = UnityEngine.Random.Range(0, GameManager.puzzleSize);
        GameManager.instance.NodeDelete(GameManager.instance.puzzle[x, y]);

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
        if (GameManager.instance.gameState == GameState.EndTurn && GameManager.instance.foundPatternCount > 0)
        {
            //�� ���� �� �ൿ Ƚ���� �����ִٸ� 10% Ȯ���� ���� ������ ����Ѵ�.
            int percent = UnityEngine.Random.Range(0, 10);
            if (percent == 0)
            {

            }
        }
    }

    public void IndomitableWill()
    {
        if (GameManager.instance.player.health <= 0)
        {
            //������ ��������ʰ�
            GameManager.instance.player.health = 1;
        }
    }
    public void MemoriesOfWandering()
    {
        // ������ ����� �� ���߾� 9���� ����� �ı��ȴ�.
        HashSet<Node> nodes = new HashSet<Node>();
        int[] dir = { -1, 0, 1 };
        int x = GameManager.puzzleSize / 2;
        int y = GameManager.puzzleSize / 2;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                int newX = x + dir[i];
                int newY = y + dir[j];

                nodes.Add(GameManager.instance.puzzle[newX, newY]);
            }
        }

        GameManager.instance.NodeDelete(nodes);
    }

    public void OmenOfHell()
    {
        // ������ ����� �� ���߾� 9���� ����� ������ ��ϵ��� �ı��ȴ�.
        HashSet<Node> nodes = new HashSet<Node>();

        for (int i = 0; i < GameManager.puzzleSize; i++)
        {
            for (int j = 0; j < GameManager.puzzleSize; j++)
            {
                if (i == 0 || i == GameManager.puzzleSize - 1 || j == 0 || j == GameManager.puzzleSize - 1)
                    nodes.Add(GameManager.instance.puzzle[i, j]);
            }
        }

        GameManager.instance.NodeDelete(nodes);
    }

    public void ManifestationOfWill()
    {
        GameManager.instance.maxMovementCount--;
        GameManager.instance.player.power += 10;
    }
    public void KeyOfDoor()
    {
        if (playerMovement.canvasActivated)
        {
            GameManager.instance.enemy.health -= 50 / GameManager.instance.enemy.maxHealth;
        }

        if (GameManager.instance.foundPatternCount == 5)
        {
            GameManager.instance.enemy.health = GameManager.instance.enemy.maxHealth;
        }
    }
    public void HeroRoad()
    {
        GameManager.instance.player.power *= 2;
        GameManager.instance.enemy.power *= 2;
    }

    public void ChosenOne()
    {
        GameManager.instance.player.power += 8;
    }
}
