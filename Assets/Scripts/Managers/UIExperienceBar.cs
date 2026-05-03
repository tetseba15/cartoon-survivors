using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIExperienceBar : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image expFillImage; 
    [SerializeField] private TextMeshProUGUI levelText; 

    private void Start()
    {
        ExperienceManager.Instance.OnExperienceChanged += UpdateExpBar;
        ExperienceManager.Instance.OnLevelUp += UpdateLevelText;

        // Initial update
        UpdateLevelText(ExperienceManager.Instance.CurrentLevel);
        UpdateExpBar(0f, 1f); 
    }

    private void OnDestroy()
    {
        if (ExperienceManager.Instance != null)
        {
            ExperienceManager.Instance.OnExperienceChanged -= UpdateExpBar;
            ExperienceManager.Instance.OnLevelUp -= UpdateLevelText;
        }
    }

    private void UpdateExpBar(float currentExp, float targetExp)
    {
        expFillImage.fillAmount = currentExp / targetExp;
    }

    private void UpdateLevelText(int newLevel)
    {
        levelText.text = $"LVL {newLevel}";

        // VFX for the text
    }
}
