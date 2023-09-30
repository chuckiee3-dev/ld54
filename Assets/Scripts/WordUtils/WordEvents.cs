public class WordEvents : IWordEvents
{
    public IWordEvents.WordSpawned OnWordSpawned { get; set; }
    public IWordEvents.WordDestroyed OnWordDestroyed { get; set; }
    public IWordEvents.WordProgressUpdated OnWordProgressUpdated { get; set; }
}
