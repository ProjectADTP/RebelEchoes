using UnityEngine;

using TMPro;

public class MonsterCounterUI : MonoBehaviour
{
    [SerializeField] private GameObject monsterCounterPanel;
    [SerializeField] private TMP_Text monsterCountText;

    private void Start()
    {
        HideMonsterCounter();
    }

    public void ShowMonsterCounter(int count)
    {
        monsterCounterPanel?.SetActive(true);
        UpdateMonsterCounter(count);
    }
    
    public void HideMonsterCounter()
    {
        monsterCounterPanel?.SetActive(false);
    }
    
    private void UpdateMonsterCounter(int count)
    {
        if (monsterCountText != null)
        {
            monsterCountText.text = $"{count}";
        }
    }
}