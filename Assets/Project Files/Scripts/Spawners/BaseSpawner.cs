using UnityEngine;

public class BaseSpawner : MonoBehaviour
{
    [SerializeField] private Base _basePrefab;
    [SerializeField] private NavMeshUpdater _navMeshUpdater;

    public Base Spawn(Vector3 position)
    {
        Base newBase = Instantiate(_basePrefab, position, Quaternion.identity);

        _navMeshUpdater.UpdateNavMesh();

        return newBase;
    }
}
