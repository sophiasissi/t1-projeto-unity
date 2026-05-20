using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void StartGame()
    {
        GameSession.Reset();
        SceneManager.LoadScene("TutorialScene");
    }

    public void TryAgain()
    {
        Time.timeScale = 1f;
        GameSession.Reset();
        SceneManager.LoadScene("Level2Scene");
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        GameSession.Reset();
        SceneManager.LoadScene("MenuScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}