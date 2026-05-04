using UnityEngine;

public class PlayerStats : Entity
{
    [Header("Movement")]
    [SerializeField] private float _sprintModifier = 2.25f;
    [SerializeField] private float _backstepModifier = 0.9f;

    [Header("Focus")]
    [SerializeField] private float _focusZoom = 1.2f;

    public float BackstepModifier => _backstepModifier;
    public float SprintModifier => _sprintModifier;
    public float FocusZoom => _focusZoom;

    public static PlayerStats Instance {  get; private set; }

    protected new void Awake()
    {
        base.Awake();
        Instance = this;
    }

    private void OnEnable()
    {
        Block.GuardBreak += OnGuardBreak;
    }

    private void OnDisable()
    {
        Block.GuardBreak -= OnGuardBreak;
    }

    public override void TakeDamage(float damage, Vector2 attackerPos, float knockbackForce)
    {
        if (Block.IsBlocking && Block.IsInBlockAngle(attackerPos))
            damage = Block.FilterDamage(damage, attackerPos);
        else
        {
            ApplyKnockback(attackerPos, knockbackForce);
            StartRecovering(Stats.DamagedRecoveryTime);
        }

        _currentHealth -= damage;

        HealthChanged?.Invoke(_currentHealth, Stats.MaxHealth);

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Die();
        }
    }

    protected override void Die()
    {
        SceneLoader.Instance.LoadScene(SceneLoader.Instance.CurrentScene);
    }

    private void OnGuardBreak()
    {
        StartRecovering(Stats.Block.GuardBreakRecovering);
    }
}