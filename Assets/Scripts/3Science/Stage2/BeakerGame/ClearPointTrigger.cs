using UnityEngine;
using System.Collections;

public class ClearPointTrigger : MonoBehaviour
{
    public int roundIndex = 0;
    public float delay = 2f;

    bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 디버그용 로그
        Debug.Log("Trigger 들어온 것: " + other.name);

        if (!other.transform.root.CompareTag("Player"))
            return;

        if (triggered) return; // 이미 한 번 발동했으면 무시
        triggered = true;

        // 2초 기다렸다가 라운드 넘기기
        StartCoroutine(DelayClearRoutine());
    }

    IEnumerator DelayClearRoutine()
    {
        Debug.Log("클리어 포인트 도달, " + delay + "초 후 라운드 이동");

        yield return new WaitForSeconds(delay);

        if (BeakerGameManager.Instance != null)
        {
            Debug.Log("클리어 → BeakerGameManager로 라운드 넘기기");
            BeakerGameManager.Instance.OnReachClearPoint(roundIndex);
        }
        else
        {
            Debug.LogWarning("BeakerGameManager.Instance 가 null 입니다.");
        }
    }
}
