using UnityEngine;

public class MeleeCombatController : CombatController
{
    [Header("Melee Attack")]
    [SerializeField] private Transform AttackPoint;
    [SerializeField] private LayerMask AttackableLayer;

    public override void Attack()
    {
        if (_data == null) return;

        Collider2D[] targets = Physics2D.OverlapCircleAll(AttackPoint.position, _data.Range, AttackableLayer);

        bool hitAnything = false;

        if (targets.Length > 0)
        {
            foreach (Collider2D t in targets)
            {
                if (t.gameObject == gameObject) continue;

                if (t.TryGetComponent(out IDamagable damageable))
                {
                    damageable.TakeDamage(_data.Damage, transform.position, _data.KnockbackForce);
                    hitAnything = true;

                    if (!_data.Splash)
                        break; 
                }
            }
        }

        Attacked?.Invoke(hitAnything);
    }

#if UNITY_EDITOR
private void OnDrawGizmos()
{
    if (AttackPoint == null || _data == null) return;

    Gizmos.DrawWireSphere(AttackPoint.position, _data.Range);
}
#endif
}