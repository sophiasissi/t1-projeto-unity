using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject collectiblePrefab;

    [Header("Posições")]
    public float[] lanes = { -2.2f, 0f, 2.2f };
    public float spawnY = 6f;

    [Header("Tempo")]
    public float startDelay = 2f;
    public float spawnInterval = 3f;

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
            SpawnCollectible();
            timer = 0f;
        }
    }

    void EnableSpawn()
    {
        canSpawn = true;
        SpawnCollectible();
    }

    void SpawnCollectible()
    {
        if (collectiblePrefab == null)
        {
            Debug.LogWarning("CollectibleSpawner: collectiblePrefab não foi colocado no Inspector.");
            return;
        }

        if (lanes == null || lanes.Length == 0)
        {
            Debug.LogWarning("CollectibleSpawner: nenhuma faixa foi configurada.");
            return;
        }

        int randomLaneIndex = Random.Range(0, lanes.Length);

        Vector3 spawnPosition = new Vector3(
            lanes[randomLaneIndex],
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
    }
}