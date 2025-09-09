using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LevelButton : MonoBehaviour
{
    [SerializeField] private TMP_Text levelNameText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text starsText;
    [SerializeField] private Button selectButton;
    
    private LevelSaveData levelData;
    public event System.Action<string> OnLevelSelected;
    
    public void Initialize(LevelSaveData data)
    {
        levelData = data;
        UpdateUI();
    }
    
    private void UpdateUI()
    {
        if (levelNameText != null)
            levelNameText.text = levelData.levelName;
            
        if (scoreText != null)
            scoreText.text = levelData.stats.completed ? $"Score: {levelData.stats.score}" : "0";
            
        if (starsText != null)
        {
            if (levelData.stats.completed)
            {
                int stars = 0;
                
                for (int i = 0; i < levelData.stats.stars; i++)
                    stars ++;

                starsText.text = stars + "/3";
            }
            else
            {
                starsText.text = "0/3";
            }
        }
        
        if (selectButton != null)
            selectButton.onClick.AddListener(() => OnLevelSelected?.Invoke(levelData.levelName));
    }
}