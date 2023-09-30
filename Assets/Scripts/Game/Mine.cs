using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

public class Mine : MonoBehaviour
{
    private List<Worker> workers = new List<Worker>();
    [SerializeField] private float timeToMineSpace=4;
    [SerializeField] public Transform entrance;
    private bool isBeingMined = false;
    private int wordLength = 5;
    [SerializeField] private int totalWorkers = 0;
    [SerializeField] private int maxWorkers = 1;

    public string word;
    public WordView wordView;
    public IWordEvents wordEvents;
    public IBaseEvents baseEvents;
    public IDictionaryProvider dictionaryProvider;
    public IUpgradeProvider upgradeProvider;
    public WordTracker wordTracker;
    
    [Inject]
    public void Construct(IWordEvents wordEvents, IBaseEvents baseEvents, IEnemyEvents enemyEvents,IDictionaryProvider dictionaryProvider,WordTracker wordTracker, IUpgradeProvider upgradeProvider)
    {
        wordEvents.OnWordProgressUpdated += UpdateWordView;
        wordView.OnWordCompleted += WordCompleted;
        this.wordEvents = wordEvents;
        this.baseEvents = baseEvents;
        this.dictionaryProvider = dictionaryProvider;
        this.upgradeProvider = upgradeProvider;
        this.wordTracker = wordTracker;
    }

    private void Start()
    {
        UniTask.Delay(TimeSpan.FromSeconds(2)).ContinueWith((() =>
        {
            Initialize( dictionaryProvider.GetWord(wordLength + upgradeProvider.GetMineLengthModifier(), wordTracker.GetUnusedChar()));
        }));
    }

    public void WorkerArrived(Worker worker)
    {
        workers.Add(worker);
        worker.WorkInMine(timeToMineSpace);
    }
    private void WorkerRequested()
    {
        totalWorkers++;
        RequestWordIfNecessary();
        baseEvents.OnMineWorkerRequested?.Invoke();
        
    }

    private void RequestWordIfNecessary()
    {
        if (totalWorkers >= maxWorkers)
        {
            wordView.Hide();
            word = "";
        }
        else
        {
            Initialize( dictionaryProvider.GetWord(wordLength + upgradeProvider.GetMineLengthModifier(), wordTracker.GetUnusedChar()));
        }
    }

    public void WorkerLeft(Worker worker)
    {
        Debug.Log("Worker left");
        workers.Remove(worker);
        totalWorkers--;
        RequestWordIfNecessary();
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
        WorkerRequested();
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
