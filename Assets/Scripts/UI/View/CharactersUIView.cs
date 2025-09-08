using System;
using UnityEngine;
using UnityEngine.UI;

public class CharactersUIView : MonoBehaviour
{
    [SerializeField] private Button backButton;

    public event Action OnBackToPauseRequested;

    private void Start()
    {
        backButton.onClick.AddListener(() => OnBackToPauseRequested?.Invoke());
    }

    private void OnDestroy()
    {
        if (backButton != null)
            backButton.onClick.RemoveAllListeners();
    }
    
    public void Show()
    {
        gameObject.SetActive(true);
    }
}