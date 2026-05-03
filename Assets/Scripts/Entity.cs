using System;
using UnityEngine;

public abstract class Entity : MonoBehaviour, IDamageable
{
    public event Action OnDamaged;

    [SerializeField] protected float maxHealth = 100f;
    [SerializeField] protected float moveSpeed = 5f;

    public float CurrentHealth { get; protected set; }

    protected virtual void OnEnable()
    {
        CurrentHealth = maxHealth;
    }

    public virtual void TakeDamage(float amount)
    {
        CurrentHealth -= amount;

        OnDamaged?.Invoke();

        if(CurrentHealth <= 0)
        {
            Die();
        }
    }

    public abstract void Die();
}
