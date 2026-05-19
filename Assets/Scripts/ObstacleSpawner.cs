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
    public float spawnInterval = 1.4f;

    [Header("Velocidade da fase")]
    public float objectSpeed = 5f;

    [Header("Controle de repetição")]
    public int maxSameLaneInSequence = 2;
    public float chanceToForceDifferentLane = 0.7f;

    private float timer = 0f;
    private bool canSpawn = false;

    private int lastLaneIndex = -1;
    private int sameLaneCount = 0;

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

        int laneIndex = ChooseLaneIndex();
        int obstacleIndex = Random.Range(0, obstaclePrefabs.Length);

        Vector3 spawnPosition = new Vector3(
            lanes[laneIndex],
            spawnY,
            0f
        );

        GameObject newObstacle = Instantiate(
            obstaclePrefabs[obstacleIndex],
            spawnPosition,
            Quaternion.identity
        );

        ObstacleMover mover = newObstacle.GetComponent<ObstacleMover>();

        if (mover != null)
        {
            mover.speed = objectSpeed;
        }
    }

    int ChooseLaneIndex()
    {
        int selectedLaneIndex = Random.Range(0, lanes.Length);

        bool repeatedTooMuch = lastLaneIndex != -1 && sameLaneCount >= maxSameLaneInSequence;
        bool shouldTryDifferentLane = lastLaneIndex != -1 && Random.value < chanceToForceDifferentLane;

        if ((repeatedTooMuch || shouldTryDifferentLane) && lanes.Length > 1)
        {
            int attempts = 0;

            while (selectedLaneIndex == lastLaneIndex && attempts < 10)
            {
                selectedLaneIndex = Random.Range(0, lanes.Length);
                attempts++;
            }
        }

        if (selectedLaneIndex == lastLaneIndex)
        {
            sameLaneCount++;
        }
        else
        {
            sameLaneCount = 1;
            lastLaneIndex = selectedLaneIndex;
        }

        return selectedLaneIndex;
    }
}