public class GameEvents : IGameEvents
{
    public IGameEvents.GameStart OnGameStart { get; set; }
    public IGameEvents.GameOver OnGameOver { get; set; }
    public IGameEvents.WaveCompleted OnWaveCompleted { get; set; }
    public IGameEvents.WaveSpawnCompleted OnWaveSpawnCompleted { get; set; }
}
