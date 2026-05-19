using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
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

        GameOver();
    }

    void GameOver()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.GameOver();
        }
    }
}