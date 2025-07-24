using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private ResourceDispatcher _resourceDispatcher;
    [SerializeField] private BotSpawner _botSpawner;
    [SerializeField] private BaseSpawner _baseSpawner;
    [SerializeField] private Base _base;

    private void Awake()
    {
        _base.Initialize(_resourceDispatcher, _botSpawner, _baseSpawner);
    }
}
