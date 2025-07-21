using UnityEngine;

public class Collector : MonoBehaviour
{
    [SerializeField] private Transform _holder;

    public Resource CurrentResource { get; private set; }
    public bool HaveResource => CurrentResource != null;

    public bool TryCollect(Resource resource)
    {
        if (resource == null || CurrentResource != null)
            return false;

        CurrentResource = resource;
        CurrentResource.DisablePhysics();
        CurrentResource.transform.SetParent(_holder);
        CurrentResource.transform.localPosition = Vector3.zero; 

        return true;
    }

    public Resource Drop(Vector3 newPosition)
    {
        if (CurrentResource == null)
            return null;

        Resource resource = CurrentResource;

        CurrentResource.transform.position = newPosition;
        CurrentResource.transform.SetParent(null);
        CurrentResource = null;

        return resource;
    }
}
