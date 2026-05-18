using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            GameManager.instance.GameOver();
        }

        if (collision.CompareTag("Coffee"))
        {
            GameManager.instance.AddCoffee();
            Destroy(collision.gameObject);
        }
    }
}