using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Random = System.Random;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private SpawnableEnemy enemyPrefab;
    [SerializeField] List<Transform> spawnPoints;
    [SerializeField] List<Wave> regularWaves;
    [SerializeField] List<Wave> endGameWaves;
    private Dictionary<string, SpawnableEnemy> enemies = new();
    private IEnemyEvents enemyEvents;
    private WordTracker wordTracker;
    private IObjectResolver objectResolver;
    private IGameEvents gameEvents;
    private float spawnTimer;
    private int regularWaveIndex;
    private float delayModifier = -3;
    private int endlessIndex;
    private int mobIndex;
    [SerializeField] private float waveDelay;
    private bool allEnemiesSpawned;
    private float maxWait = 2f;
    private float checkTimer;
    private List<int> spawnIndices = new List<int>();
    private List<int> shuffled = new List<int>();

    private Random r = new Random();
    private int spawnIndex;
    [Inject]
    public void Construct(IEnemyEvents enemyEvents, WordTracker wordTracker, IObjectResolver objectResolver,
        IGameEvents gameEvents)
    {
        this.enemyEvents = enemyEvents;
        this.wordTracker = wordTracker;
        this.objectResolver = objectResolver;
        this.gameEvents = gameEvents;
        this.enemyEvents.OnEnemySpawned += EnemySpawned;
        this.enemyEvents.OnEnemyDied += EnemyDied;
        this.enemyEvents.OnRequestEnemySpawnAtPos += SpawnEnemyAtPos;
        this.gameEvents.OnWaveSpawnCompleted += WaveSpawnComplete;
    }

    private void WaveSpawnComplete(int wave)
    {
        allEnemiesSpawned = true;
    }

    private void Start()
    {
        regularWaveIndex = 0;
        gameEvents.OnGameStart?.Invoke();
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            spawnIndices.Add(i);
        }

        CreateShuffledList();
        SpawnNextWave();
    }

    private void CreateShuffledList()
    {
        int n = spawnIndices.Count;
        shuffled.Clear();
        for (int i = 0; i < spawnIndices.Count; i++)
        {
            shuffled.Add(spawnIndices[i]);
        }

        shuffled.Shuffle();
    }

    private IEnumerator SpawnWave(Wave wave)
    {
        yield return new WaitForSeconds(waveDelay);
        for (int k = 0; k < wave.enemies.Count; k++)
        {
            var mob = wave.enemies[k];
            for (int i = 0; i < mob.enemies.Count; i++)
            {
                for (int j = 0; j < mob.amounts[i]; j++)
                {
                    SpawnEnemyAtPos(mob.enemies[i], spawnPoints[shuffled.Count >0 ? shuffled[spawnIndex % shuffled.Count]: spawnIndices[UnityEngine.Random.Range(0, spawnIndices.Count)]].transform.position);
                    float delayEnemy = j == mob.amounts.Count - 1
                        ? 0
                        : UnityEngine.Random.Range(wave.delayRangeWithinEnemySpawn.x,
                            wave.delayRangeWithinEnemySpawn.y);
                    spawnIndex++;
                    yield return new WaitForSeconds(delayEnemy);
                }
            }

            float delayMob = k == mob.enemies.Count - 1
                ? 0
                : UnityEngine.Random.Range(wave.delayRangeBetweenMobs.x, wave.delayRangeBetweenMobs.y);
            yield return new WaitForSeconds(delayMob);
        }

        gameEvents.OnWaveSpawnCompleted?.Invoke(regularWaveIndex + endlessIndex + 1);
    }

    private void EnemySpawned(string word, SpawnableEnemy enemy)
    {
        enemies.Add(word, enemy);
    }

    private void Update()
    {
        if (allEnemiesSpawned && enemies.Count == 0)
        {
            checkTimer += Time.deltaTime;
            if (checkTimer >= maxWait)
            {
                checkTimer = 0;
                allEnemiesSpawned = false;
                SpawnNextWave();
                gameEvents.OnWaveCompleted?.Invoke(regularWaveIndex + endlessIndex);
            }
        }
    }

    private void EnemyDied(string word)
    {
        if (enemies.ContainsKey(word))
        {
            enemies.Remove(word);
        }
    }

    private void SpawnNextWave()
    {
        if (regularWaveIndex < regularWaves.Count)
        {
            StartCoroutine(SpawnWave(regularWaves[regularWaveIndex]));
            regularWaveIndex++;
        }
        else
        {
            StartCoroutine(SpawnWave(endGameWaves[endlessIndex]));
            endlessIndex++;
            endlessIndex = endlessIndex % endGameWaves.Count;
        }
    }

    private void SpawnEnemyAtPos(EnemySO enemySo, Vector3 pos)
    {
        spawnTimer = 0;
        var word = wordTracker.GetRandomUnusedWordWithSpaces(enemySo.enemyData.wordLength,
            enemySo.enemyData.spaceRequiredPerHp);
        if (!enemies.Keys.Contains(word))
        {
            var enemy = objectResolver.Instantiate(enemyPrefab, pos, Quaternion.identity);
            enemy.SetData(enemySo.enemyData);
            enemy.Initialize(word);
        }
        else
        {
            Debug.LogWarning("Got used word from dictionary!");
        }
    }
}