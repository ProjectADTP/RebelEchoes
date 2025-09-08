using UnityEngine;

public class EscapeLevel : BaseLevel
{
    [SerializeField] private ExitPoint exitPoint;

    protected override void SetLevelType()
    {
        GameStats.Instance?.SetLevelType(GameStats.LevelType.Escape);
    }

    protected override void StartLevel()
    {
        exitPoint.Entered += CompleteLevel;
    }

    private void CompleteLevel()
    {
        exitPoint.Entered -= CompleteLevel;
        
        GameStats.Instance?.StopLevelTimer();
        uiPresenter?.ShowWinView();
    }
}