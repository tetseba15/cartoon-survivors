using UnityEngine;

public class PlayerMeleeWeapon : MonoBehaviour
{
    [Header("Weapon Stats")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float attackRadius = 3f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip hitSpammedSound;

    private float lastAttackTime;

    private void Update()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            ExecuteSweepAttack();
        }
    }

    private void ExecuteSweepAttack()
    {
        AudioManager.Instance.PlaySound(attackSound, 0.8f);


        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(transform.position, attackRadius, enemyLayer);

        if (enemiesHit.Length > 0)
        {
            lastAttackTime = Time.time;

            foreach (Collider2D enemy in enemiesHit)
            {
                IDamageable damageable = enemy.GetComponent<IDamageable>();
                damageable?.TakeDamage(damage);
            }

            // Event for animations or sfx
            AudioManager.Instance.PlaySpammedSound(hitSpammedSound, 0.5f, 0.05f);


            // OnWeaponFired?.Invoke();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
