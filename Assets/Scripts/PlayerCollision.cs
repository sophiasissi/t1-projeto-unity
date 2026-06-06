using System.Collections;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private PlayerController playerController;
    private bool isDying = false;

    [Header("Efeito de morte")]
    public GameObject deathExplosionPrefab;
    public float delayBeforeGameOver = 0.8f;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDying)
        {
            return;
        }

        if (collision.CompareTag("Coffee"))
        {
            CollectCoffee(collision.gameObject);
            return;
        }

        if (collision.CompareTag("Obstacle"))
        {
            HandleObstacleCollision(collision.gameObject);
            return;
        }
    }

    void CollectCoffee(GameObject coffee)
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.AddCoffee();
        }

        Destroy(coffee);
    }

    void HandleObstacleCollision(GameObject obstacle)
    {
        ObstacleType obstacleType = obstacle.GetComponent<ObstacleType>();

        if (obstacleType != null && obstacleType.requiredAction == ObstacleType.RequiredAction.Jump)
        {
            if (playerController != null && playerController.IsJumping)
            {
                return;
            }
        }

        StartCoroutine(GameOverWithExplosion());
    }

    IEnumerator GameOverWithExplosion()
    {
        isDying = true;

        if (deathExplosionPrefab != null)
        {
            Instantiate(deathExplosionPrefab, transform.position, Quaternion.identity);
        }

        DisablePlayerVisualAndCollision();

        yield return new WaitForSeconds(delayBeforeGameOver);

        if (GameManager.instance != null)
        {
            GameManager.instance.GameOver();
        }
    }

    void DisablePlayerVisualAndCollision()
    {
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer renderer in renderers)
        {
            renderer.enabled = false;
        }

        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }

        if (playerController != null)
        {
            playerController.enabled = false;
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}