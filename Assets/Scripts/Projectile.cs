using UnityEngine;
using UnityEngine.EventSystems;

public class Projectile : MonoBehaviour, IPoolable
{
    [Header("Settings")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private string poolTag;
    [SerializeField] private AudioClip hitSpammedSound;




    private Vector2 _direction;
    private float _damage;
    private float _currentLifeTime;

    public void Setup(Vector2 direction, float damage)
    {
        _direction = direction.normalized;
        _damage = damage;
        _currentLifeTime = 0f;
    }


    private void Update()
    {
        _currentLifeTime += Time.deltaTime;

        if (_currentLifeTime >= lifeTime)
        {
            ReturnToPool();
            return;
        }

        float moveDistance = speed * Time.deltaTime;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, _direction, moveDistance, targetLayer);

        if(hit.collider != null)
        {
            IDamageable damageable= hit.collider.GetComponent<IDamageable>();

            damageable?.TakeDamage(_damage);
            AudioManager.Instance.PlaySpammedSound(hitSpammedSound, 0.3f, 0.03f);

            ReturnToPool();
            return;
        }


        transform.Translate(_direction * moveDistance, Space.World);

    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    IDamageable damageable = collision.GetComponent<IDamageable>();

    //    if (damageable != null) 
    //    {
    //        damageable.TakeDamage(_damage);

    //        IPoolable poolable = GetComponent<IPoolable>();
    //        if(poolable != null)
    //        {
    //            poolable.ReturnToPool();
    //        }
    //    }
    //}

    public void ReturnToPool()
    {
        PoolManager.Instance.ReturnToPool(poolTag, this.gameObject);
    }
}
