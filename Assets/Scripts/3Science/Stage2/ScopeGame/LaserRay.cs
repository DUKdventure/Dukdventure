using UnityEngine;

public class LaserRay : MonoBehaviour
{
    public LineRenderer line;
    public float maxDist = 100f;
    public int maxBounce = 10;
    public LayerMask hitMask;

    SamplePlate samplePlate;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        samplePlate = FindAnyObjectByType<SamplePlate>();
    }

    void Update()
    {
        CastLaser();
    }

    void CastLaser()
    {
        Vector3 origin = transform.position;
        Vector3 dir = transform.right;

        line.positionCount = 1;
        line.SetPosition(0, origin);

        for (int i = 0; i < maxBounce; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(origin, dir, maxDist, hitMask);

            if (!hit.collider)
            {
                line.positionCount++;
                line.SetPosition(line.positionCount - 1, origin + dir * maxDist);
                break;
            }

            Vector3 hitPos = hit.point;
            line.positionCount++;
            line.SetPosition(line.positionCount - 1, hitPos);

            //Mirror 반사
            if (hit.collider.CompareTag("Mirror"))
            {
                dir = Vector2.Reflect(dir, hit.normal);
                origin = hitPos + dir * 0.01f;
                continue;
            }

            //SamplePlate 흐릿 이미지 켜기
            if (hit.collider.CompareTag("SamplePlate"))
            {
                samplePlate.ShowBlur();
                origin = hitPos + dir * 0.01f;
                continue;
            }

            //FocusLens 선명 이미지 켜기
            if (hit.collider.CompareTag("FocusLens"))
            {
                samplePlate.ShowSharp();
                break;
            }

            //그 외 충돌 → 레이저 멈춤
            break;
        }
    }

}
