using System.Collections;
using MainProject.Stats;
using UnityEngine;

public class BlockController : MonoBehaviour, IBlock
{
    public event System.Action GuardBreak;
    public event System.Action Parried;

    [SerializeField] private Transform Model;

    private bool _canRegenStamina;
    private float _parryTimer;
    private BlockData _data;

    private Coroutine _currentRegenDelay;

    public float CurrentStamina { get; private set; }
    public bool IsBlocking { get; private set; } = false;
    public bool CanParryNow { get; private set; } = false;

    private void Update()
    {
        CheckParry();
        CheckStamina();
    }

    public bool CanBlock()
    {
        return CurrentStamina >= _data.StaminaCostPerHit;
    }

    public bool IsInBlockAngle(Vector2 attackerPosition)
    {
        // 1. Находим направление от нас к атакующему
        // Если результат положительный — атакующий справа, если отрицательный — слева
        float directionToAttacker = attackerPosition.x - transform.position.x;

        // 2. Проверяем, куда смотрит наш персонаж (через localScale или переменную)
        // Допустим, transform.right.x > 0, если персонаж смотрит вправо
        float lookDirection = Model.localScale.x > 0 ? 1 : -1;

        // 3. Сравниваем знаки. Если они одинаковые — мы смотрим на врага
        // Например: враг справа (+) и мы смотрим вправо (+) -> Блок
        // Враг слева (-) и мы смотрим вправо (+) -> Удар в спину
        return (directionToAttacker > 0 && lookDirection > 0) || (directionToAttacker < 0 && lookDirection < 0);
    }

    public void SetData(BlockData data)
    { 
        _data = data;
        CurrentStamina = data.MaxStamina;
    }

    public float FilterDamage(float incomingDamage, Vector2 attackerPosition)
    {
        if (!IsBlocking)
            return incomingDamage;

        Debug.Log("Удар заблокирован!");

        CurrentStamina -= _data.StaminaCostPerHit;
        if (CurrentStamina <= 0)
        {
            GuardBreak?.Invoke();
            StopBlocking();
        }

        if (_data.CanParry && CanParryNow)
        {
            Debug.Log("Удар парирован!!");
            Parried.Invoke();
            return 0;
        }

        return incomingDamage * (1f - _data.DamageReduction);
    }

    public void StartBlocking()
    {
        if (_currentRegenDelay != null)
            StopCoroutine(_currentRegenDelay);

        IsBlocking = true;
        _canRegenStamina = false;

        if (_data.CanParry)
            _parryTimer = _data.ParryTime;
    }

    public void StopBlocking()
    {
        IsBlocking = false;

        if (_data.CanParry)
            _parryTimer = 0;

        _currentRegenDelay = StartCoroutine(WaitRegenDelay());
    }

    private void CheckParry()
    {
        if (_parryTimer > 0)
        {
            CanParryNow = true;

            _parryTimer -= Time.deltaTime;

            if (_parryTimer <= 0)
                CanParryNow = false;
        }
    }

    private void CheckStamina()
    {
        if (_data == null || !_canRegenStamina) return;

        if (CurrentStamina <= _data.MaxStamina)
            CurrentStamina += Time.deltaTime * _data.Regeneration;
        else
            CurrentStamina = _data.MaxStamina;
    }

    private IEnumerator WaitRegenDelay()
    {
        _canRegenStamina = false;
        yield return new WaitForSeconds(_data.RegenerationDelay);
        _canRegenStamina = true;
    }
}