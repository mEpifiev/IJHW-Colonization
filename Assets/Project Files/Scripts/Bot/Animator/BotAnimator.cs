using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BotAnimator : MonoBehaviour
{
    private readonly int Walk = Animator.StringToHash("IsWalk");

    private Animator _animator;

    private void Awake() =>
        _animator = GetComponent<Animator>();

    public void PlayWalkAnimation(bool isWalk) =>
        _animator.SetBool(Walk, isWalk);
}
