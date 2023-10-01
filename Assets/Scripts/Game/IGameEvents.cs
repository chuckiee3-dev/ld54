public interface IGameEvents
{
    public delegate void GameStart();
    public GameStart OnGameStart { get; set; }
    public delegate void GameOver();
    public GameOver OnGameOver { get; set; }
    public delegate void WaveCompleted(int wave);
    public WaveCompleted OnWaveCompleted { get; set; }
    public delegate void WaveSpawnCompleted(int wave);
    public WaveSpawnCompleted OnWaveSpawnCompleted { get; set; }
}
