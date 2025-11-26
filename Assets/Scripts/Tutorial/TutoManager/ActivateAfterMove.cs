using UnityEngine;

public class ActivateAfterMove : MonoBehaviour
{
    public PopMoveEffect effect;           // 팡 이동 스크립트
    public GameObject[] objectsToActivate; // 애니메이션 끝나고 켜질 것들

    void Start()
    {
        // PopMoveEffect가 끝나면 실행될 함수 등록
        effect.onMoveComplete = () =>
        {
            foreach (var obj in objectsToActivate)
                obj.SetActive(true);
        };
    }
}
