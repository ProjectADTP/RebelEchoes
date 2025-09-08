using UnityEngine;
using System.Collections;

public class MonsterHuntLevel : BaseLevel
{
    [Header("Monsters")]
    [SerializeField] private Transform monstersParent;

    private int totalMonsters;
    private int aliveMonsters;

    protected override void SetLevelType()
    {
        GameStats.Instance?.SetLevelType(GameStats.LevelType.Cleanup);
    }

    protected override void StartLevel()
    {
        if (monstersParent != null)
        {
            StartCoroutine(CheckMonsters());
        }
    }

    private IEnumerator CheckMonsters()
    {
        yield return new WaitForSeconds(0.1f);

        if (monstersParent != null)
        {
            totalMonsters = monstersParent.childCount;
            aliveMonsters = totalMonsters;

            monsterCounterUI?.ShowMonsterCounter(aliveMonsters);
            StartCoroutine(MonitorMonsters());
        }
    }

    private IEnumerator MonitorMonsters()
    {
        while (aliveMonsters > 0)
        {
            aliveMonsters = 0;
            
            for (int i = 0; i < monstersParent.childCount; i++)
            {
                var monster = monstersParent.GetChild(i).GetChild(0).GetComponent<EnemyHealth>();
                
                if (monster != null && monster.IsAlive())
                {
                    aliveMonsters++;
                }
            }
            
            monsterCounterUI?.ShowMonsterCounter(aliveMonsters);
            yield return new WaitForSeconds(0.1f);
        }
        
        monsterCounterUI?.HideMonsterCounter();
        CompleteLevel();
    }

    private void CompleteLevel()
    {
        GameStats.Instance?.StopLevelTimer();
        uiPresenter?.ShowWinView();
    }
}