using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ReagentCard : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI nameText;

    RectTransform rt;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    public void Set(ReagentData data)
    {
        if (icon)
            icon.sprite = data.sprite;
        if (nameText)
            nameText.text = data.displayName;
    }

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
}
