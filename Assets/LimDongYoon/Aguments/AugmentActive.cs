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
    private GameManager gameManager;

    private void Awake()
    {
        if (instance == null) { instance = this; }
    }

    private void Start()
    {
        gameManager = GameManager.instance;
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
        if (gameManager.foundPatternCount == 2 && gameManager.gameState == GameState.EndTurn)
        {
            gameManager.enemy.GetDamage(nodeBase, 10);
        }
    }

    public void Fatereject()
    {
        //������ �ϼ����� ���ϰ� �� ���Ḧ ���� ��� ���� ��ü ����
        if (gameManager.foundPatternCount == 0)
        {
            HashSet<Node> nodes = new HashSet<Node>();
            for (int i = 0; i < GameManager.puzzleSize; i++)
            {
                for (int j = 0; j < GameManager.puzzleSize; j++)
                {
                    nodes.Add(gameManager.puzzle[i, j]);
                }
            }

            gameManager.NodeDelete(nodes);
        }
    }

    public void Eclipse()
    {
        if (playerMovement.canvasActivated)
        {
            // ���߾ӿ� ������ �� ���� ����� ����
            //GameNodeType.None;
        }

        if (gameManager.gameState == GameState.EndTurn)
        {
            gameManager.player.health += 5 / gameManager.player.health;
        }
    }

    public void MastersAmulet()
    {
        if (gameManager.gameState == GameState.End)
        {
            gameManager.player.power += 1;
        }
    }

    public void NaturalTalent()
    {
        gameManager.maxMovementCount += 1;
    }

    public void BrokenHorn()
    {
        // �� ���� ����� �� ������ ��� �Ѱ��� �ı��ȴ�.
        int x = UnityEngine.Random.Range(0, GameManager.puzzleSize);
        int y = UnityEngine.Random.Range(0, GameManager.puzzleSize);
        gameManager.NodeDelete(gameManager.puzzle[x, y]);

        gameManager.enemy.health -= 10 / gameManager.enemy.health;
    }

    public void KingChoice()
    {
        if (gameManager.foundPatternCount == 1)
        {
            gameManager.player.GetDamage(nodeBase, 0);
        }
    }

    public void RoyalEmblem()
    {
        if (gameManager.gameState == GameState.End)
        {
            gameManager.player.health += 30 / gameManager.player.maxHealth;
        }
    }
    public void IdolOfJealous()
    {
        if (gameManager.enemy.health > gameManager.player.health)
        {
            gameManager.player.power += 10;
        }
    }
    public void Meteor()
    {
        if (gameManager.gameState == GameState.EndTurn)
        {
            gameManager.enemy.health -= 10;
        }
    }
    public void IdolOfRejection()
    {
        if (gameManager.gameState == GameState.EndTurn && gameManager.foundPatternCount > 0)
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
        if (gameManager.player.health <= 0)
        {
            //������ ��������ʰ�
            gameManager.player.health = 1;
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

                nodes.Add(gameManager.puzzle[newX, newY]);
            }
        }

        gameManager.NodeDelete(nodes);
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
                    nodes.Add(gameManager.puzzle[i, j]);
            }
        }

        gameManager.NodeDelete(nodes);
    }

    public void ManifestationOfWill()
    {
        gameManager.maxMovementCount--;
        gameManager.player.power += 10;
    }
    public void KeyOfDoor()
    {
        if (playerMovement.canvasActivated)
        {
            gameManager.enemy.health -= 50 / gameManager.enemy.maxHealth;
        }

        if (gameManager.foundPatternCount == 5)
        {
            gameManager.enemy.health = gameManager.enemy.maxHealth;
        }
    }
    public void HeroRoad()
    {
        gameManager.player.power *= 2;
        gameManager.enemy.power *= 2;
    }

    public void ChosenOne()
    {
        gameManager.player.power += 8;
    }
}
