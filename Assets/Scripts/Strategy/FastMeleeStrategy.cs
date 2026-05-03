using UnityEngine;

public class FastMeleeStrategy : BaseAttackStrategy
{
    private LayerMask targetLayer;

    public FastMeleeStrategy(LayerMask targetLayer) : base(damage: 5f, attackRange: .5f, cooldown: 0.5f) 
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
