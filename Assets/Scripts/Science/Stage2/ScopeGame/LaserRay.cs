using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserRay : MonoBehaviour
{
    [Header("레이저 설정")]
    public float maxDistance = 50f;
    public int maxBounce = 10;
    public LayerMask hitMask; //Mirror, Lens, Wall, Goal 포함

    LineRenderer line;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
    }

    void Update()
    {
        CastLaser();
    }

    void CastLaser()
    {
        List<Vector3> points = new List<Vector3>();

        Vector3 origin = transform.position;
        Vector3 dir = transform.right; //오른쪽 방향으로 쏘기 (Emitter의 local X)

        points.Add(origin);

        for (int i = 0; i < maxBounce; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(origin, dir, maxDistance, hitMask);

            if (hit.collider == null)
            {
                //아무것도 안 맞으면 직선으로 끝까지
                points.Add(origin + dir * maxDistance);
                break;
            }

            //맞은 지점
            Vector3 hitPoint = hit.point;
            points.Add(hitPoint);

            //Goal에 닿았는지 체크
            if (hit.collider.CompareTag("Goal"))
            {
                Goal goal = hit.collider.GetComponent<Goal>();
                if (goal != null)
                {
                    goal.OnHitByLaser();
                }
                break;
            }

            //Mirror에 닿으면 반사
            if (hit.collider.CompareTag("Mirror"))
            {
                Vector2 inDir = dir;
                Vector2 normal = hit.normal;
                Vector2 reflectDir = Vector2.Reflect(inDir, normal).normalized;

                dir = reflectDir;
                origin = hitPoint + (Vector3)dir * 0.01f; // 살짝 앞으로
                continue;
            }

            //Lens에 닿으면 굴절(간단 버전: 렌즈가 정해준 방향으로 강제)
            if (hit.collider.CompareTag("Lens"))
            {
                Lens lens = hit.collider.GetComponent<Lens>();
                if (lens != null)
                {
                    dir = lens.GetOutDirection(dir, hit.normal);
                    origin = hitPoint + (Vector3)dir * 0.01f;
                    continue;
                }
            }

            //Wall 이나 기타: 여기서 끝
            break;
        }

        line.positionCount = points.Count;
        line.SetPositions(points.ToArray());
    }
}
