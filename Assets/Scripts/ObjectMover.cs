using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    public float speed = 5f;
    public float destroyX = -10f;

    void Update()
    {
        if (GameManager.instance != null && !GameManager.instance.gameRunning)
        {
            return;
        }

        transform.Translate(Vector3.left * speed * Time.deltaTime);

        if (transform.position.x < destroyX)
        {
            Destroy(gameObject);
        }
    }
}