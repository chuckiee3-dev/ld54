using System;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;

public class Worker: MonoBehaviour
{
    public bool occupied;
    public float movementSpeed;
    public Mine mine;
    public Tower tower;
    public SpriteRenderer sr;
    private Vector3 targetPos;
    private bool shouldMove;
    private TargetBuilding targetBuilding;
    private void OnEnable()
    {
        mine = FindFirstObjectByType<Mine>();
        tower = FindFirstObjectByType<Tower>();
        sr.enabled = false;
    }

    public void GoMine()
    {
        occupied = true;
        transform.position = tower.entrance.position;
        targetPos = mine.entrance.position;
        targetBuilding = TargetBuilding.Mine;
        sr.enabled = true;
        shouldMove = true;
    }
    
    public void GoTower()
    {
        Debug.Log("Go tower");
        transform.position = mine.entrance.position;
        targetPos = tower.entrance.position;
        targetBuilding = TargetBuilding.Tower;
        sr.enabled = true;
        shouldMove = true;
    }

    private void Update()
    {
        if (!shouldMove)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * movementSpeed);
        if ((targetPos - transform.position).sqrMagnitude <= 0.01f)
        {
            EnterBuilding();
            shouldMove = false;
        }
    }

    private void EnterBuilding()
    {
        switch (targetBuilding)
        {
            case TargetBuilding.Mine:
                mine.WorkerArrived(this);
                sr.enabled = false;
                break;
            case TargetBuilding.Tower:
                tower.WorkerArrived(this);
                sr.enabled = false;
                occupied = false;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void WorkInMine(float durationInSeconds)
    {
        UniTask.Delay(TimeSpan.FromSeconds(durationInSeconds)).ContinueWith((() =>
        {
            mine.WorkerLeft(this);
            GoTower();
        }));
    }
}
