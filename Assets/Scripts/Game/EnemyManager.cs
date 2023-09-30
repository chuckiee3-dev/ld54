using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private int maxMainEnemiesOnScreen = 5;
    [SerializeField] private SpawnableEnemy enemyPrefab;
    [SerializeField] private EnemySO enemySo;
    private Dictionary<string, SpawnableEnemy> enemies = new ();
    public float delayBetweenSpawns;
    private IEnemyEvents enemyEvents;
    private IWordEvents wordEvents;
    private WordTracker wordTracker;
    private IDictionaryProvider dictionaryProvider;
    private IObjectResolver objectResolver;
    private float spawnTimer;
    private int enemiesOnScreen;
    [Inject]
    public void Construct(IEnemyEvents enemyEvents, IWordEvents wordEvents, WordTracker wordTracker, IDictionaryProvider dictionaryProvider, IObjectResolver objectResolver)
    {
        this.enemyEvents = enemyEvents;
        this.wordEvents = wordEvents;
        this.wordTracker = wordTracker;
        this.dictionaryProvider = dictionaryProvider;
        this.objectResolver = objectResolver;
        this.enemyEvents.OnEnemySpawned += EnemySpawned;
        this.enemyEvents.OnEnemyDied += EnemyDied;
    }

    private void EnemySpawned(string word, SpawnableEnemy enemy)
    {
        enemiesOnScreen++;            
        enemies.Add(word, enemy);
    }

    private void EnemyDied(string word)
    {
        if (enemies.ContainsKey(word))
        {
            enemies.Remove(word);
            enemiesOnScreen--;
        }
    }

    private void Update()
    {
        if (maxMainEnemiesOnScreen <= enemiesOnScreen)
        {
            return;
        }
        spawnTimer += Time.deltaTime;
        if (delayBetweenSpawns <= spawnTimer)
        {
            SpawnEnemy();
        }

    }

    private void SpawnEnemy()
    {
        spawnTimer = 0;
        var word = dictionaryProvider.GetWord(4, wordTracker.GetUnusedChar());
        if (!enemies.Keys.Contains(word))
        {
            var enemy = objectResolver.Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            enemy.SetData(enemySo.enemyData);
            enemy.Initialize(word);
        }
        else
        {
            Debug.LogWarning("Got used word from dictionary!");
        }
    }
}
