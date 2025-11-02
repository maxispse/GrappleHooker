using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    public GameObject pauseMenuUI;  // Assign your PauseMenu Panel here
    private bool isPaused = false;

    // Called when the Pause button is clicked
    public void OnPauseButton()
    {
        Pause();
    }

    // Called when the Resume button is clicked
    public void OnResumeButton()
    {
        Resume();
    }

    public void OnRestartButton()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void OnMainMenuButton() 
    {
        SceneManager.LoadScene("SampleScene");
    }
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;   // freezes game time
        isPaused = true;
    }

    void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;   // resumes game time
        isPaused = false;
    }
}
