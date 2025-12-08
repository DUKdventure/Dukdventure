using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DragText : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    public GameObject S;

    public string textValue; // 이 Text가 가진 실제 글자

    private void Awake()
    {
        canvas = FindFirstObjectByType<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        textValue = GetComponentInChildren<TMP_Text>().text.Trim();

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {   
       
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        CanvasGroup cg = S.GetComponent<CanvasGroup>();
        if (cg == null) cg = S.AddComponent<CanvasGroup>();

        cg.alpha = 0f;   // ← 요거 하나면 투명해짐

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }
}
