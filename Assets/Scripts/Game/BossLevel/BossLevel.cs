using UnityEngine;

public class BossLevel : BaseLevel
{
    [Header("Boss")]
    [SerializeField] private EnemyHealth bossHealth;

    protected override void SetLevelType()
    {
        GameStats.Instance?.SetLevelType(GameStats.LevelType.BossBattle);
    }

    protected override void StartLevel()
    {
        if (!bossHealth || !bossHealthUI)
            return;
        
        bossHealthUI.Show();
        bossHealth.OnEntityDied += OnBossDefeated;
    }

    private void OnBossDefeated()
    {
        bossHealthUI?.Hide(); 
        CompleteLevel();
    }

    private void CompleteLevel()
    {
        GameStats.Instance?.StopLevelTimer();
        uiPresenter?.ShowWinView();
    }
}