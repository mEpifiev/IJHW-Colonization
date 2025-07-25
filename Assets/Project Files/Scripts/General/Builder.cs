using UnityEngine;

public class Builder : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Flag _flagPrefab;
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField] private float _minBuildDistance = 5f;


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
        Vector3 targetPosition = hit.point;

        Collider[] colliders = Physics.OverlapSphere(targetPosition, _minBuildDistance, _obstacleMask);

        if (colliders.Length > 0)
        {
            _selectedBase = null;
            return;
        }

        if (_selectedBase.FlagController.TryGetFlag(out Flag flag) == false)
        {
            flag = Instantiate(_flagPrefab, targetPosition, Quaternion.identity);    
            _selectedBase.FlagController.SetFlag(flag);
        }
        else
        {
            flag.transform.position = targetPosition;
        }

        _selectedBase = null;
    }

}
