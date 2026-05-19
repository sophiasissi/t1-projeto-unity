using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialMessages : MonoBehaviour
{
    [Header("Textos do tutorial")]
    public TMP_Text tutorialMessageText;
    public TMP_Text keyText;
    public TMP_Text keyArrowText;

    [Header("Botões visuais das teclas")]
    public GameObject keyButton;
    public GameObject keyArrowButton;

    [Header("UI do tutorial")]
    public GameObject tutorialUI;
    public GameObject tutorialMainPanel;

    [Header("Player")]
    public PlayerController playerController;

    [Header("Objetos planejados do tutorial")]
    public GameObject step1ObstacleLeft;
    public GameObject step2ObstacleRight;
    public GameObject step3ObstacleJump;
    public GameObject step4Coffee;

    [Header("Spawners normais do jogo")]
    public GameObject obstacleSpawner;
    public GameObject collectibleSpawner;

    [Header("Configuração")]
    public float timeAfterCorrectInput = 1.5f;
    public float timeBeforeNextScene = 2.0f;
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
        SaveStartPositions();
        StartTutorial();
    }

    void Update()
    {
        if (tutorialFinished)
        {
            return;
        }

        CheckInputForCurrentStep();
        CheckCoffeeStep();
    }

    void SaveStartPositions()
    {
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
        currentStep = 0;
        waiting = false;
        tutorialFinished = false;

        if (tutorialUI != null)
        {
            tutorialUI.SetActive(true);
        }

        if (tutorialMainPanel != null)
        {
            tutorialMainPanel.SetActive(true);
        }

        if (obstacleSpawner != null)
        {
            obstacleSpawner.SetActive(false);
        }

        if (collectibleSpawner != null)
        {
            collectibleSpawner.SetActive(false);
        }

        HideAllTutorialObjects();

        if (playerController != null)
        {
            playerController.tutorialMode = true;
        }

        ShowStep(0);
    }

    void HideAllTutorialObjects()
    {
        SetObjectActive(step1ObstacleLeft, false);
        SetObjectActive(step2ObstacleRight, false);
        SetObjectActive(step3ObstacleJump, false);
        SetObjectActive(step4Coffee, false);
    }

    void ShowStep(int step)
    {
        HideAllTutorialObjects();
        ShowKeyButtons(true);

        if (step == 0)
        {
            SetMessage(
                "PRESSIONE A OU ←\nPARA IR PARA A ESQUERDA",
                "A",
                "←"
            );

            ActivateTutorialObject(step1ObstacleLeft, step1StartPos);
            SetAllowedCommand(PlayerController.TutorialCommand.Left);
        }
        else if (step == 1)
        {
            SetMessage(
                "PRESSIONE D OU →\nPARA IR PARA A DIREITA",
                "D",
                "→"
            );

            ActivateTutorialObject(step2ObstacleRight, step2StartPos);
            SetAllowedCommand(PlayerController.TutorialCommand.Right);
        }
        else if (step == 2)
        {
            SetMessage(
                "PRESSIONE W, ESPAÇO OU ↑\nPARA PULAR",
                "W",
                "↑"
            );

            ActivateTutorialObject(step3ObstacleJump, step3StartPos);
            SetAllowedCommand(PlayerController.TutorialCommand.Jump);
        }
        else if (step == 3)
        {
            ShowKeyButtons(false);

            SetMessage(
                "COLETE CAFÉS\nPARA GANHAR PONTOS!",
                "",
                ""
            );

            ActivateTutorialObject(step4Coffee, step4StartPos);
            SetAllowedCommand(PlayerController.TutorialCommand.Any);
        }
        else if (step == 4)
        {
            ShowKeyButtons(false);

            SetMessage(
                "TUTORIAL CONCLUÍDO!\nPREPARE-SE PARA A FASE 2...",
                "",
                ""
            );

            HideAllTutorialObjects();
            SetAllowedCommand(PlayerController.TutorialCommand.Any);

            Invoke(nameof(GoToNextScene), timeBeforeNextScene);
        }
    }

    void ActivateTutorialObject(GameObject obj, Vector3 startPosition)
    {
        if (obj == null)
        {
            Debug.LogWarning("Objeto do tutorial não foi conectado no Inspector.");
            return;
        }

        obj.transform.position = startPosition;
        obj.SetActive(true);
    }

    void SetMessage(string message, string key, string arrow)
    {
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
        if (playerController != null)
        {
            playerController.SetTutorialCommand(command);
        }
    }

    void CheckInputForCurrentStep()
    {
        if (waiting)
        {
            return;
        }

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
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                CorrectInput();
            }
        }
    }

    void CheckCoffeeStep()
    {
        if (currentStep != 3 || waiting)
        {
            return;
        }

        if (step4Coffee == null || !step4Coffee.activeInHierarchy)
        {
            CorrectInput();
        }
    }

    void CorrectInput()
    {
        waiting = true;
        Invoke(nameof(NextStep), timeAfterCorrectInput);
    }

    void NextStep()
    {
        currentStep++;
        waiting = false;
        ShowStep(currentStep);
    }

    void GoToNextScene()
    {
        tutorialFinished = true;

        HideAllTutorialObjects();

        if (playerController != null)
        {
            playerController.DisableTutorialMode();
        }

        SceneManager.LoadScene(nextSceneName);
    }

    void SetObjectActive(GameObject obj, bool active)
    {
        if (obj != null)
        {
            obj.SetActive(active);
        }
    }
}