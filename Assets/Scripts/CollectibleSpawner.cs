using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject collectiblePrefab;

    [Header("Posições")]
    public float[] lanes = { -2.2f, 0f, 2.2f };
    public float spawnY = 6f;

    [Header("Tempo")]
    public float startDelay = 3f;
    public float minSpawnInterval = 4f;
    public float maxSpawnInterval = 6f;
    public float retryDelay = 0.7f;

    [Header("Velocidade da fase")]
    public float objectSpeed = 5f;

    [Header("Controle")]
    public int maxActiveCollectibles = 1;
    public float minDistanceFromObstacle = 2.5f;
    public float laneTolerance = 0.4f;

    private float timer = 0f;
    private float nextSpawnTime = 0f;
    private bool canSpawn = false;
    private int lastLaneIndex = -1;

    void Start()
    {
        timer = 0f;
        canSpawn = false;
        nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);

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

        if (PlayerAlreadyCollectedEnoughCoffee())
        {
            return;
        }

        if (CountActiveCollectibles() >= maxActiveCollectibles)
        {
            return;
        }

        timer += Time.deltaTime;

        if (timer >= nextSpawnTime)
        {
            bool spawned = TrySpawnCollectible();

            if (spawned)
            {
                timer = 0f;
                nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            }
            else
            {
                timer = nextSpawnTime - retryDelay;
            }
        }
    }

    void EnableSpawn()
    {
        canSpawn = true;
    }

    bool TrySpawnCollectible()
    {
        if (collectiblePrefab == null)
        {
            Debug.LogWarning("CollectibleSpawner: o prefab do café não foi colocado no Inspector.");
            return false;
        }

        if (lanes == null || lanes.Length == 0)
        {
            Debug.LogWarning("CollectibleSpawner: nenhuma faixa foi configurada.");
            return false;
        }

        int laneIndex = ChooseSafeLaneIndex();

        if (laneIndex == -1)
        {
            return false;
        }

        Vector3 spawnPosition = new Vector3(
            lanes[laneIndex],
            spawnY,
            0f
        );

        GameObject newCollectible = Instantiate(
            collectiblePrefab,
            spawnPosition,
            Quaternion.identity
        );

        ObstacleMover mover = newCollectible.GetComponent<ObstacleMover>();

        if (mover != null)
        {
            mover.speed = objectSpeed;
        }

        lastLaneIndex = laneIndex;

        return true;
    }

    int ChooseSafeLaneIndex()
    {
        int[] laneIndexes = CreateShuffledLaneIndexes();

        for (int i = 0; i < laneIndexes.Length; i++)
        {
            int laneIndex = laneIndexes[i];

            if (laneIndex == lastLaneIndex && lanes.Length > 1)
            {
                continue;
            }

            if (LaneIsSafe(lanes[laneIndex]))
            {
                return laneIndex;
            }
        }

        for (int i = 0; i < laneIndexes.Length; i++)
        {
            int laneIndex = laneIndexes[i];

            if (LaneIsSafe(lanes[laneIndex]))
            {
                return laneIndex;
            }
        }

        return -1;
    }

    int[] CreateShuffledLaneIndexes()
    {
        int[] indexes = new int[lanes.Length];

        for (int i = 0; i < lanes.Length; i++)
        {
            indexes[i] = i;
        }

        for (int i = 0; i < indexes.Length; i++)
        {
            int randomIndex = Random.Range(i, indexes.Length);
            int temp = indexes[i];
            indexes[i] = indexes[randomIndex];
            indexes[randomIndex] = temp;
        }

        return indexes;
    }

    bool LaneIsSafe(float laneX)
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        foreach (GameObject obstacle in obstacles)
        {
            if (obstacle == null)
            {
                continue;
            }

            float obstacleX = obstacle.transform.position.x;
            float obstacleY = obstacle.transform.position.y;

            bool sameLane = Mathf.Abs(obstacleX - laneX) <= laneTolerance;
            bool tooClose = Mathf.Abs(obstacleY - spawnY) <= minDistanceFromObstacle;

            if (sameLane && tooClose)
            {
                return false;
            }
        }

        return true;
    }

    bool PlayerAlreadyCollectedEnoughCoffee()
    {
        if (GameManager.instance == null)
        {
            return false;
        }

        return GameManager.instance.GetCoffeeCount() >= GameManager.instance.coffeesToFinishLevel;
    }

    int CountActiveCollectibles()
    {
        GameObject[] coffees = GameObject.FindGameObjectsWithTag("Coffee");
        return coffees.Length;
    }
}