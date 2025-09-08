using UnityEngine;

public class WavesLevel : BaseLevel
{
    [Header("Waves")]
    [SerializeField] private WaveSpawner waveSpawner;

    protected override void SetLevelType()
    {
        GameStats.Instance?.SetLevelType(GameStats.LevelType.Waves);
    }

    protected override void StartLevel()
    {
        if (waveSpawner != null)
        {
            waveSpawner.OnAllWavesCompleted += OnWavesCompleted;
            waveSpawner.OnWaveProgress += OnWaveProgress;
            
            waveProgressUI?.ShowWaveUI();
            waveSpawner.StartWaves();
        }
    }

    private void OnWaveProgress(int currentWave, int totalWaves, float progress)
    {
        waveProgressUI?.UpdateWaveProgress(currentWave, totalWaves, progress);
    }

    private void OnWavesCompleted()
    {
        waveProgressUI?.HideWaveUI();
        CompleteLevel();
    }

    private void CompleteLevel()
    {
        GameStats.Instance?.StopLevelTimer();
        uiPresenter?.ShowWinView();
    }
}