using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Resource : MonoBehaviour
{
    public event Action<Resource> Released;

    public Rigidbody Rigidbody { get; private set; }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }    

    public void DisablePhysics()
    {
        Rigidbody.isKinematic = true;
    }

    public void Release()
    {
        Reset();
        Released?.Invoke(this);
    }

    private void Reset()
    {
        Rigidbody.isKinematic = false;
    }
}
