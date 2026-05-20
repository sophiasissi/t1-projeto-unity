using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WinSceneManager : MonoBehaviour
{
    [Header("Textos da tela de vitória")]
    public TMP_Text finalScoreText;
    public TMP_Text finalCoffeeText;

    void Start()
    {
        if (finalScoreText != null)
        {
            finalScoreText.text = GameSession.totalScore.ToString("0000");
        }

        if (finalCoffeeText != null)
        {
            finalCoffeeText.text = GameSession.totalCoffee.ToString();
        }
    }

    public void BackToMenu()
    {
        GameSession.Reset();
        SceneManager.LoadScene("MenuScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}