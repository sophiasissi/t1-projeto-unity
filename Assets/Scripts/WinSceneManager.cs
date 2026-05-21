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
        Time.timeScale = 1f;

        if (finalScoreText != null)
        {
            finalScoreText.text = GameSession.gameOverScore.ToString("0000");
            finalScoreText.transform.SetAsLastSibling();
        }

        if (finalCoffeeText != null)
        {
            finalCoffeeText.text = GameSession.gameOverCoffee.ToString();
            finalCoffeeText.transform.SetAsLastSibling();
        }

        Debug.Log("Tela de vitória - Pontos: " + GameSession.gameOverScore + " | Cafés: " + GameSession.gameOverCoffee);
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