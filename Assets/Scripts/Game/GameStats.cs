using UnityEngine;

public class GameStats : MonoBehaviour
{
    public enum LevelType
    {
        Cleanup,
        Waves,
        BossBattle,
        Escape
    }

    private LevelStats currentStats = new ();
    public LevelType currentLevelType = LevelType.Cleanup;

    [Header("Score Settings")]
    [SerializeField] private int baseWinPoints = 1000;

    [Header("Player Components")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerBuffs playerBuffs;
    
    public static GameStats Instance { get; private set; }

    private float levelStartTime;

    private void Awake()
    {
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
    }

    public int CalculateScore()
    {
        if (!currentStats.completed) 
            return 0;
        
        return baseWinPoints + Mathf.RoundToInt(currentStats.timeTaken);
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

        return stars;
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
        if (playerHealth != null) playerHealth.OnPlayerHit += OnPlayerGotDamage;
        if (playerBuffs != null) playerBuffs.OnBuffActivated += OnPlayerUsedBuff;
    }

    private void UnsubscribeFromEvents()
    {
        if (playerHealth != null) playerHealth.OnPlayerHit -= OnPlayerGotDamage;
        if (playerBuffs != null) playerBuffs.OnBuffActivated -= OnPlayerUsedBuff;
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
}