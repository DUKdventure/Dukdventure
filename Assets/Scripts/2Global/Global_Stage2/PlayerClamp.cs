using UnityEngine;

public class PlayerClamp : MonoBehaviour
{
    // 움직임 제한 범위
    public float minX = -330f;
    public float maxX = 330f;
    public float minY = -200f;
    public float maxY = 200f;

    void Update()
    {
        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        transform.position = pos;
    }
}
