using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("References")]
    public RunnerPlayerController player;

    [Header("Spawn")]
    public float spawnDistanceAhead = 15f;     // 플레이어보다 얼마나 앞에 스폰할지
    public float minSpawnInterval = 5f;        // 장애물 사이 최소 간격 (월드 x 거리)
    public float maxSpawnInterval = 10f;       // 장애물 사이 최대 간격

    [Header("Prefab")]
    public GameObject jumpBoxPrefab;           // 점프해야 하는 상자
    public GameObject noJumpHazardPrefab;      // 점프하면 죽는 장애물

    [Header("Position")]
    public float groundY = 0f;                 // 바닥 장애물의 y 위치
    public float noJumpHazardY = 1.5f;         // 머리 위 함정 y 위치 (상황에 맞게 조정)

    float nextSpawnX;

    void Start()
    {
        if (player == null)
        {
            Debug.LogWarning("ObstacleSpawner: player가 설정되지 않음");
            enabled = false;
            return;
        }

        // 첫 스폰 위치 잡기
        float firstOffset = Random.Range(minSpawnInterval, maxSpawnInterval);
        nextSpawnX = player.transform.position.x + firstOffset;
    }

    void Update()
    {
        if (StageManager.Instance != null && StageManager.Instance.enabled == false)
            return;

        float playerX = player.transform.position.x;

        // 플레이어가 nextSpawnX에 가까워지면 새 장애물 생성
        if (playerX + spawnDistanceAhead > nextSpawnX)
        {
            SpawnObstacle(nextSpawnX);
            // 다음 스폰 위치 갱신
            float interval = Random.Range(minSpawnInterval, maxSpawnInterval);
            nextSpawnX += interval;
        }
    }

    void SpawnObstacle(float spawnX)
    {
        // 어떤 장애물로 할지 랜덤
        int type = Random.Range(0, 2); // 0: 점프 박스, 1: 점프하면 죽는 함정

        if (type == 0 && jumpBoxPrefab != null)
        {
            Vector3 pos = new Vector3(spawnX, groundY, 0f);
            Instantiate(jumpBoxPrefab, pos, Quaternion.identity);
        }
        else if (type == 1 && noJumpHazardPrefab != null)
        {
            Vector3 pos = new Vector3(spawnX, noJumpHazardY, 0f);
            Instantiate(noJumpHazardPrefab, pos, noJumpHazardPrefab.transform.rotation);
        }
    }
}
