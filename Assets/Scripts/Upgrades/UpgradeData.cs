using UnityEngine;

public abstract class UpgradeData : ScriptableObject
{
    public string upgradeName;
    [TextArea] public string description;
    public Sprite icon; 

    public abstract void ApplyUpgrade(GameObject player);

    public virtual bool CanBeChosen(GameObject player)
    {
        return true; 
    }
}