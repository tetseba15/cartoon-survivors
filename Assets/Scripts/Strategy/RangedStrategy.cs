using UnityEngine;

public class RangedStrategy : BaseAttackStrategy
{
    private string projectilePoolTag;
    private float projectileDamage;

    public RangedStrategy(string poolTag) : base(damage: 5f, attackRange: 8f, cooldown: 3f) 
    {
        this.projectilePoolTag = poolTag;
        this.projectileDamage = 10f;
    }

    public override void ExecuteAttack(Transform attacker, Transform target)
    {
        GameObject projectileObj = PoolManager.Instance.SpawnFromPool(
            projectilePoolTag,
            attacker.position,
            Quaternion.identity
            );

        if(projectileObj != null)
        {
            Vector2 direction = (target.position - attacker.position).normalized;

            //
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            projectileObj.transform.rotation = Quaternion.Euler(0, 0, angle);

            Projectile projectile = projectileObj.GetComponent<Projectile>();
            projectile.Setup(direction, projectileDamage);
        }
        else
        {
            Debug.LogWarning("No hay projectileObj");
        }

    }
}
