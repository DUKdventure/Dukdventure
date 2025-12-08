using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    [Header("참조")]
    public RunnerPlayerController player;
    public RatChaser rat;

    [Header("타임 제한 (버티기 시간)")]
    public float stageDuration = 30f;      // 몇 초 버티면 문 등장?
    float timer;
    bool doorSpawned = false;
    bool gameEnded = false;

    [Header("문 스폰 설정")]
    public GameObject doorPrefab;
    public float doorSpawnOffsetX = 8f;    // 플레이어보다 얼마 앞에 문이 생길지

    [Header("UI")]
    public TextMeshProUGUI timerText;                 // 없으면 그냥 null로 둬도 됨

    [Header("Panel")]
    public ClearPanel clearPanel;
    public FailPanel failPanel;

    [Header("Scene")]
    [SerializeField] string clearNextSceneName;   // 클리어 이동할 씬
    [SerializeField] string failNextSceneName;    // 실패에서 '나가기' 이동할 씬
    [SerializeField] string retrySceneName;

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
        Time.timeScale = 1f;

        timer = stageDuration;

        Debug.Log($"[StageManager] Start / stageDuration = {stageDuration}");
    }

    void Update()
    {
        if (gameEnded) return;

        // 타이머 감소
        if (!doorSpawned)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                timer = 0f;
                SpawnDoor();
            }
        }

        // UI 표시
        if (timerText != null)
        {
            timerText.text = $"{timer:0.0}s";
        }
    }

    void SpawnDoor()
    {
        if (doorPrefab == null || player == null) return;

        Vector3 spawnPos = player.transform.position + new Vector3(doorSpawnOffsetX, 0.81f, 0f);
        Instantiate(doorPrefab, spawnPos, Quaternion.identity);
        doorSpawned = true;
        Debug.Log("문 등장!");
    }

    // 문에 닿았을 때 DoorGoal이 호출
    public void OnStageClear()
    {
        if (gameEnded) return;

        gameEnded = true;
        Debug.Log("스테이지 클리어!");

        Time.timeScale = 0f;

        if (clearPanel != null)
        {
            // 점수 시스템 없으면 일단 0 넘겨도 됨
            int score = 0;

            clearPanel.ShowClear(
                score,
                OnClickRetryFromClear,  // 재도전
                OnClickNextFromClear,   // 다음
                OnClickMenuFromClear    // 메뉴
            );
        }
    }

    // 쥐에게 잡혔거나, 장애물에 죽었을 때 호출하면 됨
    public void OnGameOver()
    {
        if (gameEnded) return;

        gameEnded = true;
        Debug.Log("게임 오버!");

        Time.timeScale = 0f;

        if (failPanel != null)
        {
            int score = 0;

            failPanel.ShowFail(
                score,
                OnClickRetryFromFail,
                OnClickExitFromFail
            );
        }
    }

    void OnClickRetryFromClear()
    {
        Time.timeScale = 1f;
        string sceneName = string.IsNullOrEmpty(retrySceneName)
            ? SceneManager.GetActiveScene().name
            : retrySceneName;

        SceneManager.LoadScene(sceneName);
    }

    void OnClickNextFromClear()
    {
        Time.timeScale = 1f;

        if (!string.IsNullOrEmpty(clearNextSceneName))
        {
            SceneManager.LoadScene(clearNextSceneName);
        }
        else
        {
            // 비워두면 일단 현재 씬 재로드
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void OnClickMenuFromClear()
    {
        Time.timeScale = 1f;

        if (!string.IsNullOrEmpty(failNextSceneName))
        {
            SceneManager.LoadScene(failNextSceneName);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    void OnClickRetryFromFail()
    {
        Time.timeScale = 1f;
        string sceneName = string.IsNullOrEmpty(retrySceneName)
            ? SceneManager.GetActiveScene().name
            : retrySceneName;

        SceneManager.LoadScene(sceneName);
    }

    void OnClickExitFromFail()
    {
        Time.timeScale = 1f;

        if (!string.IsNullOrEmpty(failNextSceneName))
        {
            SceneManager.LoadScene(failNextSceneName);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
