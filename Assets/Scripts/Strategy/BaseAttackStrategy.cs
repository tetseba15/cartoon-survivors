using UnityEngine;

public abstract class BaseAttackStrategy
{
    protected float damage;
    protected float attackRange;
    protected float cooldown;
    protected float lastAttackTime;

    public float AttackRange => attackRange;

    public BaseAttackStrategy(float damage, float attackRange, float cooldown)
    {
        this.damage = damage;
        this.attackRange = attackRange;
        this.cooldown = cooldown;
        this.lastAttackTime = 0f;
    }


    public bool CanAttack(Transform attacker, Transform target)
    {
        float distance = Vector2.Distance(attacker.position, target.position);

        if(distance <= attackRange && Time.time >= lastAttackTime + cooldown)
        {
            lastAttackTime = Time.time;
            return true;
        }
        return false;
    }

    public abstract void ExecuteAttack(Transform attacker, Transform target);

}
