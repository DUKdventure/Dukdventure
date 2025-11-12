using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReagentQueueView : MonoBehaviour
{
    [Header("Layout")]
    public RectTransform slotRoot;
    public ReagentCard cardPrefab;
    public int visibleCount = 4;
    public Vector2 frontPos = new Vector2(0, 60);
    public Vector2 stepOffset = new Vector2(0, -40);
    public float frontScale = 1.0f;
    public float scaleStep = -0.08f; //뒤로 갈수록 작아짐
    public float moveDur = 0.18f;

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

    public void PushBack(ReagentData data, bool instant = false)
    {
        var c = Instantiate(cardPrefab, slotRoot);
        c.Set(data);

        //처음 생성 시 맨 뒤 위치/스케일로 놓기
        int idx = Mathf.Min(cards.Count, visibleCount - 1);
        var pos = GetPos(idx);
        var sca = GetScale(idx);
        c.RT.anchoredPosition = pos;
        c.transform.localScale = sca;
        cards.Add(c);

        if (!instant) RelayoutForward();    //전체를 목표 위치로 보정
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
        front.StartCoroutine(RemoveAnim(front, () =>
        {
            Destroy(front.gameObject);
        }));

        //나머지 앞으로 한 칸씩 이동
        for (int i = 0; i < cards.Count; i++)
        {
            var targetPos = GetPos(i);
            var targetScale = GetScale(i);
            StartCoroutine(cards[i].AnimateTo(targetPos, targetScale, moveDur));

        }
        StartCoroutine(Delay(moveDur, () => onAfterShift?.Invoke()));
    }

    public void RelayoutForward()
    {
        for (int i = 0; i < cards.Count && i < visibleCount; i++)
        {
            var targetPos = GetPos(i);
            var targetScale = GetScale(i);
            StartCoroutine(cards[i].AnimateTo(targetPos, targetScale, moveDur));
        }
    }

    Vector2 GetPos(int i)
    {
        return frontPos + stepOffset * i;
    }
    Vector3 GetScale(int i)
    {
        return Vector3.one * Mathf.Max(0.2f, frontScale + scaleStep * i);
    }

    IEnumerator RemoveAnim(ReagentCard c, Action onDone)
    {
        float dur = moveDur;
        float t = 0;
        var startS = c.transform.localScale;
        var startPos = c.RT.anchoredPosition;
        var endS = startS * 0.7f;
        var endPos = startPos + new Vector2(0, 30f);
        var img = c.icon; var txt = c.nameText;
        var imgColor = img ? img.color : Color.white;
        var txtColor = txt ? txt.color : Color.white;

        while (t < dur)
        {
            t += Time.deltaTime;
            float k = t / dur;
            c.transform.localScale = Vector3.Lerp(startS, endS, k);
            c.RT.anchoredPosition = Vector2.Lerp(startPos, endPos, k);
            if (img) img.color = new Color(imgColor.r, imgColor.g, imgColor.b, 1f - k);
            if (txt) txt.color = new Color(txtColor.r, txtColor.g, txtColor.b, 1f - k);
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
