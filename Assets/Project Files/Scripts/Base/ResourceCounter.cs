using System;
using UnityEngine;

public class ResourceCounter : MonoBehaviour
{
    public event Action<int> Changed;

    public int Count { get; private set; } = 0;

    private void Awake()
    {
        Changed?.Invoke(Count);
    }

    public void Add()
    {
        Count++;

        Changed?.Invoke(Count);
    }

    public void Spend(int amount)
    {
        if (amount > Count)
            return;

        Count -= amount;

        Changed?.Invoke(Count);
    }
}
