using TMPro;
using UnityEngine;

[RequireComponent(typeof(ResourceCounter))]
public class CrystalCounterView : MonoBehaviour
{
    [SerializeField] private TMP_Text _view;

    private ResourceCounter _counter;

    private void Awake()
    {
        _counter = GetComponent<ResourceCounter>();
    }

    private void OnEnable()
    {
        _counter.Changed += OnDisplayChanged;
    }

    private void OnDisable()
    {
        _counter.Changed -= OnDisplayChanged;
    }

    private void OnDisplayChanged(int count)
    {
        _view.text = count.ToString();
    }
}
