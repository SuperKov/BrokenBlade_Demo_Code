using System.Collections;
using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Entity : MonoBehaviour, IDamagable
{
    public System.Action<float, float> HealthChanged;
    public System.Action RecoveringStoped;
    public System.Action Dead;

    [field: SerializeField] public string Id { get; private set; } = "New Entity";

    [Header("General")]
    [SerializeField] protected bool _isEnemy = true;

    [Header("Statistic")]
    [Expandable] // Атрибут NaughtyAttributes для редактирования SO прямо здесь
    [SerializeField] protected EntityConfig _stats;

    [Header("Controllers")]
    [SerializeField] [ShowIf(nameof(_canMove))] private MoveController movement;
    [SerializeField] [ShowIf(nameof(_canAttack))] private CombatController combat;
    [SerializeField] [ShowIf(nameof(_canBlock))] private BlockController block;

    [Header("Flags")]
    public bool InCutscene = false;

    protected float _currentHealth;
    protected Rigidbody2D _rb;

    private Coroutine _currentRecovering;

    public EntityConfig Stats => _stats;

    public bool IsEnemy => _isEnemy;
    public bool IsRecovering { get; protected set; }

    // Ссылки на контроллеры. Они могут быть null!
    public MoveController Movement => movement;
    public CombatController Combat => combat;
    public BlockController Block => block;

    private bool _canMove => Stats != null && Stats.CanMove;
    private bool _canBlock => Stats != null && Stats.CanBlock;
    private bool _canAttack => Stats != null && Stats.CanAttack;

    protected void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        if (_stats == null) return;

        _currentHealth = _stats.MaxHealth;

        if (Stats.CanMove && movement == null)
            movement = GetComponent<MoveController>();
        if (Stats.CanBlock && block == null)
            combat = GetComponent<CombatController>();
        if (Stats.CanAttack && combat == null)
            block = GetComponent<BlockController>();
    }

    public void SwitchCutsceneMode(bool mode)
    {
        InCutscene = mode;
    }

    public virtual void StartRecovering(float time)
    {
        if (_currentRecovering != null) return;
        _currentRecovering = StartCoroutine(WaitRecovering(time));
    }

    public virtual void TakeDamage(float damage, Vector2 attackerPos, float knockbackForce)
    {
        StartRecovering(Stats.DamagedRecoveryTime);

        _currentHealth -= damage;

        HealthChanged?.Invoke(_currentHealth, Stats.MaxHealth);

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Die();
        }
    }

    public virtual void ApplyKnockback(Vector3 sender, float force)
    {
        if (!Stats.IsKnockable) return;

        Vector2 direction = (transform.position - sender).normalized;

        direction.y += 0.3f;
        direction = direction.normalized;

        _rb.linearVelocity = Vector2.zero;

        _rb.AddForce(direction * force, ForceMode2D.Impulse);
    }

    protected virtual void Die()
    {
        Dead?.Invoke();

        if (_currentRecovering != null)
            StopCoroutine(_currentRecovering);

        _currentHealth = Stats.MaxHealth;
        IsRecovering = false;
    }

    private IEnumerator WaitRecovering(float time)
    {
        IsRecovering = true;
        yield return new WaitForSeconds(time);
        IsRecovering = false;

        RecoveringStoped?.Invoke();
        _currentRecovering = null;
    }
}