using UnityEngine;

[RequireComponent(typeof(StateMachine))]
public class EntityController : Entity
{
    [Header("Other Controllers")]
    [SerializeField] private HumanAnimator _animator;
    [SerializeField] private EntityDetector _detector;

    [Header("State Configuration")]
    public StateNode InitialState;
    public StateNode RecoveringState;

    public StateMachine StateMachine {  get; private set; }

    public HumanAnimator Animator => _animator;
    public EntityDetector Detector => _detector;

    protected new void Awake()
    {
        base.Awake();

        StateMachine = GetComponent<StateMachine>();

        StateNode[] states = GetComponents<StateNode>();
        foreach (StateNode state in states)
            state.Initialize(this);

        if (Movement != null) Movement.SetData(Stats.Movement);
        if (Combat != null) Combat.SetData(Stats.Combat);
        if (Block != null) Block.SetData(Stats.Block);
    }

    private void Start()
    {
        if (InitialState != null)
            StateMachine.Initialize(InitialState);
        else
            Debug.LogError($"[Entity] {gameObject.name}: Не задано начальное состояние!");
    }

    private void Update()
    {
        StateMachine.CurrentState?.UpdateState();
    }

    public override void TakeDamage(float damage, Vector2 attackerPos, float knockbackForce)
    {
        if (Block == null)
        {
            base.TakeDamage(damage, attackerPos, knockbackForce);
            return;
        }    

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
        base.Die();
        transform.position = Vector2.zero;
    }

    public override void StartRecovering(float time)
    {
        base.StartRecovering(time);
        StateMachine.ChangeState(RecoveringState);
    }
}