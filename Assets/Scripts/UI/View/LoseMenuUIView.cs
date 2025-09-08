using System;
using UnityEngine;
using UnityEngine.UI;

public class LoseMenuUIView : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button restartButton;
    [SerializeField] private Button infoButton;
    
    public event Action OnRestartRequested;
    public event Action OnInfoRequested;

    private void Start()
    {
        restartButton.onClick.AddListener(() => OnRestartRequested?.Invoke());
        infoButton.onClick.AddListener(() => OnInfoRequested?.Invoke());
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
    }
}