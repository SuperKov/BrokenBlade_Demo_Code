using MainProject.Stats;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEntityStats", menuName = "Configs/Entity Stats")]
public class EntityConfig : ScriptableObject
{
    [Header("General")]
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private float _damagedRecoveryTime = 0.2f;

    [Header("Properties")]
    [SerializeField] private bool _isKnockable = true;

    [Header("Move")]
    [SerializeField] private bool _canMove;
    [BoxGroup("Move")] [ShowIf(nameof(_canMove))] [SerializeField] private MovementData _movement;

    [Header("Combat")]
    [SerializeField] private bool _canAttack;
    [BoxGroup("Attack")] [ShowIf(nameof(_canAttack))] [SerializeField] private CombatData _combat;

    [Header("Block")]
    [SerializeField] private bool _canBlock;
    [BoxGroup("Block")] [ShowIf(nameof(_canBlock))] [SerializeField] private BlockData _block;

    public MovementData Movement => _movement;
    public CombatData Combat => _combat;
    public BlockData Block => _block;

    public bool IsKnockable => _isKnockable;
    public float MaxHealth => _maxHealth;
    public float DamagedRecoveryTime => _damagedRecoveryTime;

    public bool CanAttack => _canAttack;
    public bool CanMove => _canMove;
    public bool CanBlock => _canBlock;
}