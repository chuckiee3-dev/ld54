using UnityEngine;
using VContainer;

public class BaseSpawner : MonoBehaviour
{
    public Mine minePrefab;
    public Tower towerPrefab;
    private IObjectResolver objectResolver;

    [Inject]
    private void Construct(IObjectResolver objectResolver)
    {
        this.objectResolver = objectResolver;
    }

}
