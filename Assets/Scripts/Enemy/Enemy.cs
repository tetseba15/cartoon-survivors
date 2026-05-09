using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct RoleSpriteMapping
{
    public EnemyRole role;
    public Sprite sprite;
}

public class Enemy : Entity, IPoolable
{
    [SerializeField] private string poolTag = "Enemy";

    private bool isElite;

    [Header("Visual Variants")]
    [SerializeField] private SpriteRenderer spriteRenderer; 
    [SerializeField] private List<RoleSpriteMapping> spriteMappings;

    [SerializeField] private Dictionary<EnemyRole, Sprite> _enemySprite;

    private EnemyRole currentRole;


    private BaseAttackStrategy currentAttackStrategy;
    private Transform playerTarget;

    protected void Awake()
    {
        _enemySprite = new Dictionary<EnemyRole, Sprite>();

        foreach(var mapping in spriteMappings)
        {
            if (!_enemySprite.ContainsKey(mapping.role))
            {
                _enemySprite.Add(mapping.role, mapping.sprite);
            }
        }
    }

    public bool IsMarked { get; private set; }
    private int currentMarkDepth;

    public void ApplyMark(int depth)
    {
        IsMarked = true;
        currentMarkDepth = depth;
    }

    public void Setup(BaseAttackStrategy attackStrategy, Transform target, EnemyRole role, bool isEliteSpawn = false)
    {
        IsMarked = false;
        currentMarkDepth = 0;

        currentAttackStrategy = attackStrategy;
playerTarget = target;
        currentRole = role;
        isElite = isEliteSpawn;

        if(_enemySprite.TryGetValue(role, out Sprite correctSprite))
        {
            spriteRenderer.sprite = correctSprite;
        }
        else
        {
            Debug.LogWarning($"No Sprite assigned for role {role} in {gameObject.name}");
        }

        float hpMultiplier = role switch
        {
            EnemyRole.HeavyMelee => 3f,  
            EnemyRole.Ranged => 0.7f,    
            EnemyRole.FastMelee => 1f,   
            _ => 1f
        };

        if (isElite)
        {
            hpMultiplier *= 15f; 
            transform.localScale = new Vector3(1.5f, 1.5f, 1f); 
            spriteRenderer.color = new Color(1f, 0.4f, 0.4f); 
        }
        else
        {
            
            transform.localScale = Vector3.one;
            spriteRenderer.color = Color.white;
        }


        CurrentHealth = maxHealth * hpMultiplier;
    }

    

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (playerTarget == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTarget.position);

        if (distanceToPlayer > currentAttackStrategy.AttackRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerTarget.position, moveSpeed * Time.deltaTime);
        }

        if (currentAttackStrategy != null && currentAttackStrategy.CanAttack(transform, playerTarget))
        {
            currentAttackStrategy.ExecuteAttack(transform, playerTarget);
        }



    }

    public override void TakeDamage(float amount)
    {

        base.TakeDamage(amount);

    }

    public override void Die()
    {
        if (IsMarked)
        {
            ExplosionManager.Instance.QueueExplosion(transform.position, currentMarkDepth);
        }

        GameObject gemObj = PoolManager.Instance.SpawnFromPool("ExpGem", transform.position, Quaternion.identity);

        if (isElite)
        {
            if (gemObj != null)
                gemObj.GetComponent<ExperienceGem>().Setup(50f, true);


        }
        else if (currentRole == EnemyRole.HeavyMelee) 
        {
            if (gemObj != null) gemObj.GetComponent<ExperienceGem>().Setup(15f);


            float dropChance = Random.Range(0f, 100f);
            if (dropChance <= 2f) // 2% chance of magnet drop
            {
                PoolManager.Instance.SpawnFromPool("Magnet", transform.position, Quaternion.identity);
            }
        }

        else
        {
            if (gemObj != null) gemObj.GetComponent<ExperienceGem>().Setup(10f);
        }

        ReturnToPool();

    }

    public void ReturnToPool()
    {
        PoolManager.Instance.ReturnToPool(poolTag, this.gameObject);
    }
}
