using System.Collections.Generic;
using UnityEngine;

public enum EnemySpecies
{
    Goblin,
    Skeleton,
    Slime
}

public enum EnemyRole
{
    FastMelee,
    HeavyMelee,
    Ranged
}

[System.Serializable]
public struct RoleSpawnChance
{
    public EnemyRole role;
    [Range(1, 100)] public int weight; // Stadistic weight
}

[System.Serializable]
public class Wave
{
    public string waveName;
    public float waveStartTime;
    public float spawnInterval;
    public List<EnemySpecies> allowedSpecies;

    // use weigthed random
    public List<RoleSpawnChance> allowedRolesWithWeights;

    [Header("Elite Settings")]
    [Range(0f, 100f)] public float eliteSpawnChance;
}

public class WaveManager : MonoBehaviour
{
    [Header("Wave Configuration")]
    public List<Wave> waves;

    [Header("Spawn Settings")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float spawnRadius = 15f;

    private int currentWaveIndex = 0;
    private float globalTimer = 0f;
    private float nextSpawnTime = 0f;

    private void Update()
    {
        if (GameManager.Instance.IsGameOver) return;

        // Game watch
        globalTimer += Time.deltaTime;

        if (currentWaveIndex < waves.Count - 1)
        {
            if (globalTimer >= waves[currentWaveIndex + 1].waveStartTime)
            {
                currentWaveIndex++;
                Debug.Log($"Iniciando oleada: {waves[currentWaveIndex].waveName}");
            }
        }

        if (globalTimer >= nextSpawnTime)
        {
            SpawnEnemyForCurrentWave();
            nextSpawnTime = globalTimer + waves[currentWaveIndex].spawnInterval;
        }
    }

    private void SpawnEnemyForCurrentWave()
    {
        if (waves.Count == 0) return;

        Wave currentWave = waves[currentWaveIndex];

        // species can be weighted in the future
        EnemySpecies randomSpecies = currentWave.allowedSpecies[Random.Range(0, currentWave.allowedSpecies.Count)];

        // weight selected
        int totalWeight = 0;
        foreach (var chance in currentWave.allowedRolesWithWeights)
        {
            totalWeight += chance.weight;
        }

        int randomValue = Random.Range(0, totalWeight);
        EnemyRole randomRole = EnemyRole.FastMelee;

        foreach (var chance in currentWave.allowedRolesWithWeights)
        {
            if (randomValue < chance.weight)
            {
                randomRole = chance.role;
                break;
            }
            randomValue -= chance.weight;
        }

        Vector2 spawnDirection = Random.insideUnitCircle.normalized;
        Vector3 spawnPosition = playerTransform.position + (Vector3)(spawnDirection * spawnRadius);

        string tag = GetPoolTag(randomSpecies);
        GameObject enemyObj = PoolManager.Instance.SpawnFromPool(tag, spawnPosition, Quaternion.identity);

        if (enemyObj != null)
        {
            Enemy enemyComponent = enemyObj.GetComponent<Enemy>();
            BaseAttackStrategy strategy = CreateStrategy(randomRole);

            bool spawnAsElite = Random.Range(0f, 100f) < currentWave.eliteSpawnChance;

            enemyComponent.Setup(strategy, playerTransform, randomRole,spawnAsElite);
        }
    }

    private string GetPoolTag(EnemySpecies species)
    {
        return species switch
        {
            EnemySpecies.Goblin => "Goblin",
            EnemySpecies.Skeleton => "Skeleton",
            EnemySpecies.Slime => "Slime",
            _ => "Goblin"
        };
    }

    private BaseAttackStrategy CreateStrategy(EnemyRole role)
    {
        return role switch
        {
            EnemyRole.FastMelee => new FastMeleeStrategy(LayerMask.GetMask("Player")),
            EnemyRole.HeavyMelee => new HeavyMeleeStrategy(LayerMask.GetMask("Player")),
            EnemyRole.Ranged => new RangedStrategy("SkeletonArrow"),
            _ => new FastMeleeStrategy(LayerMask.GetMask("Player"))
        };
    }
}