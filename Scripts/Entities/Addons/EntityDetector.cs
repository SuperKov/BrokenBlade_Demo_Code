using UnityEngine;
using System;

[RequireComponent(typeof(CircleCollider2D))]
public class EntityDetector : MonoBehaviour
{
    public event Action<Entity> TargetSelected;

    [Header("Detecter")]
    [SerializeField] private float DetectRange = 10f;
    [SerializeField] private LayerMask DetectLayer;

    private CircleCollider2D _collider;
    private Entity _target;

    public Entity CurrentTarget => _target;

    private void Awake()
    {
        _collider = GetComponent<CircleCollider2D>();

        _collider.isTrigger = true;
        _collider.radius = DetectRange;
        _collider.callbackLayers = DetectLayer;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == this ||
        !collision.TryGetComponent(out Entity entity) ||
        _target != null) return;

        _target = entity;
        TargetSelected?.Invoke(entity);
    }

    public void ResetTarget()
    {
        _target = null;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, DetectRange);
    }
#endif
}