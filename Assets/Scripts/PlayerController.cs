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
    public float jumpForce = 7f;

    [Tooltip("Tempo em que o player fica protegido contra obstáculo de pulo, como a catraca.")]
    public float jumpActionDuration = 1.4f;

    [Header("Modo tutorial")]
    public bool tutorialMode = false;
    public TutorialCommand allowedCommand = TutorialCommand.Any;

    private Rigidbody2D rb;
    private bool isJumpingAction = false;
    private Coroutine jumpCoroutine;

    public bool IsJumping
    {
        get { return isJumpingAction; }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        currentLane = Mathf.Clamp(currentLane, 0, lanePositions.Length - 1);

        Vector3 startPosition = transform.position;
        startPosition.x = lanePositions[currentLane];
        transform.position = startPosition;
    }

    void Update()
    {
        if (GameManager.instance != null && !GameManager.instance.gameRunning)
        {
            return;
        }

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

        if (isJumpingAction)
        {
            return;
        }

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

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