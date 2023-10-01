using System;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using VContainer;

public class Worker: MonoBehaviour
{
    public bool occupied;
    public float movementSpeed;
    public Light2D workerLight;
    public Mine mine;
    public Tower tower;
    public SpriteRenderer sr;
    private Vector3 targetPos;
    private bool shouldMove;
    private TargetBuilding targetBuilding;
    private float defaultIntensity;
    public int spacePerMine = 1;

    private void OnEnable()
    {
        mine = FindFirstObjectByType<Mine>();
        tower = FindFirstObjectByType<Tower>();
        sr.enabled = false;
        defaultIntensity = workerLight.intensity > 0.1f ? workerLight.intensity : .4f;
        workerLight.intensity = 0;
    }

    public void GoMine()
    {
        sr.flipX = false;
        occupied = true;
        transform.position = tower.entrance.position;
        targetPos = mine.entrance.position;
        targetBuilding = TargetBuilding.Mine;
        sr.enabled = true;
        workerLight.intensity = defaultIntensity;
        shouldMove = true;
    }
    
    public void GoTower()
    {
        sr.flipX = true;
        Debug.Log("Go tower");
        transform.position = mine.entrance.position;
        targetPos = tower.entrance.position;
        targetBuilding = TargetBuilding.Tower;
        sr.enabled = true;
        workerLight.intensity = defaultIntensity;
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
                workerLight.intensity = 0;
                sr.enabled = false;
                break;
            case TargetBuilding.Tower:
                tower.WorkerArrived(this);
                workerLight.intensity = 0;
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
