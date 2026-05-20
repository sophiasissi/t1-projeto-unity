using TMPro;
using UnityEngine;

public class ResultScreen : MonoBehaviour
{
    [Header("Textos da tela final")]
    public TMP_Text scoreText;
    public TMP_Text coffeeText;

    void Start()
    {
        if (scoreText != null)
        {
            scoreText.text = GameSession.gameOverScore.ToString("0000");
            scoreText.transform.SetAsLastSibling();
        }
        else
        {
            Debug.LogWarning("Score Text não foi atribuído no ResultScreen.");
        }

        if (coffeeText != null)
        {
            coffeeText.text = GameSession.gameOverCoffee.ToString();
            coffeeText.transform.SetAsLastSibling();
        }
        else
        {
            Debug.LogWarning("Coffee Text não foi atribuído no ResultScreen.");
        }

        Debug.Log("Resultado final - Pontos: " + GameSession.gameOverScore + " | Cafés: " + GameSession.gameOverCoffee);
    }
}