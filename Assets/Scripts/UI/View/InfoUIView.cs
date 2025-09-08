using System;
using UnityEngine;
using UnityEngine.UI;

public class InfoUIView : MonoBehaviour
{
    [SerializeField] private Button backButton;

    public event Action OnBackRequested;

    private void Start()
    {
        backButton.onClick.AddListener(() => OnBackRequested?.Invoke());
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