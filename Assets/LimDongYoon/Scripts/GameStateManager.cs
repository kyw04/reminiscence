using System;
using System.Collections.Generic;
using UnityEngine;
using Map;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    
    public static GameStateManager Instance { get; private set; }

    private MapManager mapManager;

    [Header("개발자 모드")] 
    public bool playerWinMode = false;
    public bool playerLoseMdoe = false;

    public float maxHealth = 100;
    public float health = 100;

    public Image hpBar;


    private List<Augment> tempAguments;
    public List<Augment> aguments = new List<Augment>();
    public List<Augment> equipedAguments = new List<Augment>();

    private List<Pattern> tempPatterns;
    public List<Pattern> patterns = new List<Pattern>();
    public List<Pattern> equipedPatterns = new List<Pattern>();


    public int stageLevel = 1;

    public CurrentBattleEnemyInfo currentBattlleInfo;
    public BattleResult LastBattleResult { get; set; }

    public MapNode mapNode;
    public Point point;


    private void OnEnable()
    {
        if (SceneManager.GetActiveScene().name == "StageScene")
        {
            hpBar = FindFirstObjectByType<hpBarTag>().GetComponent<Image>();
            mapManager = FindAnyObjectByType<MapManager>();
        }
    }

    public void Start()
    {

        currentBattlleInfo = new CurrentBattleEnemyInfo
        {
            currentStageLevel = 1,
            nodeElementalType = NodeElementalType.Fire,
            isBoss = false
        };
    }
    private void Awake()
    {
        tempAguments = aguments;
        tempPatterns = patterns;
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
    public IEnumerator UpdateHealthBar()
    {
        if(!hpBar)
            hpBar = FindFirstObjectByType<hpBarTag>().GetComponent<Image>(); 
        
        float duration = 10f; // 체력 바가 업데이트되는데 걸리는 시간 (초)
        float elapsed = 0f;

        float startFill = hpBar.fillAmount;
        float endFill = health / maxHealth;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            hpBar.fillAmount = Mathf.Lerp(startFill, endFill, elapsed / duration);
            yield return null;
        }

        // 최종 값으로 설정
       hpBar.fillAmount = endFill;
    }
    public void InitGame()
    {
        StartCoroutine(InitGameNextFrame());

        maxHealth = 100;
        health = 100;
        HealthImageUpdate();
        aguments = tempAguments;
        patterns = tempPatterns;
        equipedAguments = null;
        //equipedPatterns = null;
        stageLevel = 1;
        LastBattleResult = BattleResult.None;
        
        
    }
   
    public IEnumerator InitGameNextFrame()
    {

        //yield return new WaitForSeconds(1f);
        
        yield return null;
        FindAnyObjectByType<MapManager>().GenerateNewMap();
    }
}


public enum BattleResult
{
    None,
    Win,
    Lose
}

public class CurrentBattleEnemyInfo
{
    public int currentStageLevel = 0;
    public NodeElementalType nodeElementalType;
    public bool isBoss;
}

