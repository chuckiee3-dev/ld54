using UnityEngine;

public interface IEnemyEvents
{
    public delegate void EnemySpawned(string word, SpawnableEnemy enemy);
    public EnemySpawned OnEnemySpawned { get; set; }
    public delegate void EnemyDied(string word);
    public EnemyDied OnEnemyDied { get; set; }
    public delegate void RequestEnemySpawn(EnemySO enemySo,Vector3 pos);
    public RequestEnemySpawn OnRequestEnemySpawnAtPos { get; set; }
}
