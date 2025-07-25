using UnityEngine;

public class FlagController : MonoBehaviour
{
    private Flag _currentFlag;

    public bool IsFlagBuilded { get; private set; } = false;
    public Vector3 FlagPosition => _currentFlag.transform.position;

    public bool TryGetFlag(out Flag flag)
    {
        if (_currentFlag != null)
        {
            flag = _currentFlag;
            return true;
        }

        flag = null;
        return false;
    }

    public void SetFlag(Flag flag)
    {
        if (_currentFlag != null)
            return;

        _currentFlag = flag;
        IsFlagBuilded = true;
    }

    public void ClearFlag()
    {
        if (_currentFlag != null)
            Destroy(_currentFlag.gameObject);

        _currentFlag = null;
        IsFlagBuilded = false;

    }
}
