using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    // Obstáculos disponíveis para sorteio
    public GameObject[] obstaclePrefabs;

    [Header("Posições")]
    // Faixas onde os obstáculos podem aparecer
    public float[] lanes = { -2.2f, 0f, 2.2f };

    // Altura inicial do spawn
    public float spawnY = 6f;

    [Header("Tempo")]
    // Tempo antes de começar o spawn
    public float startDelay = 1f;

    // Intervalo entre obstáculos
    public float spawnInterval = 1.4f;

    [Header("Velocidade da fase")]
    // Velocidade dos obstáculos nesta fase
    public float objectSpeed = 5f;

    [Header("Controle de repetição")]
    // Limite de repetição na mesma faixa
    public int maxSameLaneInSequence = 2;

    // Chance de tentar mudar a faixa
    public float chanceToForceDifferentLane = 0.7f;

    private float timer = 0f;
    private bool canSpawn = false;

    private int lastLaneIndex = -1;
    private int sameLaneCount = 0;

    void Start()
    {
        // Prepara o spawner no início da fase
        timer = 0f;
        canSpawn = false;

        // Libera o spawn após um pequeno atraso
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

        // Conta o tempo entre os spawns
        timer += Time.deltaTime;

        // Cria obstáculo quando chega no intervalo
        if (timer >= spawnInterval)
        {
            SpawnObstacle();
            timer = 0f;
        }
    }

    void EnableSpawn()
    {
        // Ativa o spawner
        canSpawn = true;

        // Cria o primeiro obstáculo
        SpawnObstacle();
    }

    void SpawnObstacle()
    {
        // Garante que há obstáculos configurados
        if (obstaclePrefabs == null || obstaclePrefabs.Length == 0)
        {
            Debug.LogWarning("ObstacleSpawner: nenhum prefab foi colocado no Inspector.");
            return;
        }

        // Garante que há faixas configuradas
        if (lanes == null || lanes.Length == 0)
        {
            Debug.LogWarning("ObstacleSpawner: nenhuma faixa foi configurada.");
            return;
        }

        // Escolhe faixa e obstáculo
        int laneIndex = ChooseLaneIndex();
        int obstacleIndex = Random.Range(0, obstaclePrefabs.Length);

        // Define posição de criação
        Vector3 spawnPosition = new Vector3(
            lanes[laneIndex],
            spawnY,
            0f
        );

        // Cria o obstáculo na cena
        GameObject newObstacle = Instantiate(
            obstaclePrefabs[obstacleIndex],
            spawnPosition,
            Quaternion.identity
        );

        // Aplica velocidade ao obstáculo criado
        ObstacleMover mover = newObstacle.GetComponent<ObstacleMover>();

        if (mover != null)
        {
            mover.speed = objectSpeed;
        }
    }

    int ChooseLaneIndex()
    {
        // Sorteia uma faixa inicial
        int selectedLaneIndex = Random.Range(0, lanes.Length);

        // Verifica repetição da faixa anterior
        bool repeatedTooMuch = lastLaneIndex != -1 && sameLaneCount >= maxSameLaneInSequence;
        bool shouldTryDifferentLane = lastLaneIndex != -1 && Random.value < chanceToForceDifferentLane;

        // Tenta evitar repetir sempre a mesma faixa
        if ((repeatedTooMuch || shouldTryDifferentLane) && lanes.Length > 1)
        {
            int attempts = 0;

            while (selectedLaneIndex == lastLaneIndex && attempts < 10)
            {
                selectedLaneIndex = Random.Range(0, lanes.Length);
                attempts++;
            }
        }

        // Atualiza controle de repetição
        if (selectedLaneIndex == lastLaneIndex)
        {
            sameLaneCount++;
        }
        else
        {
            sameLaneCount = 1;
            lastLaneIndex = selectedLaneIndex;
        }

        // Retorna a faixa escolhida
        return selectedLaneIndex;
    }
}