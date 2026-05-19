using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] obstaclePrefabs;

    [Header("Posições")]
    public float[] lanes = { -2.2f, 0f, 2.2f };
    public float spawnY = 6f;

    [Header("Tempo")]
    public float startDelay = 1f;
    public float spawnInterval = 1.5f;

    [Header("Velocidade da fase")]
    public float objectSpeed = 5f;

    private float timer = 0f;
    private bool canSpawn = false;

    void Start()
    {
        timer = 0f;
        canSpawn = false;

        Invoke(nameof(EnableSpawn), startDelay);
    }

    void Update()
    {
        if (GameManager.instance != null && !GameManager.instance.gameRunning)
        {
            return;
        }

        if (!canSpawn)
        {
            return;
        }

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnObstacle();
            timer = 0f;
        }
    }

    void EnableSpawn()
    {
        canSpawn = true;
        SpawnObstacle();
    }

    void SpawnObstacle()
    {
        if (obstaclePrefabs == null || obstaclePrefabs.Length == 0)
        {
            Debug.LogWarning("ObstacleSpawner: nenhum prefab foi colocado no Inspector.");
            return;
        }

        if (lanes == null || lanes.Length == 0)
        {
            Debug.LogWarning("ObstacleSpawner: nenhuma faixa foi configurada.");
            return;
        }

        int randomLaneIndex = Random.Range(0, lanes.Length);
        int randomObstacleIndex = Random.Range(0, obstaclePrefabs.Length);

        Vector3 spawnPosition = new Vector3(
            lanes[randomLaneIndex],
            spawnY,
            0f
        );

        GameObject newObstacle = Instantiate(
            obstaclePrefabs[randomObstacleIndex],
            spawnPosition,
            Quaternion.identity
        );

        ObstacleMover mover = newObstacle.GetComponent<ObstacleMover>();

        if (mover != null)
        {
            mover.speed = objectSpeed;
        }
    }
}