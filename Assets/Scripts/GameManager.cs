using System.Collections;
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
    public TMP_Text levelCompleteText;

    [Header("Configuração da fase")]
    public string phaseName = "Fase 1 - Tutorial";
    public string nextSceneName = "Level2Scene";
    public int coffeesToFinishLevel = 5;
    public float delayBeforeNextScene = 1f;

    [Header("Pontuação")]
    public int pointsPerCoffee = 100;

    private int score = 0;
    private int coffeeCount = 0;
    private bool levelFinished = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Time.timeScale = 1f;
        gameRunning = true;
        levelFinished = false;

        if (phaseText != null)
        {
            phaseText.text = phaseName;
        }

        if (levelCompleteText != null)
        {
            levelCompleteText.gameObject.SetActive(false);
        }

        UpdateUI();
    }

    public void AddCoffee()
    {
        if (!gameRunning || levelFinished)
        {
            return;
        }

        coffeeCount++;
        score += pointsPerCoffee;

        UpdateUI();

        if (coffeeCount >= coffeesToFinishLevel)
        {
            FinishLevel();
        }
    }

    public void AddScore(int points)
    {
        if (!gameRunning || levelFinished)
        {
            return;
        }

        score += points;
        UpdateUI();
    }

    public int GetScore()
    {
        return score;
    }

    public int GetCoffeeCount()
    {
        return coffeeCount;
    }

    void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "PONTUAÇÃO " + score.ToString("0000");
        }

        if (coffeeText != null)
        {
            coffeeText.text = "CAFÉS COLETADOS  " + coffeeCount + "/" + coffeesToFinishLevel;
        }
    }

    public void SetTutorialText(string message)
    {
        if (tutorialText != null)
        {
            tutorialText.text = message;
        }
    }

    public void ClearTutorialText()
    {
        if (tutorialText != null)
        {
            tutorialText.text = "";
        }
    }

    public void GameOver()
    {
        if (!gameRunning || levelFinished)
        {
            return;
        }

        gameRunning = false;
        levelFinished = true;
        Time.timeScale = 1f;

        GameSession.SaveGameOverState(
            score,
            coffeeCount,
            SceneManager.GetActiveScene().name
        );

        SceneManager.LoadScene("GameOverScene");
    }

    public void FinishLevel()
    {
        if (levelFinished)
        {
            return;
        }

        levelFinished = true;
        gameRunning = false;

        GameSession.AddLevelResult(score, coffeeCount);

        StartCoroutine(LoadNextSceneAfterDelay());
    }

    IEnumerator LoadNextSceneAfterDelay()
    {
        if (levelCompleteText != null)
        {
            levelCompleteText.gameObject.SetActive(true);
            levelCompleteText.text = "Fase concluída!";
        }

        yield return new WaitForSeconds(delayBeforeNextScene);

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    public void RestartCurrentScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}