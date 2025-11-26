using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using TMPro;

public class GameOverPanel : MonoBehaviour
{
    public CanvasGroup cg;          //Panel_GameOver에 붙은 CanvasGroup
    public TextMeshProUGUI title;   //"TIME UP!"
    public TextMeshProUGUI scoreText;

    [Header("Buttons")]
    public Button retryButton;      // 실패일 때만 활성
    public Button nextButton;       // 성공일 때만 활성
    public Button quitButton;

    public float fadeDuration = 0.25f;

    void Reset()
    { 
        cg = GetComponent<CanvasGroup>(); 
    }

    public void HideInstant()
    {
        if (!cg) return;
        gameObject.SetActive(false);
        cg.alpha = 0f;
        cg.interactable = false;
        cg.blocksRaycasts = false;

        ClearListeners();
    }

    void ClearListeners()
    {
        if (retryButton) retryButton.onClick.RemoveAllListeners();
        if (nextButton) nextButton.onClick.RemoveAllListeners();
        if (quitButton) quitButton.onClick.RemoveAllListeners();
    }

    public void ShowClear(string titleMsg, int score, UnityAction onNext, UnityAction onQuit = null)
    {
        SetupCommon(titleMsg, score);

        //버튼 가시성
        if (retryButton) retryButton.gameObject.SetActive(false);
        if (nextButton) nextButton.gameObject.SetActive(true);

        //리스너
        ClearListeners();
        if (nextButton && onNext != null) nextButton.onClick.AddListener(onNext);
        if (quitButton && onQuit != null) quitButton.onClick.AddListener(onQuit);

        StartCoroutine(FadeIn());
    }

    public void ShowFail(string titleMsg, int score, UnityAction onRetry, UnityAction onQuit = null)
    {
        SetupCommon(titleMsg, score);

        if (retryButton) retryButton.gameObject.SetActive(true);
        if (nextButton) nextButton.gameObject.SetActive(false);

        ClearListeners();
        if (retryButton && onRetry != null) retryButton.onClick.AddListener(onRetry);
        if (quitButton && onQuit != null) quitButton.onClick.AddListener(onQuit);

        StartCoroutine(FadeIn());
    }

    void SetupCommon(string titleMsg, int score)
    {
        gameObject.SetActive(true);
        if (title) title.text = titleMsg;
        if (scoreText) scoreText.text = $"Score {score}";
    }

    IEnumerator FadeIn()
    {
        cg.alpha = 0f;
        cg.interactable = false;
        cg.blocksRaycasts = false;
        

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime; //타임스케일 0이어도 보이게
            cg.alpha = Mathf.Clamp01(t / fadeDuration);
            yield return null;
        }
        cg.alpha = 1f;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }
}
