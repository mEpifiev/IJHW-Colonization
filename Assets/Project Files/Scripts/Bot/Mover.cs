using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(BotAnimator))]
public class Mover : MonoBehaviour
{
    private const int MaxAvoidancePriority = 100;

    [SerializeField] private float _stoppingDistance = 0.5f;

    private NavMeshAgent _agent;
    private BotAnimator _animator;

    private bool _isMoving;

    public event Action<Vector3> DestinationReached;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<BotAnimator>();

        _agent.stoppingDistance = _stoppingDistance;
        _agent.avoidancePriority = Random.Range(0, MaxAvoidancePriority);

    }

    private void Update()
    {
        _animator.PlayWalkAnimation(_isMoving);

        if(_isMoving && _agent.pathPending == false)
        {
            if(_agent.remainingDistance <= _stoppingDistance)
            {
                if (_agent.hasPath == false || _agent.velocity.sqrMagnitude == 0f)
                {
                    _isMoving = false;
                    DestinationReached?.Invoke(_agent.destination);
                }
            }
        }
    }

    public void MoveTo(Vector3 position)
    {
        _isMoving = true;
        _agent.SetDestination(position);
    }
}
