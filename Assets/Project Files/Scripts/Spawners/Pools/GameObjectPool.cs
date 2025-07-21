using UnityEngine;
using UnityEngine.Pool;

public class GameObjectPool<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private Transform _container;
    [SerializeField] private T _prefab;
    [SerializeField] private int _capacity;
    [SerializeField] private int _maxSize;

    private ObjectPool<T> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<T>(
            createFunc: CreateObject,
            actionOnGet: OnGetObject,
            actionOnRelease: OnReleaseObject,
            actionOnDestroy: OnDestroyObject,
            collectionCheck: true,
            defaultCapacity: _capacity,
            maxSize: _maxSize);
    }

    public T Get() =>
        _pool.Get();

    public void Release(T obj) =>
        _pool.Release(obj);

    protected virtual T CreateObject()
    {
        T obj = Instantiate(_prefab, _container);

        return obj;
    }

    protected virtual void OnGetObject(T obj)
    {
        obj.gameObject.SetActive(true);
    }

    protected virtual void OnReleaseObject(T obj)
    {
        obj.gameObject.SetActive(false);
    }

    protected virtual void OnDestroyObject(T obj)
    {
        Destroy(obj.gameObject);
    }
}