using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Controle")]
    public bool gameRunning = true;

    [Header("HUD")]
    public TMP_Text scoreText;
    public TMP_Text coffeeText;
    public TMP_Text tutorialText;
    public TMP_Text phaseText;

    [Header("Configuração da fase")]
    public string phaseName = "Fase 1 - Tutorial";
    public string nextSceneName = "Level2Scene";
    public int coffeesToFinishLevel = 5;

    private int score = 0;
    private int coffeeCount = 0;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Time.timeScale = 1f;
        gameRunning = true;

        UpdateUI();

        if (phaseText != null)
        {
            phaseText.text = phaseName;
        }
    }

    public void AddCoffee()
    {
        coffeeCount++;
        score += 100;

        UpdateUI();

        if (coffeeCount >= coffeesToFinishLevel)
        {
            FinishLevel();
        }
    }

    public void AddScore(int points)
    {
        score += points;
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
            coffeeText.text = "Cafés: " + coffeeCount + "/" + coffeesToFinishLevel;
        }
    }

    public void SetTutorialText(string message)
    {
        if (tutorialText != null)
        {
            tutorialText.text = message;
        }
    }

    public void GameOver()
    {
        gameRunning = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameOverScene");
    }

    public void FinishLevel()
    {
        gameRunning = false;
        Time.timeScale = 1f;

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}