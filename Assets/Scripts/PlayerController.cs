using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimento por faixas")]
    public float[] lanes = { -2f, 0f, 2f };
    public int currentLane = 1;
    public float laneChangeSpeed = 10f;

    [Header("Pulo")]
    public float jumpForce = 7f;

    [Header("Abaixar")]
    public float slideDuration = 0.7f;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    private bool isGrounded = false;
    private bool isSliding = false;

    private float originalColliderHeight;
    private Vector2 originalColliderOffset;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        originalColliderHeight = boxCollider.size.y;
        originalColliderOffset = boxCollider.offset;

        transform.position = new Vector3(lanes[currentLane], transform.position.y, transform.position.z);
    }

    void Update()
    {
        if (GameManager.instance != null && !GameManager.instance.gameRunning)
        {
            return;
        }

        HandleLaneInput();
        HandleJump();
        HandleSlide();
    }

    void FixedUpdate()
    {
        MoveToLane();
    }

    void HandleLaneInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (currentLane > 0)
            {
                currentLane--;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (currentLane < lanes.Length - 1)
            {
                currentLane++;
            }
        }
    }

    void MoveToLane()
    {
        Vector2 targetPosition = new Vector2(lanes[currentLane], rb.position.y);
        Vector2 newPosition = Vector2.Lerp(rb.position, targetPosition, laneChangeSpeed * Time.fixedDeltaTime);

        rb.MovePosition(newPosition);
    }

    void HandleJump()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    void HandleSlide()
    {
        if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && !isSliding && isGrounded)
        {
            StartCoroutine(Slide());
        }
    }

    System.Collections.IEnumerator Slide()
    {
        isSliding = true;

        boxCollider.size = new Vector2(boxCollider.size.x, originalColliderHeight / 2f);
        boxCollider.offset = new Vector2(originalColliderOffset.x, originalColliderOffset.y - originalColliderHeight / 4f);

        yield return new WaitForSeconds(slideDuration);

        boxCollider.size = new Vector2(boxCollider.size.x, originalColliderHeight);
        boxCollider.offset = originalColliderOffset;

        isSliding = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = false;
        }
    }
}