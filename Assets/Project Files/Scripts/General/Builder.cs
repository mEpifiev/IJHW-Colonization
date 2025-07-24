using UnityEngine;

public class Builder : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;

    [SerializeField] private Flag _flagPrefab;

    private Camera _camera;
    private Base _selectedBase;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        _inputReader.BuildButtonClicked += OnBuildButtonClicked;
    }

    private void OnDisable()
    {
        _inputReader.BuildButtonClicked -= OnBuildButtonClicked;
    }

    private void OnBuildButtonClicked(Vector3 mousePosition)
    {
        Ray ray = _camera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit) == false)
            return;

        if (hit.collider.TryGetComponent(out Base selectedBase))
        {
            _selectedBase = selectedBase;
        }
        else if (hit.collider.TryGetComponent(out Plane _) && _selectedBase)
        {
            Build(hit);
        }      
    }

    private void Build(RaycastHit hit)
    {
        Vector3 targetPosition = new Vector3(hit.point.x, hit.point.y, hit.point.z);

        if (_selectedBase.IsFlagBuilded == false)
        {
            Flag flag = Instantiate(_flagPrefab, targetPosition, Quaternion.identity);
            flag.transform.SetParent(_selectedBase.transform);
            _selectedBase.SetFlag(flag);
        }
        else
        {
            if (_selectedBase.TryGetFlag(out Flag flag))
            {
                flag.transform.position = targetPosition;
            }
        }

        _selectedBase = null;
    }

}
