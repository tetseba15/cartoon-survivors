using System;
using UnityEngine;

public abstract class PlayerWeapon : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] protected float damage = 10f;
    [SerializeField] protected float cooldown = 1.5f;
    [SerializeField] protected LayerMask enemyLayer;

    public float Cooldown => cooldown;

    protected float lastAttackTime;

    public event Action OnWeaponFired;

    private void Awake()
    {
        lastAttackTime = Time.time;
    }

    protected virtual void Update()
    {
        if (Time.time >= lastAttackTime + cooldown)
        {
            lastAttackTime += cooldown;
            ExecuteAttack();

            // Notify SFX & VFX
            OnWeaponFired?.Invoke();
        }
    }

    public void AddDamage(float amount)
    {
        damage *= amount;
    }

    // Each weapon defines its damage
    protected abstract void ExecuteAttack();
}
