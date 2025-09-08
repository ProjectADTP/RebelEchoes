using System;
using UnityEngine;
using UnityEngine.UI;

public class MainUIView : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button infoButton;
    
    public event Action OnPauseRequested;
    public event Action OnInfoRequested;
    
    private void Start()
    {
        pauseButton.onClick.AddListener(() => OnPauseRequested?.Invoke());
        infoButton.onClick.AddListener(() => OnInfoRequested?.Invoke());
    }

    private void OnDestroy()
    {
        if (pauseButton != null)
            pauseButton.onClick.RemoveAllListeners();
    }
    
    public void Show()
    {
        gameObject.SetActive(true);
    }
}