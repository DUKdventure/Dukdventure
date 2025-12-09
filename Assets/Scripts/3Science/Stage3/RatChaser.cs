using UnityEngine;

public class RatChaser : MonoBehaviour
{
    [Header("Reference")]
    public RunnerPlayerController player;
    public float baseSpeed = 4.5f;      // 쥐 기본 속도
    public float catchDistance = 0.5f;  // 이 거리 이하로 붙으면 잡힘

    [Header("Skill")]
    public float slowDuration = 2f;     // 느려지는 시간
    public float slowSpeedMultiplier = 0.3f;

    float slowTimer = 0f;

    void Update()
    {
        if (player == null) return;

        float currentSpeed = baseSpeed;

        // 플레이어 능력으로 느려진 상태면
        if (slowTimer > 0f)
        {
            slowTimer -= Time.deltaTime;
            currentSpeed *= slowSpeedMultiplier;
        }

        // 쥐가 플레이어 방향으로 쫓아감 (x축만)
        Vector3 pos = transform.position;
        float dir = Mathf.Sign(player.transform.position.x - pos.x); // 1 또는 -1

        pos.x += currentSpeed * dir * Time.deltaTime;
        transform.position = pos;

        
    }

    // 플레이어 능력 발동 시 호출
    public void OnPlayerAbility()
    {
        slowTimer = slowDuration;

        // 시각적인 연출: 위에서 상자 떨어뜨리기 등은 여기서 Instantiate
        // (원하면 따로 스크립트로 빼도 됨)
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Player와 닿았을 때만
        if (!other.CompareTag("Player")) return;

        Debug.Log("쥐와 충돌! 게임오버");
        StageManager.Instance?.OnGameOver();
    }
}
