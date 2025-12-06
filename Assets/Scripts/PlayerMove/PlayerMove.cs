using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerMove : MonoBehaviour
{
    [Header("이동 속도")]
    public float moveSpeed = 3f;

    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;

    //입력 & 이동 벡터
    Vector2 input;
    Vector2 movement;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        //처음 바라보는 방향: 아래(앞)
        //anim.SetFloat("LastX", 0f);
        //anim.SetFloat("LastY", -1f);
    }

    void Update()
    {
        //1. 입력 받기 (기본 Input 시스템)
        float x = Input.GetAxisRaw("Horizontal"); //A,D / ←,→
        float y = Input.GetAxisRaw("Vertical");   //W,S / ↑,↓

        input = new Vector2(x, y);

        //2. 대각선 방지 → 4방향 전용
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
        {
            //좌우 우선
            input.y = 0f;
        }
        else
        {
            //상하 우선
            input.x = 0f;
        }

        //3. 정규화된 이동 벡터
        movement = input.normalized;

        //4. 애니메이터 파라미터 세팅
        float speed = movement.sqrMagnitude;  //0이면 정지, >0이면 이동 중

        anim.SetFloat("Speed", speed);

        if (speed > 0.001f)
        {
            //현재 이동 방향
            anim.SetFloat("Horizontal", movement.x);
            anim.SetFloat("Vertical", movement.y);

            //마지막 바라본 방향 저장 (Idle용)
            anim.SetFloat("LastX", movement.x);
            anim.SetFloat("LastY", movement.y);

            //좌우 플립 (오른쪽 기준으로 스프라이트 그려놨다고 가정)
            if (movement.x < 0) sr.flipX = false;   
            else if (movement.x > 0) sr.flipX = true; 
        }
    }

    void FixedUpdate()
    {
        //5. 실제 이동 처리 (물리)
        if (movement.sqrMagnitude > 0.001f)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }
}
