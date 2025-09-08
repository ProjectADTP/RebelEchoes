using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseUIView : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button charactersButton;
    [SerializeField] private Button infoButton;
    
    public event Action OnResumeRequested;
    public event Action OnOptionsRequested;
    public event Action OnCharactersRequested;
    public event Action OnInfoRequested;

    private void Start()
    {
        resumeButton.onClick.AddListener(() => OnResumeRequested?.Invoke());
        optionsButton.onClick.AddListener(() => OnOptionsRequested?.Invoke());
        charactersButton.onClick.AddListener(() => OnCharactersRequested?.Invoke());
        infoButton.onClick.AddListener(() => OnInfoRequested?.Invoke());
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        if (resumeButton != null)
            resumeButton.onClick.RemoveAllListeners();
        
        if (optionsButton != null)
            optionsButton.onClick.RemoveAllListeners();
        
        if (charactersButton != null)
            charactersButton.onClick.RemoveAllListeners();
        
        if (infoButton != null)
            infoButton.onClick.RemoveAllListeners();
    }
}