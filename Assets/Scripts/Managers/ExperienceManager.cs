using UnityEngine;
using System;

public class ExperienceManager : MonoBehaviour
{
    public static ExperienceManager Instance { get; private set; }

    public int CurrentLevel { get; private set; } = 1;
    public float CurrentExp { get; private set; } = 0f;

    [Header("Progression Curve")]
    // How much exp per level
    [SerializeField] private float[] expToNextLevel = { 100f, 250f, 600f, 1200f, 2500f };

    public event Action<float, float> OnExperienceChanged; // Current exp, target exp
    public event Action<int> OnLevelUp;                    // New level

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddExperience(float amount)
    {
        CurrentExp += amount;

        float targetExp = GetExpTargetForCurrentLevel();

        // In case the player gets too much exp
        while (CurrentExp >= targetExp && CurrentLevel <= expToNextLevel.Length)
        {
            CurrentExp -= targetExp; // Save the rest for the next level
            CurrentLevel++;

            OnLevelUp?.Invoke(CurrentLevel);

            targetExp = GetExpTargetForCurrentLevel();
        }

        // Always notify exp to UI
        OnExperienceChanged?.Invoke(CurrentExp, targetExp);
    }

    private float GetExpTargetForCurrentLevel()
    {
        if (CurrentLevel <= expToNextLevel.Length)
        {
            return expToNextLevel[CurrentLevel - 1];
        }
        return expToNextLevel[expToNextLevel.Length - 1];
    }
}
