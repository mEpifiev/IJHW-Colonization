using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Scanner))]
public class ResourceDispatcher : MonoBehaviour
{
    private Scanner _scanner;

    private List<Resource> _scannedResources = new();
    private HashSet<Resource> _assignedResources = new();

    private void Awake()
    {
        _scanner = GetComponent<Scanner>();
    }

    private void OnEnable()
    {
        _scanner.ResourceScanned += AddResources;
    }

    private void OnDisable()
    {
        _scanner.ResourceScanned -= AddResources;
    }

    public Resource GetClosestResource(Vector3 position)
    {
        if (_scannedResources.Count == 0)
            return null;

        Resource closestResource = _scannedResources
            .Where(resource => _assignedResources.Contains(resource) == false)
            .OrderBy(resource => (position - resource.transform.position).sqrMagnitude)
            .FirstOrDefault();

        if (closestResource != null)
            _assignedResources.Add(closestResource);

        return closestResource;
    }

    public void RemoveResource(Resource resource)
    {
        _assignedResources.Remove(resource);
        _scannedResources.Remove(resource);
    }

    private void AddResources(List<Resource> resources)
    {
        _scannedResources.Clear();
        _scannedResources.AddRange(resources);
    }
}
