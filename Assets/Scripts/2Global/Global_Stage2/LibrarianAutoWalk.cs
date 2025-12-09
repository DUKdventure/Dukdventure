using UnityEngine;

public class LibrarianAutoWalk : MonoBehaviour
{
    public float moveSpeed = 2f;      
    public Transform[] waywalk;     

    private int index = 0;           
    private Animator animator;
    
    SpriteRenderer sr;

    void Start()
    {
        animator = GetComponent<Animator>();
        sr=GetComponent<SpriteRenderer>();
        
        if (waywalk.Length > 0)
            transform.position = waywalk[0].position;
    }

    void Update()
    {
        MoveToWaypoint();
    }

    void MoveToWaypoint()
    {
        if (waywalk.Length == 0) return;

        Transform target = waywalk[index];
        Vector3 dir = target.position - transform.position;

        // 거의 도착 → 다음 index로 이동
        if (dir.magnitude < 0.05f)
        {
            animator.SetBool("IsWalking", false);

            index++;

            // 배열 끝까지 갔다면 → 0으로 다시 돌아가기 (무한 반복)
            if (index >= waywalk.Length)
                index = 0;

            return;
        }

        // 걷는 중
        animator.SetBool("IsWalking", true);

        // 이동
        transform.position += dir.normalized * moveSpeed * Time.deltaTime;

        // 캐릭터 방향 회전
        if (dir.x <0)
            sr.flipX = true;  
    }
}
