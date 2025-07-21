using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _scanRadius = 30f;
    [SerializeField] private float _delay = 3f;

    private List<Resource> _resources = new();

    private Collider[] _resourcesBuffer = new Collider[10];

    public event Action<List<Resource>> ResourceScanned;

    private void Start()
    {
        StartCoroutine(ScanRoutine());
    }

    private void Scan()
    {
        _resources.Clear();

        int hitCount = Physics.OverlapSphereNonAlloc(transform.position, _scanRadius, _resourcesBuffer, _layerMask);

        for (int i = 0; i < hitCount; i++)
        {
            Collider collider = _resourcesBuffer[i];

            if(collider.TryGetComponent(out Resource resource))
                _resources.Add(resource);
        }
    }

    private IEnumerator ScanRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(_delay);

        while(enabled)
        {
            yield return wait;

            Scan();
            ResourceScanned?.Invoke(_resources);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _scanRadius);
    }
}
