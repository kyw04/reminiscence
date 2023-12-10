using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float currentHealth;
    public float maxHealth;
    public NodeBase attribute;
    // 증강체
    public List<Pattern> patterns;

    public PlayerData()
    {
        this.currentHealth = 100.0f;
        this.maxHealth = 100.0f;
        this.attribute = null;
        this.patterns = new List<Pattern>();
    }

    public PlayerData(float currentHealth, float maxHealth, NodeBase attribute, List<Pattern> patterns)
    {
        this.currentHealth = currentHealth;
        this.maxHealth = maxHealth;
        this.attribute = attribute;
        this.patterns = patterns;
    }
}

[System.Serializable]
public class GameData
{
    public PlayerData playerData;
}

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }
    private string dataPath;
    public GameData gameData;

    void Awake()
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

    void Start()
    {
        dataPath = Path.Combine(Application.persistentDataPath, "GameData.json");
        //SaveGameData(); // 테스트용
        LoadGameData();
    }

    public void SaveGameData()
    {
        string jsonData = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(dataPath, jsonData);
        Debug.Log("Game data saved.");
    }

    public void LoadGameData()
    {
        if (File.Exists(dataPath))
        {
            string jsonData = File.ReadAllText(dataPath);
            gameData = JsonUtility.FromJson<GameData>(jsonData);
            Debug.Log("Game data loaded.");
        }
        else
        {
            Debug.LogError("not found data path");
        }
    }

    // 게임 종료 시 데이터 저장
    private void OnApplicationQuit()
    {
        SaveGameData();
    }
}
