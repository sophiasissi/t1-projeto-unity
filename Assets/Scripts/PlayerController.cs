using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum TutorialCommand
    {
        Any,
        Left,
        Right,
        Jump
    }

    [Header("Movimento por faixas")]
    public float[] lanePositions = { -2.2f, 0f, 2.2f };
    public int currentLane = 1;
    public float laneMoveSpeed = 10f;

    [Header("Pulo")]
    public float jumpForce = 8f;
    public int maxJumps = 2;

    [Tooltip("Tempo mínimo em que o player fica protegido contra obstáculo de pulo, como a catraca.")]
    public float jumpActionDuration = 1.4f;

    [Tooltip("Margem para considerar que o player voltou ao chão.")]
    public float groundTolerance = 0.25f;

    [Header("Modo tutorial")]
    public bool tutorialMode = false;
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
            return isJumpingAction || !isGrounded || jumpCount > 0;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        currentLane = Mathf.Clamp(currentLane, 0, lanePositions.Length - 1);

        Vector3 startPosition = transform.position;
        startPosition.x = lanePositions[currentLane];
        transform.position = startPosition;

        startY = transform.position.y;
        ResetJumps();
    }

    void Update()
    {
        if (GameManager.instance != null && !GameManager.instance.gameRunning)
        {
            return;
        }

        TryResetJumpsByPosition();

        HandleInput();
        MoveToLane();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (CanUseCommand(TutorialCommand.Left))
            {
                MoveLeft();
            }
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (CanUseCommand(TutorialCommand.Right))
            {
                MoveRight();
            }
        }

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
        if (!tutorialMode)
        {
            return true;
        }

        return allowedCommand == command || allowedCommand == TutorialCommand.Any;
    }

    void MoveLeft()
    {
        if (currentLane > 0)
        {
            currentLane--;
        }
    }

    void MoveRight()
    {
        if (currentLane < lanePositions.Length - 1)
        {
            currentLane++;
        }
    }

    void MoveToLane()
    {
        Vector3 targetPosition = transform.position;
        targetPosition.x = lanePositions[currentLane];

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            laneMoveSpeed * Time.deltaTime
        );
    }

    void Jump()
    {
        if (rb == null)
        {
            return;
        }

        if (jumpCount >= maxJumps)
        {
            return;
        }

        isGrounded = false;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        jumpCount++;

        if (jumpCoroutine != null)
        {
            StopCoroutine(jumpCoroutine);
        }

        jumpCoroutine = StartCoroutine(JumpActionRoutine());
    }

    IEnumerator JumpActionRoutine()
    {
        isJumpingAction = true;

        yield return new WaitForSeconds(jumpActionDuration);

        isJumpingAction = false;
        jumpCoroutine = null;
    }

    void TryResetJumpsByPosition()
    {
        if (rb == null)
        {
            return;
        }

        if (transform.position.y <= startY + groundTolerance && rb.linearVelocity.y <= 0.1f)
        {
            ResetJumps();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.name.Contains("Ground"))
        {
            ResetJumps();
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.name.Contains("Ground"))
        {
            ResetJumps();
        }
    }

    void ResetJumps()
    {
        jumpCount = 0;
        isGrounded = true;
    }

    public void SetTutorialCommand(TutorialCommand command)
    {
        tutorialMode = true;
        allowedCommand = command;
    }

    public void DisableTutorialMode()
    {
        tutorialMode = false;
        allowedCommand = TutorialCommand.Any;
    }
}