using UnityEngine;

public class PlayerColorEffect : MonoBehaviour
{
    private SpriteRenderer sr;
    private Color originalColor;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    public void HitEffect()
    {
        StopAllCoroutines();
        StartCoroutine(HitFlash());
    }

    private System.Collections.IEnumerator HitFlash()
    {
        // 🔴 플레이어 빨갛게
        sr.color = Color.red;
        yield return new WaitForSeconds(0.2f);

        // 원래 색으로
        sr.color = originalColor;
    }
}
