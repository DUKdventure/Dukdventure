using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI 패널")]
    public ClearPanel clearPanel;
    public FailPanel failPanel;
    public int score = 0;
    public TextMeshProUGUI timerText;

    [Header("Timer")]
    public bool useTimeLimit = false;
    public float timeLimit = 30f;   // 예: 30초 안에 성공해야 함

    float timeLeft;
    bool isGameOver = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        if (clearPanel) clearPanel.HideInstant();
        if (failPanel) failPanel.HideInstant();

        timeLeft = timeLimit;
        Time.timeScale = 1f;

        UpdateTimerUI();
    }

    void Update()
    {
        if (isGameOver) return;
        if (!useTimeLimit) return;

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            UpdateTimerUI();
            OnFail();
            return;
        }

        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        //텍스트 표시 (MM:SS 형식)
        if (timerText != null)
        {
            int t = Mathf.CeilToInt(timeLeft);
            int minutes = t / 60;
            int seconds = t % 60;
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }

    public void OnClear()
    {
        if (isGameOver) return;
        isGameOver = true;

        Debug.Log("클리어!");

        Time.timeScale = 0f;  // 게임 일시정지

        if (clearPanel)
        {
            clearPanel.ShowClear(
                score,
                OnClickRetry,          // 다시하기
                OnClickReturnSuccess,  // 다음/돌아가기(성공 위치)
                OnClickExitToMain      // 메뉴로/나가기
            );
        }
    }

    public void OnFail()
    {
        if (isGameOver) return;
        isGameOver = true;

        Debug.Log("실패!");

        Time.timeScale = 0f;

        if (failPanel)
        {
            failPanel.ShowFail(
                score,
                OnClickRetry,      // 다시하기
                OnClickExitToMain  // 나가기(실패/포기 위치)
            );
        }
    }

    //"다시하기" : 이 미니게임 씬 다시 로드
    public void OnClickRetry()
    {
        Time.timeScale = 1f;
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    //"성공 후 돌아가기" : 성공 위치로 메인씬 복귀
    public void OnClickReturnSuccess()
    {
        Time.timeScale = 1f;

        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.MiniGameClear();
        }
    }

    // "나가기" : 실패/포기 위치로 메인씬 복귀
    public void OnClickExitToMain()
    {
        Time.timeScale = 1f;

        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.MiniGameExit();
        }
    }
}
