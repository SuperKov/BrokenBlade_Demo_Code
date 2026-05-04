using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public class AttackNode : StateNode
{
    [SerializeField] private StateNode onAttacked;

    [Header("Options")]
    [SerializeField] private float attackDist = 2f;
    [SerializeField] private float onMissRecovery = 0.5f;

    [Foldout("Animations")] [SerializeField] private string AttackId = "Attack";
    [Foldout("Animations")] [SerializeField] private string WalkId = "Walk";
    [Foldout("Animations")][SerializeField] private string ArmsCombatIdleId = "CombatIdle";

    private bool _blockAttack;

    private Coroutine _currentMissRecovery;
    private Entity _target;

    public override void Enter()
    {
        Controller.Movement.BlockMoving = false;
        _blockAttack = false;

        _target = Controller.Detector.CurrentTarget;

        Controller.Combat.Attacked += OnAttacked;
    }

    public override void UpdateState()
    {
        if (_target == null || _blockAttack) return;

        float dist = Vector2.Distance(transform.position, _target.transform.position);
        float direction = Mathf.Sign(_target.transform.position.x - transform.position.x);

        if (dist > attackDist)
        {
            Controller.Animator.BodyAnimator.Play(WalkId);
            Controller.Animator.ArmsAnimator.Play(ArmsCombatIdleId);

            Controller.Movement.SetDirection(direction);
        }
        else if (!Controller.Combat.IsAttacking)
        {
            if (_target.Stats.CanBlock && _target.Block.IsBlocking)
            {
                TransitionTo(onAttacked);
                return;
            }

            Controller.Animator.Play(AttackId, 1 / Controller.Stats.Combat.AttackRate);
            
            Controller.Combat.StartDelayedAttack();
            _blockAttack = true;

            Controller.Movement.BlockMoving = true;
            Controller.Movement.SetDirection(0);
        }
    

        Controller.Movement.HandleFlip(Mathf.Sign(_target.transform.position.x - transform.position.x));
    }

    public override void Exit()
    {
        Controller.Movement.BlockMoving = false;
        Controller.Combat.StopAttack();

        _target = null;

        if (_currentMissRecovery != null)
            StopCoroutine(_currentMissRecovery);

        Controller.Combat.Attacked -= OnAttacked;
    }

    private void OnAttacked(bool hit)
    {
        if (hit)
            TransitionTo(onAttacked);
        else
            _currentMissRecovery = StartCoroutine(WaitMissRecovery());
    }

    private IEnumerator WaitMissRecovery()
    {
        yield return new WaitForSeconds(onMissRecovery);
        TransitionTo(onAttacked);
    }
}
