using UnityEngine;

[System.Serializable]
public class Wave
{
    [SerializeField] private int monsterCount = 5;
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private float delayBeforeNextWave = 3f;
    [SerializeField] private EnemyMover monsterPrefab;

    public int MonsterCount => monsterCount;
    public float SpawnInterval => spawnInterval;
    public float DelayBeforeNextWave => delayBeforeNextWave;
    public EnemyMover MonsterPrefab => monsterPrefab;
}