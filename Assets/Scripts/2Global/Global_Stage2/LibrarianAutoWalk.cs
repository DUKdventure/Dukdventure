using UnityEngine;

public class LibrarianAutoWalk : MonoBehaviour
{
    public float moveSpeed = 2f;      
    public Transform[] waywalk;    // 이름 변경됨!

    private int targetIndex = 0;        
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (waywalk.Length > 0)
        {
            // 첫 시작 지점은 랜덤
            targetIndex = Random.Range(0, waywalk.Length);
            transform.position = waywalk[targetIndex].position;

            // 다음 목표 지점도 랜덤으로 설정
            targetIndex = Random.Range(0, waywalk.Length);
        }
    }

    void Update()
    {
        MoveToWaypoint();
    }

    void MoveToWaypoint()
    {
        if (waywalk.Length == 0) return;

        Transform target = waywalk[targetIndex];
        Vector3 dir = target.position - transform.position;

        // 거의 도착 → Idle + 다음 랜덤 위치 선택
        if (dir.magnitude < 0.05f)
        {
            animator.SetBool("IsWalking", false);

            // 다음 목적지 랜덤 선택
            targetIndex = Random.Range(0, waywalk.Length);
            return;
        }

        // 걷는 중
        animator.SetBool("IsWalking", true);

        // 이동
        transform.position += dir.normalized * moveSpeed * Time.deltaTime;

        // 바라보는 방향 회전
        if (dir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }
}
