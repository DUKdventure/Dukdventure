using UnityEngine;
using System.Collections;

public class PlayerSafeCheck : MonoBehaviour
{
    [Header("Check")]
    public Transform groundCheck;
    public float checkRadius = 0.1f;

    [Header("Safe Layer(Ground)")]
    public LayerMask safeLayerMask;

    [Header("Respawn")]
    public Transform respawnPoint;
    public Vector3 spawnPos;

    [Header("Effect")]
    public float respawnDelay = 0.6f;      // 회색으로 있는 시간
    public Color deathColor = Color.gray;

    // 점프 상태 외부에서 세팅
    public bool IsJumping { get; set; } = false;

    bool isRespawning = false;

    SpriteRenderer spriteRenderer;
    Color originalColor;
    PlayerJumpTopDown jumper;

    void Awake()
    {
        if (respawnPoint != null)
            spawnPos = respawnPoint.position;
        else
            spawnPos = transform.position;

        // 시작 위치를 스폰 위치로
        transform.position = spawnPos;

        // 이동/점프 스크립트 찾기
        jumper = GetComponent<PlayerJumpTopDown>();

        // SpriteRenderer 찾기:
        // 점프 스크립트에 연결해둔 spriteRoot 밑에서 가져오면 제일 좋음
        if (jumper != null && jumper.spriteRoot != null)
        {
            spriteRenderer = jumper.spriteRoot.GetComponentInChildren<SpriteRenderer>();
        }
        else
        {
            // 그래도 못 찾으면 자기 자식들에서 한 번 더 검색
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    void Update()
    {
        // 점프 중에는 판자 체크 X (공중이니까 벗어나도 됨)
        if (isRespawning || IsJumping) return;

        // 평소에는 계속 판자 위인지 체크
        if (!IsOnSafePlatform())
        {
            OnFail();
        }
    }

    /// <summary>
    /// 발밑에 안전한 판자가 있는지 체크
    /// </summary>
    public bool IsOnSafePlatform()
    {
        if (groundCheck == null)
        {
            Debug.LogWarning("groundCheck 가 설정되어 있지 않습니다.");
            return true; // 설정 안 되어 있으면 그냥 죽지 않게 처리
        }

        Collider2D col = Physics2D.OverlapCircle(
            groundCheck.position,
            checkRadius,
            safeLayerMask
        );

        return col != null;
    }

    /// <summary>
    /// 점프가 끝나고 착지한 '그 프레임'에 호출
    /// </summary>
    public void CheckAfterLanding()
    {
        if (isRespawning) return;

        if (!IsOnSafePlatform())
        {
            OnFail();
        }
    }

    void OnFail()
    {
        if (isRespawning) return;

        StartCoroutine(RespawnRoutine());
        // FailPanel 쓰고 싶으면 여기 대신
        // FailPanel.Instance.ShowFail(...); 이런 식으로 호출
    }

    IEnumerator RespawnRoutine()
    {
        isRespawning = true;
        IsJumping = false;

        // 1) 움직임/점프 잠시 막기
        if (jumper != null)
            jumper.enabled = false;

        // 2) 색 회색으로 변경
        if (spriteRenderer != null)
            spriteRenderer.color = deathColor;

        // 3) 잠깐 대기 (연출 시간)
        yield return new WaitForSeconds(respawnDelay);

        // 4) 위치를 스폰 위치로 되돌리기
        transform.position = spawnPos;

        // 점프 높이 연출도 바닥으로 리셋
        if (jumper != null && jumper.spriteRoot != null)
        {
            jumper.spriteRoot.localPosition = Vector3.zero;
        }

        // 5) 색 원래대로
        if (spriteRenderer != null)
            spriteRenderer.color = originalColor;

        // 6) 다시 조작 가능
        if (jumper != null)
            jumper.enabled = true;

        isRespawning = false;
    }
    public void SetSpawnPoint(Transform newSpawn, bool teleportPlayer = true)
    {
        respawnPoint = newSpawn;

        if (respawnPoint != null)
            spawnPos = respawnPoint.position;
        else
            spawnPos = transform.position;

        if (teleportPlayer)
        {
            transform.position = spawnPos;

            // 점프 높이도 리셋
            var jumper = GetComponent<PlayerJumpTopDown>();
            if (jumper != null && jumper.spriteRoot != null)
            {
                jumper.spriteRoot.localPosition = Vector3.zero;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}
