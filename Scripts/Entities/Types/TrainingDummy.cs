using UnityEngine;

public class TrainingDummy : Entity
{
    [SerializeField] private FrameAnimator Anim;

    [Header("Animation")]
    [SerializeField] private string TakeDamageId = "TakeDamage";

    public override void TakeDamage(float damage, Vector2 attackerPos, float knockbackForce)
    {
        Vector3 rotation = Anim.transform.localEulerAngles;
        rotation.y = attackerPos.x < transform.position.x ? 0 : 180;
        Anim.transform.localEulerAngles = rotation;

        Anim.Play(TakeDamageId);
    }

    protected override void Die() { }
}