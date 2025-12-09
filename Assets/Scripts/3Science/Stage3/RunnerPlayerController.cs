using UnityEngine;

public class RunnerPlayerController : MonoBehaviour
{
    [Header("Move & Jump")]
    public float runSpeed = 5f;          // 기본 달리기 속도
    public float stopSpeed = 1f;         // 멈출 때 (키 안 누를 때) 속도
    public float jumpForce = 7f;

    [Header("땅 체크")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;

    public bool IsStunned { get; private set; }
    float stunTimer = 0f;

    Rigidbody2D rb;
    Animator anim;

    public bool IsGrounded { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsRunningInput { get; private set; }   //달리기 키 입력 여부

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (IsStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0f)
            {
                IsStunned = false;
            }
        }

        //땅 체크
        IsGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        if (IsGrounded && rb.linearVelocity.y <= 0.01f)
            IsJumping = false;

        // 달리기 키 (→, D)
        IsRunningInput = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);

        if (IsStunned) return;

        // 점프 (스페이스)
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            IsJumping = true;
        }

        if (!IsStunned)
        {
            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                IsJumping = true;
            }
        }

        //여기서 애니메이션 파라미터 업데이트
        UpdateAnimation();
    }

    void FixedUpdate()
    {
        float targetSpeed;

        if (IsStunned)
        {
            targetSpeed = 0f;                 // 완전 멈춤
            // 또는 약간만 굴러가게 하고 싶으면: targetSpeed = 0.5f;
        }
        else
        {
            targetSpeed = IsRunningInput ? runSpeed : stopSpeed;
        }

        rb.linearVelocity = new Vector2(targetSpeed, rb.linearVelocity.y);
    }

    public void ApplyStun(float duration)
    {
        IsStunned = true;
        stunTimer = duration;
    }

    void UpdateAnimation()
    {
        if (anim == null) return;

        float speedX = Mathf.Abs(rb.linearVelocity.x);

        anim.SetFloat("Speed", speedX);
        anim.SetBool("IsGrounded", IsGrounded);
        anim.SetBool("IsStunned", IsStunned);
    }
}