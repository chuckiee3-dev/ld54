using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class BaseWordedItem : MonoBehaviour
{
    public string word;
    public WordView wordView;
    public IWordEvents wordEvents;
    public IBaseEvents baseEvents;
    public IEnemyEvents enemyEvents;
    public IDictionaryProvider dictionaryProvider;
    public IUpgradeProvider upgradeProvider;
    public WordTracker wordTracker;
    public IObjectResolver objectResolver;
    [Inject]
    public void Construct(IWordEvents wordEvents, IBaseEvents baseEvents, IEnemyEvents enemyEvents,IDictionaryProvider dictionaryProvider,WordTracker wordTracker, IUpgradeProvider upgradeProvider,
        IObjectResolver objectResolver)
    {
        wordEvents.OnWordProgressUpdated += UpdateWordView;
        wordView.OnWordCompleted += WordCompleted;
        this.wordEvents = wordEvents;
        this.baseEvents = baseEvents;
        this.enemyEvents = enemyEvents;
        this.dictionaryProvider = dictionaryProvider;
        this.upgradeProvider = upgradeProvider;
        this.wordTracker = wordTracker;
        this.objectResolver = objectResolver;
    }
    public virtual void Initialize(string word)
    {
        this.word = word;
        wordView.Show();
        wordView.UpdateStatus(word, new List<bool>());
        wordEvents.OnWordSpawned?.Invoke(word);
    }
    public virtual void WordCompleted()
    {
        wordView.Hide();
        wordEvents.OnWordDestroyed?.Invoke(word);
    }
    public virtual void UpdateWordView(string s, List<bool> bools)
    {
        if (s == word)
        {
            wordView.UpdateStatus(s, bools);
        }
    }

    public virtual void OnDisable()
    {
        wordEvents.OnWordProgressUpdated -= UpdateWordView;
    }
}
