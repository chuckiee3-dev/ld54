using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class Tower : MonoBehaviour
{
    private List<Worker> workers;
    [SerializeField]private Worker workerPrefab;
    [SerializeField] public Transform entrance;
    private int spaceCount = 0;
    private int maxSpaceCount = 3;
    private IObjectResolver objectResolver;
    private void Start()
    {
        Initialize("??{{}}");
        wordView.Hide();
    }

    public bool HasWorker()
    {
        return workers.Count > 0;
    }
    private void SendWorker()
    {
        foreach (var worker in workers)
        {
            if (!worker.occupied)
            {
                worker.GoMine();
                workers.Remove(worker);
                break;
            }
        }
    }


    public void WorkerArrived(Worker worker)
    {
        workers.Add(worker);
        spaceCount++;            
        Debug.Log("Plus one space!");
        spaceCount = Math.Clamp(spaceCount, 0, maxSpaceCount);
    }
    
    public string word;
    public WordView wordView;
    public IWordEvents wordEvents;
    public IBaseEvents baseEvents;
    public IUpgradeProvider upgradeProvider;
    public WordTracker wordTracker;
    [Inject]
    public void Construct(IWordEvents wordEvents, IBaseEvents baseEvents,WordTracker wordTracker, IUpgradeProvider upgradeProvider,IObjectResolver objectResolver)
    {
        wordEvents.OnWordProgressUpdated += UpdateWordView;
        wordView.OnWordCompleted += WordCompleted;
        this.wordEvents = wordEvents;
        this.baseEvents = baseEvents;
        this.upgradeProvider = upgradeProvider;
        this.wordTracker = wordTracker;
        this.objectResolver = objectResolver;
    }
    public  void Initialize(string word)
    {
        this.word = word;
        wordView.Show();
        wordView.UpdateStatus(word, new List<bool>());
        wordEvents.OnWordSpawned?.Invoke(word);
        workers = new List<Worker>();
        workers.Add(objectResolver.Instantiate(workerPrefab, transform.position,Quaternion.identity));
        baseEvents.OnMineWorkerRequested += SendWorker;
    }
    public  void WordCompleted()
    {
        wordView.Hide();
        wordEvents.OnWordDestroyed?.Invoke(word);
    }
    public  void UpdateWordView(string s, List<bool> bools)
    {
        if (s == word)
        {
            wordView.UpdateStatus(s, bools);
        }
    }

    public  void OnDisable()
    {
        wordEvents.OnWordProgressUpdated -= UpdateWordView;
        baseEvents.OnMineWorkerRequested -= SendWorker;
    }
}
