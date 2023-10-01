using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class SpawnableEnemy : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private CircleCollider2D rangeTrigger;
    private EnemyData enemyData;
    private Attackable target;
    private bool hasTarget;
    private bool hasAnimation;
    private int framesPlayed;
    private float animTimer;
   [SerializeField]private float attackTimer;
    private List<float> attackAnimTotal = new List<float>();
    private List<float> walkAnimTotal = new List<float>();
    private List<bool> correctness = new List<bool>();
    private int lifetimeSpawns;
    private int totalDamageTaken = 0;
    public string word;
    public WordView wordView;
    public IWordEvents wordEvents;
    public IBaseEvents baseEvents;
    public IEnemyEvents enemyEvents;
    public IDictionaryProvider dictionaryProvider;
    public IUpgradeProvider upgradeProvider;
    public WordTracker wordTracker;
    public IObjectResolver objectResolver;
    private string initialWord;
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
    public  void UpdateWordView(string s, List<bool> bools)
    {
        if (s == word)
        {
            wordView.UpdateStatus(s, bools);
        }
    }

    public  void OnDisable()
    {
        if (wordEvents != null)
        {
            wordEvents.OnWordProgressUpdated -= UpdateWordView;
        }
    }
    public  void WordCompleted()
    {
        totalDamageTaken++;
        if (totalDamageTaken >= enemyData.hp)
        {
            Die();
        }
        else
        {
            wordEvents.OnWordDestroyed?.Invoke(word);
            word = wordTracker.GetRandomUnusedWordWithSpaces(enemyData.wordLength, enemyData.spaceRequiredPerHp);
            wordView.UpdateStatus(word,new List<bool>());
            wordEvents.OnWordSpawned?.Invoke(word);
        }
    }

    private void Die()
    {
        wordView.Hide();
        wordEvents.OnWordDestroyed?.Invoke(word);
        enemyEvents.OnEnemyDied?.Invoke(initialWord);
        gameObject.SetActive(false);
    }

    public void SetData(EnemyData data)
    {
        this.enemyData = data;
    }
    public  void Initialize( string word)
    {
        this.word = word;
        this.initialWord = word;
        wordView.Show();
        wordView.UpdateStatus(word, new List<bool>());
        wordEvents.OnWordSpawned?.Invoke(word);
        enemyEvents.OnEnemySpawned?.Invoke(word, this);
        sr.sprite = enemyData.visual;
        sr.transform.localScale = Vector3.one *  enemyData.visualScale;
        rangeTrigger.radius = enemyData.attackRange;
        float animTotal = 0;
        for (int i = 0; i < enemyData.attackAnimFrameDuration.Length; i++)
        {
            animTotal += enemyData.attackAnimFrameDuration[i];
            attackAnimTotal.Add(animTotal);
        }

        animTotal = 0;
        for (int i = 0; i < enemyData.walkAnimFrameDuration.Length; i++)
        {
            animTotal += enemyData.walkAnimFrameDuration[i];
            walkAnimTotal.Add(animTotal);
        }

        hasAnimation = enemyData.walkAnim.Length > 0 && enemyData.attackAnim.Length > 0;
        rb.velocity = Vector2.left * enemyData.movementSpeed;
    }

    public void Update()
    {
        animTimer = animTimer + Time.deltaTime;

        if (hasTarget)
        {
            attackTimer += Time.deltaTime;
            ProcessAttack();
        }

        if (!hasAnimation)
        {
            return;
        }

        PlayAnimations();
    }

    private void PlayAnimations()
    {
    }

    private void ProcessAttack()
    {
        if (attackTimer >= enemyData.timeBetweenAttacks)
        {
            attackTimer -= enemyData.timeBetweenAttacks;
            target.Damage(enemyData.damageAmount);
            if (enemyData.spawnOnAttack )
            {
                if (enemyData.maxSpawnCount == 0 || lifetimeSpawns < enemyData.maxSpawnCount)
                {
                    enemyEvents.OnRequestEnemySpawnAtPos?.Invoke(enemyData.spawnedItem, transform.position + Vector3.left*.1f);
                    lifetimeSpawns++;
                }
            }
            Debug.Log("Enemy attack!");
            if (enemyData.dieOnAttack)
            {
                Die();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTarget)
        {
            return;
        }

        if (other.TryGetComponent<Attackable>(out var newTarget))
        {
            SetTarget(newTarget);
        }
    }

    private void SetTarget(Attackable newTarget)
    {
        target = newTarget;
        rb.velocity = Vector2.zero;
        hasTarget = true;
        animTimer = 0;
        attackTimer = 0;
        framesPlayed = 0;
    }

}