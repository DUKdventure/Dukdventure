using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//각 시약에 정답 색상(DyeColor)을 저장해두는 단순 데이터 홀더
//ReagentVIew에서 시약 생성 시 자동으로 추가됨
public class ReagentCardDataHolder : MonoBehaviour { public DyeColor target; }

/*
 * 시약들이 뒤에서 앞으로 줄지어 나오고,
 * 정답 시 맨 앞 시약이 사라지고 뒤 시약들이 앞으로 이동하는 연출을 담당한다.
 *
 * 주요 기능:
 *  - PushBack() : 새 시약 생성 및 뒤에 추가
 *  - RemoveFrontAndShiftForward() : 맨 앞 시약 제거 + 나머지 앞으로 이동
 *  - RelayoutForward() : 전체 시약 재배치 (애니메이션 포함)
 *
 * 사용 위치:
 *  - ReagentGame(메인 게임 매니저)에서 호출
 */
public class ReagentView : MonoBehaviour
{
    [Header("Layout")]
    public RectTransform slotRoot;
    public ReagentCard cardPrefab;
    public int visibleCount = 5;
    public Vector2 frontPos = new Vector2(0, -60);
    public Vector2 stepOffset = new Vector2(0, 70);
    public float frontScale = 1.0f;
    public float scaleStep = -0.08f; //뒤로 갈수록 작아짐
    public float moveDur = 0.18f;

    [Header("Brightness")]
    public float frontBrightness = 1f;       
    public float brightnessStep = -0.15f;

    readonly List<ReagentCard> cards = new List<ReagentCard>();
    
    //현재 카드 개수
    public int Count => cards.Count;
    public ReagentCard Front => cards.Count > 0 ? cards[0] : null;

    public void ClearAll()
    {
        for (int i = cards.Count - 1; i >= 0; i--)
        {
            Destroy(cards[i].gameObject);
        }
        cards.Clear();
    }

    void RefreshSiblingOrder()
    {
        // cards[0] = 맨 앞 카드 -> 가장 마지막 sibling (화면 맨 위)
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].transform.SetSiblingIndex(cards.Count - 1 - i);
        }
    }

    public void PushBack(ReagentData data, bool instant = false)
    {
        var c = Instantiate(cardPrefab, slotRoot);
        c.Set(data);

        var holder = c.gameObject.AddComponent<ReagentCardDataHolder>();
        holder.target = data.targetColor;

        //처음 생성 시 맨 뒤 위치/스케일로 놓기
        int idx = Mathf.Min(cards.Count, visibleCount - 1);

        c.RT.anchoredPosition = GetPos(idx);
        c.transform.localScale = GetScale(idx);

        float brightness = Mathf.Clamp(frontBrightness + brightnessStep * idx, 0.2f, 1f);
        c.SetDarkness(brightness);

        cards.Add(c);

        //전체를 목표 위치로 보정
        if (!instant) RelayoutForward();
        RefreshSiblingOrder();
    }

    public void RemoveFrontAndShiftForward(Action onAfterShift = null)
    {
        if(cards.Count == 0)
        {
            onAfterShift?.Invoke();
            return;
        }

        //맨 앞 페이드/축소 제거
        var front = cards[0];
        cards.RemoveAt(0);
        front.StartCoroutine(RemoveAnim(front, () => Destroy(front.gameObject)));

        //나머지 앞으로 한 칸씩 이동
        for (int i = 0; i < cards.Count; i++)
        {
            var card = cards[i];
            var pos = GetPos(i);
            var scale = GetScale(i);

            StartCoroutine(card.AnimateTo(pos, scale, moveDur));

            // 밝기 적용
            float brightness = Mathf.Clamp(frontBrightness + brightnessStep * i, 0.2f, 1f);
            card.SetDarkness(brightness);
        }

        RefreshSiblingOrder();

        StartCoroutine(Delay(moveDur, () => onAfterShift?.Invoke()));
    }

    public void RelayoutForward()
    {
        for (int i = 0; i < cards.Count && i < visibleCount; i++)
        {
            var card = cards[i];

            var pos = GetPos(i);
            var scale = GetScale(i);

            StartCoroutine(card.AnimateTo(pos, scale, moveDur));

            // 밝기 설정
            float brightness = Mathf.Clamp(frontBrightness + brightnessStep * i, 0.2f, 1f);
            card.SetDarkness(brightness);
        }

        RefreshSiblingOrder();
    }

    Vector2 GetPos(int i) => frontPos + stepOffset * i;

    Vector3 GetScale(int i) => Vector3.one * Mathf.Max(0.2f, frontScale + scaleStep * i);

    IEnumerator RemoveAnim(ReagentCard c, Action onDone)
    {
        float dur = moveDur;
        float t = 0;
        var startS = c.transform.localScale;
        var endS = startS * 0.7f;

        var startPos = c.RT.anchoredPosition;
        var endPos = startPos + new Vector2(0, 30f);
        var img = c.icon;
        var imgColor = img ? img.color : Color.white;

        while (t < dur)
        {
            t += Time.deltaTime;
            float k = t / dur;
            c.transform.localScale = Vector3.Lerp(startS, endS, k);
            c.RT.anchoredPosition = Vector2.Lerp(startPos, endPos, k);
            if (img) img.color = new Color(imgColor.r, imgColor.g, imgColor.b, 1f - k);
            yield return null;
        }
        onDone?.Invoke();
    }

    IEnumerator Delay(float sec, Action a)
    {
        yield return new WaitForSeconds(sec);
        a?.Invoke();
    }
}
