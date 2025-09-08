using UnityEngine;

public class BossProgressUI : MonoBehaviour
{
    [SerializeField] private GameObject bossPanel;

    public void Show()
    {
        bossPanel.SetActive(true);
    }
    
    public void Hide()
    {
        bossPanel.SetActive(false);
    }
}