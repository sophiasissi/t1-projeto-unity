using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    public float speed = 4f;
    public float destroyY = -6f;

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y < destroyY)
        {
            Destroy(gameObject);
        }
    }
}