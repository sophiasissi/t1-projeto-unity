using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject collectiblePrefab;

    [Header("Posições")]
    public float[] lanes = { -2.2f, 0f, 2.2f };
    public float spawnY = 6f;

    [Header("Tempo")]
    public float startDelay = 1.5f;
    public float spawnInterval = 2f;

    [Header("Velocidade da fase")]
    public float objectSpeed = 5f;

    [Header("Controle")]
    public int maxActiveCollectibles = 2;

    private float timer = 0f;
    private bool canSpawn = false;
    private int lastLaneIndex = -1;

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

        if (PlayerAlreadyCollectedEnoughCoffee())
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
            Debug.LogWarning("CollectibleSpawner: o prefab do café não foi colocado no Inspector.");
            return;
        }

        if (lanes == null || lanes.Length == 0)
        {
            Debug.LogWarning("CollectibleSpawner: nenhuma faixa foi configurada.");
            return;
        }

        if (PlayerAlreadyCollectedEnoughCoffee())
        {
            return;
        }

        if (CountActiveCollectibles() >= maxActiveCollectibles)
        {
            Debug.Log("CollectibleSpawner: já existem cafés ativos suficientes na tela.");
            return;
        }

        int laneIndex = ChooseLaneIndex();

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
        else
        {
            Debug.LogWarning("CollectibleSpawner: o prefab do café não tem ObstacleMover.");
        }

        Debug.Log("Café criado na faixa " + laneIndex + " na posição " + spawnPosition);
    }

    int ChooseLaneIndex()
    {
        int selectedLaneIndex = Random.Range(0, lanes.Length);

        if (lanes.Length > 1)
        {
            int attempts = 0;

            while (selectedLaneIndex == lastLaneIndex && attempts < 10)
            {
                selectedLaneIndex = Random.Range(0, lanes.Length);
                attempts++;
            }
        }

        lastLaneIndex = selectedLaneIndex;
        return selectedLaneIndex;
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