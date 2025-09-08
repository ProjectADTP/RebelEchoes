using UnityEngine;

public abstract class BaseLevel : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] protected PlayerHealth player;

    [Header("UI")]
    [SerializeField] protected UIPresenter uiPresenter;
    [SerializeField] protected MonsterCounterUI monsterCounterUI;
    [SerializeField] protected WaveProgressUI waveProgressUI;
    [SerializeField] protected BossProgressUI bossHealthUI;

    [Header("Scene Management")]
    [SerializeField] protected string nextSceneName = "Level2";

    public static BaseLevel Instance { get; private set; }

    protected virtual void Awake()
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

    protected virtual void Start()
    {
        if (player != null)
        {
            player.OnPlayerDied += OnPlayerDied;
        }
        
        SetLevelType();
        
        StartLevel();
    }

    protected abstract void SetLevelType();
    protected abstract void StartLevel();
    
    public void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
        }
    }

    public void RestartLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }
    
    private void OnPlayerDied()
    {
        HideAllUI();
        uiPresenter?.ShowLoseView();
    }

    private void HideAllUI()
    {
        monsterCounterUI?.HideMonsterCounter();
        waveProgressUI?.HideWaveUI();
        bossHealthUI?.Hide();
    }
}