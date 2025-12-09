using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTrigger : MonoBehaviour
{
    public string nextSceneName;              // 이동할 씬 이름
    public Animator doorAnimator;             // 문 애니메이터
    private bool triggered = false;           // 중복 실행 방지

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered) return;

        if (collision.CompareTag("Player"))
        {
            triggered = true;

            // 문 애니메이션 실행
            if (doorAnimator != null)
                doorAnimator.SetTrigger("Open");
            SceneManager.LoadScene(nextSceneName);
        }
    }

    // 애니메이션 끝나는 지점에서 Animation Event로 호출
    public void OnDoorOpened()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("nextSceneName이 비어있습니다!");
        }
    }
}
