using UnityEngine;
using UnityEngine.UI;

public class LaserRay : MonoBehaviour
{
    public LineRenderer line;
    public float maxDist = 100f;
    public int maxBounce = 10;
    public LayerMask hitMask;

    public float requiredFocusTime = 3f;

    public Slider focusSlider;

    SamplePlate samplePlate;

    float focusTimer = 0f;
    bool isSharpOn = false;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        samplePlate = FindAnyObjectByType<SamplePlate>();

        if (focusSlider != null)
            focusSlider.value = 0f;
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

        bool hitPetriThisFrame = false;
        bool hitFocusThisFrame = false;

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
            if (hit.collider.CompareTag("Petri"))
            {
                hitPetriThisFrame = true;

                origin = hitPos + dir * 0.3f;
                continue;
            }

            //FocusLens 선명 이미지 켜기
            if (hit.collider.CompareTag("FocusLens"))
            {
                hitFocusThisFrame = true;
                break;
            }

            //그 외 충돌 → 레이저 멈춤
            break;
        }

        //=== 포커스 시간 체크 ===
        if (!isSharpOn)
        {
            if (hitFocusThisFrame && hitPetriThisFrame)
            {
                focusTimer += Time.deltaTime;

                if (focusTimer >= requiredFocusTime)
                {
                    isSharpOn = true;

                    // 샘플 선명하게
                    if (samplePlate != null)
                        samplePlate.ShowSharp();

                    // 여기서 클리어 처리
                    if (GameManager.Instance != null)
                        GameManager.Instance.OnClear();
                }
            }
            else
            {
                //이번 프레임에 포커스 렌즈를 안 맞았으면 시간 리셋
                focusTimer = 0f;
            }
        }

        if (samplePlate != null)
        {
            if (hitPetriThisFrame)
            {
                // 페트리에 빛이 닿아 있는 동안만 보이기
                if (isSharpOn)
                    samplePlate.ShowSharp();  // 초점 맞았으면 선명
                else
                    samplePlate.ShowBlur();   // 아직 초점 안 맞았으면 흐릿
            }
            else
            {
                // 빛이 페트리에서 벗어나면 다시 안 보이게
                samplePlate.HideAll();
            }
        }

        // === 게이지 UI 업데이트 ===
        if (focusSlider != null)
        {
            if (isSharpOn)
            {
                focusSlider.value = 1f; // 초점 맞으면 꽉 찬 상태로
            }
            else
            {
                float t = Mathf.Clamp01(focusTimer / requiredFocusTime);
                focusSlider.value = t;
            }
        }
    }

}
