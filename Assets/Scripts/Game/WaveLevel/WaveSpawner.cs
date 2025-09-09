using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private List<Wave> waves;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform monstersParent;

    private int currentWaveIndex;
    private int monstersSpawnedInWave;
    private int monstersAliveInWave;
    
    private WaitForSeconds monsterCheckWait;
    
    public event Action OnAllWavesCompleted;
    public event Action<int, int, float> OnWaveProgress;

    private void Awake()
    {
        monsterCheckWait = new WaitForSeconds(0.5f);
        currentWaveIndex = 0;
        monstersSpawnedInWave = 0;
        monstersAliveInWave = 0;
    }

    public void StartWaves()
    {
        if (waves.Count > 0)
        {
            StartCoroutine(SpawnWave());
        }
    }

    private IEnumerator SpawnWave()
    {
        Wave currentWave = waves[currentWaveIndex];
        monstersSpawnedInWave = 0;
        monstersAliveInWave = 0;
        
        OnWaveProgress?.Invoke(currentWaveIndex + 1, waves.Count, 0f);

        while (monstersSpawnedInWave < currentWave.MonsterCount)
        {
            SpawnMonster(currentWave.MonsterPrefab);
            monstersSpawnedInWave++;
            monstersAliveInWave++;

            yield return new WaitForSeconds(currentWave.SpawnInterval);
        }
        
        OnWaveProgress?.Invoke(currentWaveIndex + 1, waves.Count, 0f);
        
        while (monstersAliveInWave > 0)
        {
            float progress = 1f - (float)monstersAliveInWave / currentWave.MonsterCount;
            OnWaveProgress?.Invoke(currentWaveIndex + 1, waves.Count, progress);
            
            yield return monsterCheckWait;
        }
        
        OnWaveProgress?.Invoke(currentWaveIndex + 1, waves.Count, 1f);

        
        if (currentWaveIndex >= waves.Count - 1)
        {
            OnAllWavesCompleted?.Invoke();
            
            yield break;
        }
        
        yield return new WaitForSeconds(currentWave.DelayBeforeNextWave);

        currentWaveIndex++;
        StartCoroutine(SpawnWave());
    }

    private void SpawnMonster(EnemyMover prefab)
    {
        if (spawnPoints.Length == 0 || prefab == null) 
        {
            return;
        }

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        EnemyMover monsterGO = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        
        if (monstersParent != null)
        {
            monsterGO.transform.SetParent(monstersParent);
        }

        monsterGO.GetComponent<PlayerDetector>().SetRadiusCheck(30f);
        
        EnemyHealth monster = monsterGO.transform.GetChild(0).GetComponent<EnemyHealth>();
        
        if (monster != null)
        {
            monster.OnEntityDied += OnMonsterDied;
        }
    }

    private void OnMonsterDied()
    {
        monstersAliveInWave--;
    }
}