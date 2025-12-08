using UnityEngine;
using System.Collections;

public class BlinkingPlank : MonoBehaviour
{
    [Header("보이는 시간 / 사라지는 시간")]
    public float visibleTime = 1.5f;  // 판자가 보이는 시간
    public float hiddenTime = 1.0f;   // 판자가 사라져 있는 시간

    [Header("시작 상태")]
    public bool startVisible = true;  // 처음에 보이는 상태로 시작할지

    SpriteRenderer[] spriteRenderers;
    Collider2D[] colliders;

    bool isVisible;

    void Awake()
    {
        // 자식까지 포함해서 SpriteRenderer / Collider2D 싹 긁어오기
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        colliders = GetComponentsInChildren<Collider2D>();
    }

    void OnEnable()
    {
        // 시작 상태 설정
        isVisible = startVisible;
        ApplyState(isVisible);

        // 코루틴 시작
        StartCoroutine(BlinkRoutine());
    }

    IEnumerator BlinkRoutine()
    {
        while (true)
        {
            if (isVisible)
            {
                // 보이는 상태 유지
                yield return new WaitForSeconds(visibleTime);
                SetVisible(false);
            }
            else
            {
                // 사라진 상태 유지
                yield return new WaitForSeconds(hiddenTime);
                SetVisible(true);
            }
        }
    }

    void SetVisible(bool v)
    {
        isVisible = v;
        ApplyState(v);
    }

    void ApplyState(bool v)
    {
        // 눈에 보이는지
        if (spriteRenderers != null)
        {
            foreach (var sr in spriteRenderers)
            {
                if (sr != null) sr.enabled = v;
            }
        }

        // 콜라이더 on/off
        if (colliders != null)
        {
            foreach (var col in colliders)
            {
                if (col != null) col.enabled = v;
            }
        }
    }

    /// <summary>
    /// 라운드 재시작할 때 처음 상태로 되돌리고 싶으면 외부에서 호출
    /// </summary>
    public void ResetPlank()
    {
        isVisible = startVisible;
        ApplyState(isVisible);
    }
}
