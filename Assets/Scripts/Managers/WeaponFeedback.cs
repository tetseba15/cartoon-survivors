using UnityEngine;

public class WeaponFeedback : MonoBehaviour
{
    [Header("Visuals & Audio")]
    [SerializeField] private string vfxPoolTag = "AuraVFX"; // Pooled VFX
    [SerializeField] private AudioClip fireSound;
    private AudioSource audioSource;
    private PlayerWeapon weapon;

    private void Awake()
    {
        weapon = GetComponent<PlayerWeapon>();
        audioSource = GetComponent<AudioSource>(); 
    }

    private void OnEnable()
    {
        weapon.OnWeaponFired += TriggerFeedback;
    }

    private void OnDisable()
    {
        weapon.OnWeaponFired -= TriggerFeedback;
    }

    private void TriggerFeedback()
    {
        if (fireSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(fireSound);
        }

        // Spawn VFX
        GameObject vfx = PoolManager.Instance.SpawnFromPool(vfxPoolTag, transform.position, Quaternion.identity);

        if (vfx != null)
        {
            // VFX follows player
            vfx.transform.SetParent(this.transform);

            // VFX Return To Pool...
        }
    }
}
