using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Instância global para outros scripts acessarem o GameManager
    public static GameManager instance;

    [Header("Controle")]
    // Indica se o jogo ainda está rodando
    public bool gameRunning = true;

    [Header("HUD")]
    // Textos exibidos na tela
    public TMP_Text scoreText;
    public TMP_Text coffeeText;
    public TMP_Text tutorialText;
    public TMP_Text phaseText;
    public TMP_Text levelCompleteText;

    [Header("Configuração da fase")]
    // Nome exibido da fase atual
    public string phaseName = "Fase 1 - Tutorial";

    // Próxima cena a ser carregada
    public string nextSceneName = "Level2Scene";

    // Quantidade de cafés necessária para finalizar a fase
    public int coffeesToFinishLevel = 5;

    // Tempo antes de trocar de cena
    public float delayBeforeNextScene = 1f;

    [Header("Pontuação")]
    // Pontos ganhos por café coletado
    public int pointsPerCoffee = 100;

    private int score = 0;
    private int coffeeCount = 0;
    private bool levelFinished = false;

    void Awake()
    {
        // Salva a referência deste GameManager
        instance = this;
    }

    void Start()
    {
        // Garante que o jogo começa em tempo normal
        Time.timeScale = 1f;

        // Inicializa controles da fase
        gameRunning = true;
        levelFinished = false;

        // Mostra o nome da fase na HUD
        if (phaseText != null)
        {
            phaseText.text = phaseName;
        }

        // Esconde a mensagem de fase concluída no início
        if (levelCompleteText != null)
        {
            levelCompleteText.gameObject.SetActive(false);
        }

        // Atualiza pontuação e cafés na tela
        UpdateUI();
    }

    public void AddCoffee()
    {
        // Impede coleta depois do fim da fase
        if (!gameRunning || levelFinished)
        {
            return;
        }

        // Soma café e pontuação
        coffeeCount++;
        score += pointsPerCoffee;

        UpdateUI();

        // Finaliza a fase se atingiu a meta
        if (coffeeCount >= coffeesToFinishLevel)
        {
            FinishLevel();
        }
    }

    public void AddScore(int points)
    {
        // Impede pontuação depois do fim da fase
        if (!gameRunning || levelFinished)
        {
            return;
        }

        // Soma pontos extras
        score += points;
        UpdateUI();
    }

    public int GetScore()
    {
        // Retorna a pontuação atual
        return score;
    }

    public int GetCoffeeCount()
    {
        // Retorna os cafés coletados
        return coffeeCount;
    }

    void UpdateUI()
    {
        // Atualiza o texto da pontuação
        if (scoreText != null)
        {
            scoreText.text = "PONTUAÇÃO " + score.ToString("0000");
        }

        // Atualiza o texto dos cafés
        if (coffeeText != null)
        {
            coffeeText.text = "CAFÉS COLETADOS  " + coffeeCount + "/" + coffeesToFinishLevel;
        }
    }

    public void SetTutorialText(string message)
    {
        // Exibe mensagem do tutorial
        if (tutorialText != null)
        {
            tutorialText.text = message;
        }
    }

    public void ClearTutorialText()
    {
        // Limpa mensagem do tutorial
        if (tutorialText != null)
        {
            tutorialText.text = "";
        }
    }

    public void GameOver()
    {
        // Evita chamar Game Over mais de uma vez
        if (!gameRunning || levelFinished)
        {
            return;
        }

        // Para a lógica da fase
        gameRunning = false;
        levelFinished = true;
        Time.timeScale = 1f;

        // Salva os dados antes de ir para a tela de derrota
        GameSession.SaveGameOverState(
            score,
            coffeeCount,
            SceneManager.GetActiveScene().name
        );

        // Carrega a cena de Game Over
        SceneManager.LoadScene("GameOverScene");
    }

    public void FinishLevel()
    {
        // Evita finalizar a fase mais de uma vez
        if (levelFinished)
        {
            return;
        }

        // Marca a fase como encerrada
        levelFinished = true;
        gameRunning = false;

        string currentSceneName = SceneManager.GetActiveScene().name;

        // Se for a última fase, salva estado de vitória
        if (nextSceneName == "WinScene")
        {
            GameSession.SaveWinState(score, coffeeCount, currentSceneName);
        }
        else
        {
            // Caso contrário, soma o resultado da fase ao total
            GameSession.AddLevelResult(score, coffeeCount);
        }

        // Aguarda um tempo antes de trocar de cena
        StartCoroutine(LoadNextSceneAfterDelay());
    }

    IEnumerator LoadNextSceneAfterDelay()
    {
        // Mostra mensagem de conclusão
        if (levelCompleteText != null)
        {
            levelCompleteText.gameObject.SetActive(true);
            levelCompleteText.text = "Fase concluída!";
        }

        // Espera antes de trocar de cena
        yield return new WaitForSeconds(delayBeforeNextScene);

        // Carrega a próxima cena configurada
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    public void RestartCurrentScene()
    {
        // Reinicia a cena atual
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}