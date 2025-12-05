using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClearPanel : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Button nextButton;
    [SerializeField] Button menuButton;
    [SerializeField] Button retryButton;

    System.Action onRetry;
    System.Action onNext;
    System.Action onMenu;

    
    /// <summary>
    /// 클리어 패널 표시 + 버튼 콜백 세팅
    /// </summary>
    public void ShowClear(int score,
                          System.Action onRetry,
                          System.Action onNext,
                          System.Action onMenu)
    {
        this.onRetry = onRetry;
        this.onNext = onNext;
        this.onMenu = onMenu;

        //텍스트 세팅
        if (scoreText) scoreText.text = $"{score}";

        //버튼 리스너 초기화 후 다시 연결
        if (retryButton)
        {
            retryButton.onClick.RemoveAllListeners();
            retryButton.onClick.AddListener(() => this.onRetry?.Invoke());
        }

        if (nextButton)
        {
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(() => this.onNext?.Invoke());
        }

        if (menuButton)
        {
            menuButton.onClick.RemoveAllListeners();
            menuButton.onClick.AddListener(() => this.onMenu?.Invoke());
        }
        Debug.Log("ClearPanel.ShowClear() 호출, score = " + score);
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 바로 숨기기 (애니메이션 필요하면 나중에 추가)
    /// </summary>
    public void HideInstant()
    {
        gameObject.SetActive(false);
    }
}
