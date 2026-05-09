using UnityEngine;
using System.Collections;

public class ChainExplosion : MonoBehaviour
{
    [SerializeField] private float radius = 2f;
    [SerializeField] private float damage = 20f;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private string poolTag = "ChainExplosion";

    public void Setup(int depth)
    {
        StopAllCoroutines();
        // For visual feedback in placeholder
        transform.localScale = Vector3.zero;
        StartCoroutine(ExplosionRoutine(depth));
    }

    private IEnumerator ExplosionRoutine(int depth)
    {
        // Simple scale up effect for placeholder
        float timer = 0;
        Vector3 targetScale = Vector3.one * radius * 2;
        
        // Immediate scan to ensure rapid chains
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayer);
        
        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null && enemy.CurrentHealth > 0)
            {
                if (depth > 0)
                {
                    enemy.ApplyMark(depth - 1);
                }
                enemy.TakeDamage(damage);
            }
        }

        while(timer < duration)
        {
            timer += Time.deltaTime;
            transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, timer / (duration * 0.2f));
            yield return null;
        }

        ReturnToPool();
    }

    private void ReturnToPool()
    {
        PoolManager.Instance.ReturnToPool(poolTag, this.gameObject);
    }
}
