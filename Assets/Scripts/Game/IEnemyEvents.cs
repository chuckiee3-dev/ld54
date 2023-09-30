public interface IEnemyEvents
{
    public delegate void EnemySpawned(string word, SpawnableEnemy enemy);
    public EnemySpawned OnEnemySpawned { get; set; }
    public delegate void EnemyDied(string word);
    public EnemyDied OnEnemyDied { get; set; }
}
