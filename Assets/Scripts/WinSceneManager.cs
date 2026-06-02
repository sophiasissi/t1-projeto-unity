using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WinSceneManager : MonoBehaviour
{
    [Header("Textos da tela de vitória")]
    // Textos que mostram o resultado final
    public TMP_Text finalScoreText;
    public TMP_Text finalCoffeeText;

    void Start()
    {
        // Garante que o tempo está normal na tela de vitória
        Time.timeScale = 1f;

        // Mostra a pontuação final salva no GameSession
        if (finalScoreText != null)
        {
            finalScoreText.text = GameSession.gameOverScore.ToString("0000");
            finalScoreText.transform.SetAsLastSibling();
        }

        // Mostra a quantidade final de cafés salva no GameSession
        if (finalCoffeeText != null)
        {
            finalCoffeeText.text = GameSession.gameOverCoffee.ToString();
            finalCoffeeText.transform.SetAsLastSibling();
        }

        // Log para conferir os valores no console
        Debug.Log("Tela de vitória - Pontos: " + GameSession.gameOverScore + " | Cafés: " + GameSession.gameOverCoffee);
    }

    public void BackToMenu()
    {
        // Zera os dados e volta para o menu
        Time.timeScale = 1f;
        GameSession.Reset();
        SceneManager.LoadScene("MenuScene");
    }

    public void QuitGame()
    {
        // Fecha o jogo
        Application.Quit();
    }
}