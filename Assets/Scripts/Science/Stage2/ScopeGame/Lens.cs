using UnityEngine;

public class Lens : MonoBehaviour
{
    [Header("Direction")]
    public Vector2 outDirection = Vector2.up;

    // 필요하면 회전 고려해서 local→world 방향 변환
    public Vector2 GetOutDirection(Vector2 inDir, Vector2 hitNormal)
    {
        // 현재 렌즈의 회전값을 적용한 방향
        Vector2 dir = transform.TransformDirection(outDirection);
        return dir.normalized;
    }
}
