using UnityEngine;

public class NoJumpHazard : MonoBehaviour
{
    [Header("스턴 설정")]
    public float stunDuration = 0.8f;      // 몇 초 동안 멈출지
    public bool knockback = true;
    public float knockbackPower = 2f;      // 뒤로 밀리는 힘

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        RunnerPlayerController player = other.GetComponent<RunnerPlayerController>();
        if (player == null) return;

        // 점프 상태인 채로 닿았을 때만 페널티
        if (player.IsJumping)
        {
            Debug.Log("점프 금지 구간에서 점프 → 휘청!");

            // 스턴 걸기
            player.ApplyStun(stunDuration);

            // 살짝 뒤로 밀기(선택)
            if (knockback)
            {
                Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    // 왼쪽(뒤쪽)으로 살짝 튕겨나가기
                    Vector2 vel = rb.linearVelocity;
                    vel.x = -knockbackPower;
                    rb.linearVelocity = vel;
                }
            }

            // 여기서 바로 GameOver는 안 함
            // 쥐가 알아서 따라와서 잡게 두기
        }
        else
        {
            // 점프 안 했으면 그냥 스쳐 지나감 (패널티 없음)
        }
    }
}
