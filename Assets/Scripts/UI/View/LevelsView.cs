using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class LevelsView : MonoBehaviour
{
    [SerializeField] private Transform levelButtonsContainer;
    [SerializeField] private Button backButton;

    private List<LevelButton> levelButtons;
    
    public event Action OnBackToPauseRequested;

    private void Start()
    {
        backButton.onClick.AddListener(() => OnBackToPauseRequested?.Invoke());
        
        levelButtons = new List<LevelButton>();
        
        foreach (LevelButton button in levelButtonsContainer.GetComponentsInChildren<LevelButton>())
        {
            levelButtons.Add(button);
        }
        
        LoadLevelData();
    }
    
    public void Show()
    {
        gameObject.SetActive(true);
    }
    
    private void LoadLevelData()
    {
        Dictionary<string, LevelSaveData> testData = GameStats.Instance.LoadAllLevelData();
        Debug.Log("Найдено уровней: " + testData.Count);
    
        foreach(var kvp in testData)
        {
            if (kvp.Value != null && kvp.Value.stats != null)
            {
                Debug.Log($"Уровень: {kvp.Key}, Звезды: {kvp.Value.stats.stars}, Очки: {kvp.Value.stats.score}");
            }
            else
            {
                Debug.Log($"Уровень: {kvp.Key} - данные повреждены или отсутствуют");
            }
        }
        
        List<LevelSaveData> levelData = GameStats.Instance.GetAllLevelData();
        int i = 0;
        
        foreach (LevelSaveData data in levelData)
        {
            CreateLevelButton(data, i);
            i++;
        }
    }
    
    private void CreateLevelButton(LevelSaveData data, int indexButton)
    {
        LevelButton button = levelButtons[indexButton];
        
        if (button != null)
        {
            button.Initialize(data);
            button.OnLevelSelected += OnLevelSelected;
        }
    }
    
    private void OnLevelSelected(string levelName)
    {
        LoadLevel(levelName);
    }
    
    private void LoadLevel(string levelName)
    {
        string[] split = levelName.Split(' ');
        string lvlName = split[0] + "_" + split[1];
        
        UnityEngine.SceneManagement.SceneManager.LoadScene(lvlName);
    }
    
    private void OnDestroy()
    {
        if (backButton != null)
            backButton.onClick.RemoveAllListeners();
    }
}