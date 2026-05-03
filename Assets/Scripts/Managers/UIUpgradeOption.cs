using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIUpgradeOption : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image iconImage;
    [SerializeField] private Button selectButton; 

    private UpgradeData currentUpgrade;
    private GameObject playerRef;

    private void Awake()
    {
        
        selectButton.onClick.AddListener(OnOptionSelected);
    }

    public void Setup(UpgradeData upgradeData, GameObject player)
    {
        currentUpgrade = upgradeData;
        playerRef = player;

        nameText.text = upgradeData.upgradeName;
        descriptionText.text = upgradeData.description;

        if (upgradeData.icon != null)
        {
            iconImage.sprite = upgradeData.icon;
            iconImage.enabled = true;
        }
        else
        {
            iconImage.enabled = false;
        }
    }

    private void OnOptionSelected()
    {
        if (currentUpgrade != null && playerRef != null)
        {
            currentUpgrade.ApplyUpgrade(playerRef);

            UIManager.Instance.CloseCurrentMenu();
        }
    }
}