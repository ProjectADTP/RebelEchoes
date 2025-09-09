using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIPresenter : MonoBehaviour
{
    [Header("Views")]
    [SerializeField] private MainUIView mainView;
    [SerializeField] private PauseUIView pauseView;
    [SerializeField] private OptionsUIView optionsView;
    [SerializeField] private LevelsView levelsView;
    [SerializeField] private InfoUIView infoView;
    [SerializeField] private WinMenuUIView winView;
    [SerializeField] private LoseMenuUIView loseView;

    private Dictionary<string, MonoBehaviour> views;
    private string previousScreen = "Main";

    private void Start()
    {
        InitializeViews();
        SubscribeToEvents();
        ShowMainView();
    }

    public void ShowWinView()
    {
        Invoke(nameof(InitializeWinDelay), 2f);
    }
    
    public void ShowLoseView()
    {
        Invoke(nameof(InitializeLoseDelay), 2f);
    }

    private void InitializeWinDelay()
    {
        HideAllViews();
        previousScreen = "Win";
        winView.Show();
        
        GetBaseLevel()?.PauseGame();
    }
    
    private void InitializeLoseDelay()
    {
        HideAllViews();
        previousScreen = "Lose";
        loseView.Show();
        
        GetBaseLevel()?.PauseGame();
    }
    private void InitializeViews()
    {
        views = new Dictionary<string, MonoBehaviour>
        {
            { "Main", mainView },
            { "Pause", pauseView },
            { "Options", optionsView },
            { "Characters", levelsView },
            { "Info", infoView },
            { "Win", winView },
            { "Lose", loseView }
        };
    }

    private void SubscribeToEvents()
    {
        mainView.OnPauseRequested += ShowPauseView;
        mainView.OnInfoRequested += () => ShowInfoFrom("Main");
        
        pauseView.OnResumeRequested += ShowMainView;
        pauseView.OnOptionsRequested += ShowOptionsView;
        pauseView.OnCharactersRequested += ShowCharactersView;
        pauseView.OnInfoRequested += () => ShowInfoFrom("Pause");

        optionsView.OnBackToPauseRequested += ShowPauseView;
        levelsView.OnBackToPauseRequested += ShowPauseView;
        
        infoView.OnBackRequested += OnInfoBackRequested;
        
        winView.OnRestartRequested += OnRestartRequested;
        winView.OnInfoRequested += () => ShowInfoFrom("Win");
        winView.OnNextLevelRequested += LoadNextScene;
        
        loseView.OnRestartRequested += OnRestartRequested;
        loseView.OnInfoRequested += () => ShowInfoFrom("Lose");
    }

    private void OnInfoBackRequested()
    {
        switch (previousScreen)
        {
            case "Win":
                ShowWinView();
                break;
            case "Lose":
                ShowLoseView();
                break;
            case "Pause":
                ShowPauseView();
                break;
            default:
                ShowMainView();
                break;
        }
    }

    private void ShowInfoFrom(string fromScreen)
    {
        previousScreen = fromScreen;
        ShowInfoView();
    }

    private void LoadNextScene()
    {
        GetBaseLevel()?.LoadNextScene();
    }
    
    private void OnRestartRequested()
    {
        GetBaseLevel()?.RestartLevel();
    }
    
    private void HideAllViews()
    {
       foreach (var view in views.Values.Where(view => view && view.gameObject.activeSelf))
            view.gameObject.SetActive(false);
    }

    private void ShowMainView()
    {
        HideAllViews();
        mainView.Show();
        GetBaseLevel()?.ResumeGame(); 
    }

    private void ShowPauseView()
    {
        HideAllViews();
        pauseView.Show();
        GetBaseLevel()?.PauseGame();
    }

    private void ShowOptionsView()
    {
        HideAllViews();
        previousScreen = "Pause";
        optionsView.Show();
    }

    private void ShowCharactersView()
    {
        HideAllViews();
        previousScreen = "Pause";
        levelsView.Show();
    }

    private void ShowInfoView()
    {
        HideAllViews();
        infoView.Show();
        GetBaseLevel()?.PauseGame();
    }
    
    private BaseLevel GetBaseLevel()
    {
        return BaseLevel.Instance;
    }
}