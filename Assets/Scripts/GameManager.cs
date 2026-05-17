using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool gameRunning = true;

    public TMP_Text scoreText;
    public TMP_Text coffeeText;
    public TMP_Text tutorialText;

    public GameObject gameOverPanel;

    private int score = 0;
    private int coffeeCount = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        Time.timeScale = 1;
        gameRunning = true;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        UpdateUI();

        if (tutorialText != null)
        {
            tutorialText.text = "Pressione ESPAÇO para pular!";
        }
    }

    void Update()
    {
        if (!gameRunning)
        {
            return;
        }

        score += Mathf.RoundToInt(Time.deltaTime * 5);
        UpdateUI();
    }

    public void AddScore(int points)
    {
        score += points;
        coffeeCount++;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Pontos: " + score.ToString("0000");
        }

        if (coffeeText != null)
        {
            coffeeText.text = "Cafés: " + coffeeCount;
        }
    }

    public void GameOver()
    {
        gameRunning = false;
        Time.timeScale = 0;

        if (tutorialText != null)
        {
            tutorialText.text = "Você se atrasou para a aula!";
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}