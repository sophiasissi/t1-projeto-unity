using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    public GameObject collectiblePrefab;

    public float spawnInterval = 3f;
    public float minY = -1.2f;
    public float maxY = 1.6f;

    private float timer = 0f;

    void Update()
    {
        if (GameManager.instance != null && !GameManager.instance.gameRunning)
        {
            return;
        }

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnCollectible();
            timer = 0f;
        }
    }

    void SpawnCollectible()
    {
        float randomY = Random.Range(minY, maxY);
        Vector3 spawnPosition = new Vector3(transform.position.x, randomY, 0);

        Instantiate(
            collectiblePrefab,
            spawnPosition,
            Quaternion.identity
        );
    }
}