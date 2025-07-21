using System;
using UnityEngine;

public class ResourceCounter : MonoBehaviour
{
    private int _count = 0;

    public event Action<int> Changed;

    private void Awake()
    {
        Changed?.Invoke(_count);
    }

    public void Add()
    {
        _count++;
        Changed?.Invoke(_count);
    }
}
