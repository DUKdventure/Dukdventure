using UnityEngine;

public class BookShelf : MonoBehaviour
{
    private int bookStage = 0;  
    // 0 = 책 없음, 1 = 초록, 2 = 노랑, 3 = 빨강

    public void SetBookStage(int stage)
    {
        bookStage = stage;
    }

    private void OnMouseDown()
    {   
        Debug.Log($"클릭한 책장: {gameObject.name} / bookStage = {bookStage}, currentStage = {GameStageManager.Instance.currentStage}");


        int currentStage = GameStageManager.Instance.currentStage;

        // 책 없음
        if (bookStage == 0)
        {
            Debug.Log("여기엔 책이 없어요.");
            return;
        }

        // 아직 이 책 찾을 단계가 아닌 경우
        if (bookStage != currentStage)
        {
            Debug.Log("아직 이 책을 찾을 단계가 아니야!");
            return;
        }

        // 올바른 책 클릭
        GameStageManager.Instance.FoundBook(bookStage);
    }
}
