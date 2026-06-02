using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Comandos usados no tutorial
    public enum TutorialCommand
    {
        Any,
        Left,
        Right,
        Jump
    }

    [Header("Movimento por faixas")]
    // Posições das três faixas
    public float[] lanePositions = { -2.2f, 0f, 2.2f };

    // Faixa atual do player
    public int currentLane = 1;

    // Velocidade de troca entre faixas
    public float laneMoveSpeed = 10f;

    [Header("Pulo")]
    // Força aplicada no pulo
    public float jumpForce = 8f;

    // Quantidade máxima de pulos
    public int maxJumps = 2;

    [Tooltip("Tempo mínimo em que o player fica protegido contra obstáculo de pulo, como a catraca.")]
    // Tempo em que o pulo conta como ação válida
    public float jumpActionDuration = 1.4f;

    [Tooltip("Margem para considerar que o player voltou ao chão.")]
    // Margem para detectar o chão
    public float groundTolerance = 0.25f;

    [Header("Modo tutorial")]
    // Ativa restrição de comandos no tutorial
    public bool tutorialMode = false;

    // Comando permitido no momento do tutorial
    public TutorialCommand allowedCommand = TutorialCommand.Any;

    private Rigidbody2D rb;

    private int jumpCount = 0;
    private bool isGrounded = true;
    private bool isJumpingAction = false;

    private float startY;
    private Coroutine jumpCoroutine;

    public bool IsJumping
    {
        get
        {
            // Indica se o player está pulando
            return isJumpingAction || !isGrounded || jumpCount > 0;
        }
    }

    void Start()
    {
        // Pega o Rigidbody do player
        rb = GetComponent<Rigidbody2D>();

        // Garante que a faixa inicial é válida
        currentLane = Mathf.Clamp(currentLane, 0, lanePositions.Length - 1);

        // Posiciona o player na faixa inicial
        Vector3 startPosition = transform.position;
        startPosition.x = lanePositions[currentLane];
        transform.position = startPosition;

        // Salva a altura inicial do chão
        startY = transform.position.y;

        // Libera os pulos no início
        ResetJumps();
    }

    void Update()
    {
        // Para o controle se o jogo terminou
        if (GameManager.instance != null && !GameManager.instance.gameRunning)
        {
            return;
        }

        // Verifica se voltou ao chão
        TryResetJumpsByPosition();

        // Lê comandos e move o player
        HandleInput();
        MoveToLane();
    }

    void HandleInput()
    {
        // Movimento para esquerda
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (CanUseCommand(TutorialCommand.Left))
            {
                MoveLeft();
            }
        }

        // Movimento para direita
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (CanUseCommand(TutorialCommand.Right))
            {
                MoveRight();
            }
        }

        // Comando de pulo
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (CanUseCommand(TutorialCommand.Jump))
            {
                Jump();
            }
        }
    }

    bool CanUseCommand(TutorialCommand command)
    {
        // Fora do tutorial, qualquer comando é permitido
        if (!tutorialMode)
        {
            return true;
        }

        // No tutorial, só aceita o comando liberado
        return allowedCommand == command || allowedCommand == TutorialCommand.Any;
    }

    void MoveLeft()
    {
        // Vai uma faixa para a esquerda
        if (currentLane > 0)
        {
            currentLane--;
        }
    }

    void MoveRight()
    {
        // Vai uma faixa para a direita
        if (currentLane < lanePositions.Length - 1)
        {
            currentLane++;
        }
    }

    void MoveToLane()
    {
        // Define a posição alvo da faixa atual
        Vector3 targetPosition = transform.position;
        targetPosition.x = lanePositions[currentLane];

        // Move suavemente até a faixa
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            laneMoveSpeed * Time.deltaTime
        );
    }

    void Jump()
    {
        // Cancela se não tiver Rigidbody
        if (rb == null)
        {
            return;
        }

        // Impede passar do limite de pulos
        if (jumpCount >= maxJumps)
        {
            return;
        }

        isGrounded = false;

        // Zera a velocidade vertical antes do impulso
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

        // Aplica o impulso do pulo
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        jumpCount++;

        // Reinicia a rotina do pulo
        if (jumpCoroutine != null)
        {
            StopCoroutine(jumpCoroutine);
        }

        jumpCoroutine = StartCoroutine(JumpActionRoutine());
    }

    IEnumerator JumpActionRoutine()
    {
        // Marca o pulo como ação válida
        isJumpingAction = true;

        yield return new WaitForSeconds(jumpActionDuration);

        // Encerra a ação de pulo
        isJumpingAction = false;
        jumpCoroutine = null;
    }

    void TryResetJumpsByPosition()
    {
        if (rb == null)
        {
            return;
        }

        // Reseta os pulos quando volta perto da altura inicial
        if (transform.position.y <= startY + groundTolerance && rb.linearVelocity.y <= 0.1f)
        {
            ResetJumps();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Reseta pulo ao encostar no chão
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.name.Contains("Ground"))
        {
            ResetJumps();
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        // Mantém pulo resetado enquanto está no chão
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.name.Contains("Ground"))
        {
            ResetJumps();
        }
    }

    void ResetJumps()
    {
        // Libera os pulos novamente
        jumpCount = 0;
        isGrounded = true;
    }

    public void SetTutorialCommand(TutorialCommand command)
    {
        // Ativa o modo tutorial com comando específico
        tutorialMode = true;
        allowedCommand = command;
    }

    public void DisableTutorialMode()
    {
        // Desativa as restrições do tutorial
        tutorialMode = false;
        allowedCommand = TutorialCommand.Any;
    }
}