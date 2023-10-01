using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using VContainer;

public class Mine : MonoBehaviour
{
    private List<Worker> workers = new List<Worker>();
    [FormerlySerializedAs("bar")] [SerializeField] private List<MineProgressBar> bars;
    [SerializeField] private float timeToMineSpace=4;
    [SerializeField] public Transform entrance;
    private int wordLength = 5;
    [SerializeField] private int totalWorkers = 0;
    [SerializeField] private int maxWorkers = 1;
    public Sprite emptySprite;
    public Sprite occupiedSprite;
    public Light2D light2d;
    public SpriteRenderer sr;
    public string word;
    public WordView wordView;
    public IWordEvents wordEvents;
    public IBaseEvents baseEvents;
    public IDictionaryProvider dictionaryProvider;
    public IUpgradeProvider upgradeProvider;
    public WordTracker wordTracker;
    private float defaultIntensity;
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
        sr.sprite = emptySprite;
        defaultIntensity = light2d.intensity;
        light2d.intensity = 0;
        UniTask.Delay(TimeSpan.FromSeconds(2)).ContinueWith((() =>
        {
            Initialize( dictionaryProvider.GetWord(wordLength + upgradeProvider.GetMineLengthModifier(), wordTracker.GetUnusedChar()));
        }));
        foreach (var bar in bars)
        {
            bar.isInUse = false;
            bar.SetInvisible();
        }
    }

    public void WorkerArrived(Worker worker)
    {
        workers.Add(worker);
        
        worker.WorkInMine(timeToMineSpace);
        sr.sprite = occupiedSprite;
        light2d.intensity = defaultIntensity;
        
        foreach (var bar in bars)
        {
            if (!bar.isInUse)
            {
                bar.FillInSeconds(timeToMineSpace);
                break;
            }
        }
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
        sr.sprite = emptySprite;
        light2d.intensity = 0;
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
