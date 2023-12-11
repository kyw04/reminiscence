
using Map;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAugment", menuName = "Augment/Augment")]
[System.Serializable]
public class Augment : ScriptableObject
{
    public enum ActionType
    {
        TurnEnd,
        SceneLoadAndBlockSpawn,
        SceneEnd,
        Always,
        EnemyAttack,
        SceneLoadAndEnemySpawn,
        Continuous,
        Both,
        OnHit,
        TurnStart,
        SceneLoad
    }
    public enum AugmentType
    {
        TwinBlades = 40000,
        ChosenOne = 40001,
        FateRejector = 40002,
        Eclipse = 40003,
        MastersAmulet = 40004,
        NaturalTalent = 40005,
        BrokenHornOfTheBeast = 40006,
        KingsChoice = 40007,
        RoyalSigil = 40008,
        IdolOfJealousy = 40009,
        MirrorOfBetrayal = 40010,
        Meteor = 40011,
        IdolOfRejection = 40012,
        ShieldOfWill = 40013,
        ConcentrationStaff = 40014,
        DiffractionStaff = 40015,
        NecklaceOfWill = 40016,
        SealedMemoriesNecklace = 40017,
        BambooNecklace = 40018,
        HeroicNecklace = 40019,
        InfiniteWell = 40020
       
    }
    
    public int id;
    public string name;
    public string description;
    public ActionType actionType;
    public int priority;
    public AugmentType augmentType;
    public NodeBase nodeBase;
    public PlayerMovement playerMovement;


    public Augment(int id, string name, string description, ActionType actionType)
    {
        this.id = id;
        this.name = name;
        this.description = description;
        this.actionType = actionType;
    }

    // 각 증강체의 특별한 행동을 여기에 구현할 수 있습니다.
    public void ActivateEffect()
    {
        switch (id)
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
        if(playerMovement.canvasActivated)
        {
           
        }
    }

    public void TeacherHat()
    {
        if(GameManager.instance.gameState == GameState.End)
        {
 
        }
    }
}

