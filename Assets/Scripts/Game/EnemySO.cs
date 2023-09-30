using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Reusable/Enemy",  order = 1)]
public class EnemySO : ScriptableObject
{
    public EnemyData enemyData;
}