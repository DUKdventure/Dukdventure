using UnityEngine;

public class MovingPlankVertical : MonoBehaviour
{
    public float moveDistance = 1f;

    public float moveSpeed = 1f;

    public bool startUpwards = true;

    float startY;        // 시작 y 위치
    int moveDir;

    void Awake()
    {
        // 시작 위치 저장
        startY = transform.position.y;

        // 처음 방향 설정
        moveDir = startUpwards ? 1 : -1;
    }

    void Update()
    {
        // 현재 방향으로 이동
        float delta = moveSpeed * Time.deltaTime * moveDir;
        transform.position += new Vector3(0f, delta, 0f);

        float offset = transform.position.y - startY;

        // 위쪽 한계 도달
        if (offset >= moveDistance)
        {
            // 살짝 오버한 경우 정확히 경계에 붙여주고
            transform.position = new Vector3(transform.position.x, startY + moveDistance, transform.position.z);
            // 방향 반전(이제 아래로)
            moveDir = -1;
        }
        // 아래쪽 한계 도달
        else if (offset <= -moveDistance)
        {
            transform.position = new Vector3(transform.position.x, startY - moveDistance, transform.position.z);
            moveDir = 1;  // 이제 위로
        }
    }

    /// <summary>
    /// 라운드 리셋 시 처음 위치/방향으로 되돌리고 싶을 때 호출
    /// </summary>
    public void ResetPlank()
    {
        transform.position = new Vector3(transform.position.x, startY, transform.position.z);
        moveDir = startUpwards ? 1 : -1;
    }
}
