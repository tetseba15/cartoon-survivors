using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
    public static ExplosionManager Instance { get; private set; }

    [SerializeField] private int maxExplosionsPerFrame = 5;
    [SerializeField] private string explosionPoolTag = "ChainExplosion";

    private struct ExplosionRequest
    {
        public Vector2 position;
        public int depth;
    }

    private Queue<ExplosionRequest> explosionQueue = new Queue<ExplosionRequest>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void QueueExplosion(Vector2 position, int depth)
    {
        explosionQueue.Enqueue(new ExplosionRequest { position = position, depth = depth });
    }

    private void Update()
    {
        int processedThisFrame = 0;
        while (explosionQueue.Count > 0 && processedThisFrame < maxExplosionsPerFrame)
        {
            ExplosionRequest request = explosionQueue.Dequeue();
            SpawnExplosion(request.position, request.depth);
            processedThisFrame++;
        }
    }

    private void SpawnExplosion(Vector2 position, int depth)
    {
        GameObject explosionObj = PoolManager.Instance.SpawnFromPool(explosionPoolTag, position, Quaternion.identity);
        if (explosionObj != null)
        {
            ChainExplosion explosion = explosionObj.GetComponent<ChainExplosion>();
            if (explosion != null)
            {
                explosion.Setup(depth);
            }
        }
    }
}
