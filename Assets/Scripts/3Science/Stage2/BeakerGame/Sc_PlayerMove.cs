using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class Sc_PlayerMove : MonoBehaviour
{
    [Header("이동 속도")]
    public float moveSpeed = 3f;

    [Header("점프 연출")]
    public Transform spriteRoot;   // SpriteRoot
    public Transform shadow;       // Shadow
    public float jumpHeight = 0.4f;    // 얼마나 위로 튀어오를지
    public float jumpDuration = 0.35f; // 점프 시간

    bool isJumping = false;
    float jumpTimer = 0f;

    Vector2 input;
    Vector2 movement;

    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;

    Vector3 spriteBaseLocalPos;
    Vector3 shadowBaseLocalScale;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = spriteRoot.GetComponent<SpriteRenderer>();

        // SpriteRoot의 기본 위치 저장 (y=0일 때)
        spriteBaseLocalPos = spriteRoot.localPosition;

        if (shadow != null)
            shadowBaseLocalScale = shadow.localScale;

        rb.gravityScale = 0f;  // 탑뷰라서 0
        rb.freezeRotation = true;
    }

    void Update()
    {
        // ===== 1. 입력 / 이동 처리 (네가 쓰던 방식 그대로) =====
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        input = new Vector2(x, y);

        // 대각선 방지
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            input.y = 0f;
        else
            input.x = 0f;

        movement = input.normalized;

        float speed = movement.sqrMagnitude;
        anim.SetFloat("Speed", speed);
        if (speed > 0.001f)
        {
            anim.SetFloat("Horizontal", movement.x);
            anim.SetFloat("Vertical", movement.y);
            anim.SetFloat("LastX", movement.x);
            anim.SetFloat("LastY", movement.y);

            if (movement.x < 0) sr.flipX = false;
            else if (movement.x > 0) sr.flipX = true;
        }

        // ===== 2. 점프 입력 =====
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            isJumping = true;
            jumpTimer = jumpDuration;
            anim.SetTrigger("Jump");  // Animator에 Jump 트리거
        }

        // ===== 3. 점프 연출 업데이트 =====
        if (isJumping)
        {
            jumpTimer -= Time.deltaTime;
            UpdateJumpVisual();

            if (jumpTimer <= 0f)
            {
                isJumping = false;
                // 착지 시점 정리
                spriteRoot.localPosition = spriteBaseLocalPos;
                if (shadow != null)
                    shadow.localScale = shadowBaseLocalScale;
            }
        }
        else
        {
            // 혹시 값이 어긋나 있으면 기본값으로 고정
            spriteRoot.localPosition = spriteBaseLocalPos;
        }
    }

    void FixedUpdate()
    {
        // 탑뷰 이동
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void UpdateJumpVisual()
    {
        // 0 ~ 1 사이 진행도
        float t = 1f - (jumpTimer / jumpDuration);
        t = Mathf.Clamp01(t);

        // 부드러운 궤적: sin 곡선 (0 → 위로 → 다시 0)
        float height = Mathf.Sin(t * Mathf.PI) * jumpHeight;

        // SpriteRoot를 위로 띄우기
        Vector3 pos = spriteBaseLocalPos;
        pos.y += height;
        spriteRoot.localPosition = pos;

        // 그림자 작게 만들기 (높이 높을수록 작고 옅게)
        if (shadow != null)
        {
            float scaleFactor = 1f - Mathf.Clamp01(height / jumpHeight) * 0.4f; // 최대 40% 축소
            shadow.localScale = shadowBaseLocalScale * scaleFactor;

            var srShadow = shadow.GetComponent<SpriteRenderer>();
            if (srShadow != null)
            {
                Color c = srShadow.color;
                c.a = 0.5f + (1f - Mathf.Clamp01(height / jumpHeight)) * 0.5f; // 위로 갈수록 좀 옅게
                srShadow.color = c;
            }
        }
    }
}
