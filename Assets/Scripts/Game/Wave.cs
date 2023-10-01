using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Wave", menuName = "Typing/Wave", order = 2)]
public class Wave : ScriptableObject
{
    [FormerlySerializedAs("delayRangeBetweenGroups")] [FormerlySerializedAs("delayRangeWithinGroups")] public Vector2 delayRangeBetweenMobs;
    [FormerlySerializedAs("delayRangeWithinSameGroup")] public Vector2 delayRangeWithinEnemySpawn;
    public List<EnemyMob> enemies;
}
