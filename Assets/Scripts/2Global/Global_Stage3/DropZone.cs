using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DropZone : MonoBehaviour, IDropHandler
{
    public TMP_Text sentenceText;   // 전체 문장이 들어있는 TMP
    public string correctAnswer;    // 정답 단어
    public string placeholder = "(        )";  // 대체될 부분

    public void OnDrop(PointerEventData eventData)
    {
        DragText dragged = eventData.pointerDrag.GetComponent<DragText>();
        if (dragged == null) return;

        string droppedWord = dragged.textValue;

        // 1) 문장 안의 placeholder를 드래그된 단어로 교체
        sentenceText.text = sentenceText.text.Replace(placeholder, droppedWord);

        // 2) 드래그된 단어 비활성화
        dragged.gameObject.SetActive(false);

        // 3) 정답 판정
        if (droppedWord == correctAnswer)
        {
            Debug.Log("정답! 성공!");
        }
        else
        {
            Debug.Log("오답! 실패!");
        }
    }
}
