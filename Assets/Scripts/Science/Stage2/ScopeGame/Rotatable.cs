using UnityEngine;

public class Rotatable : MonoBehaviour
{
    public float rotateSpeed = 100f;

    void OnMouseDrag()
    {
        // 마우스 X 이동량으로 회전
        float rot = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
        transform.Rotate(0, 0, -rot);
    }
}
