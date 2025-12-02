using UnityEngine;

public class Goal : MonoBehaviour
{
    bool cleared = false;

    public void OnHitByLaser()
    {
        if (cleared) return;
        cleared = true;

        Debug.Log("클리어!");

        // TODO: 탈출 연출
        // - 문 열리는 애니메이션
        // - 씬 전환
        // - 클리어 UI 패널 표시 등
    }
}
