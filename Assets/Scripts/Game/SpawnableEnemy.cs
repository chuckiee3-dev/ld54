using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class SpawnableEnemy : BaseWordedItem
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

    public override void WordCompleted()
    {
        base.WordCompleted();
        enemyEvents.OnEnemyDied?.Invoke(word);
        gameObject.SetActive(false);
    }

    public void SetData(EnemyData data)
    {
        this.enemyData = data;
    }
    public override void Initialize( string word)
    {
        this.word = word;
        base.Initialize(word);
        enemyEvents.OnEnemySpawned?.Invoke(word, this);
        correctness.Clear();
        wordView.Show();
        wordView.UpdateStatus(word, correctness);
        sr.sprite = enemyData.visual;
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
        rb.velocity = Vector2.left;
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
            Debug.Log("Enemy attack!");
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