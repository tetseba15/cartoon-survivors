using UnityEngine;

public enum WeaponType { Aura, Ranged, Sword }

[CreateAssetMenu(fileName = "New Weapon Upgrade", menuName = "Bullet Heaven/Upgrades/Weapon Upgrade")]
public class WeaponUpgrade : UpgradeData
{
    [Header("Weapon Settings")]
    public WeaponType weaponType;
    public GameObject weaponPrefab;

    public override void ApplyUpgrade(GameObject player)
    {
        Transform weaponsContainer = player.transform.Find("Weapons");
        if (weaponsContainer != null)
        {
            Instantiate(weaponPrefab, weaponsContainer);
        }
    }

    public override bool CanBeChosen(GameObject player)
    {
        if (weaponType == WeaponType.Aura)
        {
            return player.GetComponentInChildren<AuraWeapon>() == null;
        }
        
        if (weaponType == WeaponType.Ranged)
        {
            return player.GetComponentInChildren<RangedWeapon>() == null;
        }

        if (weaponType == WeaponType.Sword)
        {
            return player.GetComponentInChildren<DirectionalMeleeWeapon>() == null;
        }

        return true;
    }
}