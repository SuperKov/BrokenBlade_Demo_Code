using UnityEngine;

public interface IDamagable
{
    public void TakeDamage(float damage, Vector2 attackerPos, float knockbackForce);
}
