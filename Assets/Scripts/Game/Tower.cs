using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using VContainer;
using VContainer.Unity;

public class Tower : MonoBehaviour
{
    private List<Worker> workers;
    [SerializeField]private Worker workerPrefab;
    [SerializeField] public Transform entrance;
    [SerializeField] public TextMeshProUGUI spaceTmp;
    private string spacePrefix = "Space: ";
    private int spaceCount = 0;
    private int maxSpaceCount = 3;
    private IObjectResolver objectResolver;
    
    public string word;
    public WordView wordView;
    public IWordEvents wordEvents;
    public IBaseEvents baseEvents;
    public IUpgradeProvider upgradeProvider;
    public IInputEvents inputEvents;
    public WordTracker wordTracker;
    public Sprite emptySprite;
    public Sprite occupiedSprite;
    public Light2D light2d;
    public SpriteRenderer sr;
    private float defaultIntensity;
    [Inject]
    public void Construct(IWordEvents wordEvents, IBaseEvents baseEvents,WordTracker wordTracker, IUpgradeProvider upgradeProvider,IObjectResolver objectResolver, IInputEvents inputEvents)
    {
        wordEvents.OnWordProgressUpdated += UpdateWordView;
        wordView.OnWordCompleted += WordCompleted;
        baseEvents.OnSpaceRequested += TryGrantSpace;
        this.wordEvents = wordEvents;
        this.baseEvents = baseEvents;
        this.upgradeProvider = upgradeProvider;
        this.wordTracker = wordTracker;
        this.inputEvents = inputEvents;
        this.objectResolver = objectResolver;
    }

    private void TryGrantSpace(int amount)
    {
        Debug.Log("TryGrantSpace");
        if (amount <= spaceCount)
        {
            Debug.Log("amount <= spaceCount");
            spaceCount -= amount;
            spaceCount = Math.Clamp(spaceCount, 0, maxSpaceCount);
            spaceTmp.text = spacePrefix + spaceCount;
            baseEvents.OnSpaceGranted?.Invoke(amount);
        }
        else
        {
            Debug.Log("non");
            baseEvents.OnSpaceRejected?.Invoke(spaceCount, amount);
        }
    }

    private void Start()
    {
        sr.sprite = emptySprite;
        defaultIntensity = light2d.intensity;
        light2d.intensity = 0;
        Initialize("||??{{}}");
        wordView.Hide();
        spaceTmp.text = spacePrefix + spaceCount;
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
                
                sr.sprite = emptySprite;
                light2d.intensity = 0;

                break;
            }
        }
    }


    public void WorkerArrived(Worker worker)
    {
        workers.Add(worker);
        EarnSpace(worker.spacePerMine);
        sr.sprite = occupiedSprite;
        light2d.intensity = defaultIntensity;

    }

    public void EarnSpace(int earnedAmount)
    {
        spaceCount+=earnedAmount;
        spaceCount = Math.Clamp(spaceCount, 0, maxSpaceCount);
        spaceTmp.text = spacePrefix + spaceCount;
        baseEvents.OnSpaceEarned?.Invoke(1);
    }


    public  void Initialize(string word)
    {
        this.word = word;
        wordView.Show();
        wordView.UpdateStatus(word, new List<bool>());
        wordEvents.OnWordSpawned?.Invoke(word);
        workers = new List<Worker>();
        sr.sprite = occupiedSprite;
        light2d.intensity = defaultIntensity;

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
        baseEvents.OnSpaceRequested -= TryGrantSpace;
    }
}
