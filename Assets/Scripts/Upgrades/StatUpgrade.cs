using UnityEngine;

public enum StatType
{
    MoveSpeed,
    Damage,
    AreaRadius,
    ProjectileCount,
    AngleRadius
}

[CreateAssetMenu(fileName = "New Stat Upgrade", menuName = "Bullet Heaven/Upgrades/Stat Upgrade")]
public class StatUpgrade : UpgradeData
{
    public StatType statToUpgrade;
    public float amount;
    public float angle;

    public override void ApplyUpgrade(GameObject player)
    {
        switch (statToUpgrade)
        {
            case StatType.MoveSpeed:
                player.GetComponent<Player>()?.AddSpeed(amount);
                break;

            case StatType.Damage:
                // cycles trough all weapons
                PlayerWeapon[] weapons = player.GetComponentsInChildren<PlayerWeapon>();
                foreach (var weapon in weapons)
                {
                    weapon.AddDamage(amount); 
                }
                break;

            case StatType.AreaRadius:
                AuraWeapon aura = player.GetComponentInChildren<AuraWeapon>();
                aura?.AddArea(amount); 
                break;

            case StatType.ProjectileCount:
                RangedWeapon ranged = player.GetComponentInChildren<RangedWeapon>();
                ranged?.AddProjectile(Mathf.RoundToInt(amount), angle); 
                break;

            case StatType.AngleRadius:
                DirectionalMeleeWeapon directional = player.GetComponentInChildren<DirectionalMeleeWeapon>();
                directional?.AddRadius(amount, angle);
                break;
        }
    }

    public override bool CanBeChosen(GameObject player)
    {
        if (player == null) return true;

        switch (statToUpgrade)
        {
            case StatType.AreaRadius:
                bool hasAura = player.GetComponentInChildren<AuraWeapon>(true) != null;
                //bool hasDirectional = player.GetComponentInChildren<DirectionalMeleeWeapon>(true) != null;

                return hasAura /*|| hasDirectional*/;

            case StatType.ProjectileCount:
                bool hasRanged = player.GetComponentInChildren<RangedWeapon>(true) != null;
                return hasRanged;

            case StatType.AngleRadius:
                bool hasMelee = player.GetComponentInChildren<DirectionalMeleeWeapon>(true) != null;
                return hasMelee;

                //Global upgrades
            case StatType.MoveSpeed:
            case StatType.Damage:
            default:
                return true;
        }
    }
}