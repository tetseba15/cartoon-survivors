using UnityEngine;

public class HeavyMeleeStrategy : BaseAttackStrategy
{
    private LayerMask targetLayer;

    public HeavyMeleeStrategy(LayerMask targetLayer) : base(damage: 15f, attackRange: .75f, cooldown: 2.0f) 
    {
        this.targetLayer = targetLayer;
    }

    public override void ExecuteAttack(Transform attacker, Transform target)
    {

        Collider2D[] hits = Physics2D.OverlapCircleAll(attacker.position, attackRange, targetLayer);

        foreach (Collider2D hit in hits)
        {
            if (hit.transform == target)
            {
                IDamageable damageable = hit.GetComponent<IDamageable>();
                damageable?.TakeDamage(damage);
                return;
            }
        }

    }
}
