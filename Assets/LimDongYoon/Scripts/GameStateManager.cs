using UnityEngine;
using Map;
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }
    [Header("개발자 모드")] 
    
    public bool playerWinMode = false;
    public bool playerLoseMdoe = false;

    
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
}

public enum BattleResult
{
    None,
    Win,
    Lose
}

