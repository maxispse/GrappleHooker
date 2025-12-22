using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuUI : MonoBehaviour
{
    public string gameSceneName = "Game"; 

    public void OnPlayButton()
    {
        SceneManager.LoadScene(gameSceneName);
        Time.timeScale = 1f;   // resumes game time
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}
