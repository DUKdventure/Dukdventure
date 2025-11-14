using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 15f;
    private Rigidbody rb;
    private Vector3 inputDir;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        inputDir = new Vector3(x, 0, z).normalized;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + inputDir * moveSpeed * Time.fixedDeltaTime);

        if (inputDir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(inputDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 0.2f);
        }
    }
}
