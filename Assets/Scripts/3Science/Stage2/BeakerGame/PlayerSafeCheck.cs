using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSafeCheck : MonoBehaviour
{
    [Header("발밑 체크 포인트")]
    public Transform groundCheck;
    public float checkRadius = 0.1f;

    [Header("안전한 레이어 (나무판자)")]
    public LayerMask safeLayerMask;

    [Header("리스폰 설정")]
    public Transform respawnPoint;
    public Vector3 spawnPos;

    // 점프 상태 외부에서 세팅
    public bool IsJumping { get; set; } = false;

    void Start()
    {
        if (respawnPoint != null)
            spawnPos = respawnPoint.position;
        else
        {
            spawnPos = transform.position;   // 혹시 안 넣었으면 현재 위치를 스폰으로
            Debug.LogWarning($"{name} : respawnPoint가 비어있어서 현재 위치를 스폰으로 사용합니다.");
        }
        transform.position = spawnPos;
    }

    void Update()
    {
        // 점프 중에는 판자 체크 X (공중이니까 벗어나도 됨)
        if (IsJumping) return;

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
        if (!IsOnSafePlatform())
        {
            OnFail();
        }
    }

    void OnFail()
    {
        transform.position = spawnPos;
        IsJumping = false;
        // FailPanel 쓰고 싶으면 여기 대신
        // FailPanel.Instance.ShowFail(...); 이런 식으로 호출
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
