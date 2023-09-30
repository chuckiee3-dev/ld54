using System.Collections.Generic;

public interface IWordEvents
{
    public delegate void WordSpawned(string word);
    public WordSpawned OnWordSpawned { get; set; }
    public delegate void WordDestroyed(string word);
    public WordDestroyed OnWordDestroyed { get; set; }
    
    public delegate void WordProgressUpdated(string word, List<bool> correctness);
    public WordProgressUpdated OnWordProgressUpdated { get; set; }
}
