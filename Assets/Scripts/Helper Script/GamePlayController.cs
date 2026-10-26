using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; // Add this namespace for TextMeshPro

public class GamePlayController : MonoBehaviour
{
    public static GamePlayController instance;
    public GameObject fruit_Pick, bomb_pick;
    private float min_X = -4.3f, max_X = 4.288f, min_Y = -2.56f, max_Y = 2.23f;
    private float z_Pos = 5.7f;

    // Changed from Text to TextMeshProUGUI
    [SerializeField]
    private TextMeshProUGUI score_Text;

    private int scoreCount;

    public GameObject gameOverPanel; // Drag your UI Panel here in Inspector
    private bool isGameOver;

    [SerializeField] private GameObject pause_Panel; // Assign your new Pause Panel here
    private bool is_Paused;

    public void PauseGame()
    {
        is_Paused = true;
        pause_Panel.SetActive(true); // Shows the menu
        Time.timeScale = 0f;        // Freezes the snake and spawning
    }

    public void ResumeGame()
    {
        is_Paused = false;
        pause_Panel.SetActive(false); // Hides the menu
        Time.timeScale = 1f;         // Unfreezes the game
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // IMPORTANT: Unfreeze time so the menu works!
        SceneManager.LoadScene("Main Menu"); // Use the exact name of your menu scene
    }
    public void QuitGame()
    {
        Application.Quit(); // Closes the app (only in the final build)
        Debug.Log("Player Quit the Game");
    }

    public void GameOver()
    {
        int CurrentScore = scoreCount;
        int oldHighScore = PlayerPrefs.GetInt("HighScore", 0);

        if(CurrentScore > oldHighScore)
        {
            PlayerPrefs.SetInt("HighScore", CurrentScore);
            PlayerPrefs.Save();
        }
        if (isGameOver) return;

        isGameOver = true;
        CancelSpawning();

        // Show the UI
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        // Optional: Stop all movement
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
    void Awake()
    {
        MakeInstance();
    }

    private void Start()
    {
        Invoke("StartSpawning", 0.5f);
    }

    void Update()
    {
        // Use the new Input System to check for the Escape key
        var keyboard = UnityEngine.InputSystem.Keyboard.current;
        
        if (keyboard != null && keyboard.escapeKey.wasPressedThisFrame)
        {
            if (is_Paused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void StartSpawning()
    {
        StartCoroutine(SpawnPicks());
    }

    public void CancelSpawning()
    {
        CancelInvoke("StartSpawning");
    }

    IEnumerator SpawnPicks()
    {
        yield return new WaitForSeconds(Random.Range(1f, 1.5f));

        if (Random.Range(0, 10) >= 2)
        {
            Instantiate(fruit_Pick, new Vector3(Random.Range(min_X, max_X), Random.Range(min_Y, max_Y), z_Pos), Quaternion.identity);

        }
        else
        {
            Instantiate(bomb_pick, new Vector3(Random.Range(min_X, max_X), Random.Range(min_Y, max_Y), z_Pos), Quaternion.identity);
        }

        Invoke("StartSpawning", 0f);
    }

    public void IncreaseScore()
    {
        scoreCount++;
        score_Text.text = "Score: " + scoreCount;
    }

    public void DecreaseScore()
    {
        scoreCount--;

        // Optional: Prevent score from going below 0
        if (scoreCount < 0)
        {
            scoreCount = 0;
        }

        score_Text.text = "Score: " + scoreCount;
    }
}