using UnityEngine;

public class GameStageManager : MonoBehaviour
{
    public static GameStageManager Instance;

    public int currentStage = 1;  
    // 1 = 초록 → 2 = 노랑 → 3 = 빨강

    public GameObject flashTarget;  // Inspector에서 연결

    private void Awake()
    {
        Instance = this;
    }

    public void FoundBook(int stage)
    {
        Debug.Log($"[정답] stage {stage} 책 찾음!");

        // 단계별 UI 표시
        if (stage == 1) UIManager.Instance.ShowGreenBook();
        else if (stage == 2) UIManager.Instance.ShowYellowBook();
        else if (stage == 3) UIManager.Instance.ShowRedBook();

        currentStage++;

        // 3개의 책을 모두 찾았을 때
        if (currentStage > 3)
        {
            Debug.Log("모든 책을 찾았어요!");

            // 🔥 섬광 효과가 끝난 후 Clear 패널을 띄우기
            FlashSpawner.Instance.SpawnFlashAt(flashTarget, () =>
            {
                UIManager.Instance.ShowClear();
            });
        }
    }
}
