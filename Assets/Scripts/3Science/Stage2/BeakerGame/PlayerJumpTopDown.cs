using UnityEngine;

[RequireComponent(typeof(PlayerSafeCheck))]
public class PlayerJumpTopDown : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed = 3f;

    [Header("Jump")]
    public float jumpPower = 5f;   // 점프 시작 속도
    public float gravity = -20f;   // 점프 중 아래로 당기는 값(음수)

    [Header("Child")]
    public Transform spriteRoot;   // 캐릭터 스프라이트가 달린 자식 오브젝트

    [Header("Animation")]
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    Vector2 moveInput;       // 현재 입력 방향
    Vector2 lastMoveDir = Vector2.down;

    float height = 0f;            // 현재 점프 높이
    float velocity = 0f;  // 점프 속도
    bool isJumping = false;

    PlayerSafeCheck safeCheck;
    string currentAnimState = "";

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
        UpdateAnimation();
    }

    void HandleMove()
    {
        float h = Input.GetAxisRaw("Horizontal"); // A,D 또는 ←,→
        float v = Input.GetAxisRaw("Vertical");   // W,S 또는 ↑,↓

        moveInput = new Vector2(h, v).normalized;

        if (moveInput.sqrMagnitude > 0.001f)
        {
            // 실제 위치 이동
            transform.position += (Vector3)moveInput * moveSpeed * Time.deltaTime;

            // 마지막 이동 방향 갱신 (0일 때는 갱신 X)
            lastMoveDir = moveInput;
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

    void UpdateAnimation()
    {
        if (animator == null) return;

        // 움직이는 중? (점프 중일 땐 Walk 안 씀)
        bool isMoving = moveInput.sqrMagnitude > 0.001f && !isJumping;

        // ===== 방향 계산 (Front / Back / Side) =====
        string dirStr = "Front"; // 기본값: 아래 보고 있음

        // lastMoveDir 기준으로 방향 결정
        if (Mathf.Abs(lastMoveDir.x) > Mathf.Abs(lastMoveDir.y))
        {
            // 좌우가 더 크면 Side
            dirStr = "Side";

            // 오른쪽/왼쪽에 따라 스프라이트 뒤집기
            if (spriteRenderer != null)
            {
                if (lastMoveDir.x > 0.01f) spriteRenderer.flipX = true; // 기본을 오른쪽 바라보는 스프라이트라고 가정
                else if (lastMoveDir.x < -0.01f) spriteRenderer.flipX = false;
            }
        }
        else
        {
            // 위/아래가 더 크면 Front / Back
            if (lastMoveDir.y > 0.01f)
                dirStr = "Back";
            else
                dirStr = "Front";
        }

        // ===== 상태에 따라 재생할 애니메이션 이름 결정 =====
        string nextState;

        if (isJumping)
        {
            nextState = dirStr + "_Jump";   // 예: "Front_Jump"
        }
        else if (isMoving)
        {
            nextState = dirStr + "_Walk";   // 예: "Side_Walk"
        }
        else
        {
            nextState = dirStr + "_Idle";   // 예: "Back_Idle"
        }

        // 현재 재생 중인 상태와 같으면 다시 재생하지 않음 (애니메이션 리셋 방지)
        if (nextState == currentAnimState) return;

        animator.CrossFade(nextState, 0.1f);
        currentAnimState = nextState;
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
