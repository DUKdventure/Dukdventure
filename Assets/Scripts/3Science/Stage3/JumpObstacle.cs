using UnityEngine;

public class JumpObstacle : MonoBehaviour
{
    public float pushBackSpeed = 1.5f;  // x축으로 살짝 뒤로 미는 힘(왼쪽)

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player")) return;

        var controller = collision.collider.GetComponent<RunnerPlayerController>();
        if (controller == null) return;

        //점프 중이거나, 공중에 떠 있으면 건들지 않기
        if (!controller.IsGrounded)
            return;

        var rb = collision.collider.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        //상자의 뒤쪽 방향(-right)으로 밀기 (회전 반영)
        Vector2 dir = -(Vector2)transform.right;

        Vector2 v = rb.linearVelocity;
        v.x = dir.x * pushBackSpeed;
        rb.linearVelocity = v;
    }
}
