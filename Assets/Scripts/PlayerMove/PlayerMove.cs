using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private float moveSpeed = 50f;
    private Rigidbody2D rb;
    private Vector2 inputDir;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float x = -Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        inputDir = new Vector2(x, y).normalized;

        // 좌우 반전 처리
        if (x > 0) spriteRenderer.flipX = true;
    if (x < 0) spriteRenderer.flipX = false;

    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + inputDir * moveSpeed * Time.fixedDeltaTime);
    }
}
