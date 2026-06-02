using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    [Header("Prefab")]
    // Prefab do café/coletável
    public GameObject collectiblePrefab;

    [Header("Posições")]
    // Faixas onde o café pode aparecer
    public float[] lanes = { -2.2f, 0f, 2.2f };

    // Altura inicial do spawn
    public float spawnY = 6f;

    [Header("Tempo")]
    // Tempo antes de começar a criar cafés
    public float startDelay = 3f;

    // Intervalo mínimo e máximo entre cafés
    public float minSpawnInterval = 4f;
    public float maxSpawnInterval = 6f;

    // Tempo para tentar criar novamente caso falhe
    public float retryDelay = 0.7f;

    [Header("Velocidade da fase")]
    // Velocidade do café nesta fase
    public float objectSpeed = 5f;

    [Header("Controle")]
    // Máximo de cafés ativos ao mesmo tempo
    public int maxActiveCollectibles = 1;

    // Distância mínima em relação aos obstáculos
    public float minDistanceFromObstacle = 2.5f;

    // Margem para considerar que está na mesma faixa
    public float laneTolerance = 0.4f;

    private float timer = 0f;
    private float nextSpawnTime = 0f;
    private bool canSpawn = false;
    private int lastLaneIndex = -1;

    void Start()
    {
        // Prepara o spawner no início da fase
        timer = 0f;
        canSpawn = false;

        // Sorteia o primeiro tempo de spawn
        nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);

        // Libera o spawn após um atraso inicial
        Invoke(nameof(EnableSpawn), startDelay);
    }

    void Update()
    {
        // Para o spawn se o jogo terminou
        if (GameManager.instance != null && !GameManager.instance.gameRunning)
        {
            return;
        }

        // Aguarda o spawn ser liberado
        if (!canSpawn)
        {
            return;
        }

        // Para se o jogador já coletou o necessário
        if (PlayerAlreadyCollectedEnoughCoffee())
        {
            return;
        }

        // Evita muitos cafés ativos na cena
        if (CountActiveCollectibles() >= maxActiveCollectibles)
        {
            return;
        }

        // Conta o tempo entre os spawns
        timer += Time.deltaTime;

        // Tenta criar café quando chega no tempo sorteado
        if (timer >= nextSpawnTime)
        {
            bool spawned = TrySpawnCollectible();

            if (spawned)
            {
                // Reinicia o tempo após criar o café
                timer = 0f;
                nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            }
            else
            {
                // Tenta novamente em pouco tempo
                timer = nextSpawnTime - retryDelay;
            }
        }
    }

    void EnableSpawn()
    {
        // Ativa o spawner
        canSpawn = true;
    }

    bool TrySpawnCollectible()
    {
        // Garante que o prefab foi configurado
        if (collectiblePrefab == null)
        {
            Debug.LogWarning("CollectibleSpawner: o prefab do café não foi colocado no Inspector.");
            return false;
        }

        // Garante que há faixas configuradas
        if (lanes == null || lanes.Length == 0)
        {
            Debug.LogWarning("CollectibleSpawner: nenhuma faixa foi configurada.");
            return false;
        }

        // Escolhe uma faixa segura
        int laneIndex = ChooseSafeLaneIndex();

        // Cancela se nenhuma faixa estiver segura
        if (laneIndex == -1)
        {
            return false;
        }

        // Define a posição do café
        Vector3 spawnPosition = new Vector3(
            lanes[laneIndex],
            spawnY,
            0f
        );

        // Cria o café na cena
        GameObject newCollectible = Instantiate(
            collectiblePrefab,
            spawnPosition,
            Quaternion.identity
        );

        // Aplica velocidade ao café criado
        ObstacleMover mover = newCollectible.GetComponent<ObstacleMover>();

        if (mover != null)
        {
            mover.speed = objectSpeed;
        }

        // Guarda a última faixa usada
        lastLaneIndex = laneIndex;

        return true;
    }

    int ChooseSafeLaneIndex()
    {
        // Cria uma ordem aleatória das faixas
        int[] laneIndexes = CreateShuffledLaneIndexes();

        // Primeiro tenta evitar repetir a última faixa
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

        // Se não encontrar, aceita repetir a faixa anterior
        for (int i = 0; i < laneIndexes.Length; i++)
        {
            int laneIndex = laneIndexes[i];

            if (LaneIsSafe(lanes[laneIndex]))
            {
                return laneIndex;
            }
        }

        // Nenhuma faixa segura
        return -1;
    }

    int[] CreateShuffledLaneIndexes()
    {
        // Cria lista de índices das faixas
        int[] indexes = new int[lanes.Length];

        for (int i = 0; i < lanes.Length; i++)
        {
            indexes[i] = i;
        }

        // Embaralha os índices
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
        // Busca obstáculos ativos na cena
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        foreach (GameObject obstacle in obstacles)
        {
            if (obstacle == null)
            {
                continue;
            }

            float obstacleX = obstacle.transform.position.x;
            float obstacleY = obstacle.transform.position.y;

            // Verifica se há obstáculo próximo na mesma faixa
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
        // Verifica se existe GameManager
        if (GameManager.instance == null)
        {
            return false;
        }

        // Confere se a meta de cafés já foi atingida
        return GameManager.instance.GetCoffeeCount() >= GameManager.instance.coffeesToFinishLevel;
    }

    int CountActiveCollectibles()
    {
        // Conta cafés ativos na cena
        GameObject[] coffees = GameObject.FindGameObjectsWithTag("Coffee");
        return coffees.Length;
    }
}