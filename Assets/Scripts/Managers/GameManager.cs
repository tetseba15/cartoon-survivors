using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static CharacterData SelectedCharacter { get; set; }

    public bool IsGameOver { get; private set; }

    private InputSystem_Actions _inputActions;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _inputActions = new InputSystem_Actions();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        Player.OnPlayerDeath += HandlePlayerDeath;

        if (_inputActions != null)
        {
            _inputActions.UI.Enable();
            _inputActions.UI.Cancel.performed += OnPausePerformed;
        }
    }

    private void OnDisable()
    {
        Player.OnPlayerDeath -= HandlePlayerDeath;

        if (_inputActions != null)
        {
            _inputActions.UI.Cancel.performed -= OnPausePerformed;
            _inputActions.UI.Disable();
        }
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

    }

    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        if (IsGameOver) return;
        TogglePause();
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