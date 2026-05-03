using UnityEngine;

public class RangedWeapon : PlayerWeapon
{
    [Header("Ranged Settings")]
    [SerializeField] private float attackRange = 8f;
    [SerializeField] private string projectilePoolTag = "PlayerBullet";

    [SerializeField] private int projectileCount = 1;
    [SerializeField] private float spreadAngle = 50f;

    [SerializeField] private AudioClip attackSound;


    protected override void ExecuteAttack()
    {

        Transform closestEnemy = GetClosestEnemy();

        if (closestEnemy == null) return;

        // angle and direction of target
        Vector2 baseDirection = (closestEnemy.position - transform.position).normalized;
        float baseAngle = Mathf.Atan2(baseDirection.y, baseDirection.x) * Mathf.Rad2Deg;


        //FX
        AudioManager.Instance.PlaySound(attackSound, 0.3f);


        for (int i = 0; i < projectileCount; i++)
        {
            float currentAngle = baseAngle;

            if (projectileCount > 1)
            {
                float randomOffset = Random.Range(-spreadAngle / 2f, spreadAngle / 2f);
                currentAngle += randomOffset;
            }

            Vector2 fireDirection = new Vector2(
                Mathf.Cos(currentAngle * Mathf.Deg2Rad),
                Mathf.Sin(currentAngle * Mathf.Deg2Rad)
            );

            GameObject projectileObj = PoolManager.Instance.SpawnFromPool(projectilePoolTag, transform.position, Quaternion.identity);

            if (projectileObj != null)
            {
                projectileObj.transform.rotation = Quaternion.Euler(0, 0, currentAngle);

                Projectile projectile = projectileObj.GetComponent<Projectile>();

                projectile.Setup(fireDirection, damage);
            }
        }
    }

    private Transform GetClosestEnemy()
    {
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (Collider2D potentialTarget in enemiesInRange)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;

            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget.transform;
            }
        }

        return bestTarget;
    }

    public void AddProjectile(float amount, float angle)
    {
        projectileCount += Mathf.RoundToInt(amount);
        spreadAngle += angle;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}