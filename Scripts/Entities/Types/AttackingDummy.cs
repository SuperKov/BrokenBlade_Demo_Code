using UnityEngine;

public class AttackingDummy : Entity
{
    [Header("Animation")]
    [SerializeField] private FrameAnimator Anim;

    [SerializeField] private string TakeDamageId = "TakeDamage";
    [SerializeField] private string AttackId = "Attack";

    private void Start()
    {
        Combat.SetData(Stats.Combat);
    }

    private void Update()
    {
        if (!Combat.IsAttacking && !IsRecovering)
        {
            Combat.StartDelayedAttack();
            Anim.Play(AttackId);
        }
    }

    public override void TakeDamage(float damage, Vector2 attackerPos, float knockbackForce)
    {
        base.TakeDamage(damage, attackerPos, knockbackForce);

        Vector3 rotation = Anim.transform.localEulerAngles;
        rotation.y = attackerPos.x < transform.position.x ? 0 : 180;
        Anim.transform.localEulerAngles = rotation;

        Anim.Play(TakeDamageId);
    }

    protected override void Die() { }
}
