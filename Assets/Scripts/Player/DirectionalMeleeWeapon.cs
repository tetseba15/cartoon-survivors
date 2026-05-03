using UnityEngine;

public class DirectionalMeleeWeapon : PlayerWeapon
{
    [Header("Slash Settings")]
    [SerializeField] private float attackRadius = 3f;
    [SerializeField] private float coneAngle = 120f; // Attack angle
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip hitSpammedSound;

    public float AttackRadius => attackRadius;
    public float ConeAngle => coneAngle;

    // Last player direction
    private Vector2 facingDirection = Vector2.right;

    public Vector2 FacingDirection => facingDirection;

    protected override void Update()
    {
        
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector2 inputDir = new Vector2(moveX, moveY).normalized;

        if (inputDir != Vector2.zero)
        {
            facingDirection = inputDir;
        }

        //Cooldown
        base.Update();
    }

    protected override void ExecuteAttack()
    {
        AudioManager.Instance.PlaySound(attackSound, 0.2f);

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRadius, enemyLayer);

        foreach (Collider2D hit in hits)
        {
            Vector2 dirToEnemy = (hit.transform.position - transform.position).normalized;

            if (Vector2.Angle(facingDirection, dirToEnemy) <= coneAngle / 2f)
            {
                IDamageable damageable = hit.GetComponent<IDamageable>();
                damageable?.TakeDamage(damage);
                AudioManager.Instance.PlaySpammedSound(hitSpammedSound, 0.4f, 0.03f);
            }
        }
    }

    // Upgrades
    public void AddRadius(float radius, float angle)
    {
        attackRadius += radius;
        coneAngle = Mathf.Clamp(coneAngle + angle, 0f, 240f); // Max angle
    }    

    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        Vector3 rightLimit = Quaternion.Euler(0, 0, coneAngle / 2f) * Vector2.right * attackRadius;
        Vector3 leftLimit = Quaternion.Euler(0, 0, -coneAngle / 2f) * Vector2.right * attackRadius;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + rightLimit);
        Gizmos.DrawLine(transform.position, transform.position + leftLimit);
    }
}