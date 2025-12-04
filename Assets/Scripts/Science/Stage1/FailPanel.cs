using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FailPanel : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Button retryButton;
    [SerializeField] Button exitButton;   // "나가기" 버튼

    System.Action onRetry;
    System.Action onExit;

    

    /// <summary>
    /// 실패 패널 표시 + 버튼 콜백 세팅
    /// </summary>
    public void ShowFail(int score,
                         System.Action onRetry,
                         System.Action onExit)
    {
        this.onRetry = onRetry;
        this.onExit = onExit;

        if (scoreText) scoreText.text = $"{score}";

        if (retryButton)
        {
            retryButton.onClick.RemoveAllListeners();
            retryButton.onClick.AddListener(() => this.onRetry?.Invoke());
        }

        if (exitButton)
        {
            exitButton.onClick.RemoveAllListeners();
            // 여기서 바로 onExit 호출해도 되고,
            // "저장되지 않습니다" 팝업 띄우는 함수를 먼저 부른 뒤
            // 팝업에서 최종 확인 시 onExit() 호출하게 해도 됨.
            exitButton.onClick.AddListener(() => this.onExit?.Invoke());
        }

        gameObject.SetActive(true);
    }

    public void HideInstant()
    {
        gameObject.SetActive(false);
    }
}
