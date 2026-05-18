using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;

    public float[] lanes = { -2f, 0f, 2f };
    public float spawnY = 6f;

    public float startDelay = 1.5f;
    public float spawnInterval = 2f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnObstacle), startDelay, spawnInterval);
    }

    void SpawnObstacle()
    {
        int laneIndex = Random.Range(0, lanes.Length);
        int obstacleIndex = Random.Range(0, obstaclePrefabs.Length);

        Vector3 spawnPosition = new Vector3(lanes[laneIndex], spawnY, 0f);

        Instantiate(obstaclePrefabs[obstacleIndex], spawnPosition, Quaternion.identity);
    }
}