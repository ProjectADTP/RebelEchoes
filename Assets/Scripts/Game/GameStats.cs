using UnityEngine;
using System;
using System.Collections.Generic;

public class GameStats : MonoBehaviour
{
    public enum LevelType
    {
        Cleanup,
        Waves,
        BossBattle,
        Escape
    }

    private LevelStats currentStats = new();
    public LevelType currentLevelType = LevelType.Cleanup;

    [Header("Score Settings")]
    [SerializeField] private int baseWinPoints = 1000;

    [Header("Player Components")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerBuffs playerBuffs;
    
    [SerializeField] private string currentLevelName;
    
    public static GameStats Instance { get; private set; }

    private float levelStartTime;
    
    private void Awake()
    {
        if (playerHealth == null)
            playerHealth = FindObjectOfType<PlayerHealth>();
            
        if (playerBuffs == null)
            playerBuffs = FindObjectOfType<PlayerBuffs>();
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SaveStartLevelData();
        LoadAllLevelData();
        StartLevelTimer();
        SubscribeToEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    public void StopLevelTimer()
    {
        currentStats.timeTaken = Time.time - levelStartTime;
        currentStats.completed = true;
        currentStats.score = CalculateScore();
        currentStats.stars = GetStars();

        SaveLevelData();
    }

    public int CalculateScore()
    {
        if (!currentStats.completed) 
            return 0;
        
        return Mathf.Max(0, baseWinPoints + Mathf.RoundToInt(currentStats.timeTaken * 10));
    }

    public int GetStars()
    {
        if (!currentStats.completed) 
            return 0;

        int stars = 0;
        float timeLimit = 0f;
        int buffLimit = 0;
        int damageLimit = 0;
        
        switch (currentLevelType)
        {
            case LevelType.Cleanup:
                timeLimit = 180f; buffLimit = 2; damageLimit = 3;
                break;
            case LevelType.Waves:
                timeLimit = 120f; buffLimit = 3; damageLimit = 3;
                break;
            case LevelType.BossBattle:
                timeLimit = 60f; buffLimit = 2; damageLimit = 2;
                break;
            case LevelType.Escape:
                timeLimit = 120f; buffLimit = 999; damageLimit = 3;
                break;
        }

        if (currentStats.timeTaken < timeLimit) stars++;
        if (currentStats.buffsUsed < buffLimit) stars++;
        if (currentStats.damageTaken <= damageLimit) stars++;

        return Mathf.Clamp(stars, 0, 3);
    }

    public void SetLevelType(LevelType type)
    {
        currentLevelType = type;
        ResetStats();
        StartLevelTimer();
    }

    private void ResetStats()
    {
        currentStats = new LevelStats();
    }
    
    private void SubscribeToEvents()
    {
        if (playerHealth != null) 
            playerHealth.OnPlayerHit += OnPlayerGotDamage;
        if (playerBuffs != null) 
            playerBuffs.OnBuffActivated += OnPlayerUsedBuff;
    }

    private void UnsubscribeFromEvents()
    {
        if (playerHealth != null) 
            playerHealth.OnPlayerHit -= OnPlayerGotDamage;
        if (playerBuffs != null) 
            playerBuffs.OnBuffActivated -= OnPlayerUsedBuff;
    }

    private void OnPlayerGotDamage()
    {
        currentStats.damageTaken++;
    }

    private void OnPlayerUsedBuff()
    {
        currentStats.buffsUsed++;
    }

    private void StartLevelTimer()
    {
        levelStartTime = Time.time;
    }

    private void SaveStartLevelData()
    {
        Dictionary<string, LevelSaveData> allLevelData = LoadAllLevelData();
        
        if (allLevelData.ContainsKey(currentLevelName))
            return;
        
        currentStats.completed = true;
        currentStats.score = 0;
        currentStats.stars = 0;
        
        allLevelData[currentLevelName] = new LevelSaveData(currentLevelName, currentStats);
        
        SaveAllLevelData(allLevelData);
    }

    private void SaveLevelData()
    {
        if (string.IsNullOrEmpty(currentLevelName))
            return;
        
        Dictionary<string, LevelSaveData> allLevelData = LoadAllLevelData();
        
        if (allLevelData.ContainsKey(currentLevelName))
        {
            LevelSaveData existingData = allLevelData[currentLevelName];
            
            if (IsBetterResult(currentStats, existingData.stats))
            {
                allLevelData[currentLevelName] = new LevelSaveData(currentLevelName, currentStats);
            }
        }
        else
        {
            allLevelData[currentLevelName] = new LevelSaveData(currentLevelName, currentStats);
        }
        
        SaveAllLevelData(allLevelData);
    }
    
    private bool IsBetterResult(LevelStats newStats, LevelStats oldStats)
    {
        if (newStats.stars > oldStats.stars) 
            return true;
        if (newStats.stars < oldStats.stars)
            return false;
        
        if (newStats.score > oldStats.score)
            return true;
        if (newStats.score < oldStats.score)
            return false;
        
        return newStats.timeTaken < oldStats.timeTaken;
    }

    private void SaveAllLevelData(Dictionary<string, LevelSaveData> data)
    {
        string json = JsonUtility.ToJson(new SerializationWrapper(data));
        PlayerPrefs.SetString("LevelSaveData", json);
        PlayerPrefs.Save();
    }
    
    public Dictionary<string, LevelSaveData> LoadAllLevelData()
    {
        Dictionary<string, LevelSaveData> data = new Dictionary<string, LevelSaveData>();
    
        try
        {
            if (PlayerPrefs.HasKey("LevelSaveData"))
            {
                string json = PlayerPrefs.GetString("LevelSaveData");
            
                if (!string.IsNullOrEmpty(json))
                {
                    SerializationWrapper wrapper = JsonUtility.FromJson<SerializationWrapper>(json);
                
                    if (wrapper != null && wrapper.levelDataList != null)
                    {
                        foreach (var item in wrapper.levelDataList)
                        {
                            if (item != null)
                            {
                                data[item.levelName] = item;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Ошибка загрузки: " + e.Message);
        }
    
        return data;
    }

    public List<LevelSaveData> GetAllLevelData()
    {
        Dictionary<string, LevelSaveData> data = LoadAllLevelData();
        return new List<LevelSaveData>(data.Values);
    }

    [Serializable]
    private class SerializationWrapper
    {
        public List<LevelSaveData> levelDataList;
        
        public SerializationWrapper(Dictionary<string, LevelSaveData> data)
        {
            levelDataList = new List<LevelSaveData>(data.Values);
        }
    }
}

