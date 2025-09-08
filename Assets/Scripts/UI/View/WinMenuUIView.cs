using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinMenuUIView : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button restartButton;
    [SerializeField] private Button infoButton;
    [SerializeField] private Button nextLevelButton;
    
    [Header("Score UI")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private Image[] starImages;
    
    public event Action OnRestartRequested;
    public event Action OnInfoRequested;
    public event Action OnNextLevelRequested;
    
    private void Start()
    {
        restartButton.onClick.AddListener(() => OnRestartRequested?.Invoke());
        infoButton.onClick.AddListener(() => OnInfoRequested?.Invoke());
        nextLevelButton.onClick.AddListener(() => OnNextLevelRequested?.Invoke());
    }
    
    private void OnDestroy()
    {
        if (restartButton != null)
            restartButton.onClick.RemoveAllListeners();
        if (infoButton != null)
            infoButton.onClick.RemoveAllListeners();
    }
    
    public void Show()
    {
        gameObject.SetActive(true);
        DisplayResults();
    }
    
    private void DisplayResults()
    {
        GameStats gameStats = GameStats.Instance;
        
        if (gameStats == null)
            return;

        int score = gameStats.CalculateScore();
        int stars = gameStats.GetStars();
        
        
        if (scoreText != null)
        {
            scoreText.text = $"Очки: {score}";
            scoreText.gameObject.SetActive(true);
        }
        
        for (int i = 0; i < stars; i++)
        {
            if (starImages[i] != null)
            {
                starImages[i].gameObject.SetActive(true);
            }
        }
    }
}