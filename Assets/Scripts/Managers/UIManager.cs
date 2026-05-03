using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Screens")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject levelUpScreen;
    [SerializeField] private GameObject victoryScreen;

    [Header("Timer")]
    [SerializeField] private TMP_Text timeText;

    [SerializeField] private AudioClip LevelUpSFX;
    

    private Stack<GameObject> menuStack = new Stack<GameObject>();

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

        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
        levelUpScreen.SetActive(false);
    }

    private void Start()
    {
        ExperienceManager.Instance.OnLevelUp += HandleLevelUp;

        if (victoryScreen != null) victoryScreen.SetActive(false);
    }

    private void Update()
    {
        timeText.text = string.Format("{0:00}:{1:00}", TimeManager.Instance.Minutes, TimeManager.Instance.Seconds);       
    }

    private void OnDestroy()
    {
        if (ExperienceManager.Instance != null)
        {
            ExperienceManager.Instance.OnLevelUp -= HandleLevelUp;
        }
    }

    public void ShowVictoryScreen()
    {
        OpenMenu(victoryScreen);
    }

    public void OpenMenu(GameObject menuPrefab)
    {
        menuPrefab.SetActive(true);
        menuStack.Push(menuPrefab);
    }

    public void CloseCurrentMenu()
    {
        if (menuStack.Count > 0)
        {
            GameObject current = menuStack.Pop();
            current.SetActive(false);
        }

        if (menuStack.Count == 0)
        {
            Time.timeScale = 1f;
        }
    }

    public void ShowGameOverScreen()
    {
        OpenMenu(gameOverScreen);
    }

    public void ShowLevelUpScreen()
    {
        AudioManager.Instance.PlaySound(LevelUpSFX, 0.5f);
        Time.timeScale = 0f;
        OpenMenu(levelUpScreen);
    }

    public void ShowPauseScreen()
    {
        OpenMenu(pauseScreen);
    }

    private void HandleLevelUp(int newLevel)
    {
        ShowLevelUpScreen();
    }

    public bool IsPauseScreenOnTop()
    {
        return menuStack.Count > 0 && menuStack.Peek() == pauseScreen;
    }
}