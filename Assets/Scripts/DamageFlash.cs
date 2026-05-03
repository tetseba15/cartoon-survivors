using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Entity))]
public class DamageFlash : MonoBehaviour
{
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private float flashDuration = 0.1f;

    private SpriteRenderer spriteRenderer;
    private Entity entity;
    private Color originalColor;
    private float currentFlashTimer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        entity = GetComponent<Entity>();
    }
    private void OnEnable()
    {
        entity.OnDamaged += TriggerFlash;

        currentFlashTimer = 0f;
    }
    private void OnDisable()
    {
        entity.OnDamaged -= TriggerFlash;
    }

    private void TriggerFlash()
    {
        if (currentFlashTimer <= 0)
        {
            originalColor = spriteRenderer.color;
        }

        spriteRenderer.color = flashColor;
        currentFlashTimer = flashDuration;
    }

    private void Update()
    {
        if (currentFlashTimer > 0)
        {
            currentFlashTimer -= Time.deltaTime;

            if (currentFlashTimer <= 0)
            {
                spriteRenderer.color = originalColor;
            }
        }
    }
}
