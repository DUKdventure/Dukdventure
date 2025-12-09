using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DropZone : MonoBehaviour, IDropHandler
{
    public TMP_Text sentenceText;
    public string correctAnswer;
    public string placeholder = "(        )";

    public string nextScene1;   // 정답일 때 이동할 씬
    public string nextScene2;   // 오답일 때 이동할 씬
    int index = 0;

    
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("🔻 OnDrop 호출됨");

        DragText dragged = eventData.pointerDrag.GetComponent<DragText>();
        if (dragged == null) return;

        string droppedWord = dragged.textValue;

        // placeholder를 드래그한 단어로 교체
        sentenceText.text = sentenceText.text.Replace(placeholder, droppedWord);

        dragged.gameObject.SetActive(false);

        // 정답
        if (droppedWord == correctAnswer)
        {
            Debug.Log("정답! 흰색 페이드!");
            Color whiteFade = Color.white;
            G_GameStateManager.Instance.MiniGameClear(index);
            index+=1;
            FadeManager.Instance.PlayFade(whiteFade, nextScene1);
        }
        else
        {
            Debug.Log("오답! 검은색 페이드!");
            Color blackFade = Color.black;
            FadeManager.Instance.PlayFade(blackFade, nextScene2);  // ← 변수 사용
        }
    }
}
