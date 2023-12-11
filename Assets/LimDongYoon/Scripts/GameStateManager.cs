using System.Collections.Generic;
using UnityEngine;
using Map;
using UnityEngine.UI;
public class GameStateManager : MonoBehaviour
{
    
    public static GameStateManager Instance { get; private set; }
    [Header("개발자 모드")] 
    public bool playerWinMode = false;
    public bool playerLoseMdoe = false;

    public float maxHealth = 100;
    public float health = 100;

    public Image hpBar; 



    public List<Augment> aguments = new List<Augment>();
    private CurrentBattleEnemy currentBattleEnemy;
    public BattleResult LastBattleResult { get; set; }

    public MapNode mapNode;
    public Point point;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InitGameState()
    {
        LastBattleResult = BattleResult.None;
    }

    public void StartBattle(MapNode _mapNode)
    {
        mapNode = _mapNode;
    }
    public BattleResult GetBattleResult()
    {
        
        var temp = LastBattleResult;
        if (LastBattleResult == BattleResult.None)
        {
            Debug.Log("배틀결과가 없습니다.");
            return BattleResult.None;
        }
        
        LastBattleResult = BattleResult.None;
        mapNode = null;
        return temp;
    }

    public void SetBattleResult(BattleResult result)
    {
        switch (result)
        {
            case BattleResult.Win:

                break;
            case BattleResult.Lose:
                break;
        }
        LastBattleResult = result;
    }
    public void HealthImageUpdate()
    {
        Debug.Log("Update HP image");
        if(!hpBar)
        hpBar = FindFirstObjectByType<hpBarTag>().GetComponent<Image>(); 
        hpBar.fillAmount = health / maxHealth;

    }

}


public enum BattleResult
{
    None,
    Win,
    Lose
}

public class CurrentBattleEnemy
{
    public int currentStage = 0;
    public NodeElementalType nodeElementalType;
    public bool isBoss;
    
}

