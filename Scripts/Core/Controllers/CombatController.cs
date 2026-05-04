using System.Collections;
using MainProject.Stats;
using UnityEngine;

public abstract class CombatController : MonoBehaviour, IAttack
{
    public System.Action<bool> Attacked;
    public System.Action AttackStarted;
    public System.Action AttackEnded;

    protected CombatData _data;
    protected Coroutine _currentAttack;

    public bool IsAttacking { get; private set; } = false;

    public void SwitchCanAttack(bool mode) => IsAttacking = mode;

    public void StartDelayedAttack()
    {
        if (IsAttacking) return;

        if (_currentAttack != null)
            StopCoroutine(_currentAttack);

        _currentAttack = StartCoroutine(AttackSequence());

        AttackStarted?.Invoke();
    }

    private IEnumerator AttackSequence()
    {
        IsAttacking = true;

        AttackStarted?.Invoke();

        // 1. Фаза замаха (Anticipation)
        yield return new WaitForSeconds(_data.GetAttackDelay());

        Attack();

        // 2. Фаза оставшаяся (Aniamtion)
        yield return new WaitForSeconds(_data.GetRecoveringDelay());

        FinishAttack();
        AttackEnded?.Invoke();
    }

    public void StopAttack()
    {
        if (_currentAttack != null)
            StopCoroutine(_currentAttack);
        FinishAttack();
    }

    private void FinishAttack()
    {
        IsAttacking = false;
        _currentAttack = null;
    }

    public void SetData(CombatData data) => _data = data;

    public abstract void Attack();
}