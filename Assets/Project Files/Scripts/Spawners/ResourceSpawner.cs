using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private ResourcePool _resourcePool;
    [SerializeField] private SpawnPoint _spawnPoint;
    [SerializeField] private float _delay;
    [SerializeField] private float _spawnOffsetY = 2f;
    [SerializeField] private int _capacity = 5;

    private List<Resource> _stackedResources = new();

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(_delay);

        while (enabled)
        {
            yield return wait;

            if (_stackedResources.Count >= _capacity)
                continue;

            Resource resourse = _resourcePool.Get();

            resourse.Released += OnReleased;

            Vector3 position = _spawnPoint.transform.position + Vector3.up * _spawnOffsetY;
            resourse.transform.position = position;

            _stackedResources.Add(resourse);
        }
    }

    private void OnReleased(Resource resource)
    {
        resource.Released -= OnReleased;
        _resourcePool.Release(resource);
        _stackedResources.Remove(resource);
    }
}
