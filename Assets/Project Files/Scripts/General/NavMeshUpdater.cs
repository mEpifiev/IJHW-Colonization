using Unity.AI.Navigation;
using UnityEngine;

public class NavMeshUpdater : MonoBehaviour
{
    [SerializeField] private NavMeshSurface _navMeshSurface;

    public void UpdateNavMesh()
    {
        _navMeshSurface.BuildNavMesh();
    }
}
