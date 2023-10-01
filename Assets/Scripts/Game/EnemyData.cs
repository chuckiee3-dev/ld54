using System;
using UnityEngine;

[Serializable]
public class EnemyData
{ 
    public int wordLength;
    public int hp = 1;
    public int spaceRequiredPerHp = 1;
    public Sprite visual;
    public float visualScale;
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
    public EnemySO spawnedItem;
    public int maxSpawnCount;
    public bool dieOnAttack;
}