using UnityEngine;

public class TutorialObjectMover : MonoBehaviour
{
    [Header("Movimento")]
    public float speed = 0.8f;
    public float endY = -3.0f;

    [Header("Comportamento ao chegar no fim")]
    public bool deactivateAtEnd = true;

    private bool canMove = false;

    void OnEnable()
    {
        canMove = true;
    }

    void Update()
    {
        if (!canMove)
        {
            return;
        }

        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y <= endY)
        {
            canMove = false;

            if (deactivateAtEnd)
            {
                gameObject.SetActive(false);
            }
        }
    }
}