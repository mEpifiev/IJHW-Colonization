using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private ResourceDispatcher _resourceDispatcher;
    [SerializeField] private Base _base;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (_resourceDispatcher == null)
            return;

        _base.Initialize(_resourceDispatcher);
    }
}
