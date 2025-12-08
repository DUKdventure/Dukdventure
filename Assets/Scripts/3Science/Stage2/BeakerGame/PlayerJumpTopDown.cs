using UnityEngine;

[RequireComponent(typeof(PlayerSafeCheck))]
public class PlayerJumpTopDown : MonoBehaviour
{
    [Header("이동")]
    public float moveSpeed = 3f;

    [Header("점프")]
    public float jumpPower = 5f;   // 점프 시작 속도
    public float gravity = -20f;   // 점프 중 아래로 당기는 값(음수)

    [Header("비주얼용 스프라이트 루트")]
    public Transform spriteRoot;   // 캐릭터 스프라이트가 달린 자식 오브젝트

    float height = 0f;            // 현재 점프 높이
    float velocity = 0f;  // 점프 속도
    bool isJumping = false;

    PlayerSafeCheck safeCheck;

    void Awake()
    {
        safeCheck = GetComponent<PlayerSafeCheck>();

        if (spriteRoot == null)
        {
            // 따로 지정 안 했으면 자기 자신 기준으로
            spriteRoot = transform;
        }
    }

    void Update()
    {
        HandleMove();
        HandleJump();
        UpdateJumpHeightVisual();
    }

    void HandleMove()
    {
        float h = Input.GetAxisRaw("Horizontal"); // A,D 또는 ←,→
        float v = Input.GetAxisRaw("Vertical");   // W,S 또는 ↑,↓

        Vector3 dir = new Vector3(h, v, 0f).normalized;

        // dir이 (0,0)이 아니면 이동
        if (dir.sqrMagnitude > 0f)
        {
            transform.position += dir * moveSpeed * Time.deltaTime;
        }
    }

    void HandleJump()
    {
        // 현재 발밑이 안전한 판자인지(점프 시작 조건)
        bool groundedSafe = true;
        if (safeCheck != null)
            groundedSafe = safeCheck.IsOnSafePlatform();

        // 판자 위 + 아직 점프 중 아니고 + 스페이스 누르면 점프 시작
        if (!isJumping && groundedSafe && Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
            velocity = jumpPower;

            if (safeCheck != null)
                safeCheck.IsJumping = true;   // 점프 중에는 판자 체크 끔
        }

        if (!isJumping) return;

        // 점프 중 높이 계산
        velocity += gravity * Time.deltaTime;
        height += velocity * Time.deltaTime;

        // 착지 처리
        if (height <= 0f)
        {
            height = 0f;
            velocity = 0f;
            isJumping = false;

            if (safeCheck != null)
            {
                safeCheck.IsJumping = false;   // 다시 판자 체크 켬
                safeCheck.CheckAfterLanding(); // 착지한 위치가 판자인지 확인
            }
        }
    }

    void UpdateJumpHeightVisual()
    {
        if (spriteRoot != null)
        {
            // height 값만큼 y를 올려서 "점프한 것처럼" 보이게
            spriteRoot.localPosition = new Vector3(0f, height, 0f);
        }
    }
}
