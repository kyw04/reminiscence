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

    //Gamemanager 같은 전투시스템 클래스에서 호출
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
        Debug.Log("증강체 실행 " +augment.name + " 효과: " + augment.description );
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
        //기능구현
        if (GameManager.instance.foundPatternCount == 2 && GameManager.instance.gameState == GameState.EndTurn)
        {
            GameManager.instance.enemy.GetDamage(nodeBase, 10);
        }
    }

    public void Fatereject()
    {
        //문양을 완성하지 못하고 턴 종료를 했을 경우 퍼즐 전체 셔플
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
        int center = GameManager.puzzleSize / 2 - 1;
        center += GameManager.puzzleSize % 2 == 0 ? 0 : 1;
        Node newFixedNode = Instantiate(fixedNode, GameManager.instance.puzzle[center, center].transform.parent).GetComponent<Node>();
        newFixedNode.ChangeNodeBase(GameManager.instance.puzzle[center, center]);
        Destroy(GameManager.instance.puzzle[center, center].gameObject);
        GameManager.instance.puzzle[center, center] = newFixedNode;

        if (playerMovement.canvasActivated) // 이거 없다고 뜨는데요
        {
            // 정중앙에 움직일 수 없는 블록을 생성
            //int center = GameManager.puzzleSize / 2 - 1;
            //center += GameManager.puzzleSize % 2 == 0 ? 0 : 1;
            //Node newFixedNode = Instantiate(fixedNode, GameManager.instance.puzzle[center, center].transform.parent).GetComponent<Node>();
            //newFixedNode.ChangeNodeBase(GameManager.instance.puzzle[center, center]);
            //Destroy(GameManager.instance.puzzle[center, center].gameObject);
            //GameManager.instance.puzzle[center, center] = newFixedNode;

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
        // 적 턴이 종료될 때 랜덤한 블록 한개가 파괴된다.
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
            var enemy = GameManager.instance.enemy;
            enemy.GetDamage(10);
            
        }
    }
    public void IdolOfRejection()
    {
        if (GameManager.instance.gameState == GameState.EndTurn && GameManager.instance.foundPatternCount > 0)
        {
            //턴 종료 시 행동 횟수가 남아있다면 10% 확률로 적의 공격을 방어한다.
            int percent = UnityEngine.Random.Range(0, 10);
            if (percent == 0)
            {
                GameManager.instance.player.health += GameManager.instance.enemy.damage;
            }
        }
    }

    public void IndomitableWill()
    {
        if (GameManager.instance.player.health - GameManager.instance.enemy.damage <= 0)
        {
            //전투가 종료되지않고
            GameManager.instance.player.health = GameManager.instance.enemy.damage + 1;
        }
    }
    public void MemoriesOfWandering()
    {
        // 적턴이 종료될 때 정중앙 9개의 블록이 파괴된다.
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
        // 적턴이 종료될 때 정중앙 9개의 블록을 제외한 블록들이 파괴된다.
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
