using UnityEngine;
using System.Collections;

public class FlashObject : MonoBehaviour
{
    public float expandTime = 0.3f;
    public float fadeTime = 0.3f;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void PlayFlash(System.Action onFinished = null)
    {
        StartCoroutine(FlashRoutine(onFinished));
    }

    IEnumerator FlashRoutine(System.Action onFinished)
    {
        // 시작 상태
        transform.localScale = Vector3.one * 0.1f;
        sr.color = new Color(1, 1, 1, 1);

        // 1) 확장
        float t = 0;
        while (t < expandTime)
        {
            t += Time.deltaTime;
            float scale = Mathf.Lerp(0.1f, 10f, t / expandTime);
            transform.localScale = new Vector3(scale, scale, 1);
            yield return null;
        }

        // 2) 페이드 아웃
        t = 0;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(1f, 0f, t / fadeTime);
            sr.color = new Color(1, 1, 1, a);
            yield return null;
        }

        // 효과 끝났으니 콜백 실행
        onFinished?.Invoke();

        Destroy(gameObject);
    }
}
