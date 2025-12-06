using UnityEngine;
using System;
using System.Collections;

public class PopMoveEffect : MonoBehaviour
{
    public Transform targetPosition;
    public float moveDuration = 1.2f;

    public Vector3 popScale = new Vector3(1.3f, 1.3f, 1.3f);
    public Vector3 finalScale = Vector3.one;

    public Action onMoveComplete;  // ★ 애니메이션 끝 신호

    void OnEnable()
    {
        StartCoroutine(PopAndMove());
    }

    IEnumerator PopAndMove()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = targetPosition.position;

        Vector3 startScale = transform.localScale;

        float time = 0f;

        while (time < moveDuration)
        {
            time += Time.deltaTime;
            float t = time / moveDuration;

            float popT = EaseOutBack(t);
            float moveT = EaseOutCubic(t);

            transform.localScale = Vector3.LerpUnclamped(startScale, popScale, popT);
            transform.position = Vector3.Lerp(startPos, endPos, moveT);

            yield return null;
        }

        transform.position = endPos;
        transform.localScale = finalScale;

        // ★ 애니메이션 끝! → 신호 보내기
        onMoveComplete?.Invoke();
    }

    float EaseOutBack(float t)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1f;
        return 1 + c3 * Mathf.Pow(t - 1, 3) + c1 * Mathf.Pow(t - 1, 2);
    }

    float EaseOutCubic(float t)
    {
        return 1 - Mathf.Pow(1 - t, 3);
    }
}
