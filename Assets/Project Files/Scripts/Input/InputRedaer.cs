using System;
using UnityEngine;

public class InputReader : MonoBehaviour
{
    private const int BuildButton = 0;

    public event Action<Vector3> BuildButtonClicked;

    private void Update()
    {
        if (Input.GetMouseButtonDown(BuildButton))
            BuildButtonClicked?.Invoke(Input.mousePosition);
    }
}
