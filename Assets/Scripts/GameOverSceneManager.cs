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

        Debug.Log("Resultado final - Pontos: " + GameSession.gameOverScore + " | Cafés: " + GameSession.gameOverCoffee);
    }

    public void TryAgain()
    {
        Time.timeScale = 1f;
        GameSession.Reset();

        // Volta para a primeira fase jogável
        SceneManager.LoadScene("Level2Scene");
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        GameSession.Reset();
        SceneManager.LoadScene("MenuScene");
    }
}