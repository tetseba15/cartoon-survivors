using System.Collections.Generic;
using UnityEngine;

public class LevelUpScreen : MonoBehaviour
{
    [Header("Master Upgrade Pool")]
    [SerializeField] private List<UpgradeData> allUpgrades;

    [Header("UI References")]
    [SerializeField] private UIUpgradeOption[] optionCards;

    private GameObject player;

    private void OnEnable()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        GenerateRandomOptions();
    }

    private void GenerateRandomOptions()
    {
        List<UpgradeData> availableUpgrades = new List<UpgradeData>();
        foreach (var upgrade in allUpgrades)
        {
            if (upgrade.CanBeChosen(player))
            {
                availableUpgrades.Add(upgrade);
            }
        }

        // Algorithm Fisher-Yates 
        for (int i = 0; i < availableUpgrades.Count; i++)
        {
            UpgradeData temp = availableUpgrades[i];
            int randomIndex = Random.Range(i, availableUpgrades.Count);
            availableUpgrades[i] = availableUpgrades[randomIndex];
            availableUpgrades[randomIndex] = temp;
        }

        for (int i = 0; i < optionCards.Length; i++)
        {
            if (i < availableUpgrades.Count)
            {
                optionCards[i].gameObject.SetActive(true);
                optionCards[i].Setup(availableUpgrades[i], player);
            }
            else
            {
                optionCards[i].gameObject.SetActive(false);
            }
        }
    }
}