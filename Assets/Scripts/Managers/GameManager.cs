using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static CharacterData SelectedCharacter { get; set; }

    public bool IsGameOver { get; private set; }

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

    private void OnEnable()
    {
        Player.OnPlayerDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        Player.OnPlayerDeath -= HandlePlayerDeath;
    }

    private void Start()
    {
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.OnTimeUp += HandleVictory;
        }
    }

    private void OnDestroy()
    {
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.OnTimeUp -= HandleVictory;
        }
    }

    private void Update()
    {
        if (IsGameOver) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
    private void HandleVictory()
    {
        if (IsGameOver) return; 

        IsGameOver = true;
        Time.timeScale = 0f; 

        UIManager.Instance.ShowVictoryScreen();
    }

    public void TogglePause()
    {
        if (Time.timeScale > 0f)
        {
            Time.timeScale = 0f;
            UIManager.Instance.ShowPauseScreen();
        }
        else
        {
            if (UIManager.Instance.IsPauseScreenOnTop())
            {
                UIManager.Instance.CloseCurrentMenu();
            }
        }
    }

    private void HandlePlayerDeath()
    {
        if (IsGameOver) return;

        IsGameOver = true;
        Time.timeScale = 0f;

        UIManager.Instance.ShowGameOverScreen();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}