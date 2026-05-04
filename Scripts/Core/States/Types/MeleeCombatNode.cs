using NaughtyAttributes;
using UnityEngine;

public class MeleeCombatNode : StateNode
{
    [SerializeField] private StateNode onToFar;
    [SerializeField] private StateNode onToClose;
    [SerializeField] private StateNode onTargetLost;
    [SerializeField] private StateNode onTargetStartAttack;

    [Header("Options")]
    [SerializeField] private float optimalDist = 3f;
    [SerializeField] private float toCloseDist = 1f;
    [SerializeField] private float inactionWaitTime = 10f;
    [Range(0, 1f)] [SerializeField] private float blockChance = 1f;
    [SerializeField] private StateNode[] possibleAttacks; // Список разных атак

    [Foldout("Animations")] [SerializeField] private string CombatIdleId = "CombatIdle";

    private Entity _target;
    private float _inactionTimer;

    public override void Enter()
    {
        _target = Controller.Detector.CurrentTarget;

        _target.Dead += OnTargetDead;
        if (_target.Combat != null)
            _target.Combat.AttackStarted += OnTargetAttack;

        _inactionTimer = 0;
    }

    public override void UpdateState()
    {
        if (_target == null)
        {
            TransitionTo(onTargetLost);
            return;
        }

        float dist = Vector2.Distance(transform.position, _target.transform.position);

        _inactionTimer += Time.deltaTime;
        
        if (_inactionTimer > inactionWaitTime)
        {
            ChooseRandomAttack();
            return;
        }
        else if (dist < toCloseDist) // Слишком близко — отходим
        {
            TransitionTo(onToClose);
            return;
        }
        else if (dist > optimalDist) // Слишком далеко — бежим
        {
            TransitionTo(onToFar);
            return;
        }

        Controller.Animator.Play(CombatIdleId);
        Controller.Movement.HandleFlip(Mathf.Sign(_target.transform.position.x - transform.position.x));
    }

    public override void Exit()
    {
        if (_target.Combat != null &&_target != null)
            _target.Combat.AttackStarted -= OnTargetAttack;
        if (_target != null)
            _target.Dead -= OnTargetDead;

        _target = null;
    }

    private void ChooseRandomAttack()
    {
        StateNode attack = possibleAttacks[Random.Range(0, possibleAttacks.Length)];

        TransitionTo(attack);
    }

    private void OnTargetDead()
    {
        TransitionTo(onTargetLost);
    }

    private void OnTargetAttack()
    {
        float chance = Random.Range(0f, 1f);

        if (chance <= blockChance)
        {
            TransitionTo(onTargetStartAttack);
            Debug.Log("БЛОК");
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Vector2 pos = transform.position;
        pos.y += 0.1f;

        Vector2 t = new(pos.x + optimalDist, pos.y);

        Gizmos.DrawLine(pos, t);

        pos.y += 0.1f;
        t = new(pos.x + toCloseDist, pos.y);
        Gizmos.DrawLine(pos, t);
    }
#endif
}