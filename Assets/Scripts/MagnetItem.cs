using UnityEngine;

public class MagnetItem : MonoBehaviour, IPoolable
{
    [Header("Magnet Settings")]
    [SerializeField] private float pullRadius = 150f; 
    [SerializeField] private LayerMask gemLayer;      
    [SerializeField] private string poolTag = "Magnet";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PullAllGems(other.transform);

            // VFX o SFX

            ReturnToPool();
        }
    }

    private void PullAllGems(Transform playerTransform)
    {
        Collider2D[] allGems = Physics2D.OverlapCircleAll(transform.position, pullRadius, gemLayer);

        foreach (Collider2D hit in allGems)
        {
            ExperienceGem gem = hit.GetComponent<ExperienceGem>();
            gem?.SetTarget(playerTransform);
        }
    }

    public void ReturnToPool()
    {
        PoolManager.Instance.ReturnToPool(poolTag, gameObject);
    }
}