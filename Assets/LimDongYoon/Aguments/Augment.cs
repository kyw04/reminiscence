
using Map;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAugment", menuName = "Augment/Augment")]
[System.Serializable]
public class Augment : ScriptableObject
{
    public Sprite sprite;
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
}

