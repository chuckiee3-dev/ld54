public class EnemyEvents : IEnemyEvents
{
    public IEnemyEvents.EnemySpawned OnEnemySpawned { get; set; }
    public IEnemyEvents.EnemyDied OnEnemyDied { get; set; }
}
