using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialMessages : MonoBehaviour
{
    [Header("Textos do tutorial")]
    // Textos das instruções e teclas
    public TMP_Text tutorialMessageText;
    public TMP_Text keyText;
    public TMP_Text keyArrowText;

    [Header("Botões visuais das teclas")]
    // Botões que aparecem junto das instruções
    public GameObject keyButton;
    public GameObject keyArrowButton;

    [Header("UI do tutorial")]
    // Painéis da interface do tutorial
    public GameObject tutorialUI;
    public GameObject tutorialMainPanel;

    [Header("Player")]
    // Referência ao controle do jogador
    public PlayerController playerController;

    [Header("Objetos planejados do tutorial")]
    // Objetos usados em cada etapa
    public GameObject step1ObstacleLeft;
    public GameObject step2ObstacleRight;
    public GameObject step3ObstacleJump;
    public GameObject step4Coffee;

    [Header("Spawners normais do jogo")]
    // Spawners desativados durante o tutorial
    public GameObject obstacleSpawner;
    public GameObject collectibleSpawner;

    [Header("Configuração")]
    // Tempo entre uma instrução e outra
    public float timeAfterCorrectInput = 1.5f;

    // Tempo antes de ir para a próxima cena
    public float timeBeforeNextScene = 2.0f;

    // Cena carregada depois do tutorial
    public string nextSceneName = "Level2Scene";

    private int currentStep = 0;
    private bool waiting = false;
    private bool tutorialFinished = false;

    private Vector3 step1StartPos;
    private Vector3 step2StartPos;
    private Vector3 step3StartPos;
    private Vector3 step4StartPos;

    void Start()
    {
        // Salva posições iniciais e inicia o tutorial
        SaveStartPositions();
        StartTutorial();
    }

    void Update()
    {
        // Para verificações se terminou
        if (tutorialFinished)
        {
            return;
        }

        // Verifica comandos e coleta do café
        CheckInputForCurrentStep();
        CheckCoffeeStep();
    }

    void SaveStartPositions()
    {
        // Guarda posição inicial de cada objeto
        if (step1ObstacleLeft != null)
        {
            step1StartPos = step1ObstacleLeft.transform.position;
        }

        if (step2ObstacleRight != null)
        {
            step2StartPos = step2ObstacleRight.transform.position;
        }

        if (step3ObstacleJump != null)
        {
            step3StartPos = step3ObstacleJump.transform.position;
        }

        if (step4Coffee != null)
        {
            step4StartPos = step4Coffee.transform.position;
        }
    }

    void StartTutorial()
    {
        // Reseta chamadas pendentes
        CancelInvoke();

        currentStep = 0;
        waiting = false;
        tutorialFinished = false;

        // Ativa UI do tutorial
        if (tutorialUI != null)
        {
            tutorialUI.SetActive(true);
        }

        if (tutorialMainPanel != null)
        {
            tutorialMainPanel.SetActive(true);
        }

        // Desativa spawners normais
        if (obstacleSpawner != null)
        {
            obstacleSpawner.SetActive(false);
        }

        if (collectibleSpawner != null)
        {
            collectibleSpawner.SetActive(false);
        }

        HideAllTutorialObjects();

        // Ativa modo tutorial no player
        if (playerController != null)
        {
            playerController.tutorialMode = true;
        }

        // Começa na primeira etapa
        ShowStep(0);
    }

    void HideAllTutorialObjects()
    {
        // Esconde todos os objetos do tutorial
        SetObjectActive(step1ObstacleLeft, false);
        SetObjectActive(step2ObstacleRight, false);
        SetObjectActive(step3ObstacleJump, false);
        SetObjectActive(step4Coffee, false);
    }

    void ShowStep(int step)
    {
        // Prepara a etapa atual
        HideAllTutorialObjects();
        ShowKeyButtons(true);

        if (step == 0)
        {
            // Etapa: mover para esquerda
            SetMessage(
                "DESVIE DOS OBSTÁCULOS!\nPRESSIONE A OU ← PARA IR PARA A ESQUERDA",
                "A",
                "←"
            );

            ActivateTutorialObject(step1ObstacleLeft, step1StartPos);
            SetAllowedCommand(PlayerController.TutorialCommand.Left);
        }
        else if (step == 1)
        {
            // Etapa: mover para direita
            SetMessage(
                "CONTINUE DESVIANDO!\nPRESSIONE D OU → PARA IR PARA A DIREITA",
                "D",
                "→"
            );

            ActivateTutorialObject(step2ObstacleRight, step2StartPos);
            SetAllowedCommand(PlayerController.TutorialCommand.Right);
        }
        else if (step == 2)
        {
            // Etapa: pular
            SetMessage(
                "PULE A CATRACA!\nPRESSIONE W, ESPAÇO OU ↑",
                "W",
                "↑"
            );

            ActivateTutorialObject(step3ObstacleJump, step3StartPos);
            SetAllowedCommand(PlayerController.TutorialCommand.Jump);
        }
        else if (step == 3)
        {
            // Etapa: coletar café
            ShowKeyButtons(false);

            SetMessage(
                "\nCOLETE CAFÉS\nPARA GANHAR PONTOS!",
                "",
                ""
            );

            ActivateTutorialObject(step4Coffee, step4StartPos);
            SetAllowedCommand(PlayerController.TutorialCommand.Any);
        }
        else if (step == 4)
        {
            // Final do tutorial
            ShowKeyButtons(false);

            SetMessage(
                "\nTUTORIAL CONCLUÍDO!\n\nPREPARE-SE PARA A FASE 2...",
                "",
                ""
            );

            HideAllTutorialObjects();
            SetAllowedCommand(PlayerController.TutorialCommand.Any);

            tutorialFinished = true;
            Invoke(nameof(GoToNextScene), timeBeforeNextScene);
        }
    }

    void ActivateTutorialObject(GameObject obj, Vector3 startPosition)
    {
        // Ativa objeto da etapa
        if (obj == null)
        {
            Debug.LogWarning("Objeto do tutorial não foi conectado no Inspector.");
            return;
        }

        obj.transform.position = startPosition;
        obj.SetActive(true);

        // Evita Game Over nos obstáculos do tutorial
        if (obj.CompareTag("Obstacle"))
        {
            Collider2D collider = obj.GetComponent<Collider2D>();

            if (collider != null)
            {
                collider.enabled = false;
            }
        }
    }

    void SetMessage(string message, string key, string arrow)
    {
        // Atualiza textos da instrução
        if (tutorialMessageText != null)
        {
            tutorialMessageText.text = message;
        }

        if (keyText != null)
        {
            keyText.text = key;
        }

        if (keyArrowText != null)
        {
            keyArrowText.text = arrow;
        }
    }

    void ShowKeyButtons(bool show)
    {
        // Mostra ou esconde botões das teclas
        if (keyButton != null)
        {
            keyButton.SetActive(show);
        }

        if (keyArrowButton != null)
        {
            keyArrowButton.SetActive(show);
        }
    }

    void SetAllowedCommand(PlayerController.TutorialCommand command)
    {
        // Define comando aceito no player
        if (playerController != null)
        {
            playerController.SetTutorialCommand(command);
        }
    }

    void CheckInputForCurrentStep()
    {
        // Evita múltiplos avanços
        if (waiting)
        {
            return;
        }

        // Confere tecla da etapa atual
        if (currentStep == 0)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                CorrectInput();
            }
        }
        else if (currentStep == 1)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                CorrectInput();
            }
        }
        else if (currentStep == 2)
        {
            if (Input.GetKeyDown(KeyCode.Space) ||
                Input.GetKeyDown(KeyCode.W) ||
                Input.GetKeyDown(KeyCode.UpArrow))
            {
                CorrectInput();
            }
        }
    }

    void CheckCoffeeStep()
    {
        // Só vale na etapa do café
        if (currentStep != 3 || waiting)
        {
            return;
        }

        // Avança quando o café some/coletado
        if (step4Coffee == null || !step4Coffee.activeInHierarchy)
        {
            CorrectInput();
        }
    }

    void CorrectInput()
    {
        // Aguarda antes de ir para próxima etapa
        waiting = true;
        Invoke(nameof(NextStep), timeAfterCorrectInput);
    }

    void NextStep()
    {
        // Avança o tutorial
        currentStep++;
        waiting = false;
        ShowStep(currentStep);
    }

    void GoToNextScene()
    {
        // Finaliza tutorial e troca de cena
        HideAllTutorialObjects();

        if (playerController != null)
        {
            playerController.DisableTutorialMode();
        }

        SceneManager.LoadScene(nextSceneName);
    }

    void SetObjectActive(GameObject obj, bool active)
    {
        // Ativa ou desativa objeto com segurança
        if (obj != null)
        {
            obj.SetActive(active);

            // Reativa collider ao esconder
            if (!active && obj.CompareTag("Obstacle"))
            {
                Collider2D collider = obj.GetComponent<Collider2D>();

                if (collider != null)
                {
                    collider.enabled = true;
                }
            }
        }
    }
}