using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHealthBar : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image healthFillImage;

    private void OnEnable()
    {
        Player.OnHealthChanged += UpdateHealthBar;
    }

    private void OnDisable()
    {
        Player.OnHealthChanged -= UpdateHealthBar;
    }

    private void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        healthFillImage.fillAmount = currentHealth / maxHealth;
    }
}
