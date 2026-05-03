using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : Entity
{
    //                      current and max health
    public static event Action<float, float> OnHealthChanged;

    public static event Action OnPlayerDeath;


    [Header("References")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform weaponContainer;
    [SerializeField] private Rigidbody2D rb;

    private FlickerTest flickerTest;

    private Vector2 playerInput;

    private bool isFacingRight = true;

    void Awake()
    {
        if (GameManager.SelectedCharacter != null)
        {
            CharacterData data = GameManager.SelectedCharacter;

            maxHealth = data.baseHealth;
            moveSpeed = data.baseSpeed;

            if (spriteRenderer != null && data.characterSprite != null)
            {
                spriteRenderer.sprite = data.characterSprite;
            }

            if (data.startingWeaponPrefab != null && weaponContainer != null)
            {
                Instantiate(data.startingWeaponPrefab, weaponContainer);
            }
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
    }


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        flickerTest = GetComponent<FlickerTest>();
    }

    private void Update()
    {
        playerInput.x = Input.GetAxisRaw("Horizontal"); 
        playerInput.y = Input.GetAxisRaw("Vertical");

        Flip();
    }

    private void FixedUpdate()
    {
        Vector2 targetPosition = rb.position + playerInput.normalized * moveSpeed * Time.fixedDeltaTime;

        rb.MovePosition(targetPosition);
    }

    public void AddSpeed(float amount)
    {
        moveSpeed += amount;
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);

        //flickerTest.Ouch();

        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
    }
    
    public override void Die()
    {
        gameObject.SetActive(false);

        OnPlayerDeath?.Invoke();
    }

    private void Flip()
    {
        if(playerInput.x > 0.1 && !isFacingRight)
        {
            isFacingRight = !isFacingRight;
            //transform.Rotate(0, 180f, 0);
            spriteRenderer.flipX = false;
        }
        else if(playerInput.x < -0.1 && isFacingRight)
        {
            isFacingRight = !isFacingRight;
            //transform.Rotate(0, 180f, 0);
            spriteRenderer.flipX = true;

        }
    }

}
