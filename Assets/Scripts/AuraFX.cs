using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Animator))]
public class AuraFX : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private PlayerWeapon weaponInfo;
    private Animator animator;
    private const float baseClipDuration = 1f; 

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (weaponInfo != null)
        {
            weaponInfo.OnWeaponFired += ForcePulseSync;
            SyncAnimationSpeed();
        }
    }

    private void OnDisable()
    {
        if (weaponInfo != null)
        {
            weaponInfo.OnWeaponFired -= ForcePulseSync;
        }
    }


    
    public void SyncAnimationSpeed()
    {
        if (weaponInfo != null && weaponInfo.Cooldown > 0)
        {
            animator.speed = baseClipDuration / weaponInfo.Cooldown;
        }
    }


    void Update()
    {
        transform.Rotate(0, 0, speed * Time.deltaTime);
    }

    private void ForcePulseSync()
    {
        animator.Play(0, -1, 0f); 
    }
}
