using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    public enum BattleResult { None, Win, Lose }
    public BattleResult LastBattleResult { get; private set; }

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
