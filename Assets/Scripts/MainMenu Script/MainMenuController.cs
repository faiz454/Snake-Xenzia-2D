using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    public GameObject optionsPanel;
    public GameObject mainMenuButtons;
    public TextMeshProUGUI HighScore;

    void Start()
    {
        // 1. Hide options panel on start
        optionsPanel.SetActive(false);
        mainMenuButtons.SetActive(true);

        // 2. Load and display the High Score
        // "HighScore" is the key (name) we used to save the data
        int savedScore = PlayerPrefs.GetInt("HighScore", 0);
        HighScore.text = "High Score: " + savedScore.ToString();
    }
    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
        mainMenuButtons.SetActive(false);
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
        mainMenuButtons.SetActive(true);
    }

    public void SetGlobalVolume(float volume)
    {
        AudioListener.volume = volume;
    }
    public void PlayGame()
    {
        // Make sure this name matches your gameplay scene exactly!
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Pressed");
    }
}