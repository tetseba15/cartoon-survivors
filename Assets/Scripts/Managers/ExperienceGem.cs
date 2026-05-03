using UnityEngine;

public class ExperienceGem : MonoBehaviour, IPoolable
{
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private string poolTag = "ExpGem";
    [SerializeField] private AudioClip expPickupSound;
    private bool isEliteGem = false;
    private SpriteRenderer spriteRenderer;

    private float expAmount;
    private Transform target;
    private bool isFollowing;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Setup(float amount, bool isElite = false)
    {
        expAmount = amount;
        isFollowing = false;
        target = null;
        isEliteGem = isElite;

        if (isEliteGem)
        {
            transform.localScale = new Vector3(1.5f, 1.5f, 1f);
            spriteRenderer.color = new Color(1f, 0.4f, 0.4f);
        }
        else
        {
            transform.localScale = Vector3.one;
            spriteRenderer.color = Color.white;
        }
    }

    void Update()
    {
        if (isFollowing && target != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }
    }

    public void SetTarget(Transform playerTransform)
    {
        target = playerTransform;
        isFollowing = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ExperienceManager.Instance.AddExperience(expAmount);
            AudioManager.Instance.PlaySpammedSound(expPickupSound, 0.25f, 0.05f);
            ReturnToPool();
        }
    }

    public void ReturnToPool()
    {
        PoolManager.Instance.ReturnToPool(poolTag, gameObject);
    }
}
