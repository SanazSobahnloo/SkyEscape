using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum State { MainMenu, Playing, GameOver }
    public State CurrentState { get; private set; } = State.MainMenu;

    [Header("References (set in Inspector)")]
    public GameObject mainMenuCanvas;   // MainMenuCanvas
    public GameObject gameCanvas;       // GameCanvas (HUD)
    public GameObject gameOverCanvas;   // GameOverCanvas
    public CameraBehaviour cameraBehaviour; // reference to existing CameraBehaviour

    public Action OnGameStart;
    public Action OnGameOver;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Initialize to MainMenu
        SetState(State.MainMenu);
    }

    public void SetState(State newState)
    {
        CurrentState = newState;

        // UI handling
        if (mainMenuCanvas) mainMenuCanvas.SetActive(newState == State.MainMenu);
        if (gameCanvas) gameCanvas.SetActive(newState == State.Playing);
        if (gameOverCanvas) gameOverCanvas.SetActive(newState == State.GameOver);

        // Notify
        if (newState == State.Playing)
        {
            OnGameStart?.Invoke();
            // ensure camera/missile spawner runs
            if (cameraBehaviour != null) cameraBehaviour.gameOver = false;
        }
        else if (newState == State.GameOver)
        {
            OnGameOver?.Invoke();
            if (cameraBehaviour != null) cameraBehaviour.gameOver = true;
        }
    }

    // Called by Start button
    public void StartGame()
    {
        // If your existing CameraBehaviour's startAnimation hides the menu, ensure it's consistent
        SetState(State.Playing);
    }

    // Called by Restart button in GameOver
    public void RestartGame()
    {
        // simple approach: reload Main scene to ensure clean state
        // or alternatively reset necessary subsystems; here we reload
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Called by Back to Menu button
    public void BackToMenu()
    {
        // If you prefer to reload scene: SceneManager.LoadScene(...)
        SetState(State.MainMenu);
    }

    // Centralized call to set GameOver (can be invoked by PlaneBehaviour)
    public void HandlePlayerDeath()
    {
        // freeze gameplay-related things via CameraBehaviour (spawner) and set state
        SetState(State.GameOver);
    }
}
