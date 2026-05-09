using UnityEngine;

public class AuraWeapon : PlayerWeapon
{
    [SerializeField] private float currentRadius = 3f;
    [SerializeField] private AudioClip attackSound;


    private float baseRadius = 3.5f;
    private float baseScale = 1.4f;

    [SerializeField] private Transform visualAuraTransform;


    private void Start()
    {
        currentRadius = baseRadius;
        visualAuraTransform = transform.GetChild(0);

    }

    protected override void ExecuteAttack()
    {
        AudioManager.Instance.PlaySound(attackSound, 0.15f);


        // Detect enemies in range
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, currentRadius, enemyLayer);

        foreach (Collider2D hit in hits)
        {
            IDamageable damageable = hit.GetComponent<IDamageable>();
            damageable?.TakeDamage(damage);

            //if (hit.CompareTag("Enemy")) hit.GetComponent<Enemy>().ApplyMark(5);
        }
    }

    public void AddArea(float amount)
    {
        currentRadius += amount;

        UpdateVisualScale(currentRadius);
    }

    public void UpdateVisualScale(float currentRadius)
    {
        float scaleModifier = currentRadius / baseRadius;

        float newVisualScale = baseScale * scaleModifier;

        visualAuraTransform.localScale = new Vector3(newVisualScale, newVisualScale, 1f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, currentRadius);
    }
}
