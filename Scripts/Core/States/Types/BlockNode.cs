using NaughtyAttributes;
using System.Collections;
using UnityEngine;

public class BlockNode : StateNode
{
    [SerializeField] private StateNode onStopBlock;
    [SerializeField] private StateNode onManyAttacks;

    [Header("Options")]
    [SerializeField] private float duration = 1f;
    [SerializeField] private int maxAttacks = 2;

    [Foldout("Animations")] [SerializeField] private string BlockId = "Block";

    private Coroutine _currentBlock;
    private int _currentAttacks;

    public override void Enter()
    {
        Controller.Block.GuardBreak += OnGuardBreak;
        Controller.HealthChanged += OnAttacked;

        _currentAttacks = 0;
        if (_currentBlock != null)
            StopCoroutine(_currentBlock);

        Controller.Block.StartBlocking();
        Controller.Animator.Play(BlockId);

        _currentBlock = StartCoroutine(WaitDuration());
    }

    public override void Exit()
    {
        Controller.Block.GuardBreak -= OnGuardBreak;
        Controller.HealthChanged -= OnAttacked;

        if (_currentBlock != null)
            StopCoroutine(_currentBlock);
    }

    public override void UpdateState() { }

    private void OnGuardBreak()
    {
        Controller.StartRecovering(Controller.Stats.Block.GuardBreakRecovering);
    }

    private void OnAttacked(float value, float maxValue)
    {
        if (_currentBlock != null)
            StopCoroutine(_currentBlock);

        _currentAttacks++;

        if (_currentAttacks >= maxAttacks)
            TransitionTo(onManyAttacks);
        else
            _currentBlock = StartCoroutine(WaitDuration());
    }

    private IEnumerator WaitDuration()
    {
        yield return new WaitForSeconds(duration);

        Controller.Block.StopBlocking();

        _currentBlock = null;
        TransitionTo(onStopBlock);
    }
}