using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(Collector))]
public class Bot : MonoBehaviour
{
    [SerializeField] private float _collectionDelay = 0.5f;

    private Mover _mover;
    private Resource _ñurrentResource;
    private Vector3 _resourceDropPoint;

    public Collector Collector { get; private set; }
    public bool IsAssigned { get; private set; }

    private void Awake()
    {
        _mover = GetComponent<Mover>();
        Collector = GetComponent<Collector>();
    }

    private void OnEnable()
    {
        _mover.DestinationReached += OnDestinationReached;
    }

    private void OnDisable()
    {
        _mover.DestinationReached -= OnDestinationReached;
    }

    public void AssignTask(Resource resource, Transform dropPoint)
    {
        if (IsAssigned || resource == null || dropPoint == null)
            return;

        _ñurrentResource = resource;
        _resourceDropPoint = dropPoint.position;
        IsAssigned = true;

        _mover.MoveTo(resource.transform.position);
    }

    public Resource GiveResource(Vector3 position)
    {
        IsAssigned = false;

        return Collector.Drop(position);
    }

    private void OnDestinationReached(Vector3 position)
    {
        if(Collector.HaveResource == false && _ñurrentResource != null)
        {
            StartCoroutine(CollectResource());
        }
    }

    private IEnumerator CollectResource()
    {
        yield return new WaitForSeconds(_collectionDelay);

        if(Collector.TryCollect(_ñurrentResource))
        {
            _mover.MoveTo(_resourceDropPoint);
            _ñurrentResource = null;
        }
        else
        {
            IsAssigned = false;
        }
    }
}
