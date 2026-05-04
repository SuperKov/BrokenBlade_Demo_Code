using NaughtyAttributes;
using System;
using UnityEngine;

namespace MainProject.Stats
{
    [Serializable]
    public class MovementData
    {
        public float Speed = 5f;
        public float Acceleration = 2f;
    }

    [Serializable]
    public class CombatData
    {
        [Header("General")]
        public float Damage = 20f;
        public float Range = 1.5f;
        public float AttackRate = 1f;

        [Header("Knockback")]
        public float KnockbackForce = 1.5f;

        [Header("Splash")]
        public bool Splash = false;

        [Header("Animation")]
        public float BaseRecoveringDuration = 0.5f;
        public float BaseAttackDelay = 0.5f;

        public float GetRecoveringDelay()
        {
            return BaseRecoveringDuration / AttackRate;
        }

        public float GetAttackDelay()
        {
            return BaseAttackDelay / AttackRate;
        }
    }

    [Serializable]
    public class BlockData
    {
        [Range(0, 1)] public float DamageReduction = 0.8f; // Поглощает 80% урона

        [Header("Guardian Break")]
        public float MaxStamina = 100f;
        public float Regeneration = 10f;
        public float RegenerationDelay = 1f;
        public float StaminaCostPerHit = 10f;
        public float GuardBreakRecovering = 1f;

        [Header("Parring")]
        public bool CanParry = false;
        public float ParryTime = 0.2f;
    }
}