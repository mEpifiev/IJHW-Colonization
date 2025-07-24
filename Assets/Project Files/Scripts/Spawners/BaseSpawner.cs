using UnityEngine;

public class BaseSpawner : MonoBehaviour
{
    [SerializeField] private Base _basePrefab;

    public Base Spawn(Vector3 position)
    {
        Base newBase = Instantiate(_basePrefab, position, Quaternion.identity);

        return newBase;
    }
}
