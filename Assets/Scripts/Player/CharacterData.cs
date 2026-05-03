using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Bullet Heaven/Character Data")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public Sprite characterSprite; 
    public GameObject startingWeaponPrefab; 

    [Header("Base Stats")]
    public float baseHealth = 100f;
    public float baseSpeed = 5f;
}