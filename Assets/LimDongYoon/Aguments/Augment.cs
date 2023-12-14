
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
        FateRejector = 40001,
        Eclipse = 40002,
        MastersAmulet = 40003,
        NaturalTalent = 40004,
        BrokenHornOfTheBeast = 40005,
        KingsChoice = 40006,
        RoyalSigil = 40007,
        IdolOfJealousy = 40008,
        Meteor = 40009,
        IdolOfRejection = 40010,
        UnyieldingWill = 40011,
        WanderingMemories = 40012,
        SignOfTheAbyss = 40013,
        WillManifestation = 40014,
        KeyOfTheGate = 40015,
        HeroicPath = 40016,
        ChosenOne = 40017,
       
    }


    public int id;
    public string name;
    public string description;
    public ActionType actionType;
    public int priority;
    public AugmentType augmentType;



    public Augment(int id, string name, string description, ActionType actionType)
    {
        this.id = id;
        this.name = name;
        this.description = description;
        this.actionType = actionType;
    }
}

