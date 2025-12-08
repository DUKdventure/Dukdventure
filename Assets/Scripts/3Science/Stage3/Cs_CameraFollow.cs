using UnityEngine;

public class Cs_CameraFollow : MonoBehaviour
{
    public Transform target;
    public float followSpeed = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 pos = transform.position;
        pos.x = Mathf.Lerp(pos.x, target.position.x, followSpeed * Time.deltaTime);
        transform.position = pos;
    }
}
