using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;

    public float spawnInterval = 2f;
    public float minSpawnInterval = 0.9f;
    public float difficultyDecrease = 0.05f;

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
            SpawnObstacle();
            timer = 0f;

            if (spawnInterval > minSpawnInterval)
            {
                spawnInterval -= difficultyDecrease;
            }
        }
    }

    void SpawnObstacle()
    {
        int randomIndex = Random.Range(0, obstaclePrefabs.Length);

        Instantiate(
            obstaclePrefabs[randomIndex],
            transform.position,
            Quaternion.identity
        );
    }
}