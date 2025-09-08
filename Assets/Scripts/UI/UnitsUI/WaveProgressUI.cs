using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveProgressUI : MonoBehaviour
{
    [SerializeField] private GameObject wavePanel;
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private Slider waveProgressSlider;

    private void Start()
    {
        HideWaveUI();
    }

    public void ShowWaveUI()
    {
        wavePanel?.SetActive(true);
    }

    public void UpdateWaveProgress(int currentWave, int totalWaves, float progress)
    {
        if (waveText != null)
        {
            waveText.text = $"Волна {currentWave}/{totalWaves}";
        }

        if (waveProgressSlider != null)
        {
            waveProgressSlider.value = Mathf.Clamp01(progress);
        }
    }

    public void HideWaveUI()
    {
        wavePanel?.SetActive(false);
    }
}