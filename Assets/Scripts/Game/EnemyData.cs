using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class EnemyData
{ 
    public Sprite visual;
    public Sprite [] walkAnim;
    public float [] walkAnimFrameDuration;
    public Sprite [] attackAnim;
    public float [] attackAnimFrameDuration;
    public int damageFrame;
    public int damageAmount;
    public Color tint;
    public Color outlineTint;
    public float movementSpeed;
    public float attackRange;
    public float timeBetweenAttacks;
    public bool spawnOnAttack;
    public SpawnableEnemy spawnedItem;
    public int maxSpawnCount;
}