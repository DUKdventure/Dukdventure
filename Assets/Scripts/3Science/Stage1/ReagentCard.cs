/*
 * UI 상의  이미지 한 장을 나타내는 컴포넌트.
 * 이미지 스프라이트를 설정하고, 위치/스케일 애니메이션을 담당한다.
 *
 * 사용 위치:
 *  - ReagentQueueView가 카드를 생성 및 이동시킬 때 호출함.
 */
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ReagentCard : MonoBehaviour
{
    public Image icon;  //시약 이미지

    RectTransform rt;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    //시약 이미지 세팅
    public void Set(ReagentData data)
    {
        if (!icon || data == null) return;

        icon.sprite = data.sprite;

        var iconRT = icon.rectTransform;

        // 크기
        if (data.uiSize.x > 0f && data.uiSize.y > 0f)
            iconRT.sizeDelta = data.uiSize;
        else
            icon.SetNativeSize();

        //피봇 적용
        iconRT.pivot = data.uiPivot;

        //카드 안에서 위치 보정
        iconRT.anchoredPosition = data.uiOffset;
    }

    //지정된 위치와 스케일로 부드럽게 이동
    public IEnumerator AnimateTo(Vector2 targetPos, Vector3 targetScale, float dur)
    {
        var startPos = rt.anchoredPosition;
        var startScale= transform.localScale;
        float t = 0f;
        while (t < dur)
        {
            t += Time.deltaTime;
            float k = Mathf.SmoothStep(0, 1, t / dur);
            rt.anchoredPosition = Vector2.Lerp(startPos, targetPos, k);
            transform.localScale = Vector3.Lerp(startScale, targetScale, k);
            yield return null;
        }
        rt.anchoredPosition = targetPos;
        transform.localScale = targetScale;
    }

    public RectTransform RT => rt;

    public void SetDarkness(float brightness)
    {
        if (icon)
        {
            var c = icon.color;
            icon.color = new Color(1f * brightness, 1f * brightness, 1f * brightness, c.a);
        }
    }
    
}
