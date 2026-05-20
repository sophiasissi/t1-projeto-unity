using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverSceneManager : MonoBehaviour
{
    [Header("Textos da tela de derrota")]
    public TMP_Text finalScoreText;
    public TMP_Text finalCoffeeText;

    void Start()
    {
        if (finalScoreText != null)
        {
            finalScoreText.text = GameSession.lastRunScore.ToString("0000");
        }

        if (finalCoffeeText != null)
        {
            finalCoffeeText.text = GameSession.lastRunCoffee.ToString();
        }
    }

    public void RetryLevel()
    {
        if (!string.IsNullOrEmpty(GameSession.lastPlayedScene))
        {
            SceneManager.LoadScene(GameSession.lastPlayedScene);
        }
        else
        {
            SceneManager.LoadScene("TutorialScene");
        }
    }

    public void BackToMenu()
    {
        GameSession.Reset();
        SceneManager.LoadScene("MenuScene");
    }
}