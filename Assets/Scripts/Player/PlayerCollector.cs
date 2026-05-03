using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    [SerializeField] private float collectionRadius = 4f;
    [SerializeField] private float scanInterval = 0.2f; 
    [SerializeField] private LayerMask gemLayer;

    private float lastScanTime;

    private void Update()
    {
        if (Time.time >= lastScanTime + scanInterval)
        {
            ScanForGems();
        }
    }

    private void ScanForGems()
    {
        lastScanTime = Time.time;

        Collider2D[] gemsFound = Physics2D.OverlapCircleAll(transform.position, collectionRadius, gemLayer);

        foreach (Collider2D hit in gemsFound)
        {
            ExperienceGem gem = hit.GetComponent<ExperienceGem>();
            gem?.SetTarget(transform);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, collectionRadius);
    }
}
